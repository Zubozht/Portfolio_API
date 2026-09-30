using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Account;
using api.DTOs.Product;
using api.DTOs.ProductPhoto;
using api.Models;

namespace api.Mappers
{
    public static class ProductMappers
    {
        public static ProductDTO ToProductDTO(this Product productModel)
        {
            return new ProductDTO
            {
                Id = productModel.Id,
                Title = productModel.Title,
                Description = productModel.Description,
                CategoryId = productModel.CategoryId,
                PreviewPath = productModel.PreviewPath,
                Price = productModel.Price,
                Photos = productModel.Photos.Select(x => new ProductPhotoDTO{Id = x.ProductPhoto.Id, Path = x.ProductPhoto.Path}).ToList()
            };
        }
        
        public static Product ToProductFromCreateDTO(this CreateProductRequestDTO productDTO)
        {
            return new Product
            {
                Title = productDTO.Title,
                Description = productDTO.Description,
                CategoryId = productDTO.CategoryId,
                Price = productDTO.Price,
                PreviewPath = "",
                Photos = productDTO.ProductPhotoIds.Select(x => new ProductProductPhoto {ProductPhotoId = x}).ToList()
            };
        }
    }
}