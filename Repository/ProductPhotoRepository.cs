using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.ProductPhoto;
using api.Interfaces;
using api.Models;
using ImageMagick;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class ProductPhotoRepository : IProductPhotoRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<ProductPhotoRepository> _logger;
        public ProductPhotoRepository (ApplicationDBContext context, ILogger<ProductPhotoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<ProductPhoto>> GetAllAsync()
        {
            return await _context.ProductPhotos.Include(x => x.Products).ToListAsync();
        }
        public async Task<ProductPhoto?> GetByIdAsync(int Id)
        {
            return await _context.ProductPhotos.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<string?> GetImageByIdAsync(int Id)
        {
            var photo = await _context.ProductPhotos.FindAsync(Id);

            if (photo == null)
            {
                return null;
            }

            return photo.Path;
        }
        public async Task<ProductPhoto?> CreateAsync(ProductPhoto productPhotoModel)
        {
            await _context.ProductPhotos.AddAsync(productPhotoModel);
            await _context.SaveChangesAsync();

            if (productPhotoModel.Path != null)
            {
                using (var mainimage = new MagickImage(productPhotoModel.Path))
                {
                    mainimage.Strip();

                    if ((mainimage.Width > 2000) || (mainimage.Height > 2000))
                    {
                        mainimage.Resize(new MagickGeometry(2000, 2000)
                        {
                            IgnoreAspectRatio = false
                        });
                    }

                    uint mainquality = 100;

                    mainimage.Format = MagickFormat.Jpg;

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
                    
                    string imagepath = Path.Combine(Directory.GetCurrentDirectory(), "productphotos", $"{productPhotoModel.Id.ToString().PadLeft(6, '0')}");
                    File.Delete(productPhotoModel.Path);
                    productPhotoModel.Path = $"{imagepath}.jpg";
                    mainimage.Write(productPhotoModel.Path);
                };
            };

            await _context.SaveChangesAsync();

            return productPhotoModel;
        }
        public async Task<ProductPhoto?> UpdateAsync(int Id, UpdateProductPhotoRequestDTO productPhotoDTO)
        {
            var existingProductPhoto = await _context.ProductPhotos.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == Id);
            if (existingProductPhoto == null)
            {
                return null;
            }

            byte[]? imagebytes = null;
            if (productPhotoDTO.Image != null && productPhotoDTO.Image.Length > 0)
            {

                using (var ms = new MemoryStream())
                {
                    await productPhotoDTO.Image.CopyToAsync(ms);
                    imagebytes = ms.ToArray();
                }
            };

            if (imagebytes != null)
            {
                using (var mainimage = new MagickImage(imagebytes))
                {
                    var profile = mainimage.GetExifProfile();

                    mainimage.Strip();

                    if ((mainimage.Width > 2000) || (mainimage.Height > 2000))
                    {
                        mainimage.Resize(new MagickGeometry(2000, 2000)
                        {
                            IgnoreAspectRatio = false
                        });
                    }

                    uint mainquality = 100;

                    mainimage.Format = MagickFormat.Jpg;
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

                    await mainimage.WriteAsync(existingProductPhoto.Path);
                }
                ;
            }
            ;


            List<int> existingProductIds = existingProductPhoto.Products.Select(x => x.ProductId).ToList();
            List<int> newProductIds = productPhotoDTO.Products.ToList();
            List<ProductProductPhoto> productsToRemove = existingProductPhoto.Products.Where(x => !newProductIds.Contains(x.ProductId)).ToList();
            _context.ProductProductPhotos.RemoveRange(productsToRemove);
            List<int> productsToAdd = newProductIds.Except(existingProductIds).ToList();
            foreach (var ProductId in productsToAdd)
            {
                existingProductPhoto.Products.Add(new ProductProductPhoto{ProductPhotoId = Id, ProductId = ProductId});
            }

            await _context.SaveChangesAsync();
            return existingProductPhoto;
        }
        public async Task<ProductPhoto?> DeleteAsync(int Id)
        {
            var productPhotoModel = await _context.ProductPhotos.FindAsync(Id);

            if (productPhotoModel == null)
            {
                return null;
            }

            if (File.Exists(productPhotoModel.Path))
            {
                File.Delete(productPhotoModel.Path);
            }

            _context.ProductPhotos.Remove(productPhotoModel);
            await _context.SaveChangesAsync();
            return productPhotoModel;
        }
        public Task<bool> ProductPhotoExists(int Id)
        {
            return _context.ProductPhotos.AnyAsync(x => x.Id == Id);
        }
    }
}