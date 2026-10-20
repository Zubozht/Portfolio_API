using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Photo;

namespace api.DTOs.Tag
{
    public class CreateTagRequestDTO
    {
        [Required]
        [MinLength(2, ErrorMessage = "Tag lenght can't be less than 2 characters.")]
        [MaxLength(25, ErrorMessage = "Tag lenght can't be more than 25 characters.")]
        public string Name { get; set; } = string.Empty;
        public byte[]? PreviewImage { get; set; }
        public List<int> Photos { get; set; } = new List<int>();
    }
}