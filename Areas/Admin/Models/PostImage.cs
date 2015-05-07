using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NewsCMS.Areas.Admin.Models
{
    public class PostImage
    {
        [Key]
        public int PostImageID { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public FileType FileType { get; set; }

        public int PostID { get; set; }

        public virtual Post post { get; set; }
    }
}