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
    [Authorize]
    public class CategoriesController : Controller
    {
        private NewsCMSContext db = new NewsCMSContext();

        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Admin/Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Include(s => s.CategoryImages).SingleOrDefault(s => s.CategoryID == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName,CreateAt,UpdateAt")] Category category, HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    category.CreateAt = DateTime.Now;
                    // Upload image
                    if (upload != null && upload.ContentLength > 0)
                    {
                        var categoryImage = new CategoryImage
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.CategoryImage,
                            ContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            categoryImage.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        category.CategoryImages = new List<CategoryImage> { categoryImage };
                    }

                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes ...");
            }
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Include(s => s.CategoryImages).SingleOrDefault(s => s.CategoryID == id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
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

            var category = db.Categories.Find(id);
            if (TryUpdateModel(category, "", new string[] {"CategoryName", "UpdateAt"}))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (upload != null && upload.ContentLength > 0)
                        {
                            var categoryImage = db.Categories.Find(id);
                            if (categoryImage.CategoryImages.Any(f => f.FileType == FileType.CategoryImage))
                            {
                                db.CategoryImages.Remove(categoryImage.CategoryImages.First(f => f.FileType == FileType.CategoryImage));
                            }
                            var newCategoryImage = new CategoryImage
                            {
                                FileName = System.IO.Path.GetFileName(upload.FileName),
                                FileType = FileType.CategoryImage,
                                ContentType = upload.ContentType
                            };
                            using (var reader = new System.IO.BinaryReader(upload.InputStream))
                            {
                                newCategoryImage.Content = reader.ReadBytes(upload.ContentLength);
                            }
                            categoryImage.CategoryImages = new List<CategoryImage> { newCategoryImage };
                        }
                    }
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "unable to save changes ...");
                }
            }
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
