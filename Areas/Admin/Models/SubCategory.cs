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
    public class SubCategory
    {
        [Key]
        public int SubCategoryID { get; set; }

        [DisplayName("Subcategory")]
        public string SubCategoryName { get; set; }

        [DisplayName("Category name")]
        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<SubCategoryImage> SubCategoryImagies { get; set; }
    }
}