using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Product;
using api.DTOs.ProductPhoto;
using api.Models;

namespace api.Mappers
{
    public static class ProductPhotoMappers
    {
        public static ProductPhotoDTO ToProductPhotoDTO(this ProductPhoto productPhotoModel)
        {
            return new ProductPhotoDTO
            {
                Id = productPhotoModel.Id,
                Path = productPhotoModel.Path,
                Products = productPhotoModel.Products.Select(x => new ProductDTO{Id = x.Product.Id, Title = x.Product.Title, Description = x.Product.Description, Photos = x.Product.Photos.Select(x => new ProductPhotoDTO{Id = x.ProductPhoto.Id, Path = x.ProductPhoto.Path}).ToList()}).ToList()
            };
        }
    }
}