﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Corelib.Models;
using System.IO;
using WebUI.Areas.Member.Models;
using Corelib.Enums;
using WebUI.Models;

namespace WebUI.Areas.Member.Controllers
{
    [Authorize(Roles = "Member")]
    public class ActionMemosController : BaseMemberController
    {
        #region -- Action Results --

        public ActionResult Reply(Guid? guid)
        {
            var returnValue = base.ValidateUser();
            if (returnValue != null) return returnValue;

            if (!guid.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var actionMemo = db.ActionMemos
                .Include(t => t.Member)
                .Include(t => t.Dependent)
                .Include(t => t.Documents)
                .FirstOrDefault(t => t.Guid == guid);
            if (actionMemo == null || actionMemo.MemberId != this.Member.Id) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

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
        public ActionResult Reply(Guid? guid, ActionMemoViewModel actionMemoViewModel, string submit)
        {
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
                    if(setting.BypassHRManagerApproval)
                    {
                        actionMemo.Status = ActionMemoStatus.Replied;
                    }
                    else
                    {
                        actionMemo.Status = ActionMemoStatus.ForCorporateAdminApproval;
                    }
                    Helper.MemberActionMemo(System.Web.HttpContext.Current, actionMemo, actionMemo.Member.AccountCode, setting.BypassHRManagerApproval);
                }

                db.SaveChanges();

                return RedirectToAction("Index", "Home", new { area = "" });
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

            var model = new Document() {
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