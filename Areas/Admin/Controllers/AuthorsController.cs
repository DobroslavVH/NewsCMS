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
	[Authorize(Roles = "Admin")]
	public class AuthorsController : Controller
	{
		private NewsCMSContext db = new NewsCMSContext();

		// GET: Admin/Authors
		public ActionResult Index(int? id, string sortOrder, string searchString, string currentFilter, int? page)
		{
			// Show avatar code
			Author author = db.Authors.Include(s => s.AuthorAvatars).SingleOrDefault(s => s.AuthorID == id);
			
			// Authors variale is used by sorting and searching methods
			var authors = from s in db.Authors select s;

			
			// Searching method by first and last name
			if (!String.IsNullOrEmpty(searchString))
			{
				authors = authors.Where(s => s.FirstName.Contains(searchString) || s.LastName.Contains(searchString));
			}

			// Sorting authors by coloumn in index page code
			ViewBag.CurrentSort = sortOrder;
			ViewBag.AuthorSortParm = sortOrder == "Author" ? "author_desc" : "Author";
			ViewBag.CreateDateSortParm = sortOrder == "CrDate" ? "cr_date_desc" : "CrDate";
			ViewBag.UpdateDateSortParm = sortOrder == "UpDate" ? "up_date_desc" : "UpDate";

			// Part of paging method
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


			switch (sortOrder)
			{
				case "UpDate":
					authors = authors.OrderBy(s => s.UpdateAt);
					break;
				case "up_date_desc":
					authors = authors.OrderByDescending(s => s.UpdateAt);
					break;
				case "Author":
					authors = authors.OrderBy(s => s.FirstName);
					break;
				case "author_desc":
					authors = authors.OrderByDescending(s => s.FirstName);
					break;
				case "cr_date_desc":
					authors = authors.OrderByDescending(s => s.CreateAt);
					break;
				default:
					authors = authors.OrderBy(s => s.CreateAt);
					break;
			}


			// Return view
			return View(authors.ToPagedList(pageNumber, pageSize));


		}

		// GET: Admin/Authors/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			//Author author = db.Authors.Find(id);
			Author author = db.Authors.Include(s => s.AuthorAvatars).SingleOrDefault(s => s.AuthorID == id);
			
			if (author == null)
			{
				return HttpNotFound();
			}
			return View(author);
		}

		// GET: Admin/Authors/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Admin/Authors/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "AuthorID,FirstName,LastName,CreateAt,UpdateAt")] Author author, HttpPostedFileBase upload)
		{
			if (ModelState.IsValid)
			{
				if (upload != null && upload.ContentLength > 0)
				{
					var avatar = new AuthorAvatar
					{
						FileName = System.IO.Path.GetFileName(upload.FileName),
						FileType = FileType.Avatar,
						ContentType = upload.ContentType
					};
					using (var reader = new System.IO.BinaryReader(upload.InputStream))
					{
						avatar.Content = reader.ReadBytes(upload.ContentLength);
					}
					author.AuthorAvatars = new List<AuthorAvatar> { avatar };
				}



				author.CreateAt = DateTime.Now;

				db.Authors.Add(author);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(author);
		}

		// GET: Admin/Authors/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			//Author author = db.Authors.Find(id);
			Author author = db.Authors.Include(s => s.AuthorAvatars).SingleOrDefault(s => s.AuthorID == id);
			if (author == null)
			{
				return HttpNotFound();
			}
			return View(author);
		}

		// POST: Admin/Authors/Edit/5
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
			
			var author = db.Authors.Find(id);
			if (TryUpdateModel(author, "",
				new string[] { "FirstName", "LastName", "UpdateAt"}))
			{
				try
				{
					if (ModelState.IsValid)
					{
						if (upload != null && upload.ContentLength > 0)
						{
							var authorAvatar = db.Authors.Find(id);
							if (authorAvatar.AuthorAvatars.Any(f => f.FileType == FileType.Avatar))
							{
								db.AuthorAvatars.Remove(authorAvatar.AuthorAvatars.First(f => f.FileType == FileType.Avatar));
							}
							var avatar = new AuthorAvatar
							{
								FileName = System.IO.Path.GetFileName(upload.FileName),
								FileType = FileType.Avatar,
								ContentType = upload.ContentType
							};
							using (var reader = new System.IO.BinaryReader(upload.InputStream))
							{
								avatar.Content = reader.ReadBytes(upload.ContentLength);
							}
							authorAvatar.AuthorAvatars = new List<AuthorAvatar> { avatar };
						}

					}

					db.Entry(author).State = EntityState.Modified;
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				catch (RetryLimitExceededException  /* dex */) 
				{
					ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
				}
			}
			return View(author);
		}
			

		// GET: Admin/Authors/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Author author = db.Authors.Find(id);
			if (author == null)
			{
				return HttpNotFound();
			}
			return View(author);
		}

		// POST: Admin/Authors/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			Author author = db.Authors.Find(id);
			db.Authors.Remove(author);
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
