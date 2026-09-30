using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Category;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly ILogger<CategoryRepository> _logger;
        public CategoryRepository (ApplicationDBContext context, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.Include(c => c.Products).ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int Id)
        {
            return await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Category?> CreateAsync(Category categoryModel)
        {
            var sameCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Title == categoryModel.Title);
            if (sameCategory != null)
            {
                return null;
            }
            await _context.Categories.AddAsync(categoryModel);
            await _context.SaveChangesAsync();
            return categoryModel;
        }
        public async Task<Category?> UpdateAsync(int Id, UpdateCategoryRequestDTO categoryDTO)
        {
            var existingCategory = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(x => x.Id == Id);

            if (existingCategory == null)
            {
                return null;
            }
            
            var sameCategory = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(x => x.Title == categoryDTO.Title && x.Id != existingCategory.Id);

            if (sameCategory == null)
            {
                existingCategory.Title = categoryDTO.Title;

                await _context.SaveChangesAsync();
                return existingCategory;
            }
            else
            {
                sameCategory.Products = sameCategory.Products.Concat(existingCategory.Products).ToList();
                _context.Categories.Remove(existingCategory);
                await _context.SaveChangesAsync();
                return sameCategory;
            }
        }
        public async Task<Category?> DeleteAsync(int Id)
        {
            var categoryModel = await _context.Categories.FirstOrDefaultAsync(x => x.Id == Id);
            if (categoryModel == null)
            {
                return null;
            }
            _context.Categories.Remove(categoryModel);
            await _context.SaveChangesAsync();
            return categoryModel;
        }
    }
}