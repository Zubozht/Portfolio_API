using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.ProductPhoto;
using api.Models;

namespace api.Interfaces
{
    public interface IProductPhotoRepository
    {
        Task<List<ProductPhoto>> GetAllAsync();
        Task<ProductPhoto?> GetByIdAsync(int Id);
        Task<string?> GetImageByIdAsync(int Id);
        Task<ProductPhoto?> CreateAsync(ProductPhoto productPhotoModel);
        Task<ProductPhoto?> UpdateAsync(int Id, UpdateProductPhotoRequestDTO productPhotoDTO);
        Task<ProductPhoto?> DeleteAsync(int Id);
        Task<bool> ProductPhotoExists(int Id);
    }
}