using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Photo;

namespace api.DTOs.Tag
{
    public class TagDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string PreviewPath { get; set; } = string.Empty;

        public List<LightPhotoDTO> Photos { get; set; } = new List<LightPhotoDTO>();
    }
}