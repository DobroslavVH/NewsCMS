using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NewsCMS.Areas.Admin.Models
{
    public class Post
    {
        [Key]
        public int PostID { get; set; }



        [DisplayName("Author")]
        public int AuthorID { get; set; }
        public virtual Author Author { get; set; }

        [DisplayName("Category")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }



        [DisplayName("Image")]
        public string PostImage { get; set; }

        [Required]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Text")]
        [DataType(DataType.Text)]
        public string Text { get; set; }



        [DisplayName("Create Date")]
        [DataType(DataType.DateTime)]
        public DateTime? CreateAt { get; set; }

        [DisplayName("Update Date")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdateAt { get; set; }



        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<PostImage> PostImages { get; set; }
    }
}