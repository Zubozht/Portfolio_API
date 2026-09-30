using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Product
{
    public class ProductFilterDTO
    {
        public int? CategoryId { get; set; }
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}