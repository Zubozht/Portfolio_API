using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("tags")]
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Column(TypeName ="NVARCHAR(50)")]
        public string tag { get; set; } = string.Empty;
        [Column(TypeName ="NVARCHAR(100)")]
        public string previewpath {get; set; } = string.Empty;
        public List<PhotoTag> photos { get; set; } = new List<PhotoTag>();

    }
}