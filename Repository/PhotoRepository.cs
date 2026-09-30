using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs;
using api.DTOs.Photo;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using api.HelperFunctions;
using ImageMagick;
using System.Drawing;

namespace api.Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<PhotoRepository> _logger;
        public PhotoRepository (ApplicationDBContext context, ILogger<PhotoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        //public async Task<List<Photo>> GetAllAsync()
        //{
        //    return await _context.Photos.Include(x => x.tags).ThenInclude(x => x.Tag).ToListAsync();
        //}
        public async Task<List<Photo>> GetFilteredAsync(PhotoFilterDTO filterDTO)
        {
            var query = _context.Photos.Include(x => x.tags).ThenInclude(x => x.Tag).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterDTO.caption))
            {
                query = query.Where(x => x.caption.Contains(filterDTO.caption));
            }
            if (!string.IsNullOrWhiteSpace(filterDTO.description))
            {
                query = query.Where(x => x.description.Contains(filterDTO.description));
            }
            if (filterDTO.tagIds?.Any() ?? false)
            {
                foreach (int filtertagid in filterDTO.tagIds)
                {
                    query = query.Where(x => x.tags.Any(t => t.Tagid == filtertagid));
                }
            }

            if (filterDTO.minShutterspeed != null)
            {
                query = query.Where(x => x.shutterspeed <= filterDTO.minShutterspeed.Value);
            }
            if (filterDTO.maxShutterspeed != null)
            {
                query = query.Where(x => x.shutterspeed >= filterDTO.maxShutterspeed.Value);
            }
            if (filterDTO.minApperture.HasValue)
            {
                query = query.Where(x => x.apperture >= filterDTO.minApperture);
            }
            if (filterDTO.maxApperture.HasValue)
            {
                query = query.Where(x => x.apperture <= filterDTO.maxApperture);
            }
            if (filterDTO.minIso.HasValue)
            {
                query = query.Where(x => x.iso >= filterDTO.minIso);
            }
            if (filterDTO.maxIso.HasValue)
            {
                query = query.Where(x => x.iso <= filterDTO.maxIso);
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

            var skipNum = (filterDTO.pageNum - 1) * filterDTO.pageSize;

            return await query.Skip(skipNum).Take(filterDTO.pageSize).ToListAsync();
        }
        public async Task<Photo?> GetByIdAsync(int id)
        {
            return await _context.Photos.Include(x => x.tags).ThenInclude(x => x.Tag).FirstOrDefaultAsync(x => x.id == id);
        }
        public async Task<string?> GetImageByIdAsync(int id)
        {
            var photo = await _context.Photos.FindAsync(id);

            if (photo == null || photo.path == null)
            {
                return null;
            }

            return photo.path;
        }
        public async Task<string?> GetPreviewImageByIdAsync(int id)
        {
            var photo = await _context.Photos.FindAsync(id);

            if (photo == null || photo.previewpath == null)
            {
                return null;
            }

            return photo.previewpath;
        }
        public async Task<Photo> CreateAsync(Photo photoModel)
        {

            await _context.Photos.AddAsync(photoModel);
            await _context.SaveChangesAsync();

            if (photoModel.path != null)
            {
                using (var mainimage = new MagickImage(photoModel.path))
                using (var previewimage = new MagickImage(photoModel.path))
                {
                    var profile = mainimage.GetExifProfile();

                    if (profile != null)
                    {
                        foreach (var value in profile.Values)
                        {
                            if (value.Tag == ExifTag.ExposureTime)
                            {
                                photoModel.exposure = value.ToString();
                            }
                            if (value.Tag == ExifTag.ShutterSpeedValue)
                            {
                                var apexshutterspeed = ((SignedRational)value.GetValue()).ToDouble();
                                photoModel.shutterspeed = (int)Math.Ceiling(Math.Pow(2, apexshutterspeed));
                            }
                            if (value.Tag == ExifTag.FNumber)
                            {
                                var rational = value.GetValue() as Rational?;
                                if (rational.HasValue)
                                {
                                    photoModel.apperture = (float)rational.Value.ToDouble();
                                };
                            }
                            if (value.Tag == ExifTag.RecommendedExposureIndex)
                            {
                                int.TryParse(value.ToString(), out int intvalue);
                                photoModel.iso = intvalue;
                            }
                            _logger.LogInformation($"{value.Tag}({value.DataType}): {value.ToString()}");
                        };
                    };

                    mainimage.Strip();
                    previewimage.Strip();

                    if ((mainimage.Width > 2000) || (mainimage.Height > 2000))
                    {
                        mainimage.Resize(new MagickGeometry(2000, 2000)
                        {
                            IgnoreAspectRatio = false
                        });
                    }

                    previewimage.Resize(new MagickGeometry(1000, 1000)
                    {
                        IgnoreAspectRatio = false
                    });

                    uint mainquality = 100;
                    uint previewquality = 100;

                    mainimage.Format = MagickFormat.Jpg;
                    previewimage.Format = MagickFormat.Jpg;

                    while (mainquality > 5)
                    {
                        mainimage.Quality = mainquality;

                        if (mainimage.ToByteArray().Length > 2000 * 1024)
                        {
                            mainquality -= 5;
                        }
                        else
                        {
                            break;
                        }
                    }

                    while (previewquality > 5)
                    {
                        previewimage.Quality = previewquality;

                        if (previewimage.ToByteArray().Length > 500 * 1024)
                        {
                            previewquality -= 5;
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    string imagepath = Path.Combine(Directory.GetCurrentDirectory(), "photos", $"{photoModel.id.ToString().PadLeft(6, '0')}");
                    File.Delete(photoModel.path);
                    photoModel.path = $"{imagepath}.jpg";
                    photoModel.previewpath = $"{imagepath}p.jpg";
                    mainimage.Write(photoModel.path);
                    previewimage.Write(photoModel.previewpath);
                };
            };

            await _context.SaveChangesAsync();

            return photoModel;
        }
        public async Task<Photo?> UpdateAsync(int id, UpdatePhotoRequestDTO photoDTO)
        {
            var existingPhoto = await _context.Photos.Include(x => x.tags).ThenInclude(x => x.Tag).FirstOrDefaultAsync(x => x.id == id);
            if (existingPhoto == null)
            {
                return null;
            }

            byte[]? imagebytes = null;
            if (photoDTO.image != null && photoDTO.image.Length > 0)
            {

                using (var ms = new MemoryStream())
                {
                    await photoDTO.image.CopyToAsync(ms);
                    imagebytes = ms.ToArray();
                }
            };

            /*if (imagebytes != null)
            {
                File.WriteAllBytes(existingPhoto.path, imagebytes);
            }*/

            existingPhoto.caption = photoDTO.caption;
            existingPhoto.description = photoDTO.description ?? "";
            existingPhoto.sortorder = photoDTO.sortorder;
            existingPhoto.exposure = null;
            existingPhoto.shutterspeed = null;
            existingPhoto.apperture = null;
            existingPhoto.shutterspeed = null;
            existingPhoto.iso = null;

            if (imagebytes != null)
            {
                using (var mainimage = new MagickImage(imagebytes))
                using (var previewimage = new MagickImage(imagebytes))
                {
                    var profile = mainimage.GetExifProfile();

                    if (profile != null)
                    {
                        foreach (var value in profile.Values)
                        {
                            if (value.Tag == ExifTag.ExposureTime)
                            {
                                existingPhoto.exposure = value.ToString();
                            }
                            if (value.Tag == ExifTag.ShutterSpeedValue)
                            {
                                var apexshutterspeed = ((SignedRational)value.GetValue()).ToDouble();
                                existingPhoto.shutterspeed = (int)Math.Ceiling(Math.Pow(2, apexshutterspeed));
                            }
                            if (value.Tag == ExifTag.FNumber)
                            {
                                var rational = value.GetValue() as Rational?;
                                if (rational.HasValue)
                                {
                                    existingPhoto.apperture = (float)rational.Value.ToDouble();
                                }
                                ;
                            }
                            if (value.Tag == ExifTag.RecommendedExposureIndex)
                            {
                                int.TryParse(value.ToString(), out int intvalue);
                                existingPhoto.iso = intvalue;
                            }
                            _logger.LogInformation($"{value.Tag}({value.DataType}): {value.ToString()}");
                        }
                        ;
                    }
                    ;

                    mainimage.Strip();
                    previewimage.Strip();

                    if ((mainimage.Width > 2000) || (mainimage.Height > 2000))
                    {
                        mainimage.Resize(new MagickGeometry(2000, 2000)
                        {
                            IgnoreAspectRatio = false
                        });
                    }

                    previewimage.Resize(new MagickGeometry(1000, 1000)
                    {
                        IgnoreAspectRatio = false
                    });

                    uint mainquality = 100;
                    uint previewquality = 100;

                    mainimage.Format = MagickFormat.Jpg;
                    previewimage.Format = MagickFormat.Jpg;

                    while (mainquality > 5)
                    {
                        mainimage.Quality = mainquality;

                        if (mainimage.ToByteArray().Length > 2000 * 1024)
                        {
                            mainquality -= 5;
                        }
                        else
                        {
                            break;
                        }
                    }

                    while (previewquality > 5)
                    {
                        previewimage.Quality = previewquality;

                        if (previewimage.ToByteArray().Length > 500 * 1024)
                        {
                            previewquality -= 5;
                        }
                        else
                        {
                            break;
                        }
                    }

                    await mainimage.WriteAsync(existingPhoto.path);
                    await previewimage.WriteAsync(existingPhoto.previewpath);
                }
                ;
            }
            ;


            List<int> existingTagids = existingPhoto.tags.Select(x => x.Tagid).ToList();
            List<int> newTagids = photoDTO.tags.ToList();
            List<PhotoTag> tagsToRemove = existingPhoto.tags.Where(x => !newTagids.Contains(x.Tagid)).ToList();
            _context.PhotoTags.RemoveRange(tagsToRemove);
            List<int> tagsToAdd = newTagids.Except(existingTagids).ToList();
            foreach (var Tagid in tagsToAdd)
            {
                existingPhoto.tags.Add(new PhotoTag{Photoid = id, Tagid = Tagid});
            }

            await _context.SaveChangesAsync();
            return existingPhoto;
        }
        public async Task<Photo?> DeleteAsync(int id)
        {
            var photoModel = await _context.Photos.FindAsync(id);

            if (photoModel == null)
            {
                return null;
            }

            if (File.Exists(photoModel.path) && File.Exists(photoModel.previewpath))
            {
                File.Delete(photoModel.path);
                File.Delete(photoModel.previewpath);
            }

            _context.Photos.Remove(photoModel);
            await _context.SaveChangesAsync();
            return photoModel;
        }
        public Task<bool> PhotoExists(int id)
        {
            return _context.Photos.AnyAsync(x => x.id == id);
        }
    }
}