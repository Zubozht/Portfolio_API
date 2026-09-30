using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Photo
{
    public class PhotoFilterDTO
    {
        public string? caption { get; set; }
        public string? description { get; set; }
        public int? sortorder { get; set; }
        public List<int>? tagIds { get; set; }
        public int? minShutterspeed { get; set; }
        public int? maxShutterspeed { get; set; }
        public float? minApperture { get; set; }
        public float? maxApperture { get; set; }
        public int? minIso { get; set; }
        public int? maxIso { get; set; }
        public string? sortBy { get; set; } = null;
        public bool isDescending { get; set; } = false;
        public int pageNum { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}