using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsCMS.DAL;

namespace NewsCMS.Areas.Admin.Controllers
{
    [Authorize]
    public class SubCategoryImagesController : Controller
    {
        private NewsCMSContext db = new NewsCMSContext();

        // GET: Admin/SubCategoryImages
        public ActionResult Index(int id)
        {
            var fileToRetrieve = db.SubCategoryImagies.Find(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
    }
}