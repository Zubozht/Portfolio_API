using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("productphotos")]
    public class ProductPhoto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName="NVARCHAR(100)")]
        public string Path { get; set; } = string.Empty;
        public List<ProductProductPhoto> Products { get; set; } = new List<ProductProductPhoto>();
    }
}