using Corelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;

namespace WebUI.Controllers
{
    public class DocumentsController : Controller
    {
        #region -- Variable Declarations --

        private IdentityDataContext db = new IdentityDataContext();

        #endregion

        #region -- Action Results --

        public async Task<ActionResult> Download(Guid? guid)
        {
            if (guid == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var document = await db.Documents.FirstOrDefaultAsync(t => t.Guid == guid);
            if (document == null) return HttpNotFound();

            return File(Server.MapPath(String.Format("~/Uploads/{0}", document.GuidFilename)), System.Net.Mime.MediaTypeNames.Application.Octet, document.Filename);
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