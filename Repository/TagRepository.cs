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
        private readonly ILogger<TagRepository> _logger;
        public TagRepository(ApplicationDBContext context, ILogger<TagRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //public async Task<List<Tag>> GetAllAsync()
        //{
        //    return await _context.Tags.Include(x => x.photos).ToListAsync();
        //}
        public async Task<List<Tag>> GetFilteredAsync(TagFilterDTO filterDTO)
        {
            var query = _context.Tags.Include(x => x.Photos).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterDTO.Name))
            {
                query = query.Where(x => x.Name.Contains(filterDTO.Name));
            }

            if (filterDTO.PhotoIDs?.Any() ?? false)
            {
                foreach (int filterphotoid in filterDTO.PhotoIDs)
                {
                    query = query.Where(x => x.Photos.Any(p => p.PhotoId == filterphotoid));
                }
            }

            if (!string.IsNullOrWhiteSpace(filterDTO.SortBy))
            {
                foreach (string colname in typeof(Tag).GetProperties().Select(x => x.Name).ToList())
                {
                    if (filterDTO.SortBy.Trim().Equals(colname, StringComparison.OrdinalIgnoreCase))
                    {
                        if (colname == "Id")
                        {
                            query = query.OrderBy($"{colname} {(filterDTO.IsDescending ? "descending" : "ascending")}");
                        }
                        else
                        {
                            query = query.OrderBy($"{colname} {(filterDTO.IsDescending ? "descending" : "ascending")}{$", Id {(filterDTO.IsDescending ? "descending" : "ascending")}"}");
                        }
                    }
                }
            }

            return await query.ToListAsync();
        }
        public async Task<Tag?> GetByIdAsync(int id)
        {
            return await _context.Tags.Include(x => x.Photos).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<string?> GetPreviewImageByIdAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);

            if (tag == null || tag.PreviewPath == null)
            {
                return null;
            }

            return tag.PreviewPath;
        }
        public async Task<Tag?> CreateAsync(Tag tagModel)
        {   
            var sametag = await _context.Tags.FirstOrDefaultAsync(x => x.Name == tagModel.Name);

            if (sametag != null)
            {
                return null;
            }
            
            if (tagModel.PreviewPath == "")
            {
                string previewpath = Path.Combine(Directory.GetCurrentDirectory(), "photos", "blanktagpreview");
                if (!File.Exists($"{previewpath}.jpg"))
                {
                    var previewimage = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAlgAAAGQCAIAAAD9V4nPAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAABM5JREFUeNrs1TEBAAAIwzDAv+dhgo9EQp92kgKAr0YCAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQAIwQACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAjBAAI5QAACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEACMEwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBwAgBMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAMEIAuLACDAAWEgYdjoP45gAAAABJRU5ErkJggg==");
                    File.WriteAllBytes($"{previewpath}.jpg", previewimage);
                }
                tagModel.PreviewPath = $"{previewpath}.jpg";
            }

            await _context.Tags.AddAsync(tagModel);
            await _context.SaveChangesAsync();
            return tagModel;
        }
        public async Task<Tag?> UpdateAsync(int id, UpdateTagRequestDTO tagDTO)
        {
            var existingTag = await _context.Tags.Include(x => x.Photos).FirstOrDefaultAsync(x => x.Id == id);

            if (existingTag == null)
            {
                return null;
            }

            List<int> existingPhotoids = existingTag.Photos.Select(x => x.PhotoId).ToList();
            List<int> newPhotoids = tagDTO.Photos.ToList();
            List<PhotoTag> photosToRemove = existingTag.Photos.Where(x => !newPhotoids.Contains(x.PhotoId)).ToList();
            _context.PhotoTags.RemoveRange(photosToRemove);
            List<int> photosToAdd = newPhotoids.Except(existingPhotoids).ToList();
            foreach (var Photoid in photosToAdd)
            {
                existingTag.Photos.Add(new PhotoTag{PhotoId = Photoid, TagId = id});
            }


            var sameTag = await _context.Tags.Include(x => x.Photos).FirstOrDefaultAsync(x => x.Name == tagDTO.Name && x.Id != existingTag.Id);

            if (sameTag == null)
            {
                existingTag.Name = tagDTO.Name;

                if (tagDTO.Photos.Any())
                {
                    Random randomphoto = new Random();
                    var existingTagPhotos = await _context.Photos.Where(x => tagDTO.Photos.Contains(x.Id)).ToListAsync();
                    existingTag.PreviewPath = existingTagPhotos[randomphoto.Next(existingTagPhotos.Count-1)].PreviewPath;
                }
                else
                {
                    string previewpath = Path.Combine(Directory.GetCurrentDirectory(), "photos", $"blanktagpreview");
                    existingTag.PreviewPath = $"{previewpath}.jpg";
                }

                await _context.SaveChangesAsync();
                return existingTag;
            }
            else
            {
                List<int> sametagPhotoids = sameTag.Photos.Select(x => x.PhotoId).ToList();
                List<int> sametagPhotosToAdd = existingPhotoids.Except(sametagPhotoids).ToList();
                foreach (var Photoid in sametagPhotosToAdd)
                {
                    sameTag.Photos.Add(new PhotoTag{PhotoId = Photoid, TagId = sameTag.Id});
                }
                
                if (sameTag.Photos.Any())
                {
                    Random randomPhoto = new Random();
                    var sameTagPhotos = await _context.Photos.Where(x => sameTag.Photos.Select(p => p.PhotoId).Contains(x.Id)).ToListAsync();
                    sameTag.PreviewPath = sameTagPhotos[randomPhoto.Next(sameTagPhotos.Count - 1)].PreviewPath;
                }
                else
                {
                    string previewPath = Path.Combine(Directory.GetCurrentDirectory(), "photos", $"blanktagpreview");
                    sameTag.PreviewPath = $"{previewPath}.jpg";
                }

                _context.Tags.Remove(existingTag);
                await _context.SaveChangesAsync();
                return sameTag;
            }
        }
        public async Task<Tag?> DeleteAsync(int id)
        {
            var tagModel = await _context.Tags.FirstOrDefaultAsync(x => x.Id == id);
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
            return _context.Tags.AnyAsync(x => x.Id == id);
        }
    }
}