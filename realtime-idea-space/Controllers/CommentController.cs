using Microsoft.AspNet.Identity;
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

                Hubs.CommentHub.NewCommentAdded(comment);

                return RedirectToAction("Details", "Idea", new { Id = comment.IdeaModelId });
            }

            return View("Create", comment);
        }

        public ActionResult SmsWebhook()
        {
            var fromNumber = Request["msisdn"];
            string commentPhoneNumber = Request["to"];
            var text = Request["text"];

            var fromUser = db.Users.First(user => user.PhoneNumber == fromNumber);

            // If there isn't a user registered with this phone number then we'll
            // treat the comment as anonymous. We can use a check to see if the
            // UserID is a GUID. If not, they're anonymous.
            string fromUserId = (fromUser != null ? fromUser.Id : fromNumber);

            Guid ideaId = db.IdeaModels.First(idea => idea.CommentPhoneNumber == commentPhoneNumber).Id;

            var comment = new CommentModel
            {
                CommentByUserId = fromUserId,
                Text = text,
                Id = Guid.NewGuid(),
                IdeaModelId = ideaId,
                Created = DateTime.Now
            };
            db.IdeaComments.Add(comment);

            try
            {
                db.SaveChanges();

                Hubs.CommentHub.NewCommentAdded(comment);
            }
            catch(Exception ex)
            {
                Nexmo.Api.SMS.Send(new Nexmo.Api.SMS.SMSRequest
                {
                    to = fromNumber,
                    text = string.Format(
                        "Sorry, an exception occurred when handling your text: {0}", ex.Message
                        ),
                    from = Config.NexmoFromNumber
                });
            }

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
