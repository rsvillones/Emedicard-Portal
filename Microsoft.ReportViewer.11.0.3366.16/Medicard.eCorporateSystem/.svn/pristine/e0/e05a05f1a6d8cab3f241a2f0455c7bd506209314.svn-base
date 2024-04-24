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
    [Authorize(Roles = "SysAd, CanViewDocumentTypes, CanAddDocumentTypes, CanEditDocumentTypes, CanDeleteDocumentTypes")]
    public class DocumentTypesController : Controller
    {
        #region -- Variable Declarations --

        private IdentityDataContext db = new IdentityDataContext();

        #endregion

        #region -- Constructor --

        public DocumentTypesController()
        {
            db.User = db.Users.FirstOrDefault(t => t.UserName == System.Web.HttpContext.Current.User.Identity.Name);
        }

        #endregion

        #region -- Action Results --

        [Authorize(Roles = "SysAd, CanViewDocumentTypes")]
        public ActionResult Index(string sortOrder, string currentFilter, int? page)
        {
            var documentTypes = from t in db.DocumentTypes where !t.Deleted select t;
            if (!string.IsNullOrEmpty(currentFilter))
            {
                documentTypes = documentTypes.Where(t => t.Name.Contains(currentFilter));
            }

            Helper.SetSortParameters<DocumentType>(this, ref documentTypes, sortOrder, currentFilter, new SortParameter() { PropertyName = "DisplayOrder" }, new List<SortParameter>() { new SortParameter() { PropertyName = "Name" } });

            return View(documentTypes.ToPagedList(page ?? 1, Config.RecordCountPerPage));
        }

        [Authorize(Roles = "SysAd, CanAddDocumentTypes")]
        public ActionResult Create()
        {
            var model = new DocumentType();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SysAd, CanAddDocumentTypes")]
        public async Task<ActionResult> Create([Bind(Include = "Id, Name, DisplayOrder, Guid")] DocumentType model)
        {
            if (ModelState.IsValid)
            {
                db.DocumentTypes.Add(model);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [Authorize(Roles = "SysAd, CanEditDocumentTypes")]
        public async Task<ActionResult> Edit(Guid? guid)
        {
            if (guid == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var documentType = await db.DocumentTypes.FirstOrDefaultAsync(t => t.Guid == guid);
            if (documentType == null) return HttpNotFound();

            return View(documentType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SysAd, CanEditDocumentTypes")]
        public async Task<ActionResult> Edit([Bind(Include = "Id, Name, DisplayOrder, Guid, CrById, Timestamp")] DocumentType model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "SysAd, CanDeleteDocumentTypes")]
        public async Task<ActionResult> Delete(Guid? guid)
        {
            if (guid == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var documentType = await db.DocumentTypes.FirstOrDefaultAsync(t => t.Guid == guid);
            if (documentType == null) return HttpNotFound();

            return View(documentType);
        }

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "SysAd, CanDeleteDocumentTypes")]
        public async Task<ActionResult> DeleteConfirmed(Guid guid)
        {
            var documentType = await db.DocumentTypes.FirstOrDefaultAsync(t => t.Guid == guid);
            db.DocumentTypes.Remove(documentType);
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