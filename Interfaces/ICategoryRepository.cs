using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Category;
using api.Models;

namespace api.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int Id);
        Task<Category?> CreateAsync(Category categoryModel);
        Task<Category?> UpdateAsync(int Id, UpdateCategoryRequestDTO categoryDTO);
        Task<Category?> DeleteAsync(int Id);
    }
}