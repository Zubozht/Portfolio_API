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
                id = tagModel.id,
                tag = tagModel.tag,
                //previewimage = tagModel.previewimage,
                photos = tagModel.photos.Select(x => new LightPhotoDTO{id = x.Photoid}).ToList()
            };
        }
        public static Tag ToTagFromCreateDTO(this CreateTagRequestDTO tagDTO)
        {
            return new Tag
            {
                tag = tagDTO.tag,
                photos = tagDTO.photos.Select(x => new PhotoTag{Photoid = x}).ToList()
            };
        }
    }
}