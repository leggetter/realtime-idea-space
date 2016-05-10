﻿using Microsoft.AspNet.Identity;
using realtime_idea_space.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Configuration;
using System.Web.Mvc;

namespace realtime_idea_space.Controllers
{
    public class CommentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Comment
        public ActionResult Index()
        {
            return View(db.IdeaComments.ToList());
        }

        // POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdeaModelId,Text,Created,CommentByUserId")] CommentModel comment)
        {
            if(comment.CommentByUserId != User.Identity.GetUserId())
            {
                ModelState.AddModelError("CommentByUserId", "The User Id did not match the logged in user");
            }

            if (ModelState.IsValid)
            {
                comment.Id = Guid.NewGuid();
                db.IdeaComments.Add(comment);
                db.SaveChanges();

                // Real-time update

                return RedirectToAction("Details", "Idea", new { Id = comment.IdeaModelId });
            }

            return View("Create", comment);
        }

        public ActionResult SmsWebhook()
        {
            // fromNumber/msisdn, to, text
            
            // fromUser based on msidn/from

            // fromUserId of user registered. Otherwise use fromNumber

            // ideaId based on comment/to phone number
            
            // Create comment and save (catch exceptions, reply with SMS?)

            // Real-time update

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // GET: Comment/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentModel ideaComment = db.IdeaComments.Find(id);
            if (ideaComment == null)
            {
                return HttpNotFound();
            }
            return View(ideaComment);
        }

        // Used by Create
        public PartialViewResult CreateComment([Bind(Include = "IdeaModelId")] CommentModel ideaComment)
        {
            ideaComment.CommentByUserId = User.Identity.GetUserId();
            return PartialView("CreateComment", ideaComment);
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentModel ideaComment = db.IdeaComments.Find(id);
            if (ideaComment == null)
            {
                return HttpNotFound();
            }
            return View(ideaComment);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdeaModelId,Text,Created,CommentByUserId")] CommentModel ideaComment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ideaComment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ideaComment);
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentModel ideaComment = db.IdeaComments.Find(id);
            if (ideaComment == null)
            {
                return HttpNotFound();
            }
            return View(ideaComment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CommentModel ideaComment = db.IdeaComments.Find(id);
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
