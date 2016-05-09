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

                var countryCode = WebConfigurationManager.AppSettings["Nexmo.BuyNumberCountry"] ?? "GB";

                var numberSearchResult = Nexmo.Api.Number.Search(new Nexmo.Api.Number.SearchRequest{
                    country = countryCode,
                    features = "SMS"
                });
                var numberDetails = numberSearchResult.numbers.First();

                var buyResponse = Nexmo.Api.Number.Buy(countryCode, numberDetails.msisdn);

                if(buyResponse.ErrorCode != "200")
                {
                    // something went wrong
                }
 
                ideaModel.CommentPhoneNumber = numberDetails.msisdn;

                db.IdeaModels.Add(ideaModel);
                db.SaveChanges();
                return RedirectToAction("Details", new { Id = ideaModel.Id });
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

        public PartialViewResult CreateComment([Bind(Include = "IdeaModelId")] CommentModel ideaComment)
        {
            ideaComment.CommentByUserId = User.Identity.GetUserId();
            return PartialView("CreateComment", ideaComment);
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
