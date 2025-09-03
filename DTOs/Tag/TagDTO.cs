using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Photo;

namespace api.DTOs.Tag
{
    public class TagDTO
    {
        public int id { get; set; }
        public string tag { get; set; } = string.Empty;
        public string previewpath { get; set; } = string.Empty;
        //public byte[]? previewimage {get; set; }
        public List<LightPhotoDTO> photos { get; set; } = new List<LightPhotoDTO>();
    }
}