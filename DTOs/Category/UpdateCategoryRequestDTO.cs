using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Category
{
    public class UpdateCategoryRequestDTO
    {
        [Required]
        [MinLength(2, ErrorMessage = "Category title lenght can't be less than 2 characters.")]
        [MaxLength(50, ErrorMessage = "Category title lenght can't be more than 50 characters.")]
        public string Title { get; set; } = string.Empty;
        //public List<int> Products { get; set; } = new List<int>();
    }
}