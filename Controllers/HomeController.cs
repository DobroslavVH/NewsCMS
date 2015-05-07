using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;
using System.Data;
using NewsCMS.DAL;
using NewsCMS.Areas.Admin.Models;
using PagedList;
using System.Data.Entity.Infrastructure;


namespace NewsCMS.Controllers
{
    public class HomeController : Controller
    {
        private NewsCMSContext db = new NewsCMSContext();


        public ActionResult Index()
        {
            //ViewBag.CurrentFilter = sortOrder;

            //if (searchString != null)
            //{
            //    page = 1;
            //}
            //else 
            //{
            //    searchString = currentFilter;
            //}
            //ViewBag.CurrentFilter = searchString;

            //int pageSize = 6;
            //int pageNumber = (page ?? 1);

            // return view from area/admin/models
            return View(db.Posts.ToList());         
            //return View(db.Posts.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Include(s => s.PostImages).SingleOrDefault(s => s.PostID == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
 
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}