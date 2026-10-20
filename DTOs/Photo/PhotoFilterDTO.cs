using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Photo
{
    public class PhotoFilterDTO
    {
        public string? Caption { get; set; }
        public string? Description { get; set; }
        public int? SortOrder { get; set; }
        public List<int>? TagIds { get; set; }
        public int? MinShutterspeed { get; set; }
        public int? MaxShutterspeed { get; set; }
        public float? MinApperture { get; set; }
        public float? MaxApperture { get; set; }
        public int? MinIso { get; set; }
        public int? MaxIso { get; set; }
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}