using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Photo;
using api.Models;

namespace api.Interfaces
{
    public interface IPhotoRepository
    {
        //Task<List<Photo>> GetAllAsync();
        Task<List<Photo>> GetFilteredAsync(PhotoFilterDTO filterDTO);
        Task<Photo?> GetByIdAsync(int id);
        Task<string?> GetImageByIdAsync(int id);
        Task<string?> GetPreviewImageByIdAsync(int id);
        Task<Photo> CreateAsync(Photo photoModel);
        Task<Photo?> UpdateAsync(int id, UpdatePhotoRequestDTO photoDTO);
        Task<Photo?> DeleteAsync(int id);
        Task<bool> PhotoExists(int id);
    }
}