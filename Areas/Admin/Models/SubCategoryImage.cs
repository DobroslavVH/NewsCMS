using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NewsCMS.Areas.Admin.Models
{
    public class SubCategoryImage
    {
        [Key]
        public int SubCategoryImageID { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public FileType FileType { get; set; }

        public int SubCategoryID { get; set; }

        public virtual SubCategory SubCategory { get; set; }
    }
}