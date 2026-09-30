using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Tag;

namespace api.DTOs.Photo
{
    public class UpdatePhotoRequestDTO
    {
        [Required]
        public IFormFile? image { get; set;}
        [Required]
        [MinLength(1, ErrorMessage = "A caption is required.")]
        [MaxLength(25, ErrorMessage = "A caption can't be over 25 characters.")]
        public string caption { get; set; } = string.Empty;
        [MaxLength(500, ErrorMessage = "A description can't be over 500 characters.")]
        public string? description { get; set; }
        public int sortorder { get; set; } = 1;
        public List<int> tags { get; set; } = new List<int>();
    }
}