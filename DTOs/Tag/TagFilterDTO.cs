using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Tag
{
    public class TagFilterDTO
    {
        public string? tag { get; set; }
        public List<int>? photoIDs { get; set; }
        public string? sortBy { get; set; } = null;
        public bool isDescending { get; set; } = false;
    }
}