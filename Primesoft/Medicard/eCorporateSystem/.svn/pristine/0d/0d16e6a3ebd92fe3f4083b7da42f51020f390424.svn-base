using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Corelib;
using Corelib.Models;
using PagedList;
using PagedList.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Authorize(Roles = "SysAd, CanViewQuestions, CanAddQuestions, CanEditQuestions, CanDeleteQuestions")]
    public class QuestionsController : Controller
    {
        #region -- Variable Declarations --

        private IdentityDataContext db = new IdentityDataContext();

        #endregion

        #region -- Constructor --

        public QuestionsController()
        {
            db.User = db.Users.FirstOrDefault(t => t.UserName == System.Web.HttpContext.Current.User.Identity.Name);
        }

        #endregion

        #region -- Action Results --

        [Authorize(Roles = "SysAd, CanViewQuestions")]
        public ActionResult Index(string sortOrder, string currentFilter, int? page)
        {
            var questions = from t in db.Questions where !t.Deleted select t;
            if (!string.IsNullOrEmpty(currentFilter))
            {
                questions = questions.Where(t => t.Description.Contains(currentFilter));
            }

            Helper.SetSortParameters<Question>(this, ref questions, sortOrder, currentFilter, new SortParameter() { PropertyName = "DisplayOrder" }, new List<SortParameter>() { new SortParameter() { PropertyName = "Description" } });

            return View(questions.ToPagedList(page ?? 1, Config.RecordCountPerPage));
        }

        [Authorize(Roles = "SysAd, CanAddQuestions")]
        public ActionResult Create()
        {
            var model = new Question();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SysAd, CanAddQuestions")]
        public async Task<ActionResult> Create([Bind(Include = "Id, Description, Type, Options, DisplayOrder, Guid")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(question);
        }

        [Authorize(Roles = "SysAd, CanEditQuestions")]
        public async Task<ActionResult> Edit(Guid? guid)
        {
            if (guid == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var question = await db.Questions.FirstOrDefaultAsync(t => t.Guid == guid);
            if (question == null) return HttpNotFound();

            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SysAd, CanEditQuestions")]
        public async Task<ActionResult> Edit([Bind(Include = "Id, Description, Type, Options, DisplayOrder, Guid, CrById, Timestamp")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<ActionResult> Delete(Guid? guid)
        {
            if (guid == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var question = await db.Questions.FirstOrDefaultAsync(t => t.Guid == guid);
            if (question == null) return HttpNotFound();

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid guid)
        {
            var question = await db.Questions.FirstOrDefaultAsync(t => t.Guid == guid);
            db.Questions.Remove(question);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion

        #region -- Overrides --

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        #endregion
    }
}
