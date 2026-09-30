using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "NVARCHAR(50)")]
        public string Title { get; set; } = string.Empty;
        [Column(TypeName = "NVARCHAR(500)")]
        public string Description { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        [Column(TypeName = "NVARCHAR(100)")]
        public string PreviewPath { get; set; } = string.Empty;
        public List<ProductProductPhoto> Photos { get; set; } = new List<ProductProductPhoto>();
    }
}