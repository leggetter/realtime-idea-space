using Microsoft.AspNet.Identity;
using realtime_idea_space.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace realtime_idea_space.Controllers
{
    public class IdeaCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: IdeaComments
        public ActionResult Index()
        {
            return View(db.IdeaComments.ToList());
        }

        // GET: IdeaComments/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdeaComment ideaComment = db.IdeaComments.Find(id);
            if (ideaComment == null)
            {
                return HttpNotFound();
            }
            return View(ideaComment);
        }

        // Used by Create
        public PartialViewResult CreateComment([Bind(Include = "IdeaModelId")] IdeaComment ideaComment)
        {
            ideaComment.CommentByUserId = User.Identity.GetUserId();
            return PartialView("CreateComment", ideaComment);
        }

        // POST: IdeaComments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdeaModelId,Text,Created,CommentByUserId")] IdeaComment ideaComment)
        {
            if(ideaComment.CommentByUserId != User.Identity.GetUserId())
            {
                ModelState.AddModelError("CommentByUserId", "The User Id did not match the logged in user");
            }

            if (ModelState.IsValid)
            {
                ideaComment.Id = Guid.NewGuid();
                db.IdeaComments.Add(ideaComment);
                db.SaveChanges();
                return RedirectToAction("Details", "Idea", new { Id = ideaComment.IdeaModelId });
            }

            return View("Create", ideaComment);
        }

        // GET: IdeaComments/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdeaComment ideaComment = db.IdeaComments.Find(id);
            if (ideaComment == null)
            {
                return HttpNotFound();
            }
            return View(ideaComment);
        }

        // POST: IdeaComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdeaModelId,Text,Created,CommentByUserId")] IdeaComment ideaComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ideaComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ideaComment);
        }

        // GET: IdeaComments/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdeaComment ideaComment = db.IdeaComments.Find(id);
            if (ideaComment == null)
            {
                return HttpNotFound();
            }
            return View(ideaComment);
        }

        // POST: IdeaComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            IdeaComment ideaComment = db.IdeaComments.Find(id);
            db.IdeaComments.Remove(ideaComment);
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
