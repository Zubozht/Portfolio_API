using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Tag;
using api.Models;

namespace api.Interfaces
{
    public interface ITagRepository
    {
        //Task<List<Tag>> GetAllAsync();
        Task<List<Tag>> GetFilteredAsync(TagFilterDTO filterDTO);
        Task<Tag?> GetByIdAsync(int id);
        Task<string?> GetPreviewImageByIdAsync(int id);
        Task<Tag?> CreateAsync(Tag tagModel);
        Task<Tag?> UpdateAsync(int id, UpdateTagRequestDTO tagDTO);
        Task<Tag?> DeleteAsync(int id);
        Task<bool> TagExists(int id);
    }
}