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
        public int Id { get; set; }
        public string Path { get; set; } = string.Empty;
        public string PreviewPath { get; set; } = string.Empty;
        public string Caption { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int SortOrder { get; set; } = 1;
        public float? Shutterspeed { get; set; }
        public string? Exposure { get; set; }
        public float? Apperture { get; set; }
        public int? Iso { get; set; }

        public List<LightTagDTO> Tags { get; set; } = new List<LightTagDTO>();
    }
}