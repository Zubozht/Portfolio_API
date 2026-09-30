using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName="NVARCHAR(32) NOT NULL UNIQUE")]
        public string OrderNumber { get; set; } = string.Empty;
        [Column(TypeName ="TIMESTAMP(6)")]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName ="TIMESTAMP(6)")]
        public DateTime UpdatedAt { get; set; }
        public enum Status {pending_payment, paid, failed, cancelled, fulfilled};
        [Column(TypeName="NVARCHAR(255) NOT NULL")]
        public string CustomerEmail { get; set; } = string.Empty;
        [Column(TypeName="NVARCHAR(255)")]
        public string? CustomerName { get; set; }
        [Column(TypeName="NVARCHAR(255)")]
        public string? CustomerStreet { get; set; }
        [Column(TypeName="NVARCHAR(255)")]
        public string? CustomerCity { get; set; }
        [Column(TypeName="NVARCHAR(32)")]
        public string? CustomerPostcode { get; set; }
        [Column(TypeName="NVARCHAR(2)")]
        public string? CustomerCountry { get; set; }
        [Column(TypeName="NVARCHAR(3) NOT NULL")]
        public string Currency { get; set; } = "EUR";
        public int SubtotalCents { get; set; }
        public int ShippingCents { get; set; } = 0;
        public int TotalCents { get; set; }
        public int DiscountCents { get; set; } = 0;
        [Column(TypeName="NVARCHAR(32)")]
        public string PaymentProvider { get; set; } = string.Empty;
        [Column(TypeName="NVARCHAR(128)")]
        public string PaymentReference { get; set; } = string.Empty;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}