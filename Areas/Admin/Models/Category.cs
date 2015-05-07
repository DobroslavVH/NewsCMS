using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsCMS.Areas.Admin.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }



        [DisplayName("Category")]
        public string CategoryName { get; set; }
        


        [DisplayName("Create Date")]
        [DataType(DataType.DateTime)]
        public DateTime? CreateAt { get; set; }
        
        [DisplayName("Update Date")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdateAt { get; set; }



        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<SubCategory> SubCategories { get; set; }
        public virtual ICollection<CategoryImage> CategoryImages { get; set; }
    }
}