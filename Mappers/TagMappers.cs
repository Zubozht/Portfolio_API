using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Photo;
using api.DTOs.Tag;
using api.Models;

namespace api.Mappers
{
    public static class TagMappers
    {
        public static TagDTO ToTagDTO(this Tag tagModel)
        {
            return new TagDTO
            {
                Id = tagModel.Id,
                Name = tagModel.Name,
                Photos = tagModel.Photos.Select(x => new LightPhotoDTO{Id = x.PhotoId}).ToList()
            };
        }
        public static Tag ToTagFromCreateDTO(this CreateTagRequestDTO tagDTO)
        {
            return new Tag
            {
                Name = tagDTO.Name,
                Photos = tagDTO.Photos.Select(x => new PhotoTag{PhotoId = x}).ToList()
            };
        }
    }
}