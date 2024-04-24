using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Corelib.Models;
using System.Data.Entity;
using Corelib.Enums;

namespace Corelib
{
    public static class Config
    {
        #region -- Properties --

        /// <summary>
        /// Gets the connection string of the eMediCard database.
        /// </summary>
        /// <value>The connection string.</value>
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Medicard.eCorporateSystem"].ConnectionString; }
        }

        /// <summary>
        /// Gets the legacy connection string of the URG database.
        /// </summary>
        /// <value>The legacy connection string.</value>
        public static string LegacyConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Medicard.LegacySystem"].ConnectionString; }
        }

        /// <summary>
        /// Gets the utilization connection string of the Utilization database.
        /// </summary>
        /// <value>The utilization connection string.</value>
        public static string UtilizationConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Medicard.UtilizationSystem"].ConnectionString; }
        }

        /// <summary>
        /// Gets the username of the user who currently login in the system.
        /// </summary>
        /// <value>User Name</value>
        public static string CurrentUser
        {
            get
            {
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User != null)
                {
                    return System.Web.HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    return "anonymous";
                }
            }
        }

        /// <summary>
        ///  Gets the record count per page.
        /// </summary>
        /// <value>The record count per page.</value>
        public static int RecordCountPerPage
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["RecordCountPerPage"]);
            }
        }

        /// <summary>
        /// Gets the items per page.
        /// </summary>
        /// <value>The items per page.</value>
        public static int ItemsPerPage
        {
            get
            {
                return Convert.ToInt32(ConfigurationManager.AppSettings["ItemsPerPage"]);
            }
        }

        /// <summary>
        /// Gets the username of the user who currently login in the system.
        /// </summary>
        /// <param name="contextBase">contains HTTP-specific information about an individual HTTP Request.</param>
        /// <returns>System.String.</returns>
        public static string GetCurrentUser(HttpContextBase contextBase)
        {
            var context = contextBase != null ? contextBase.ApplicationInstance.Context : null;
            if (context != null && context.User != null)
            {
                return System.Web.HttpContext.Current.User.Identity.Name;
            }
            else
            {
                return "anonymous";
            }
        }

        /// <summary>
        /// Gets the email address for send email notifications.
        /// </summary>
        /// <value>Sender's email address.</value>
        public static string NotificationFromEmail
        {
            get { return ConfigurationManager.AppSettings["NotificationFromEmail"]; }
        }

        /// <summary>
        /// Gets the domain of medicard.
        /// </summary>
        /// <value>Medicard's domain.</value>
        public static string Domain
        {
            get { return ConfigurationManager.AppSettings["Domain"]; }
        }

        /// <summary>
        /// Gets medicard's domain path.
        /// </summary>
        /// <value>Medicard's domain path.</value>
        public static string DomainPath
        {
            get { return ConfigurationManager.AppSettings["DomainPath"]; }
        }

        /// <summary>
        /// Gets a value indicating whether [automatic login].
        /// </summary>
        /// <value><c>true</c> if [automatic login]; otherwise, <c>false</c>.</value>
        public static bool AutoLogin
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["AutoLogin"]); }
        }

        /// <summary>
        /// Gets the email domain.
        /// </summary>
        /// <value>The email domain.</value>
        public static string EmailDomain
        {
            get { return ConfigurationManager.AppSettings["EmailDomain"]; }
        }

        /// <summary>
        /// Maps the properties and transfer the values from one class to the other class.
        /// </summary>
        /// <param name="source">The class source.</param>
        /// <param name="destination">The class destination.</param>
        /// <param name="mapId">if set to <c>true</c> [map identifier].</param>
        /// <param name="propertiesToExclude">The properties to exclude.</param>
        public static void MapProperties(object source, object destination, bool mapId = true, params string[] propertiesToExclude)
        {
            foreach (var pi in destination.GetType().GetProperties())
            {
                if (!pi.CanWrite || source.GetType().GetProperty(pi.Name) == null || (!mapId && pi.Name == "Id") || (propertiesToExclude != null && propertiesToExclude.Contains(pi.Name)))
                    continue;

                destination.GetType().GetProperty(pi.Name).SetValue(destination, source.GetType().GetProperty(pi.Name).GetValue(source));
            }
        }

        /// <summary>
        /// Gets the benefits and exclusions URL.
        /// </summary>
        /// <value>The benefits and exclusions URL.</value>
        public static string BenefitsAndExclusionsUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["BenefitsAndExclusionsUrl"];
            }
        }

        /// <summary>
        /// Gets the benefits and exclusions base URL.
        /// </summary>
        /// <value>The benefits and exclusions base URL.</value>
        public static string BenefitsAndExclusionsBaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["BenefitsAndExclusionsBaseUrl"];
            }
        }

        /// <summary>
        /// Gets the benefits and exclusions CSS path.
        /// </summary>
        /// <value>The benefits and exclusions CSS path.</value>
        public static string BenefitsAndExclusionsCssPath
        {
            get
            {
                return ConfigurationManager.AppSettings["BenefitsAndExclusionsCssPath"];
            }
        }

        /// <summary>
        /// Gets the action memo documents path.
        /// </summary>
        /// <value>The action memo documents path.</value>
        public static string ActionMemoDocumentsPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ActionMemoDocumentsPath"];
            }
        }

        /// <summary>
        /// Gets the transmittal documents path.
        /// </summary>
        /// <value>The transmittal documents path.</value>
        public static string TransmittalDocumentsPath
        {
            get
            {
                return ConfigurationManager.AppSettings["TransmittalDocumentsPath"];
            }
        }

        /// <summary>
        /// Gets the urg action memo template path.
        /// </summary>
        /// <value>The urg action memo template path.</value>
        public static string UrgActionMemoTemplatePath
        {
            get
            {
                return ConfigurationManager.AppSettings["UrgActionMemoTemplatePath"];
            }
        }

        /// <summary>
        /// Gets the urg action memo to member template path.
        /// </summary>
        /// <value>The urg action memo to member template path.</value>
        public static string UrgActionMemoToMemberTemplatePath
        {
            get
            {
                return ConfigurationManager.AppSettings["UrgActionMemoToMemberTemplatePath"];
            }
        }

        /// <summary>
        /// Gets the uploaded files path.
        /// </summary>
        /// <value>The uploaded files path.</value>
        public static string UploadedFilesPath
        {
            get
            {
                return ConfigurationManager.AppSettings["UploadedFilesPath"];
            }
        }

        /// <summary>
        /// Gets the check action memo minutes.
        /// </summary>
        /// <value>The check action memo minutes.</value>
        public static int CheckActionMemoMinutes
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["CheckActionMemoMinutes"]);
            }
        }

        /// <summary>
        /// Gets the check new corporate accounts minutes.
        /// </summary>
        /// <value>The check new corporate accounts minutes.</value>
        public static int CheckNewCorporateAccountsMinutes
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["CheckNewCorporateAccountsMinutes"]);
            }
        }

        /// <summary>
        /// Gets the send email notification time.
        /// </summary>
        /// <value>The send email notification time.</value>
        public static string SendEmailNotificationTime
        {
            get
            {
                return ConfigurationManager.AppSettings["SendEmailNotificationTime"];
            }
        }

        /// <summary>
        /// Gets the send email notification before deadline.
        /// </summary>
        /// <value>The send email notification before deadline.</value>
        public static double SendEmailNotificationBeforeDeadline
        {
            get
            {
                return Convert.ToDouble(ConfigurationManager.AppSettings["SendEmailNotificationBeforeDeadline"]);
            }
        }

        /// <summary>
        /// Gets the check member process minutes.
        /// </summary>
        /// <value>The check member process minutes.</value>
        public static int CheckMemberProcessMinutes
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["CheckMemberProcessMinutes"]);
            }
        }

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        public static string BaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["BaseUrl"];
            }
        }

        /// <summary>
        /// Gets the action memo due date days.
        /// </summary>
        /// <value>The action memo due date days.</value>
        public static int ActionMemoDueDateDays
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["ActionMemoDueDateDays"]);
            }
        }

        /// <summary>
        /// Gets the password expiration days.
        /// </summary>
        /// <value>The password expiration days.</value>
        public static int PasswordExpirationDays
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["PasswordExpirationDays"]);
            }
        }

        /// <summary>
        /// Gets the cookie expire time span.
        /// </summary>
        /// <value>The cookie expire time span.</value>
        public static int CookieExpireTimeSpan
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["CookieExpireTimeSpan"]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether [cookie sliding expiration].
        /// </summary>
        /// <value><c>true</c> if [cookie sliding expiration]; otherwise, <c>false</c>.</value>
        public static bool CookieSlidingExpiration
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["CookieSlidingExpiration"]);
            }
        }

        #endregion

        #region -- Email Template Paths --

        /// <summary>
        /// Gets the SMTP server.
        /// </summary>
        /// <value>The SMTP server.</value>
        public static string SmtpServer
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpServer"];
            }
        }

        /// <summary>
        /// Gets the SMTP username.
        /// </summary>
        /// <value>The SMTP username.</value>
        public static string SmtpUsername
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpUsername"];
            }
        }

        /// <summary>
        /// Gets the SMTP password.
        /// </summary>
        /// <value>The SMTP password.</value>
        public static string SmtpPassword
        {
            get
            {
                return ConfigurationManager.AppSettings["SmtpPassword"];
            }
        }

        /// <summary>
        /// Gets the SMTP port.
        /// </summary>
        /// <value>The SMTP port.</value>
        public static int SmtpPort
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            }
        }

        /// <summary>
        /// Gets the email layout.
        /// </summary>
        /// <value>The email layout.</value>
        public static string EmailLayout
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailLayout"];
            }
        }

        /// <summary>
        /// Gets the new access email template.
        /// </summary>
        /// <value>The new access email template.</value>
        public static string NewAccess
        {
            get
            {
                return ConfigurationManager.AppSettings["NewAccess"];
            }
        }

        /// <summary>
        /// Gets the process new endorsement batch by corporate admin to corporate admin email template.
        /// </summary>
        /// <value>The process new endorsement batch by corporate admin to corporate admin email template.</value>
        public static string ProcessNewEndorsementBatchByCorporateAdminToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["ProcessNewEndorsementBatchByCorporateAdminToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the process new directly to urg by corporate admin to urg email template.
        /// </summary>
        /// <value>The process new directly to urg by corporate admin to urg email template.</value>
        public static string ProcessNewDirectlyToUrgByCorporateAdminToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["ProcessNewDirectlyToUrgByCorporateAdminToUrg"];
            }
        }

        /// <summary>
        /// Gets the process new endorsement batch by corporate admin to member email template.
        /// </summary>
        /// <value>The process new endorsement batch by corporate admin to member email template.</value>
        public static string ProcessNewEndorsementBatchByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["ProcessNewEndorsementBatchByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the submit new member by member to corporate admin email template.
        /// </summary>
        /// <value>The submit new member by member to corporate admin email template.</value>
        public static string SubmitNewMemberByMemberToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitNewMemberByMemberToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the submit new member by member to member email template.
        /// </summary>
        /// <value>The submit new member by member to member email template.</value>
        public static string SubmitNewMemberByMemberToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitNewMemberByMemberToMember"];
            }
        }

        /// <summary>
        /// Gets the submit new member by member to urg email template.
        /// </summary>
        /// <value>The submit new member by member to urg email template.</value>
        public static string SubmitNewMemberByMemberToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitNewMemberByMemberToUrg"];
            }
        }

        /// <summary>
        /// Gets the approve new member by urg to corporate admin email template.
        /// </summary>
        /// <value>The approve new member by urg to corporate admin email template.</value>
        public static string ApproveNewMemberByUrgToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["ApproveNewMemberByUrgToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the approve new member by urg to member email template.
        /// </summary>
        /// <value>The approve new member by urg to member email template.</value>
        public static string ApproveNewMemberByUrgToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["ApproveNewMemberByUrgToMember"];
            }
        }

        /// <summary>
        /// Gets the action memo by urg to corporate admin email template.
        /// </summary>
        /// <value>The action memo by urg to corporate admin email template.</value>
        public static string ActionMemoByUrgToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["ActionMemoByUrgToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the action memo for cancellation by urg to corporate admin email template.
        /// </summary>
        /// <value>The action memo for cancellation by urg to corporate admin email template.</value>
        public static string ActionMemoForCancellationByUrgToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["ActionMemoForCancellationByUrgToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the action memo by urg to member email template.
        /// </summary>
        /// <value>The action memo by urg to member email template.</value>
        public static string ActionMemoByUrgToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["ActionMemoByUrgToMember"];
            }
        }

        /// <summary>
        /// Gets the action memo reply by member to member email template.
        /// </summary>
        /// <value>The action memo reply by member to member email template.</value>
        public static string ActionMemoReplyByMemberToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["ActionMemoReplyByMemberToMember"];
            }
        }

        /// <summary>
        /// Gets the action memo reply by corporate admin to corporate admin email template.
        /// </summary>
        /// <value>The action memo reply by corporate admin to corporate admin email template.</value>
        public static string ActionMemoReplyByCorporateAdminToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["ActionMemoReplyByCorporateAdminToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the action memo reply by member to corporate admin email template.
        /// </summary>
        /// <value>The action memo reply by member to corporate admin email template.</value>
        public static string ActionMemoReplyByMemberToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["ActionMemoReplyByMemberToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the action memo reply by member to urg email template.
        /// </summary>
        /// <value>The action memo reply by member to urg email template.</value>
        public static string ActionMemoReplyByMemberToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["ActionMemoReplyByMemberToUrg"];
            }
        }

        /// <summary>
        /// Gets the action memo reply by corporate admin to member email template.
        /// </summary>
        /// <value>The action memo reply by corporate admin to member email template.</value>
        public static string ActionMemoReplyByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["ActionMemoReplyByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the action memo reply by corporate admin to urg email template.
        /// </summary>
        /// <value>The action memo reply by corporate admin to urg email template.</value>
        public static string ActionMemoReplyByCorporateAdminToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["ActionMemoReplyByCorporateAdminToUrg"];
            }
        }

        /// <summary>
        /// Gets the transmittal by urg to corporate admin email template.
        /// </summary>
        /// <value>The transmittal by urg to corporate admin email template.</value>
        public static string TransmittalByUrgToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["TransmittalByUrgToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the transmittal by urg to member email template.
        /// </summary>
        /// <value>The transmittal by urg to member email template.</value>
        public static string TransmittalByUrgToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["TransmittalByUrgToMember"];
            }
        }

        /// <summary>
        /// Gets the process renewal by corporate admin to corporate admin email template.
        /// </summary>
        /// <value>The process renewal by corporate admin to corporate admin email template.</value>
        public static string ProcessRenewalByCorporateAdminToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["ProcessRenewalByCorporateAdminToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the process renewal by corporate admin to member email template.
        /// </summary>
        /// <value>The process renewal by corporate admin to member email template.</value>
        public static string ProcessRenewalByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["ProcessRenewalByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the process renewal by corporate admin to urg email template.
        /// </summary>
        /// <value>The process renewal by corporate admin to urg email template.</value>
        public static string ProcessRenewalByCorporateAdminToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["ProcessRenewalByCorporateAdminToUrg"];
            }
        }

        /// <summary>
        /// Gets the process cancelled member by corporate admin to urg email template.
        /// </summary>
        /// <value>The process cancelled member by corporate admin to urg email template.</value>
        public static string ProcessCancelledMemberByCorporateAdminToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["ProcessCancelledMemberByCorporateAdminToUrg"];
            }
        }

        /// <summary>
        /// Gets the process cancelled member by corporate admin to corporate admin email template.
        /// </summary>
        /// <value>The process cancelled member by corporate admin to corporate admin email template.</value>
        public static string ProcessCancelledMemberByCorporateAdminToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["ProcessCancelledMemberByCorporateAdminToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the submittion deadline notification email template.
        /// </summary>
        /// <value>The submittion deadline notification email template.</value>
        public static string SubmittionDeadlineNotification
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmittionDeadlineNotification"];
            }
        }

        /// <summary>
        /// Gets the created user account to corporate admin email template.
        /// </summary>
        /// <value>The created user account to corporate admin email template.</value>
        public static string CreatedUserAccountToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["CreatedUserAccountToCorporateAdmin"];
            }
        }
        
        #region -- ID Replacements --

        /// <summary>
        /// Gets the submit identifier replacement by member to member email template.
        /// </summary>
        /// <value>The submit identifier replacement by member to member email template.</value>
        public static string SubmitIdReplacementByMemberToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitIdReplacementByMemberToMember"];
            }
        }

        /// <summary>
        /// Gets the submit identifier replacement by member to corporate admin email template.
        /// </summary>
        /// <value>The submit identifier replacement by member to corporate admin email template.</value>
        public static string SubmitIdReplacementByMemberToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitIdReplacementByMemberToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the submit identifier replacement by member to urg email template.
        /// </summary>
        /// <value>The submit identifier replacement by member to urg email template.</value>
        public static string SubmitIdReplacementByMemberToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitIdReplacementByMemberToUrg"];
            }
        }

        /// <summary>
        /// Gets the submit identifier replacement by corporate admin to member email template.
        /// </summary>
        /// <value>The submit identifier replacement by corporate admin to member email template.</value>
        public static string SubmitIdReplacementByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitIdReplacementByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the submit identifier replacement by corporate admin to urg email template.
        /// </summary>
        /// <value>The submit identifier replacement by corporate admin to urg email template.</value>
        public static string SubmitIdReplacementByCorporateAdminToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitIdReplacementByCorporateAdminToUrg"];
            }
        }

        /// <summary>
        /// Gets the cancelled identifier replacement by corporate admin to member email template.
        /// </summary>
        /// <value>The cancelled identifier replacement by corporate admin to member email template.</value>
        public static string CancelledIdReplacementByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["CancelledIdReplacementByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the disapprove identifier replacement by corporate admin to member email template.
        /// </summary>
        /// <value>The disapprove identifier replacement by corporate admin to member email template.</value>
        public static string DisapproveIdReplacementByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["DisapproveIdReplacementByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the disapprove identifier replacement by urg to corporate admin email template.
        /// </summary>
        /// <value>The disapprove identifier replacement by urg to corporate admin email template.</value>
        public static string DisapproveIdReplacementByUrgToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["DisapproveIdReplacementByUrgToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the disapprove identifier replacement by urg to member email template.
        /// </summary>
        /// <value>The disapprove identifier replacement by urg to member email template.</value>
        public static string DisapproveIdReplacementByUrgToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["DisapproveIdReplacementByUrgToMember"];
            }
        }

        #endregion

        #region -- Amendments --

        /// <summary>
        /// Gets the submit amendment by member to member email template.
        /// </summary>
        /// <value>The submit amendment by member to member email template.</value>
        public static string SubmitAmendmentByMemberToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitAmendmentByMemberToMember"];
            }
        }

        /// <summary>
        /// Gets the submit amendment by member to corporate admin email template.
        /// </summary>
        /// <value>The submit amendment by member to corporate admin email template.</value>
        public static string SubmitAmendmentByMemberToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitAmendmentByMemberToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the submit amendment by member to urg email template.
        /// </summary>
        /// <value>The submit amendment by member to urg email template.</value>
        public static string SubmitAmendmentByMemberToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitAmendmentByMemberToUrg"];
            }
        }

        /// <summary>
        /// Gets the submit amendment by corporate admin to member email template.
        /// </summary>
        /// <value>The submit amendment by corporate admin to member email template.</value>
        public static string SubmitAmendmentByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitAmendmentByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the submit amendment by corporate admin to urg email template.
        /// </summary>
        /// <value>The submit amendment by corporate admin to urg email template.</value>
        public static string SubmitAmendmentByCorporateAdminToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitAmendmentByCorporateAdminToUrg"];
            }
        }

        /// <summary>
        /// Gets the cancelled amendment by corporate admin to member email template.
        /// </summary>
        /// <value>The cancelled amendment by corporate admin to member email template.</value>
        public static string CancelledAmendmentByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["CancelledAmendmentByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the disapprove amendment by corporate admin to member email template.
        /// </summary>
        /// <value>The disapprove amendment by corporate admin to member email template.</value>
        public static string DisapproveAmendmentByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["DisapproveAmendmentByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the declined amendment by urg to corporate admin email template.
        /// </summary>
        /// <value>The declined amendment by urg to corporate admin email template.</value>
        public static string DeclinedAmendmentByUrgToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["DeclinedAmendmentByUrgToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the declined amendment by urg to member email template.
        /// </summary>
        /// <value>The declined amendment by urg to member email template.</value>
        public static string DeclinedAmendmentByUrgToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["DeclinedAmendmentByUrgToMember"];
            }
        }

        #endregion

        #region -- Additional Dependent --

        /// <summary>
        /// Gets the submit additional dependent by member to member email template.
        /// </summary>
        /// <value>The submit additional dependent by member to member email template.</value>
        public static string SubmitAdditionalDependentByMemberToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitAdditionalDependentByMemberToMember"];
            }
        }

        /// <summary>
        /// Gets the submit additional dependent by member to corporate admin email template.
        /// </summary>
        /// <value>The submit additional dependent by member to corporate admin email template.</value>
        public static string SubmitAdditionalDependentByMemberToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitAdditionalDependentByMemberToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the submit additional dependent by member to urg email template.
        /// </summary>
        /// <value>The submit additional dependent by member to urg email template.</value>
        public static string SubmitAdditionalDependentByMemberToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitAdditionalDependentByMemberToUrg"];
            }
        }

        /// <summary>
        /// Gets the submit additional dependent by corporate admin to member email template.
        /// </summary>
        /// <value>The submit additional dependent by corporate admin to member email template.</value>
        public static string SubmitAdditionalDependentByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitAdditionalDependentByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the submit additional dependent by corporate admin to urg email template.
        /// </summary>
        /// <value>The submit additional dependent by corporate admin to urg email template.</value>
        public static string SubmitAdditionalDependentByCorporateAdminToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitAdditionalDependentByCorporateAdminToUrg"];
            }
        }

        /// <summary>
        /// Gets the cancelled additional dependent by corporate admin to member email template.
        /// </summary>
        /// <value>The cancelled additional dependent by corporate admin to member email template.</value>
        public static string CancelledAdditionalDependentByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["CancelledAdditionalDependentByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the disapprove additional dependent by corporate admin to member email template.
        /// </summary>
        /// <value>The disapprove additional dependent by corporate admin to member email template.</value>
        public static string DisapproveAdditionalDependentByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["DisapproveAdditionalDependentByCorporateAdminToMember"];
            }
        }

        #endregion

        #region -- Dependent Cancellation --

        /// <summary>
        /// Gets the submit dependent cancellation by member to member email template.
        /// </summary>
        /// <value>The submit dependent cancellation by member to member email template.</value>
        public static string SubmitDependentCancellationByMemberToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitDependentCancellationByMemberToMember"];
            }
        }

        /// <summary>
        /// Gets the submit dependent cancellation by member to corporate admin email template.
        /// </summary>
        /// <value>The submit dependent cancellation by member to corporate admin email template.</value>
        public static string SubmitDependentCancellationByMemberToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitDependentCancellationByMemberToCorporateAdmin"];
            }
        }

        /// <summary>
        /// Gets the submit dependent cancellation by member to urg email template.
        /// </summary>
        /// <value>The submit dependent cancellation by member to urg email template.</value>
        public static string SubmitDependentCancellationByMemberToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitDependentCancellationByMemberToUrg"];
            }
        }

        /// <summary>
        /// Gets the submit dependent cancellation by corporate admin to member email template.
        /// </summary>
        /// <value>The submit dependent cancellation by corporate admin to member email template.</value>
        public static string SubmitDependentCancellationByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitDependentCancellationByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the submit dependent cancellation by corporate admin to urg email template.
        /// </summary>
        /// <value>The submit dependent cancellation by corporate admin to urg email template.</value>
        public static string SubmitDependentCancellationByCorporateAdminToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["SubmitDependentCancellationByCorporateAdminToUrg"];
            }
        }

        /// <summary>
        /// Gets the cancelled dependent cancellation by corporate admin to member email template.
        /// </summary>
        /// <value>The cancelled dependent cancellation by corporate admin to member email template.</value>
        public static string CancelledDependentCancellationByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["CancelledDependentCancellationByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the disapprove dependent cancellation by corporate admin to member email template.
        /// </summary>
        /// <value>The disapprove dependent cancellation by corporate admin to member email template.</value>
        public static string DisapproveDependentCancellationByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["DisapproveDependentCancellationByCorporateAdminToMember"];
            }
        }

        #endregion

        #region -- Endorsement Listing --

        /// <summary>
        /// Gets the approve membership by corporate admin to member email template.
        /// </summary>
        /// <value>The approve membership by corporate admin to member email template.</value>
        public static string ApproveMembershipByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["ApproveMembershipByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the cancel membership by corporate admin to member email template.
        /// </summary>
        /// <value>The cancel membership by corporate admin to member email template.</value>
        public static string CancelMembershipByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["CancelMembershipByCorporateAdminToMember"];
            }
        }

        /// <summary>
        /// Gets the return membership by corporate admin to member email template.
        /// </summary>
        /// <value>The return membership by corporate admin to member email template.</value>
        public static string ReturnMembershipByCorporateAdminToMember
        {
            get
            {
                return ConfigurationManager.AppSettings["ReturnMembershipByCorporateAdminToMember"];
            }
        }

        #endregion

        #region -- Receiving Entry --

        /// <summary>
        /// Gets the receive entry urg to urg email template.
        /// </summary>
        /// <value>The receive entry urg to urg email template.</value>
        public static string ReceiveEntryUrgToUrg
        {
            get
            {
                return ConfigurationManager.AppSettings["ReceiveEntryUrgToUrg"];
            }
        }

        /// <summary>
        /// Gets the receive entry urg to corporate admin email template.
        /// </summary>
        /// <value>The receive entry urg to corporate admin email template.</value>
        public static string ReceiveEntryUrgToCorporateAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["ReceiveEntryUrgToCorporateAdmin"];
            }
        }

        #endregion

        #endregion

        #region -- Functions --
        
        /// <summary>
        /// Determines whether the amendment is simple amend.
        /// </summary>
        /// <param name="requestDataType">Type of the request data.</param>
        /// <returns><c>true</c> if [is simple amend] [the specified request data type]; otherwise, <c>false</c>.</returns>
        public static bool IsSimpleAmend(RequestDataType requestDataType)
        {
            return requestDataType == RequestDataType.LastName || requestDataType == RequestDataType.FirstName || requestDataType == RequestDataType.MiddleName ||
                requestDataType == RequestDataType.DateOfBirth || requestDataType == RequestDataType.Gender || requestDataType == RequestDataType.Address ||
                requestDataType == RequestDataType.Telephone;
        }

        /// <summary>
        /// Gets the name of the company.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>System.String.</returns>
        public static string GetCompanyName(string accountCode)
        {
            using (var legacyDb = new LegacyDataContext())
            {
                var account = legacyDb.LegacyAccounts.FirstOrDefault(t => t.Code == accountCode);
                return account != null ? account.Name : "";
            }
        }

        #endregion
    }
}
