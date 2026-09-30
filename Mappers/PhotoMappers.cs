using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Photo;
using api.DTOs.Tag;
using api.Models;
using Microsoft.VisualBasic;

namespace api.Mappers
{
    public static class PhotoMappers
    {
        public static PhotoDTO ToPhotoDTO(this Photo photoModel)
        {
            return new PhotoDTO
            {
                id = photoModel.id,
                //image = photoModel.image,
                //previewimage = photoModel.previewimage,
                path = photoModel.path,
                previewpath = photoModel.previewpath,
                caption = photoModel.caption,
                description = photoModel.description,
                sortorder = photoModel.sortorder,
                tags = photoModel.tags.Select(x => new LightTagDTO{id = x.Tag.id, tag = x.Tag.tag}).ToList(),
                exposure = photoModel.exposure,
                apperture = photoModel.apperture,
                iso = photoModel.iso
            };
        }

        /*public static Photo ToPhotoFromCreateDTO(this CreatePhotoReqestDTO photoDTO)
        {
            return new Photo
            {
                image = photoDTO.image,
                caption = photoDTO.caption,
                description = photoDTO.description,
                tags = photoDTO.tags.Select(x => new PhotoTag{Tagid = x}).ToList(),
                shutterspeed = photoDTO.shutterspeed,
                apperture = photoDTO.apperture,
                iso = photoDTO.iso
            };
        }*/
    }
}