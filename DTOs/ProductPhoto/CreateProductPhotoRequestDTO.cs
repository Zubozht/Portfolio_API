using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.ProductPhoto
{
    public class CreateProductPhotoRequestDTO
    {
        public IFormFile? Image { get; set; }
        public List<int> Products { get; set; } = new List<int>();
    }
}