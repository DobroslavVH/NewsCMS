using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsCMS.DAL;

namespace NewsCMS.Areas.Admin.Controllers
{
    public class CategoryImagesController : Controller
    {
        private NewsCMSContext db = new NewsCMSContext();

        // GET: Admin/CategoryImages
        public ActionResult Index(int id)
        {
            var fileToRetrieve = db.CategoryImages.Find(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
    }
}