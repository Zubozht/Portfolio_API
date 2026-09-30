using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.ProductPhoto;

namespace api.DTOs.Product
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string PreviewPath { get; set; } = string.Empty;
        public List<ProductPhotoDTO> Photos = new List<ProductPhotoDTO>();
    }
}