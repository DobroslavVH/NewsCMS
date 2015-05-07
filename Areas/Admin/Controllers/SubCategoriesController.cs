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
    public class SubCategoriesController : Controller
    {
        private NewsCMSContext db = new NewsCMSContext();

        // GET: Admin/SubCategories
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            // This variable is used by Sorting and Searching methods
            var subCat = from s in db.SubCategories.Include(s => s.Category) select s;

            // Searching method
            if (!String.IsNullOrEmpty(searchString))
            {
                subCat = subCat.Where(s => s.SubCategoryName.Contains(searchString) 
                                    || s.Category.CategoryName.Contains(searchString));
            }

            // Paging method
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

            int pageSize = 10;
            int pageNumber = (page ?? 1);


            // Sorting method
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "category_desc" : "";
            ViewBag.SubCategorySortParm = sortOrder == "Subcategory" ? "subcategory_desc": "Subcategory";

            switch (sortOrder)
            {
                case "Subcategory":
                    subCat = subCat.OrderBy(s => s.SubCategoryName);
                    break;
                case "subcategory_desc":
                    subCat = subCat.OrderByDescending(s => s.SubCategoryName);
                    break;
                case "category_desc":
                    subCat = subCat.OrderByDescending(s => s.Category.CategoryName);
                    break;
                default:
                    subCat = subCat.OrderBy(s => s.Category.CategoryName);
                    break;
            }
            
            return View(subCat.ToPagedList(pageNumber, pageSize));
        }

        

        // GET: Admin/SubCategories/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Admin/SubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubCategoryID,SubCategoryName,CategoryID")] SubCategory subCategory, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var image = new SubCategoryImage
                    {
                        FileName = System.IO.Path.GetFileName(upload.FileName),
                        FileType = FileType.SubCategoryImage,
                        ContentType = upload.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        image.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    subCategory.SubCategoryImagies = new List<SubCategoryImage> { image };
                }

                db.SubCategories.Add(subCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", subCategory.CategoryID);
            return View(subCategory);
        }

        // GET: Admin/SubCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //SubCategory subCategory = db.SubCategories.Find(id);
            SubCategory subCategory = db.SubCategories.Include(s => s.SubCategoryImagies).SingleOrDefault(s => s.SubCategoryID == id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", subCategory.CategoryID);
            return View(subCategory);
        }

        // POST: Admin/SubCategories/Edit/5
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

            var subCategory = db.SubCategories.Find(id);
            if (TryUpdateModel(subCategory, "", new string[] { "SubCategoryName", "CategoryID" }))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (upload != null && upload.ContentLength > 0)
                        {
                            var subCategoryImage = db.SubCategories.Find(id);
                            if (subCategoryImage.SubCategoryImagies.Any(f => f.FileType == FileType.Image))
                            {
                                db.SubCategoryImagies.Remove(subCategoryImage.SubCategoryImagies.First(f => f.FileType == FileType.Image));
                            }
                            var newSubCatImage = new SubCategoryImage
                            {
                                FileName = System.IO.Path.GetFileName(upload.FileName),
                                FileType = FileType.Image,
                                ContentType = upload.ContentType
                            };
                            using (var reader = new System.IO.BinaryReader(upload.InputStream))
                            {
                                newSubCatImage.Content = reader.ReadBytes(upload.ContentLength);
                            }
                            subCategoryImage.SubCategoryImagies = new List<SubCategoryImage> { newSubCatImage };
                        }
                    }
                    db.Entry(subCategory).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes");
                }
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", subCategory.CategoryID);
            return View(subCategory);
        }

        // GET: Admin/SubCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.SubCategories.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // POST: Admin/SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubCategory subCategory = db.SubCategories.Find(id);
            db.SubCategories.Remove(subCategory);
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
