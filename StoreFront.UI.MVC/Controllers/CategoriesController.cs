using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreFront.Data.EF;

namespace StoreFront.UI.MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private StoreFrontEntities db = new StoreFrontEntities();

        // GET: Categories
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        #region AJAX CRUD

        [AcceptVerbs(HttpVerbs.Post)]
        [Authorize(Roles = "Admin")]
        public JsonResult AjaxDelete(int id)
        {
            //Create a Category object with the provided ID.
            Category category = db.Categories.Find(id);

            //Remove that Category from the db.
            db.Categories.Remove(category);
            db.SaveChanges();

            //String together a message to return.
            string confirmMessage = string.Format($"{category.CategoryName} was deleted from the database.");

            //Return the ID and the Message.
            return Json(new { id = id, message = confirmMessage });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public JsonResult AjaxCreate(Category category)
        {
            //Add the new Category.
            db.Categories.Add(category);
            db.SaveChanges();

            //Return JSON data.
            return Json(category);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public PartialViewResult CategoryEdit(int id)
        {
            //Create a Category object from the db.
            Category category = db.Categories.Find(id);

            //Pass that Category object to the edit form.
            return PartialView(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public JsonResult AjaxEdit(Category category)
        {
            //Edit the record.
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();

            //Return a JsonResult.
            return Json(category);
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Scaffolded Code
        /*
        // GET: Categories/Details/5
        public ActionResult Details(int? id)
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

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
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

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */
        #endregion

    }
}
