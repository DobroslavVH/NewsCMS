using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsCMS.DAL;

namespace NewsCMS.Areas.Admin.Controllers
{
    public class AuthorAvatarsController : Controller
    {
        private NewsCMSContext db = new NewsCMSContext();

        // GET: Admin/AuthorAvatars
        public ActionResult Index(int id)
        {
            var fileToRetrieve = db.AuthorAvatars.Find(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }

        //public ActionResult Edit(int id)
        //{
        //    var fileToEdit = db.AuthorAvatars.Find(id);

        //}
    }
}