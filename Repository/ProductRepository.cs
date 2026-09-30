using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Product;
using api.Interfaces;
using api.Models;
using System.Linq.Dynamic.Core;
using ImageMagick;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository (ApplicationDBContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Product>> GetFilteredAsync(ProductFilterDTO filterDTO)
        {
            var query = _context.Products.Include(x => x.Photos).ThenInclude(x => x.ProductPhoto).AsQueryable();

            if (filterDTO.CategoryId != null)
            {
                query = query.Where(x => x.CategoryId.Equals(filterDTO.CategoryId));
            }

            if (!string.IsNullOrWhiteSpace(filterDTO.SortBy))
            {
                foreach (string colname in typeof(Product).GetProperties().Select(x => x.Name).ToList())
                {
                    if (filterDTO.SortBy.Trim().Equals(colname, StringComparison.OrdinalIgnoreCase))
                    {
                        query = query.OrderBy($"{colname} {(filterDTO.IsDescending ? "descending" : "ascending")}");
                    }
                }
            }
            
            var skipNum = (filterDTO.PageNum - 1) * filterDTO.PageSize;

            return await query.Skip(skipNum).Take(filterDTO.PageSize).ToListAsync();
        }
        public async Task<Product?> GetByIdAsync(int Id)
        {
            return await _context.Products.Include(x => x.Photos).ThenInclude(x => x.ProductPhoto).FirstOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<string?> GetPreviewImageByIdAsync(int Id)
        {
            var product = await _context.Products.FindAsync(Id);

            if (product == null || product.PreviewPath == null)
            {
                return null;
            }

            return product.PreviewPath;
        }
        public async Task<Product> CreateAsync(Product productModel)
        {
            await _context.Products.AddAsync(productModel);
            await _context.SaveChangesAsync();

            int photoid = productModel.Photos[0].ProductPhotoId;
            var photo = await _context.ProductPhotos.FirstOrDefaultAsync(pp => pp.Id == photoid);

            if (photo != null && photo.Path != null)
            {
                using (var previewimage = new MagickImage(photo.Path))
                {
                    previewimage.Strip();

                    previewimage.Resize(new MagickGeometry(1000, 1000)
                    {
                        IgnoreAspectRatio = false
                    });

                    uint previewquality = 100;

                    previewimage.Format = MagickFormat.Jpg;

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
                    
                    string imagepath = Path.Combine(Directory.GetCurrentDirectory(), "productphotos", $"{productModel.Id.ToString().PadLeft(6, '0')}");
                    productModel.PreviewPath = $"{imagepath}p.jpg";
                    previewimage.Write(productModel.PreviewPath);
                };
            };

            await _context.SaveChangesAsync();

            return productModel;
        }
        public async Task<Product?> UpdateAsync(int Id, UpdateProductRequestDTO productDTO)
        {
            var existingProduct = await _context.Products.Include(x => x.Photos).ThenInclude(x => x.ProductPhoto).FirstOrDefaultAsync(x => x.Id == Id);
            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.Title = productDTO.Title;
            existingProduct.Description = productDTO.Description ?? "";
            existingProduct.CategoryId = productDTO.CategoryId;
            existingProduct.Price = productDTO.Price;
            existingProduct.Photos = productDTO.ProductPhotoIds.Select(x => new ProductProductPhoto{ProductPhotoId = x}).ToList();

            string imagepath = "";
            var firstphotoid = productDTO.ProductPhotoIds.FirstOrDefault();
            if (firstphotoid != 0)
            {
                var firstphoto = await _context.ProductPhotos.FirstOrDefaultAsync(pp => pp.Id == firstphotoid);
                if (firstphoto != null && firstphoto.Path != null)
                {
                    imagepath = firstphoto.Path;
                }
            };

            if (imagepath != null)
            {
                using (var previewimage = new MagickImage(imagepath))
                {
                    previewimage.Strip();

                    previewimage.Resize(new MagickGeometry(1000, 1000)
                    {
                        IgnoreAspectRatio = false
                    });

                    uint previewquality = 100;

                    previewimage.Format = MagickFormat.Jpg;

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

                    await previewimage.WriteAsync(existingProduct.PreviewPath);
                };
            };

            await _context.SaveChangesAsync();
            return existingProduct;
        }
        public async Task<Product?> DeleteAsync(int Id)
        {
            var productModel = await _context.Products.FindAsync(Id);

            if (productModel == null)
            {
                return null;
            }

            if (File.Exists(productModel.PreviewPath))
            {
                File.Delete(productModel.PreviewPath);
            }

            _context.Products.Remove(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }
        public Task<bool> ProductExists(int Id)
        {
            return _context.Products.AnyAsync(x => x.Id == Id);
        }
    }
}