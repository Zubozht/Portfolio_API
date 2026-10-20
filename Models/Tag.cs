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
        public int Id { get; set; }
        [Column(TypeName ="NVARCHAR(50)")]
        public string Name { get; set; } = string.Empty;
        [Column(TypeName ="NVARCHAR(100)")]
        public string PreviewPath {get; set; } = string.Empty;

        public List<PhotoTag> Photos { get; set; } = new List<PhotoTag>();

    }
}