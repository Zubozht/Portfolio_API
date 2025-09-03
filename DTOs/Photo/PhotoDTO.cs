using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using api.DTOs.Tag;

namespace api.DTOs.Photo
{
    public class PhotoDTO
    {
        public int id { get; set; }
        //public byte[]? image { get; set; }
        //public byte[]? previewimage { get; set; }
        public string path { get; set; } = string.Empty;
        public string previewpath { get; set; } = string.Empty;
        public string caption { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public List<LightTagDTO> tags { get; set; } = new List<LightTagDTO>();
        public float? shutterspeed { get; set; }
        public string? exposure { get; set; }
        public float? apperture { get; set; }
        public int? iso { get; set; }
    }
}