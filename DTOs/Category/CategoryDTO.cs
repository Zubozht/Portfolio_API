using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Product;

namespace api.DTOs.Category
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}