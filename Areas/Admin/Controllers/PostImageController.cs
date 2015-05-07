using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsCMS.DAL;

namespace NewsCMS.Areas.Admin.Controllers
{
    public class PostImageController : Controller
    {
        private NewsCMSContext db = new NewsCMSContext();

        // GET: Admin/PostImage
        public ActionResult Index(int id)
        {
            var imageToRetrieve = db.PostImages.Find(id);
            return File(imageToRetrieve.Content, imageToRetrieve.ContentType);
            
        }
    }
}
