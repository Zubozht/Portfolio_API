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
        public int Id { get; set; }
        [Column(TypeName="NVARCHAR(100)")]
        public string Path { get; set; } = string.Empty;
        [Column(TypeName = "NVARCHAR(100)")]
        public string PreviewPath { get; set; } = string.Empty;
        //[Column(TypeName ="LONGBLOB")]
        //public byte[]? image { get; set; }
        //[Column(TypeName ="LONGBLOB")]
        //public byte[]? previewimage { get; set; }
        [Column(TypeName="NVARCHAR(50)")]
        public string Caption { get; set; } = string.Empty;
        [Column(TypeName="NVARCHAR(1000)")]
        public string Description { get; set; } = string.Empty;
        [Column(TypeName="INT")]
        public int SortOrder { get; set; } = 0;
        public int? Shutterspeed { get; set; }
        [Column(TypeName ="NVARCHAR(50)")]
        public string? Exposure { get; set; }
        public float? Apperture { get; set; }
        public int? Iso { get; set; }
        [Column(TypeName ="TIMESTAMP(6)")]
        public DateTime CreatedOn { get; set; }

        public List<PhotoTag> Tags { get; set; } = new List<PhotoTag>();
    }
}