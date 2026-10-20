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
                Id = photoModel.Id,
                Path = photoModel.Path,
                PreviewPath = photoModel.PreviewPath,
                Caption = photoModel.Caption,
                Description = photoModel.Description,
                SortOrder = photoModel.SortOrder,
                Tags = photoModel.Tags.Select(x => new LightTagDTO{Id = x.Tag.Id, Name = x.Tag.Name}).ToList(),
                Exposure = photoModel.Exposure,
                Apperture = photoModel.Apperture,
                Iso = photoModel.Iso
            };
        }
    }
}