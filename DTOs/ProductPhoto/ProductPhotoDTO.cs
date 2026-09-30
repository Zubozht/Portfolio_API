using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Product;

namespace api.DTOs.ProductPhoto
{
    public class ProductPhotoDTO
    {
        public int Id { get; set; }
        public string Path { get; set; } = string.Empty;
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}