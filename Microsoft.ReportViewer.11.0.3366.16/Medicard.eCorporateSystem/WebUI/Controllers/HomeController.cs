﻿using Corelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Corelib.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        #region -- Variable Declarations --

        private IdentityDataContext db = new IdentityDataContext();

        #endregion

        #region -- Constructor --

        public HomeController()
        {
            db.User = db.Users.FirstOrDefault(t => t.UserName == System.Web.HttpContext.Current.User.Identity.Name);
        }

        #endregion

        #region -- Action Results --

        public ActionResult Index()
        {
            var member = db.Members.FirstOrDefault(t => t.UserName == User.Identity.Name) ?? new Member();
            ViewBag.ActionMemos = db.ActionMemos
                .Include(t => t.Member)
                .Include(t => t.Dependent)
                .Include(t => t.EndorsementBatch)
                .Where(t => t.MemberId == member.Id).OrderByDescending(t => t.DateIssued).ToList();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
         
        #endregion

        #region -- Overrides --

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                db = null;
            }

            base.Dispose(disposing);
        }

        #endregion
   }
}