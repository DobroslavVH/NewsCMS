using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NewsCMS.Areas.Admin.Models
{
    public class CategoryImage
    {
        [Key]
        public int CategoryImageID { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public FileType FileType { get; set; }

        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
    }
}