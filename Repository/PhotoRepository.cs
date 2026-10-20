using api.Data;
using api.DTOs;
using api.DTOs.Photo;
using api.HelperFunctions;
using api.Interfaces;
using api.Models;
using ImageMagick;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

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
            var query = _context.Photos.Include(x => x.Tags).ThenInclude(x => x.Tag).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterDTO.Caption))
            {
                query = query.Where(x => x.Caption.Contains(filterDTO.Caption));
            }
            if (!string.IsNullOrWhiteSpace(filterDTO.Description))
            {
                query = query.Where(x => x.Description.Contains(filterDTO.Description));
            }
            if (filterDTO.TagIds?.Any() ?? false)
            {
                foreach (int filtertagid in filterDTO.TagIds)
                {
                    query = query.Where(x => x.Tags.Any(t => t.TagId == filtertagid));
                }
            }

            if (filterDTO.MinShutterspeed != null)
            {
                query = query.Where(x => x.Shutterspeed <= filterDTO.MinShutterspeed.Value);
            }
            if (filterDTO.MaxShutterspeed != null)
            {
                query = query.Where(x => x.Shutterspeed >= filterDTO.MaxShutterspeed.Value);
            }
            if (filterDTO.MinApperture.HasValue)
            {
                query = query.Where(x => x.Apperture >= filterDTO.MinApperture);
            }
            if (filterDTO.MaxApperture.HasValue)
            {
                query = query.Where(x => x.Apperture <= filterDTO.MaxApperture);
            }
            if (filterDTO.MinIso.HasValue)
            {
                query = query.Where(x => x.Iso >= filterDTO.MinIso);
            }
            if (filterDTO.MaxIso.HasValue)
            {
                query = query.Where(x => x.Iso <= filterDTO.MaxIso);
            }
            if (!string.IsNullOrWhiteSpace(filterDTO.SortBy))
            {
                foreach (string colname in typeof(Photo).GetProperties().Select(x => x.Name).ToList())
                {
                    if (filterDTO.SortBy.Trim().Equals(colname, StringComparison.OrdinalIgnoreCase))
                    {
                        if (colname == "Id")
                        {
                            query = query.OrderBy($"{colname} {(filterDTO.IsDescending ? "descending" : "ascending")}");
                        }
                        else if (colname == "SortOrder")
                        {
                            query = query.OrderBy($"{colname} {(filterDTO.IsDescending ? "descending" : "ascending")}, Id {(filterDTO.IsDescending ? "descending" : "ascending")}");
                        }
                        else
                        {
                            query = query.OrderBy($"{colname} {(filterDTO.IsDescending ? "descending" : "ascending")}{$", SortOrder {(filterDTO.IsDescending ? "descending" : "ascending")}"}{$", Id {(filterDTO.IsDescending ? "descending" : "ascending")}"}");
                        }
                    }
                }
            }

            var skipNum = (filterDTO.PageNum - 1) * filterDTO.PageSize;

            return await query.Skip(skipNum).Take(filterDTO.PageSize).ToListAsync();
        }
        public async Task<Photo?> GetByIdAsync(int id)
        {
            return await _context.Photos.Include(x => x.Tags).ThenInclude(x => x.Tag).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<string?> GetImageByIdAsync(int id)
        {
            var photo = await _context.Photos.FindAsync(id);

            if (photo == null || photo.Path == null)
            {
                return null;
            }

            return photo.Path;
        }
        public async Task<string?> GetPreviewImageByIdAsync(int id)
        {
            var photo = await _context.Photos.FindAsync(id);

            if (photo == null || photo.PreviewPath == null)
            {
                return null;
            }

            return photo.PreviewPath;
        }
        public async Task<Photo> CreateAsync(Photo photoModel)
        {

            await _context.Photos.AddAsync(photoModel);
            await _context.SaveChangesAsync();

            if (photoModel.Path != null)
            {
                using (var mainimage = new MagickImage(photoModel.Path))
                using (var previewimage = new MagickImage(photoModel.Path))
                {
                    var profile = mainimage.GetExifProfile();

                    if (profile != null)
                    {
                        foreach (var value in profile.Values)
                        {
                            if (value.Tag == ExifTag.ExposureTime)
                            {
                                photoModel.Exposure = value.ToString();
                            }
                            if (value.Tag == ExifTag.ShutterSpeedValue)
                            {
                                var apexshutterspeed = ((SignedRational)value.GetValue()).ToDouble();
                                photoModel.Shutterspeed = (int)Math.Ceiling(Math.Pow(2, apexshutterspeed));
                            }
                            if (value.Tag == ExifTag.FNumber)
                            {
                                var rational = value.GetValue() as Rational?;
                                if (rational.HasValue)
                                {
                                    photoModel.Apperture = (float)rational.Value.ToDouble();
                                };
                            }
                            if (value.Tag == ExifTag.RecommendedExposureIndex)
                            {
                                int.TryParse(value.ToString(), out int intvalue);
                                photoModel.Iso = intvalue;
                            }
                            //_logger.LogInformation($"{value.Tag}({value.DataType}): {value.ToString()}");
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
                    
                    string imagepath = Path.Combine(Directory.GetCurrentDirectory(), "photos", $"{photoModel.Id.ToString().PadLeft(6, '0')}");
                    File.Delete(photoModel.Path);
                    photoModel.Path = $"{imagepath}.jpg";
                    photoModel.PreviewPath = $"{imagepath}p.jpg";
                    mainimage.Write(photoModel.Path);
                    previewimage.Write(photoModel.PreviewPath);
                };
            };

            await _context.SaveChangesAsync();

            return photoModel;
        }
        public async Task<Photo?> UpdateAsync(int id, UpdatePhotoRequestDTO photoDTO)
        {
            var existingPhoto = await _context.Photos.Include(x => x.Tags).ThenInclude(x => x.Tag).FirstOrDefaultAsync(x => x.Id == id);
            if (existingPhoto == null)
            {
                return null;
            }

            byte[]? imagebytes = null;
            if (photoDTO.Image != null && photoDTO.Image.Length > 0)
            {

                using (var ms = new MemoryStream())
                {
                    await photoDTO.Image.CopyToAsync(ms);
                    imagebytes = ms.ToArray();
                }
            };

            /*if (imagebytes != null)
            {
                File.WriteAllBytes(existingPhoto.path, imagebytes);
            }*/

            existingPhoto.Caption = photoDTO.Caption;
            existingPhoto.Description = photoDTO.Description ?? "";
            existingPhoto.SortOrder = photoDTO.SortOrder;
            existingPhoto.Exposure = null;
            existingPhoto.Shutterspeed = null;
            existingPhoto.Apperture = null;
            existingPhoto.Shutterspeed = null;
            existingPhoto.Iso = null;

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
                                existingPhoto.Exposure = value.ToString();
                            }
                            if (value.Tag == ExifTag.ShutterSpeedValue)
                            {
                                var apexshutterspeed = ((SignedRational)value.GetValue()).ToDouble();
                                existingPhoto.Shutterspeed = (int)Math.Ceiling(Math.Pow(2, apexshutterspeed));
                            }
                            if (value.Tag == ExifTag.FNumber)
                            {
                                var rational = value.GetValue() as Rational?;
                                if (rational.HasValue)
                                {
                                    existingPhoto.Apperture = (float)rational.Value.ToDouble();
                                }
                                ;
                            }
                            if (value.Tag == ExifTag.RecommendedExposureIndex)
                            {
                                int.TryParse(value.ToString(), out int intvalue);
                                existingPhoto.Iso = intvalue;
                            }
                            //_logger.LogInformation($"{value.Tag}({value.DataType}): {value.ToString()}");
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

                    await mainimage.WriteAsync(existingPhoto.Path);
                    await previewimage.WriteAsync(existingPhoto.PreviewPath);
                }
                ;
            }
            ;


            List<int> existingTagids = existingPhoto.Tags.Select(x => x.TagId).ToList();
            List<int> newTagids = photoDTO.Tags.ToList();
            List<PhotoTag> tagsToRemove = existingPhoto.Tags.Where(x => !newTagids.Contains(x.TagId)).ToList();
            _context.PhotoTags.RemoveRange(tagsToRemove);
            List<int> tagsToAdd = newTagids.Except(existingTagids).ToList();
            foreach (var tagId in tagsToAdd)
            {
                existingPhoto.Tags.Add(new PhotoTag{PhotoId = id, TagId = tagId});
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

            if (File.Exists(photoModel.Path) && File.Exists(photoModel.PreviewPath))
            {
                File.Delete(photoModel.Path);
                File.Delete(photoModel.PreviewPath);
            }

            _context.Photos.Remove(photoModel);
            await _context.SaveChangesAsync();
            return photoModel;
        }
        public Task<bool> PhotoExists(int id)
        {
            return _context.Photos.AnyAsync(x => x.Id == id);
        }
    }
}