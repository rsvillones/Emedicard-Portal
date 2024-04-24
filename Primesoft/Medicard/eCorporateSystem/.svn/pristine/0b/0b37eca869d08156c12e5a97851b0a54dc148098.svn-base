using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using WebUI.Models;
using Corelib.Models;
using System.IO;
using Corelib.Enums;

namespace WebUI.Areas.CorporateAdministrator.Controllers
{
    public class ActionMemosController : BaseAccountController
    {
        #region -- Action Results --

        public ActionResult Index(string accountCode,DateTime? dateIssued,string memberName)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            var model = db.ActionMemos
                .Include(t => t.Member)
                .Include(t => t.Dependent)
                .Where(t => t.Member.AccountCode == accountCode)
                .OrderByDescending(t => t.DateIssued).ToList();

            string dateIssuedString = "";
            if (dateIssued != null)
            {
                model = model.Where(t => t.DateIssued.Date == dateIssued.Value.Date).ToList();
                dateIssuedString = dateIssued.Value.ToString("MM/dd/yyyy");
            }
            if (!string.IsNullOrEmpty(memberName))
                model = model.Where(t => t.Member.FirstName.Contains(memberName) || t.Member.MiddleName.Contains(memberName)
                    || t.Member.LastName.Contains(memberName)).ToList();

            ViewBag.DateIssued = dateIssuedString;
            ViewBag.MemberName = memberName;
            ViewBag.SearchValue = String.Format("{0} {1}", dateIssuedString, memberName).Trim();

            return View(model);
        }

        public ActionResult Reply(string accountCode, Guid? guid)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            if (!guid.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var actionMemo = db.ActionMemos
                .Include(t => t.Member)
                .Include(t => t.Dependent)
                .Include(t => t.Documents)
                .FirstOrDefault(t => t.Guid == guid);
            if (actionMemo == null || actionMemo.Member.AccountCode != accountCode) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            actionMemo.IsNew = false;
            db.SaveChanges();

            ViewBag.ActionMemo = actionMemo;
            ViewBag.DocumentTypeList = new SelectList(db.DocumentTypes.OrderBy(t => t.DisplayOrder), "Id", "Name");

            var model = new ActionMemoViewModel()
            {
                Id = actionMemo.Id,
                Guid = actionMemo.Guid,
                MemberReply = actionMemo.MemberReply,
                Documents = actionMemo.Documents
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reply(string accountCode, Guid? guid, ActionMemoViewModel actionMemoViewModel, string submit)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            if (ModelState.IsValid)
            {
                var actionMemo = db.ActionMemos
                    .Include(t => t.Member)
                    .Include(t => t.EndorsementBatch)
                    .FirstOrDefault(t => t.Guid == guid);
                db.Entry(actionMemo).CurrentValues.SetValues(actionMemoViewModel);
                db.Entry(actionMemo).State = EntityState.Modified;
                actionMemo.Documents = actionMemoViewModel.Documents;
                var actionMemoIds = actionMemo.Documents.Select(t => t.Id).ToList();
                db.Documents.RemoveRange(db.Documents.Where(t => t.ActionMemoId == actionMemo.Id && !actionMemoIds.Contains(t.Id)));
                foreach (var document in actionMemo.Documents)
                {
                    if (document.Id != 0)
                    {
                        db.Entry(document).State = EntityState.Modified;
                    }
                    else
                    {
                        db.Entry(document).State = EntityState.Added;
                    }
                }

                if (submit == "Reply")
                {
                    var setting = db.AccountSettings.FirstOrDefault(t => t.AccountCode == actionMemo.EndorsementBatch.AccountCode) ?? new AccountSetting();
                    actionMemo.Status = ActionMemoStatus.Replied;
                    Helper.CorpAdminActionMemo(System.Web.HttpContext.Current, actionMemo, actionMemo.Member.AccountCode);
                }

                db.SaveChanges();

                return RedirectToAction("Index", "ActionMemos", new { accountCode = accountCode });
            }

            return View();
        }

        public ActionResult UploadDocument(HttpPostedFileBase fileData, int documentTypeId)
        {
            var validExtensions = new List<string>()
            {
                "pdf",
                "doc",
                "docx"
            };
            var fileExtension = fileData.FileName.Substring(fileData.FileName.LastIndexOf(".") + 1);
            if (!validExtensions.Contains(fileExtension)) throw new Exception("Invalid file extension.");

            var originalFilename = fileData.FileName;
            var originalFilepath = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/Uploads/{0}", originalFilename));

            var filename = String.Format("{0}.{1}", Guid.NewGuid().ToString(), fileExtension);
            var filepath = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/Uploads/{0}", filename));

            fileData.SaveAs(filepath);

            var dictionary = new Dictionary<string, object>();
            dictionary.Add("Filename", originalFilename);
            dictionary.Add("GuidFilename", filename);

            var documentType = db.DocumentTypes.FirstOrDefault(t => t.Id == documentTypeId);

            var model = new Document()
            {
                DateUploaded = DateTime.Now,
                Filename = originalFilename,
                GuidFilename = filename,
                DocumentTypeId = documentTypeId,
                DocumentType = documentType
            };

            dictionary.Add("TableRow", RenderPartialViewToString("~/Views/Shared/_Document.cshtml", model));

            return this.Json(dictionary);
        }

        #endregion

        #region -- Functions --

        private string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName)) viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion

    }
}