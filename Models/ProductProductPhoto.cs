using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("productproductphotos")]
    public class ProductProductPhoto
    {
        public int ProductId { get; set; }
        public int ProductPhotoId { get; set; }
        public Product Product { get; set; } = null!;
        public ProductPhoto ProductPhoto { get; set;} = null!;
    }
}