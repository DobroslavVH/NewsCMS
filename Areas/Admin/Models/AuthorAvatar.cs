using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsCMS.Areas.Admin.Models
{
    public class AuthorAvatar
    {
        [Key]
        public int AvatarID { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        [StringLength(100)]
        public string ContentType { get; set; }

        public byte[] Content { get; set; }

        public FileType FileType { get; set; }

        public int AuthorID { get; set; }

        public virtual Author Author { get; set; }
    }
}