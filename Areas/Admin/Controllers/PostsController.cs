using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NewsCMS.Areas.Admin.Models;
using NewsCMS.DAL;
using System.Data.Entity.Infrastructure;
using PagedList;

namespace NewsCMS.Areas.Admin.Controllers
{
    //[Authorize]
    public class PostsController : Controller
    {
        private NewsCMSContext db = new NewsCMSContext();

        // GET: Admin/Posts
        public ActionResult Index(int? id, string sortOrder, string currentFilter, string searchString, int? page)
        {
            // Show image code
            Post post = db.Posts.Include(s => s.PostImages).SingleOrDefault(s => s.PostID == id);
            
            // "posts" variable uses for Sorting and Searching methods
            var posts = from s in db.Posts select s;

            // Paging method 
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.CurrentSort = sortOrder;

            if (searchString != null)
            {
                page = 1;
            }
            else 
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;


            // Searching method --- BY NAME!!!
            if (!String.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(s => s.Author.FirstName.Contains(searchString) || s.Author.LastName.Contains(searchString));
            }

            // Sorting method code for Category, Author First name, Create and Update dates...
            ViewBag.CategorySortParm = sortOrder == "Category" ? "category_desc" : "Category";
            ViewBag.AuthorSortParm = sortOrder == "Author" ? "author_desc" : "Author";
            ViewBag.CreateDateSortParm = sortOrder == "CrDate" ? "cr_date_desc" : "CrDate";
            ViewBag.UpdateDateSortParm = sortOrder == "UpDate" ? "up_date_desc" : "UpDate";
                        
            switch (sortOrder)
            {
                case "Category":
                    posts = posts.OrderBy(s => s.Category.CategoryName);
                    break;
                case "category_desc":
                    posts = posts.OrderByDescending(s => s.Category.CategoryName);
                    break;
                case "Author":
                    posts = posts.OrderBy(s => s.Author.FirstName);
                    break;
                case "author_desc":
                    posts = posts.OrderByDescending(s => s.Author.FirstName);
                    break;
                case "UpDate":
                    posts = posts.OrderBy(s => s.UpdateAt);
                    break;
                case "up_date_desc":
                    posts = posts.OrderByDescending(s => s.UpdateAt);
                    break;
                case "cr_date_desc":
                    posts = posts.OrderByDescending(s => s.CreateAt);
                    break;
                default:
                    posts = posts.OrderBy(s => s.CreateAt);
                    break;
            }
            // View for sorting and searching
            //return View(posts.ToList());
            
            // View for sorting, searching and PAGING
            return View(posts.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/Posts/Details/5
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

        // GET: Admin/Posts/Create
        public ActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FirstName" );
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostID,PostImage,Title,Text,CreateAt,UpdateAt,AuthorID,CategoryID")] Post post, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                post.CreateAt = DateTime.Now;

                // Upload image 
                if (upload != null && upload.ContentLength > 0)
                {
                    var postImage = new PostImage
                    {
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        FileType = FileType.PostImage,
                        ContentType = upload.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        postImage.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    post.PostImages = new List<PostImage> { postImage };
                }

                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FirstName", post.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", post.CategoryID);
            return View(post);
        }

        // GET: Admin/Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Post post = db.Posts.Find(id);
            Post post = db.Posts.Include(s => s.PostImages).SingleOrDefault(s => s.PostID == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FirstName", post.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", post.CategoryID);
            return View(post);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase upload, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var post = db.Posts.Find(id);
            if (TryUpdateModel(post, "",
                new string[] { "Title", "Text", "UpdateAt", "categoryID", "AuthorID" }))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (upload != null && upload.ContentLength > 0)
                        {
                            var postImage = db.Posts.Find(id);
                            if (postImage.PostImages.Any(f => f.FileType == FileType.PostImage))
                            {
                                db.PostImages.Remove(postImage.PostImages.First(f => f.FileType == FileType.PostImage));
                            }
                            var image = new PostImage
                            {
                                FileName = System.IO.Path.GetFileName(upload.FileName),
                                FileType = FileType.PostImage,
                                ContentType = upload.ContentType
                            };
                            using (var reader = new System.IO.BinaryReader(upload.InputStream))
                            {
                                image.Content = reader.ReadBytes(upload.ContentLength);
                            }
                            postImage.PostImages = new List<PostImage> { image };
                        }
                    }
                    post.UpdateAt = DateTime.Now;

                    db.Entry(post).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
        
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "FirstName", post.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", post.CategoryID);
            return View(post);
        }

        // GET: Admin/Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
