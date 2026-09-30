using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace api.Models
{
    [Table("orderitems")]
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int ProductId { get; set; }
        [Column(TypeName="NVARCHAR(255)")]
        public string ProductName { get; set; } = string.Empty;
        public int UnitPriceCents {get; set; }
        public int Quantity { get; set; } = 1;
        public int LineTotalCents { get; set; }
        [Column(TypeName="NVARCHAR(255)")]
        public string VariantDescription { get; set; } = string.Empty;

        [Column(TypeName="JSON")]
        public string? ExtraData { get; set; }
    }
}