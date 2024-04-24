﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.Mailer;
using System.Net.Mail;
using Corelib;
using WebUI.Models;
using Corelib.Models;

namespace WebUI.Mailers
{
    public class UserMailer : MailerBase, IUserMailer
    {
        public UserMailer()
        {
            MasterName = "_Layout";
        }

        #region -- Member Profile --

        public virtual MvcMailMessage SendMemberNotification(Member member, string userName, string password)
        {
            using (var db = new IdentityDataContext())
            {
                var emailViewModel = new EmailViewModel()
                {
                    Member = member,
                    Password = password,
                    UserName = userName,
                    Url = Helper.AbsoluteAction("Index", "Profile", new { area = "Member" })
                };

                var returnValue = new MvcMailMessage
                {
                    ViewName = "MembershipEmailNotification",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Membership Profile Notification for Member",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/MemberNotification.cshtml")
                };
                return returnValue;
            }
        }

        public virtual MvcMailMessage SendBatchSummaryNotification(List<Member> members)
        {
            using (var db = new IdentityDataContext())
            {
                var emailViewModel = new EmailViewModel()
                {
                    Members = members
                };

                var returnValue = new MvcMailMessage
                {
                    ViewName = "BatchSummaryNotification",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Membership Profile Batch Summary Notification For Corporate Administrator",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/ProcessBatchNotification.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendCorpAdminMemberProfileNotification(Member member)
        {
            using (var db = new IdentityDataContext())
            {
                var emailViewModel = new EmailViewModel()
                {
                    Member = member
                };

                var returnValue = new MvcMailMessage
                {
                    ViewName = "HRMemberProfileNotification",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Membership Profile Notification For Corporate Administrator",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/HrMemberProfileNotification.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendUrgMemberProfileNotification(Member member)
        {
            using (var db = new IdentityDataContext())
            {
                var emailViewModel = new EmailViewModel()
                {
                    Member = member
                };

                var returnValue = new MvcMailMessage
                {
                    ViewName = "URGMemberProfileNotification",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Membership Profile Notification For Underwriter",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/UrgMemberProfileNotification.cshtml")
                };

                return returnValue;
            }
        }

        #endregion

        #region -- Action Memo -- 

        public virtual MvcMailMessage SendMemberActionMemo(ActionMemo actionMemo,bool bypassHrApproval)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == actionMemo.MemberId) ?? new Member();
                Dependent dependent = null;
                if(actionMemo.DependentId != null) dependent = db.Dependents.FirstOrDefault(t => !t.Deleted && t.Id == actionMemo.DependentId);
                var emailViewModel = new EmailViewModel()
                {
                    ActionMemo = actionMemo,
                    Member = member,
                    Dependent = dependent
                };
                string subject = "Action Memo For Underwriter";
                if (!bypassHrApproval) subject = "Action Memo For Corporate Administrator";
                var returnValue = new MvcMailMessage
                {
                    ViewName = "ActionMemo",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/MemberActionMemo.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendUrgActionMemo(ActionMemo actionMemo)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == actionMemo.MemberId) ?? new Member();
                Dependent dependent = null;
                if (actionMemo.DependentId != null) dependent = db.Dependents.FirstOrDefault(t => !t.Deleted && t.Id == actionMemo.DependentId);
                var emailViewModel = new EmailViewModel()
                {
                    ActionMemo = actionMemo,
                    Member = member,
                    Dependent = dependent
                };

                var returnValue = new MvcMailMessage
                {
                    ViewName = "ActionMemo",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Action Memo For Member",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/UrgActionMemo.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendCorpAdminActionMemo(ActionMemo actionMemo)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == actionMemo.MemberId) ?? new Member();
                Dependent dependent = null;
                if (actionMemo.DependentId != null) dependent = db.Dependents.FirstOrDefault(t => !t.Deleted && t.Id == actionMemo.DependentId);
                var emailViewModel = new EmailViewModel()
                {
                    ActionMemo = actionMemo,
                    Member = member,
                    Dependent = dependent
                };

                var returnValue = new MvcMailMessage
                {
                    ViewName = "ActionMemo",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Action Memo For Underwriter",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/CorpAdminActionMemo.cshtml")
                };

                return returnValue;
            }
        }

        #endregion

        #region -- Amendment --

        public virtual MvcMailMessage SendMemberAmendment(Amendment amendment, bool bypassHrApproval)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == amendment.MemberId) ?? new Member();
                Dependent dependent = null;
                if (amendment.DependentId != null) dependent = db.Dependents.FirstOrDefault(t => !t.Deleted && t.Id == amendment.DependentId);
                var emailViewModel = new EmailViewModel()
                {
                    Amendment = amendment,
                    Member = member,
                    Dependent = dependent
                };
                string subject = "Amendment For Underwriter";
                if (!bypassHrApproval) subject = "Amendment For Corporate Administrator";
                var returnValue = new MvcMailMessage
                {
                    ViewName = "Amendment",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/MemberAmendment.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendUrgAmendment(Amendment amendment)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == amendment.MemberId) ?? new Member();
                Dependent dependent = null;
                if (amendment.DependentId != null) dependent = db.Dependents.FirstOrDefault(t => !t.Deleted && t.Id == amendment.DependentId);
                var emailViewModel = new EmailViewModel()
                {
                    Amendment = amendment,
                    Member = member,
                    Dependent = dependent
                };
                var returnValue = new MvcMailMessage
                {
                    ViewName = "Amendment",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Amendment For Member",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/UrgAmendment.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendCorpAdminAmendment(Amendment amendment)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == amendment.MemberId) ?? new Member();
                Dependent dependent = null;
                if (amendment.DependentId != null) dependent = db.Dependents.FirstOrDefault(t => !t.Deleted && t.Id == amendment.DependentId);
                var emailViewModel = new EmailViewModel()
                {
                    Amendment = amendment,
                    Member = member,
                    Dependent = dependent
                };
                var returnValue = new MvcMailMessage
                {
                    ViewName = "Amendment",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Amendment For Underwriter",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/CorpAdminAmendment.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendCorpAdminCancelAmendment(Amendment amendment)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == amendment.MemberId) ?? new Member();
                Dependent dependent = null;
                if (amendment.DependentId != null) dependent = db.Dependents.FirstOrDefault(t => !t.Deleted && t.Id == amendment.DependentId);
                var emailViewModel = new EmailViewModel()
                {
                    Amendment = amendment,
                    Member = member,
                    Dependent = dependent
                };
                var returnValue = new MvcMailMessage
                {
                    ViewName = "Amendment",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Cancelled Amendment For Member",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/CorpAdminCancelAmendment.cshtml")
                };

                return returnValue;
            }
        }

        #endregion

        #region -- ID Replacement --

        public virtual MvcMailMessage SendMemberIdReplacement(IdReplacement idReplacement, bool bypassHrApproval)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == idReplacement.MemberId) ?? new Member();
                Dependent dependent = null;
                if (idReplacement.DependentId != null) dependent = db.Dependents.FirstOrDefault(t => !t.Deleted && t.Id == idReplacement.DependentId);
                var emailViewModel = new EmailViewModel()
                {
                    IdReplacement = idReplacement,
                    Member = member,
                    Dependent = dependent
                };
                string subject = "ID Replacement For Underwriter";
                if (!bypassHrApproval) subject = "ID Replacement For Corporate Administrator";
                var returnValue = new MvcMailMessage
                {
                    ViewName = "IDReplacement",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/MemberIdReplacement.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendUrgIdReplacement(IdReplacement idReplacement)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == idReplacement.MemberId) ?? new Member();
                Dependent dependent = null;
                if (idReplacement.DependentId != null) dependent = db.Dependents.FirstOrDefault(t => !t.Deleted && t.Id == idReplacement.DependentId);
                var emailViewModel = new EmailViewModel()
                {
                    IdReplacement = idReplacement,
                    Member = member,
                    Dependent = dependent
                };
                var returnValue = new MvcMailMessage
                {
                    ViewName = "IdReplacement",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "ID Replacement For Member",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/UrgIdReplacement.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendCorpAdminIdReplacement(IdReplacement idReplacement)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == idReplacement.MemberId) ?? new Member();
                Dependent dependent = null;
                if (idReplacement.DependentId != null) dependent = db.Dependents.FirstOrDefault(t => !t.Deleted && t.Id == idReplacement.DependentId);
                var emailViewModel = new EmailViewModel()
                {
                    IdReplacement = idReplacement,
                    Member = member,
                    Dependent = dependent
                };
                var returnValue = new MvcMailMessage
                {
                    ViewName = "IdReplacement",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "ID Replacement For Underwriter",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/CorpAdminIdReplacement.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendCorpAdminCancelIdReplacement(IdReplacement idReplacement)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == idReplacement.MemberId) ?? new Member();
                Dependent dependent = null;
                if (idReplacement.DependentId != null) dependent = db.Dependents.FirstOrDefault(t => !t.Deleted && t.Id == idReplacement.DependentId);
                var emailViewModel = new EmailViewModel()
                {
                    IdReplacement = idReplacement,
                    Member = member,
                    Dependent = dependent
                };
                var returnValue = new MvcMailMessage
                {
                    ViewName = "IdReplacement",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Cancelled ID Replacement For Member",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/CorpAdminCancelIdReplacement.cshtml")
                };

                return returnValue;
            }
        }

        #endregion

        #region -- Additional Dependent --

        public virtual MvcMailMessage SendMemberAdditionalDependent(AdditionalDependent model, bool bypassHrApproval)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == model.MemberId) ?? new Member();                
                var emailViewModel = new EmailViewModel()
                {
                    AdditionalDependent = model,
                    Member = member
                };
                string subject = "Additional Dependent Notification For Underwriter";
                if (!bypassHrApproval) subject = "Additional Dependent Notification For Corporate Administrator";
                var returnValue = new MvcMailMessage
                {
                    ViewName = "AdditionalDependent",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/MemberAdditionalDependent.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendCorpAdminAdditionalDependent(AdditionalDependent model)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == model.MemberId) ?? new Member();
                var emailViewModel = new EmailViewModel()
                {
                    AdditionalDependent = model,
                    Member = member
                };
                var returnValue = new MvcMailMessage
                {
                    ViewName = "AdditionalDependent",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Additional Dependent Notification For Underwriter",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/CorpAdminAdditionalDependent.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendCorpAdminCancelAdditionalDependent(AdditionalDependent model)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == model.MemberId) ?? new Member();
                var emailViewModel = new EmailViewModel()
                {
                    AdditionalDependent = model,
                    Member = member
                };
                var returnValue = new MvcMailMessage
                {
                    ViewName = "AdditionalDependent",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Additional Dependent, Cancelled request notification for member",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/CorpAdminCancelAdditionalDependent.cshtml")
                };

                return returnValue;
            }
        }

        #endregion

        #region -- Dependent Cancellation --

        public virtual MvcMailMessage SendMemberDependentCancellation(DependentCancellation model, bool bypassHrApproval)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == model.MemberId) ?? new Member();
                var emailViewModel = new EmailViewModel()
                {
                    DependentCancellation = model,
                    Member = member
                };
                string subject = "Cancelletion for Dependent Membership Notification For Underwriter";
                if (!bypassHrApproval) subject = "Cancelletion for Dependent Membership Notification For Corporate Administrator";
                var returnValue = new MvcMailMessage
                {
                    ViewName = "DependentCancellation",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = subject,
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/MemberDependentCancellation.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendCorpAdminDependentCancellation(DependentCancellation model)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == model.MemberId) ?? new Member();
                var emailViewModel = new EmailViewModel()
                {
                    DependentCancellation = model,
                    Member = member
                };
                var returnValue = new MvcMailMessage
                {
                    ViewName = "DependentCancellation",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Cancelletion for Dependent Membership Notification For Underwriter",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/CorpAdminDependentCancellation.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendCorpAdminCancelDependentCancellation(DependentCancellation model)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == model.MemberId) ?? new Member();
                var emailViewModel = new EmailViewModel()
                {
                    DependentCancellation = model,
                    Member = member
                };
                var returnValue = new MvcMailMessage
                {
                    ViewName = "DependentCancellation",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Cancelletion for Dependent Membership, Cancelled request notification for Member",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/CorpAdminCancelDependentCancellation.cshtml")
                };

                return returnValue;
            }
        }

        #endregion

        #region -- Membership Cancellation --

        public virtual MvcMailMessage SendMembershipCancellation(CancelledMember model)
        {
            using (var db = new IdentityDataContext())
            {
                var emailViewModel = new EmailViewModel()
                {
                    CancelledMember = model
                }; 
                var returnValue = new MvcMailMessage
                {
                    ViewName = "MembershipCancellation",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Cancelletion of Membership, Notification For Notification For Member.",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/MembershipCancellation.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendCorpAdminMembershipCancellation(IEnumerable<CancelledMember> model)
        {
            using (var db = new IdentityDataContext())
            {
                var emailViewModel = new EmailViewModel(){
                    CancelledMembers = model
                };
                var returnValue = new MvcMailMessage
                {
                    ViewName = "MembershipCancellation",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Cancelletion of Membership, Notification For Underwriter.",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/CorpAdminMembershipCancellation.cshtml")
                };

                return returnValue;
            }
        }

        #endregion

        #region -- Membership Renewal --

        public virtual MvcMailMessage SendMembershipRenewal(RenewalMember model)
        {
            using (var db = new IdentityDataContext())
            {
                var emailViewModel = new EmailViewModel()
                {
                    RenewalMember = model
                };
                var returnValue = new MvcMailMessage
                {
                    ViewName = "MembershipRenewal",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Renewal of Membership, Notification For Notification For Member.",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/MembershipRenewal.cshtml")
                };

                return returnValue;
            }
        }

        public virtual MvcMailMessage SendCorpAdminMembershipRenewal(IEnumerable<RenewalMember> model)
        {
            using (var db = new IdentityDataContext())
            {
                var emailViewModel = new EmailViewModel()
                {
                    RenewalMembers = model
                };
                var returnValue = new MvcMailMessage
                {
                    ViewName = "MembershipRenewal",
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "Renewal of Membership, Notification For Underwriter.",
                    IsBodyHtml = true,
                    Body = Helper.SetEmailTemplate(emailViewModel, "~/Views/EmailTemplates/CorpAdminMembershipRenewal.cshtml")
                };

                return returnValue;
            }
        }

        #endregion

    }
}