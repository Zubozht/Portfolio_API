using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("phototags")]
    public class PhotoTag
    {
        public int PhotoId { get; set; }
        public int TagId { get; set; }

        public Photo Photo { get; set; } = null!;
        public Tag Tag { get; set;} = null!;
    }
}