using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using realtime_idea_space.Models;

namespace realtime_idea_space.Controllers
{
    [Authorize]
    public class IdeaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Idea
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.IdeaModels.ToList());
        }

        // GET: Idea/Details/5
        [AllowAnonymous]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdeaModel ideaModel = db.IdeaModels.Find(id);
            if (ideaModel == null)
            {
                return HttpNotFound();
            }
            return View(ideaModel);
        }

        // GET: Idea/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Idea/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Created,Description,CreatedByUserId")] IdeaModel ideaModel)
        {
            if (ModelState.IsValid)
            {
                ideaModel.Id = Guid.NewGuid();
                db.IdeaModels.Add(ideaModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ideaModel);
        }

        // GET: Idea/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdeaModel ideaModel = db.IdeaModels.Find(id);
            if (ideaModel == null)
            {
                return HttpNotFound();
            }
            return View(ideaModel);
        }

        // POST: Idea/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Created,Description")] IdeaModel ideaModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ideaModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ideaModel);
        }

        // GET: Idea/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdeaModel ideaModel = db.IdeaModels.Find(id);
            if (ideaModel == null)
            {
                return HttpNotFound();
            }
            return View(ideaModel);
        }

        // POST: Idea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            IdeaModel ideaModel = db.IdeaModels.Find(id);
            db.IdeaModels.Remove(ideaModel);
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
