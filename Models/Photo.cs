using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("photos")]
    public class Photo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Column(TypeName="NVARCHAR(100)")]
        public string path { get; set; } = string.Empty;
        [Column(TypeName="NVARCHAR(100)")]
        public string previewpath { get; set; } = string.Empty;
        //[Column(TypeName ="LONGBLOB")]
        //public byte[]? image { get; set; }
        //[Column(TypeName ="LONGBLOB")]
        //public byte[]? previewimage { get; set; }
        public string caption { get; set; } = string.Empty;
        [Column(TypeName ="NVARCHAR(1000)")]
        public string description { get; set; } = string.Empty;
        public List<PhotoTag> tags { get; set; } = new List<PhotoTag>();
        public int? shutterspeed { get; set; }
        [Column(TypeName ="NVARCHAR(50)")]
        public string? exposure { get; set; }
        public float? apperture { get; set; }
        public int? iso { get; set; }
        [Column(TypeName ="TIMESTAMP(6)")]
        public DateTime createdon { get; set; }

    }
}