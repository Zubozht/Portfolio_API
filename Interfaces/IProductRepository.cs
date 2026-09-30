using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Product;
using api.Models;

namespace api.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetFilteredAsync(ProductFilterDTO filterDTO);
        Task<Product?> GetByIdAsync(int Id);
        Task<string?> GetPreviewImageByIdAsync(int Id);
        Task<Product> CreateAsync(Product productModel);
        Task<Product?> UpdateAsync(int Id, UpdateProductRequestDTO productDTO);
        Task<Product?> DeleteAsync(int Id);
        Task<bool> ProductExists(int Id);
    }
}