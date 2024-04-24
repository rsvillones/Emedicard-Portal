﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Corelib;
using Corelib.Models;
using OfficeOpenXml;
using System.IO;
using System.Data.Entity;
using Corelib.Classes;
using System.Threading.Tasks;
using System.Net;
using Corelib.Enums;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Areas.CorporateAdministrator.Controllers
{
    [Authorize(Roles = "SysAd, CanViewActiveMembers")]
    public class ActiveMembersController : BaseAccountController
    {
        // GET: CorporateAdministrator/ActiveMembers
        public ActionResult Index(string accountCode, string sortOrder, string currentFilter, int? page, string messageType, string message)
        {
            var returnValue = base.ValidateAccountCode(accountCode);
            if (returnValue != null) return returnValue;

            ViewBag.MessageType = messageType;
            ViewBag.Message = message;

            var model = db.Members.Include(t => t.EndorsementBatch).Where(t => t.AccountCode == this.LegacyAccount.Code && !t.Deleted).ToList();


            return View(model.ToPagedList(page ?? 1, Config.RecordCountPerPage));
        }

        public ActionResult ActivePricipalMembers()
        {
            return View();
        }

        public ActionResult ActiveDependentMembers()
        {
            return View();
        }
    }
}