using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Tag;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace api.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDBContext _context;
        public TagRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        //public async Task<List<Tag>> GetAllAsync()
        //{
        //    return await _context.Tags.Include(x => x.photos).ToListAsync();
        //}
        public async Task<List<Tag>> GetFilteredAsync(TagFilterDTO filterDTO)
        {
            var query = _context.Tags.Include(x => x.photos).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterDTO.tag))
            {
                query = query.Where(x => x.tag.Contains(filterDTO.tag));
            }

            if (filterDTO.photoIDs?.Any() ?? false)
            {
                foreach (int filterphotoid in filterDTO.photoIDs)
                {
                    query = query.Where(x => x.photos.Any(p => p.Photoid == filterphotoid));
                }
            }

            if (!string.IsNullOrWhiteSpace(filterDTO.sortBy))
            {
                foreach (string colname in typeof(Photo).GetProperties().Select(x => x.Name).ToList())
                {
                    if (filterDTO.sortBy.Trim().Equals(colname, StringComparison.OrdinalIgnoreCase))
                    {
                        query = query.OrderBy($"{colname} {(filterDTO.isDescending ? "descending" : "ascending")}");
                    }
                }
            }

            return await query.ToListAsync();
        }
        public async Task<Tag?> GetByIdAsync(int id)
        {
            return await _context.Tags.Include(x => x.photos).FirstOrDefaultAsync(x => x.id == id);
        }
        public async Task<string?> GetPreviewImageByIdAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null || tag.previewpath == null)
            {
                return null;
            }

            return tag.previewpath;
        }
        public async Task<Tag?> CreateAsync(Tag tagModel)
        {   
            var sametag = await _context.Tags.FirstOrDefaultAsync(x => x.tag == tagModel.tag);

            if (sametag != null)
            {
                return null;
            }
            
            if (tagModel.previewpath == "")
            {
                string previewpath = Path.Combine(Directory.GetCurrentDirectory(), "photos", "blanktagpreview");
                if (!File.Exists($"{previewpath}.jpg"))
                {
                    var previewimage = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAlgAAAGQCAIAAAD9V4nPAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAABM5JREFUeNrs1TEBAAAIwzDAv+dhgo9EQp92kgKAr0YCAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAI5QAACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAuLACDAAWEgYdjoP45gAAAABJRU5ErkJggg==");
                    File.WriteAllBytes($"{previewpath}.jpg", previewimage);
                }
                tagModel.previewpath = $"{previewpath}.jpg";
            }

            await _context.Tags.AddAsync(tagModel);
            await _context.SaveChangesAsync();
            return tagModel;
        }
        public async Task<Tag?> UpdateAsync(int id, UpdateTagRequestDTO tagDTO)
        {
            var existingTag = await _context.Tags.Include(x => x.photos).FirstOrDefaultAsync(x => x.id == id);

            if (existingTag == null)
            {
                return null;
            }

            List<int> existingPhotoids = existingTag.photos.Select(x => x.Photoid).ToList();
            List<int> newPhotoids = tagDTO.photos.ToList();
            List<PhotoTag> photosToRemove = existingTag.photos.Where(x => !newPhotoids.Contains(x.Photoid)).ToList();
            _context.PhotoTags.RemoveRange(photosToRemove);
            List<int> photosToAdd = newPhotoids.Except(existingPhotoids).ToList();
            foreach (var Photoid in photosToAdd)
            {
                existingTag.photos.Add(new PhotoTag{Photoid = Photoid, Tagid = id});
            }


            var sameTag = await _context.Tags.Include(x => x.photos).FirstOrDefaultAsync(x => x.tag == tagDTO.tag && x.id != existingTag.id);

            if (sameTag == null)
            {
                existingTag.tag = tagDTO.tag;

                if (tagDTO.photos.Any())
                {
                    Random randomphoto = new Random();
                    var existingTagPhotos = await _context.Photos.Where(x => tagDTO.photos.Contains(x.id)).ToListAsync();
                    existingTag.previewpath = existingTagPhotos[randomphoto.Next(existingTagPhotos.Count-1)].previewpath;
                }
                else
                {
                    string previewpath = Path.Combine(Directory.GetCurrentDirectory(), "photos", $"blanktagpreview");
                    existingTag.previewpath = $"{previewpath}.jpg";
                }

                await _context.SaveChangesAsync();
                return existingTag;
            }
            else
            {
                List<int> sametagPhotoids = sameTag.photos.Select(x => x.Photoid).ToList();
                List<int> sametagPhotosToAdd = existingPhotoids.Except(sametagPhotoids).ToList();
                foreach (var Photoid in sametagPhotosToAdd)
                {
                    sameTag.photos.Add(new PhotoTag{Photoid = Photoid, Tagid = sameTag.id});
                }
                
                if (sameTag.photos.Any())
                {
                    Random randomphoto = new Random();
                    var sameTagPhotos = await _context.Photos.Where(x => sameTag.photos.Select(p => p.Photoid).Contains(x.id)).ToListAsync();
                    sameTag.previewpath = sameTagPhotos[randomphoto.Next(sameTagPhotos.Count - 1)].previewpath;
                }
                else
                {
                    string previewpath = Path.Combine(Directory.GetCurrentDirectory(), "photos", $"blanktagpreview");
                    sameTag.previewpath = $"{previewpath}.jpg";
                }

                _context.Tags.Remove(existingTag);
                await _context.SaveChangesAsync();
                return sameTag;
            }
        }
        public async Task<Tag?> DeleteAsync(int id)
        {
            var tagModel = await _context.Tags.FirstOrDefaultAsync(x => x.id == id);
            if (tagModel == null)
            {
                return null;
            }
            _context.Tags.Remove(tagModel);
            await _context.SaveChangesAsync();
            return tagModel;
        }
        public Task<bool> TagExists(int id)
        {
            return _context.Tags.AnyAsync(x => x.id == id);
        }
    }
}