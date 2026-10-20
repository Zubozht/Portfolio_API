using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Tag
{
    public class TagFilterDTO
    {
        public string? Name { get; set; }
        public List<int>? PhotoIDs { get; set; }
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
    }
}