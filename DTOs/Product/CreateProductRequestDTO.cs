using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Product
{
    public class CreateProductRequestDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [MaxLength(500, ErrorMessage = "A description can't be over 500 characters.")]
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public List<int> ProductPhotoIds { get; set; } = new List<int>();
    }
}