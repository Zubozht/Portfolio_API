using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Category;
using api.DTOs.Product;
using api.DTOs.ProductPhoto;
using api.Models;

namespace api.Mappers
{
    public static class CategoryMappers
    {
        public static CategoryDTO ToCategoryDTO(this Category categoryModel)
        {
            return new CategoryDTO
            {
                Id = categoryModel.Id,
                Title = categoryModel.Title,
                Products = categoryModel.Products.Select(x => new ProductDTO {Id = x.Id, Title = x.Title, Description = x.Description, CategoryId = x.CategoryId, PreviewPath = x.PreviewPath, Photos = x.Photos.Select(x => new ProductPhotoDTO{Id = x.ProductPhoto.Id, Path = x.ProductPhoto.Path}).ToList()}).ToList()
            };
        }

        public static Category ToCategoryFromCreateDTO(this CreateCategoryRequestDTO categoryDTO)
        {
            return new Category
            {
                Title = categoryDTO.Title,
                //Products = categoryDTO.Products.Select(x => new Product{Id = x}).ToList()
            };
        }
    }
}