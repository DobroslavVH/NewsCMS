using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace NewsCMS.Areas.Admin.Models
{
    public class Author
    {
        [Key]
        public int AuthorID { get; set; }



        [DisplayName("First Name")]
        public string FirstName { get; set; }
        
        [DisplayName("Second Name")]
        public string LastName { get; set; }



        [DisplayName("Create Date")]
        [DataType(DataType.DateTime)]
        public DateTime? CreateAt { get;  set; }
        
        [DisplayName("Update Date")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdateAt { get; set; }


        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<AuthorAvatar> AuthorAvatars { get; set; }
    }
}