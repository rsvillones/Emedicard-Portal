using Corelib.Enums;
using Corelib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Corelib
{
    public static class LegacyHelper
    {
        #region -- Struct --

        public struct AllMemberJson
        {
            public string id { get; set; }
            public string text { get; set; }
        }

        #endregion

        #region -- Public Functions --

        /// <summary>
        /// Gets all the legacy accounts of the selected agent.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="agentCode">The agent code.</param>
        /// <returns> IQueryable LegacyAccount .</returns>
        public static IQueryable<LegacyAccount> GetLegacyAccounts(LegacyDataContext legacyDb, string agentCode)
        {
            var disposeLegacyDb = false;
            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            var activeStatus = "ASC0000003";

            var returnValue = !String.IsNullOrEmpty(agentCode) ? legacyDb.LegacyAccounts.Where(t => t.AgentCode == agentCode && t.AccountStatusCode == activeStatus).OrderBy(t => t.Name) : new List<LegacyAccount>() as IQueryable<LegacyAccount>;

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the legacy accounts being handled by the current user.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="user">IPrincipal basic functionality of principal object.</param>
        /// <returns>IQueryable&lt;LegacyAccount&gt;.</returns>
        public static IQueryable<LegacyAccount> GetLegacyAccounts(IdentityDataContext db, LegacyDataContext legacyDb, IPrincipal user)
        {
            var disposeDb = false;
            if (db == null)
            {
                db = new IdentityDataContext();
                disposeDb = true;
            }

            var disposeLegacyDb = false;
            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            IQueryable<LegacyAccount> returnValue;

            var activeStatus = "ASC0000003";

            if (user.IsInRole("SysAd") || user.IsInRole("CanAssignAllAccounts"))
            {
                returnValue = legacyDb.LegacyAccounts.Where(t => t.AccountStatusCode == activeStatus).OrderBy(t => t.Name);
            }
            else
            {
                var username = user.Identity.Name;

                var accountCodes = new List<string>();
                var applicationUser = db.Users.FirstOrDefault(t => t.UserName == user.Identity.Name) ?? new ApplicationUser();
                var agentCode = applicationUser.AgentCode;
                if (user != null && !string.IsNullOrEmpty(agentCode))
                {
                    returnValue = legacyDb.LegacyAccounts.Where(t => t.AgentCode == agentCode && t.AccountStatusCode == activeStatus).OrderBy(t => t.Name);
                }
                else
                {
                    accountCodes = db.Accounts.Where(t => t.ApplicationUser.UserName == username).Select(t => t.Code).ToList();
                    returnValue = legacyDb.LegacyAccounts.Where(t => accountCodes.Contains(t.Code)).OrderBy(t => t.Name);
                }
            }

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets all legacy accounts.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <returns>IQueryable LegacyAccount.</returns>
        public static IQueryable<LegacyAccount> GetAllLegacyAccounts(LegacyDataContext legacyDb)
        {
            var disposeLegacyDb = false;
            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            var activeStatus = "ASC0000003";

            var returnValue = legacyDb.LegacyAccounts.Where(t => t.AccountStatusCode == activeStatus).OrderBy(t => t.Name);

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the legacy action memos.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="memberCode">The member code.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="accountCodes">The list of account codes.</param>
        /// <returns>IEnumerable IActionMemo.</returns>
        public static IEnumerable<IActionMemo> GetLegacyActionMemos(IdentityDataContext db, LegacyDataContext legacyDb, string memberCode, string accountCode, IEnumerable<string> accountCodes)
        {
            IEnumerable<IActionMemo> returnValue = null;

            var principalActionMemos = legacyDb.LegacyPrincipalActionMemos as IQueryable<IActionMemo>;
            var dependentActionMemos = legacyDb.LegacyDependentActionMemos as IQueryable<IActionMemo>;

            if (!String.IsNullOrEmpty(memberCode))
            {
                principalActionMemos = principalActionMemos.Where(t => t.PrincipalCode == memberCode);
                dependentActionMemos = dependentActionMemos.Where(t => t.PrincipalCode == memberCode);
            }
            else if (!String.IsNullOrEmpty(accountCode))
            {
                principalActionMemos = principalActionMemos.Where(t => t.AccountCode == accountCode);
                dependentActionMemos = dependentActionMemos.Where(t => t.AccountCode == accountCode);
            }
            else if (accountCodes != null)
            {
                principalActionMemos = principalActionMemos.Where(t => accountCodes.Contains(t.AccountCode));
                dependentActionMemos = dependentActionMemos.Where(t => accountCodes.Contains(t.AccountCode));
            }

            returnValue = principalActionMemos.AsEnumerable().Concat<IActionMemo>(dependentActionMemos);

            returnValue = SetActionMemoWhereCondition(returnValue);

            return returnValue;
        }

        /// <summary>
        /// Gets the legacy action memos.
        /// </summary>
        /// <param name="legacyDb">The legacy database.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <returns>IEnumerable IActionMemo.</returns>
        public static IEnumerable<IActionMemo> GetLegacyActionMemos(LegacyDataContext legacyDb, DateTime dateFrom, DateTime dateTo)
        {
            IEnumerable<IActionMemo> returnValue = null;

            var principalActionMemos = legacyDb.LegacyPrincipalActionMemos.Where(t => t.ActionMemoDate >= dateFrom && t.ActionMemoDate <= dateTo) as IQueryable<IActionMemo>;
            var dependentActionMemos = legacyDb.LegacyDependentActionMemos.Where(t => t.ActionMemoDate >= dateFrom && t.ActionMemoDate <= dateTo) as IQueryable<IActionMemo>;

            var principal = principalActionMemos.ToList();

            returnValue = principalActionMemos.AsEnumerable().Concat<IActionMemo>(dependentActionMemos);

            returnValue = SetActionMemoWhereCondition(returnValue);

            return returnValue;
        }

        /// <summary>
        /// Gets the member processed in legacy system.
        /// </summary>
        /// <param name="legacyDb">The legacy database.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <returns>IEnumerable IMemberProcess.</returns>
        public static IEnumerable<IMemberProcess> GetMemberProcesses(LegacyDataContext legacyDb, DateTime dateFrom, DateTime dateTo)
        {
            IEnumerable<IMemberProcess> returnValue = null;

            var principals = legacyDb.LegacyPrincipalProcesses.Where(t =>
                (t.UpdatedDate >= dateFrom && t.UpdatedDate <= dateTo)
                || (t.AlteredDate >= dateFrom && t.AlteredDate <= dateTo)
                || (t.AmendedDate >= dateFrom && t.AmendedDate <= dateTo)
            );
            var dependents = legacyDb.LegacyDependentProcesses.Where(t =>
                (t.UpdatedDate >= dateFrom && t.UpdatedDate <= dateTo)
                || (t.AlteredDate >= dateFrom && t.AlteredDate <= dateTo)
                || (t.AmendedDate >= dateFrom && t.AmendedDate <= dateTo)
            );

            var p = principals.ToList();
            var d = dependents.ToList();

            returnValue = principals.AsEnumerable().Concat<IMemberProcess>(dependents);

            return returnValue;
        }

        /// <summary>
        /// Gets the active member.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>LegacyMember.</returns>
        public static LegacyMember GetActiveMember(LegacyDataContext legacyDb, string accountCode, string memberCode)
        {
            return GetActiveMembers(legacyDb, accountCode, true).FirstOrDefault(t => t.Code == memberCode && String.IsNullOrEmpty(t.PrincipalMemberCode));
        }

        /// <summary>
        /// Gets the active dependents.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable LegacyMember.</returns>
        public static IEnumerable<LegacyMember> GetActiveDependents(LegacyDataContext legacyDb, string accountCode, string memberCode)
        {
            return GetActiveMembers(legacyDb, accountCode, false, "Dependent").Where(t => t.PrincipalMemberCode == memberCode);
        }

        /// <summary>
        /// Gets the active members.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="all">if set to <c>true</c> [all member].</param>
        /// <param name="type">The type of member either Principal or Dependent.</param>
        /// <returns>IEnumerable&lt;LegacyMember&gt;.</returns>
        public static IEnumerable<LegacyMember> GetActiveMembers(LegacyDataContext legacyDb, string accountCode, bool all = true, string type = "Principal")
        {
            var disposeLegacyDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            // Uncomment code below to return interim data for display editing outside medicard.
            //return new List<LegacyMember>()
            //{
            //    new LegacyMember() { FirstName = "Mark Louie", MiddleName = "Corsiga", LastName = "Medidas", DateOfBirth = new DateTime(1992, 05, 01),
            //        EmailAddress="ecorp1@medicardphils.com", Code = "324142", AppliedPlan=36909, CivilStatus="Single", Gender="Male",
            //        EffectivityDate = new DateTime(2014, 08, 22)
            //    },
            //    new LegacyMember() { FirstName = "Mark Christian", MiddleName = "Tengay", LastName = "Ocana", DateOfBirth = new DateTime(1992, 09, 01),
            //        EmailAddress="ecorp1@medicardphils.com", Code = "324143", AppliedPlan=36909, CivilStatus="Single", Gender="Male",
            //        EffectivityDate = new DateTime(2014, 08, 22)
            //    },
            //    new LegacyMember() { FirstName = "Marc Anniel", MiddleName = "Echada", LastName = "Vino", DateOfBirth = new DateTime(1993, 04, 16),
            //        EmailAddress="ecorp1@medicardphils.com", Code = "324144", AppliedPlan=36909, CivilStatus="Single", Gender="Male",
            //        EffectivityDate = new DateTime(2014, 08, 22)
            //    },  
            //    new LegacyMember() { FirstName = "Mark Joel", MiddleName = "Ducusin", LastName = "Munar", DateOfBirth = new DateTime(1991, 08, 22),
            //        EmailAddress="ecorp1@medicardphils.com", Code = "324145", AppliedPlan=36909, CivilStatus="Single", Gender="Male",
            //        EffectivityDate = new DateTime(2014, 08, 22)
            //    },
            //    new LegacyMember() { FirstName = "Mark Nicko", MiddleName = "Orpilla", LastName = "Porras", DateOfBirth = new DateTime(1992, 07, 28),
            //        EmailAddress="ecorp1@medicardphils.com", Code = "324146", AppliedPlan=36909, CivilStatus="Single", Gender="Male",
            //        EffectivityDate = new DateTime(2014, 08, 22)
            //    }
            //};

            var returnValue = ExecuteSp<LegacyMember>(legacyDb, "emed_active_member_prin_and_dep",
                new List<string>() { "accountCode" },
                new List<object>() { accountCode }
            );

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            if (!all)
            {
                if (type == "Principal")
                {
                    returnValue = returnValue.Where(t => String.IsNullOrEmpty(t.PrincipalMemberCode));
                }
                else
                {
                    returnValue = returnValue.Where(t => !String.IsNullOrEmpty(t.PrincipalMemberCode));
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the resigned members.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="all">if set to <c>true</c> [all members].</param>
        /// <param name="type">The type of member either Principal or Dependent.</param>
        /// <returns>IEnumerable LegacyMember.</returns>
        public static IEnumerable<LegacyMember> GetResignedMembers(LegacyDataContext legacyDb, string accountCode, bool all = true, string type = "Principal")
        {
            var disposeLegacyDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            var returnValue = ExecuteSp<LegacyMember>(legacyDb, "emed_resigned_member_prin_and_dep",
                new List<string>() { "accountCode" },
                new List<object>() { accountCode }
            );

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            if (!all)
            {
                if (type == "Principal")
                {
                    returnValue = returnValue.Where(t => String.IsNullOrEmpty(t.PrincipalMemberCode));
                }
                else
                {
                    returnValue = returnValue.Where(t => !String.IsNullOrEmpty(t.PrincipalMemberCode));
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Gets all members.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="all">if set to <c>true</c> [all Members].</param>
        /// <param name="type">The type of member either Principal or Dependent.</param>
        /// <returns>IEnumerable LegacyMember.</returns>
        public static IEnumerable<LegacyMember> GetAllMembers(LegacyDataContext legacyDb, string accountCode, bool all = true, string type = "Principal")
        {
            var disposeLegacyDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            IEnumerable<LegacyMember> returnValue = GetActiveMembers(legacyDb, accountCode, all, type);
            returnValue = returnValue.Concat(GetResignedMembers(legacyDb, accountCode, all, type));

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            if (!all)
            {
                if (type == "Principal")
                {
                    returnValue = returnValue.Where(t => String.IsNullOrEmpty(t.PrincipalMemberCode));
                }
                else
                {
                    returnValue = returnValue.Where(t => !String.IsNullOrEmpty(t.PrincipalMemberCode));
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Gets all dependents.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable LegacyMember.</returns>
        public static IEnumerable<LegacyMember> GetAllDependents(LegacyDataContext legacyDb, string accountCode, string memberCode)
        {
            return GetAllMembers(legacyDb, accountCode, false, "Dependent").Where(t => t.PrincipalMemberCode == memberCode);
        }
                
        /// <summary>
        /// Gets the utilization.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="memberCode">The member code.</param>
        /// <param name="username">The username.</param>
        /// <returns>IEnumerable LegacyUtilization.</returns>
        public static IEnumerable<LegacyUtilization> GetUtilization(LegacyDataContext legacyDb, string accountCode, string memberCode, string username)
        {
            // Uncomment code below to return interim data for display editing outside medicard.
            //return new List<LegacyUtilization>()
            //{
            //    new LegacyUtilization() { ControlCode = "CALL-LOG", AvailmentDate = new DateTime(2014, 08, 22), Diagnosis = "non-specific abd colic", Remarks = "", Hospital = "ST. LUKE`S MEDICAL CENTER - GLOBAL CITY", ApprovedAmount = 0 },
            //    new LegacyUtilization() { ControlCode = "MS256436", AvailmentDate = new DateTime(2014, 08, 18), Diagnosis = "GASTROESOPHAGEAL REFLUX, NOS", Remarks = "", Hospital = "", ApprovedAmount = 4320.91M },
            //    new LegacyUtilization() { ControlCode = "OP672567", AvailmentDate = new DateTime(2014, 07, 09), Diagnosis = "HYPERTENSION", Remarks = "", Hospital = "", ApprovedAmount = 0 },
            //    new LegacyUtilization() { ControlCode = "OP672567", AvailmentDate = new DateTime(2014, 07, 09), Diagnosis = "PRESBYOPIA", Remarks = "", Hospital = "", ApprovedAmount = 350 },
            //    new LegacyUtilization() { ControlCode = "CALL-LOG", AvailmentDate = new DateTime(2014, 08, 22), Diagnosis = "non-specific abd colic", Remarks = "", Hospital = "ST. LUKE`S MEDICAL CENTER - GLOBAL CITY", ApprovedAmount = 0 },
            //    new LegacyUtilization() { ControlCode = "MS256436", AvailmentDate = new DateTime(2014, 08, 18), Diagnosis = "GASTROESOPHAGEAL REFLUX, NOS", Remarks = "", Hospital = "", ApprovedAmount = 4320.91M },
            //    new LegacyUtilization() { ControlCode = "OP672567", AvailmentDate = new DateTime(2014, 07, 09), Diagnosis = "HYPERTENSION", Remarks = "", Hospital = "", ApprovedAmount = 0 },
            //    new LegacyUtilization() { ControlCode = "OP672567", AvailmentDate = new DateTime(2014, 07, 09), Diagnosis = "PRESBYOPIA", Remarks = "", Hospital = "", ApprovedAmount = 350 },
            //    new LegacyUtilization() { ControlCode = "CALL-LOG", AvailmentDate = new DateTime(2014, 08, 22), Diagnosis = "non-specific abd colic", Remarks = "", Hospital = "ST. LUKE`S MEDICAL CENTER - GLOBAL CITY", ApprovedAmount = 0 },
            //    new LegacyUtilization() { ControlCode = "MS256436", AvailmentDate = new DateTime(2014, 08, 18), Diagnosis = "GASTROESOPHAGEAL REFLUX, NOS", Remarks = "", Hospital = "", ApprovedAmount = 4320.91M },
            //    new LegacyUtilization() { ControlCode = "OP672567", AvailmentDate = new DateTime(2014, 07, 09), Diagnosis = "HYPERTENSION", Remarks = "", Hospital = "", ApprovedAmount = 0 },
            //    new LegacyUtilization() { ControlCode = "OP672567", AvailmentDate = new DateTime(2014, 07, 09), Diagnosis = "PRESBYOPIA", Remarks = "", Hospital = "", ApprovedAmount = 350 },
            //};

            var disposeLegacyDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            var legacyMember = GetActiveMember(legacyDb, accountCode, memberCode);
            if (legacyMember == null) return new List<LegacyUtilization>();


            IEnumerable<LegacyUtilization> returnValue = null;
            using (var udb = new UtilizationDataContext())
            {
                returnValue = ExecuteSp<LegacyUtilization>(udb, "CP_CLMS_REP_UTILIZATION_ALL",
                    new List<string>() { "DATE_FR", "DATE_TO", "MEMCODE", "LNAME", "FNAME", "MI", "COMP", "USER" },
                    new List<object>() { legacyMember.EffectivityDate ?? DateTime.Now.AddYears(-1), legacyMember.ValidityDate ?? DateTime.Now.AddMonths(1), legacyMember.Code, legacyMember.LastName, legacyMember.FirstName, legacyMember.MiddleName, accountCode, username }
                );
            }


            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;

        }

        /// <summary>
        /// Gets the reimbursement.
        /// </summary>
        /// <param name="legacyDb">The legacy database.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;LegacyReimbursement&gt;.</returns>
        public static IEnumerable<LegacyReimbursement> GetReimbursement(LegacyDataContext legacyDb, string accountCode, string memberCode)
        {
            var disposeLegacyDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            var legacyMember = GetActiveMember(legacyDb, accountCode, memberCode);
            if (legacyMember == null) return new List<LegacyReimbursement>();

            IEnumerable<LegacyReimbursement> returnValue = null;
            using (var udb = new ReimbursementDataContext())
            {
                returnValue = ExecuteSp<LegacyReimbursement>(udb, "REIM_Get_Reimbursement_Status_EMED",
                    new List<string>() { "member_code" },
                    new List<object>() { legacyMember.Code },
                    2
                );
            }

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;

        }

        /// <summary>
        /// Gets the reimbursement disapproved.
        /// </summary>
        /// <param name="controlCode">The control code.</param>
        /// <returns>IEnumerable ReimbursementDisapproved.</returns>
        public static IEnumerable<ReimbursementDisapproved> GetReimbursementDisapproved(string controlCode)
        {
            IEnumerable<ReimbursementDisapproved> returnValue = null;
            using (var udb = new ReimbursementDataContext())
            {
                returnValue = ExecuteSp<ReimbursementDisapproved>(udb, "Reim_Get_Reimb_Details_EMED_Disapproved",
                    new List<string>() { "control_code" },
                    new List<object>() { controlCode }
                );
            }
            return returnValue;
        }

        /// <summary>
        /// Gets the reimbursement with lacking document.
        /// </summary>
        /// <param name="controlCode">The control code.</param>
        /// <returns>IEnumerable ReimbursementWithLackingDocument.</returns>
        public static IEnumerable<ReimbursementWithLackingDocument> GetReimbursementWithLackingDocument(string controlCode)
        {
            IEnumerable<ReimbursementWithLackingDocument> returnValue = null;
            using (var udb = new ReimbursementDataContext())
            {
                returnValue = ExecuteSp<ReimbursementWithLackingDocument>(udb, "Reim_Get_Reimb_Details_EMED_lack_documents",
                    new List<string>() { "control_code" },
                    new List<object>() { controlCode }
                );
            }
            return returnValue;
        }

        /// <summary>
        /// Gets the reimbursement waiting for hospital bill.
        /// </summary>
        /// <param name="controlCode">The control code.</param>
        /// <returns>IEnumerable ReimbursementWaitingForHospitalBill .</returns>
        public static IEnumerable<ReimbursementWaitingForHospitalBill> GetReimbursementWaitingForHospitalBill(string controlCode)
        {
            IEnumerable<ReimbursementWaitingForHospitalBill> returnValue = null;
            using (var udb = new ReimbursementDataContext())
            {
                returnValue = ExecuteSp<ReimbursementWaitingForHospitalBill>(udb, "Reim_Get_Reimb_Details_EMED_WaitingHB",
                    new List<string>() { "control_code" },
                    new List<object>() { controlCode }
                );
            }
            return returnValue;
        }

        /// <summary>
        /// Gets the reimbursement detail.
        /// </summary>
        /// <param name="controlCode">The control code.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable ReimbursementDetail .</returns>
        public static IEnumerable<ReimbursementDetail> GetReimbursementDetail(string controlCode, string memberCode)
        {
            IEnumerable<ReimbursementDetail> returnValue = null;
            using (var udb = new ReimbursementDataContext())
            {
                returnValue = ExecuteSp<ReimbursementDetail>(udb, "Reim_Get_Reimb_Details_EMED",
                    new List<string>() { "control_code", "member_code" },
                    new List<object>() { controlCode, memberCode }
                );
            }
            return returnValue;
        }

        /// <summary>
        /// Gets the legacy room rates.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <param name="principal">if set to <c>true</c> [principal].</param>
        /// <returns>IEnumerable LegacyRoomRate.</returns>
        public static IEnumerable<LegacyRoomRate> GetLegacyRoomRates(string accountCode, bool principal)
        {
            var paymentMode = (principal ? 0 : 1);
            using (var legacyDb = new LegacyDataContext())
            {
                var account = legacyDb.LegacyAccounts.FirstOrDefault(t => t.Code == accountCode) ?? new LegacyAccount();

                if (principal)
                {
                    return legacyDb.LegacyRoomRates
                        .Include(t => t.LegacyPlan)
                        .Include(t => t.LegacyPaymode)
                        .Where(t => (t.AccountCode == account.Code || t.AccountCode == account.MotherCode) &&
                            t.Selected && (t.LegacyPaymode.Id == 0 || t.LegacyPaymode.Id == 5 || t.LegacyPaymode.Id == 8))
                        .ToList();
                }
                else
                {
                    return legacyDb.LegacyRoomRates
                        .Include(t => t.LegacyPlan)
                        .Include(t => t.LegacyPaymode)
                        .Where(t => (t.AccountCode == account.Code || t.AccountCode == account.MotherCode) &&
                            t.Selected && (t.LegacyPaymode.Id == 1 || t.LegacyPaymode.Id == 2 || t.LegacyPaymode.Id == 5 || t.LegacyPaymode.Id == 8 || t.LegacyPaymode.Id == 9))
                        .ToList();
                }
            }
        }

        /// <summary>
        /// Gets the legacy area.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable LegacyArea.</returns>
        public static IEnumerable<LegacyArea> GetLegacyArea(string accountCode)
        {
            using (var legacyDb = new LegacyDataContext())
            {
                return legacyDb.LegacyAreas.Where(t => t.AccountCode == accountCode).ToList();
            }
        }

        /// <summary>
        /// Gets the legacy active principals.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable LegacyPrincipalProcess.</returns>
        public static IEnumerable<LegacyPrincipalProcess> GetLegacyActivePrincipals(LegacyDataContext legacyDb, string accountCode)
        {
            var disposeLegacyDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            var returnValue = legacyDb.LegacyPrincipalProcesses
                .Where(t => t.AccountCode == accountCode && Constants.ACTIVE_OSTAT_CODES.Contains(t.StatCode)
                    && t.ValidityDate >= DateTime.Now).ToList();

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the legacy active dependents.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable LegacyDependentProcess.</returns>
        public static IEnumerable<LegacyDependentProcess> GetLegacyActiveDependents(LegacyDataContext legacyDb, string accountCode)
        {
            var disposeLegacyDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            var returnValue = legacyDb.LegacyDependentProcesses
                .Where(t => t.AccountCode == accountCode && Constants.ACTIVE_OSTAT_CODES.Contains(t.StatCode)
                    && t.ValidityDate >= DateTime.Now).ToList();

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the legacy active principal dependents.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable LegacyDependentProcess.</returns>
        public static IEnumerable<LegacyDependentProcess> GetLegacyActivePrincipalDependents(LegacyDataContext legacyDb, string accountCode, string memberCode)
        {
            var disposeLegacyDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            var returnValue = legacyDb.LegacyDependentProcesses
                .Where(t => t.AccountCode == accountCode && Constants.ACTIVE_OSTAT_CODES.Contains(t.StatCode)
                    && t.ValidityDate >= DateTime.Now && t.PrincipalCode == memberCode).ToList();

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets all legacy principal processes.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable LegacyPrincipalProcess.</returns>
        public static IEnumerable<LegacyPrincipalProcess> GetAllLegacyPrincipalProcesses(LegacyDataContext legacyDb, string accountCode)
        {
            var disposeLegacyDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            var returnValue = legacyDb.LegacyPrincipalProcesses
                    .Where(t => t.AccountCode == accountCode &&
                        ((Constants.ACTIVE_OSTAT_CODES.Contains(t.StatCode) && t.ValidityDate >= DateTime.Now) ||
                        t.StatCode == Constants.RESIGNED_OSTAT_CODE_PRINCIPAL))
                    .ToList();

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets all legacy dependent processes.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable LegacyDependentProcess.</returns>
        public static IEnumerable<LegacyDependentProcess> GetAllLegacyDependentProcesses(LegacyDataContext legacyDb, string accountCode)
        {
            var disposeLegacyDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            var returnValue = legacyDb.LegacyDependentProcesses
                        .Where(t => t.AccountCode == accountCode &&
                            ((Constants.ACTIVE_OSTAT_CODES.Contains(t.StatCode)
                            && t.ValidityDate >= DateTime.Now) ||
                            t.StatCode == Constants.RESIGNED_OSTAT_CODE_PRINCIPAL))
                         .ToList();

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets all legacy principal dependent processes.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable LegacyDependentProcess.</returns>
        public static IEnumerable<LegacyDependentProcess> GetAllLegacyPrincipalDependentProcesses(LegacyDataContext legacyDb, string accountCode, string memberCode)
        {
            var disposeLegacyDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            var returnValue = legacyDb.LegacyDependentProcesses
                        .Where(t => t.AccountCode == accountCode && t.PrincipalCode == memberCode &&
                            ((Constants.ACTIVE_OSTAT_CODES.Contains(t.StatCode)
                            && t.ValidityDate >= DateTime.Now) ||
                            t.StatCode == Constants.RESIGNED_OSTAT_CODE_PRINCIPAL))
                         .ToList();

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the name and id of the member.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable AllMemberJson.</returns>
        public static IEnumerable<AllMemberJson> GetMemberIdName(LegacyDataContext legacyDb, string accountCode)
        {
            var disposeLegacyDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            var returnValue = new List<AllMemberJson>();

            var allPrincipals = GetAllLegacyPrincipalProcesses(legacyDb, accountCode);

            var principalCodes = allPrincipals.Select(t => t.PrincipalCode).ToList();

            var allDependents = legacyDb.LegacyDependentProcesses
                    .Where(t => t.AccountCode == accountCode &&
                        ((Constants.ACTIVE_OSTAT_CODES.Contains(t.StatCode)
                        && t.ValidityDate >= DateTime.Now) ||
                        t.StatCode == Constants.RESIGNED_OSTAT_CODE_PRINCIPAL))
                    .ToList();

            var principalIdName = allPrincipals.Select(t => new { t.PrincipalCode, t.FullName }).ToList();
            var dependentIdName = allDependents.Select(t => new { t.DependentCode, t.FullName }).ToList();

            foreach (var idname in principalIdName)
            {
                returnValue.Add(new AllMemberJson()
                {
                    id = idname.PrincipalCode,
                    text = idname.FullName
                });
            }

            foreach (var idname in dependentIdName)
            {
                returnValue.Add(new AllMemberJson()
                {
                    id = idname.DependentCode,
                    text = idname.FullName
                });
            }

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Determines whether [is legacy active principal] [the specified legacy database].
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="code">The member's code.</param>
        /// <param name="firstName">The member's first name.</param>
        /// <param name="middleName">The member's middle name.</param>
        /// <param name="lastName">The member's last name.</param>
        /// <param name="dateOfBirth">The member's date of birth.</param>
        /// <returns><c>true</c> if [is legacy active principal] [the specified legacy database]; otherwise, <c>false</c>.</returns>
        public static bool IsLegacyActivePrincipal(LegacyDataContext legacyDb, string accountCode, string code, string firstName, string middleName, string lastName, DateTime dateOfBirth)
        {
            var disposeLegacyDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }
            bool returnValue;
            if (!string.IsNullOrEmpty(code))
            {
                returnValue = legacyDb.LegacyPrincipalProcesses
                               .Any(t => t.AccountCode == accountCode && Constants.ACTIVE_OSTAT_CODES.Contains(t.StatCode)
                                   && t.ValidityDate >= DateTime.Now && t.PrincipalCode == code &&
                                   t.FirstName.ToLower() == firstName.ToLower() && t.LastName.ToLower() == lastName.ToLower() && t.DateOfBirth == dateOfBirth &&
                                   ((!string.IsNullOrEmpty(t.MiddleName) && !string.IsNullOrEmpty(middleName) && t.MiddleName.ToLower() == middleName.ToLower()) || (string.IsNullOrEmpty(t.MiddleName) && string.IsNullOrEmpty(middleName)))
                                   );
            }
            else
            {
                returnValue = legacyDb.LegacyPrincipalProcesses
                               .Any(t => t.AccountCode == accountCode && Constants.ACTIVE_OSTAT_CODES.Contains(t.StatCode)
                                   && t.ValidityDate >= DateTime.Now &&
                                   t.FirstName.ToLower() == firstName.ToLower() && t.LastName.ToLower() == lastName.ToLower() && t.DateOfBirth == dateOfBirth &&
                                   ((!string.IsNullOrEmpty(t.MiddleName) && !string.IsNullOrEmpty(middleName) && t.MiddleName.ToLower() == middleName.ToLower()) || (string.IsNullOrEmpty(t.MiddleName) && string.IsNullOrEmpty(middleName)))
                                   );
            }

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Checks if associate member.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="applicationUserId">The application user id.</param>
        /// <param name="memberCode">The member code.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool CheckAssociateMember(IdentityDataContext db, string applicationUserId, string memberCode, out string errorMessage)
        {
            var disposeDb = false;

            if (db == null)
            {
                db = new IdentityDataContext();
                disposeDb = true;
            }
            errorMessage = "";
            using (var legacyDb = new LegacyDataContext())
            {
                if (!string.IsNullOrEmpty(memberCode))
                {
                    var activeMember = legacyDb.LegacyPrincipalProcesses.FirstOrDefault(t => Constants.ACTIVE_OSTAT_CODES.Contains(t.StatCode)
                        && t.ValidityDate >= DateTime.Now && t.PrincipalCode == memberCode);

                    if (activeMember == null)
                    {
                        errorMessage = "Member code does not exist in active member subscriptions";
                        return false;
                    }

                    var accounts = db.Accounts.Include(t => t.ApplicationUser).Where(t => t.ApplicationUser.Id == applicationUserId).ToList();

                    if (accounts == null || !accounts.Any(t => t.Code == activeMember.AccountCode))
                    {
                        errorMessage = "Member's Account Code does not match with the Account to associate with";
                        return false;
                    }
                }
            }

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return true;
        }

        /// <summary>
        /// Gets the assigned processor.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>assigned processor domain account</returns>
        public static string GetAssignedProcessor(string accountCode)
        {
            using (var legacyDb = new LegacyDataContext())
            {
                var legacyAccount = legacyDb.LegacyAccounts.FirstOrDefault(t => t.Code == accountCode);
                return legacyAccount != null ? legacyAccount.AssignedUser : "";
            }
        }

        #endregion

        #region -- Reports --

        /// <summary>
        /// Gets the report legacy active principals.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable LegacyActivePrincipal.</returns>
        public static IEnumerable<LegacyActivePrincipal> GetReportLegacyActivePrincipals(LegacyDataContext legacyDb, string accountCode)
        {
            var disposeDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeDb = true;
            }

            var returnValue = ExecuteSp<LegacyActivePrincipal>(legacyDb, "emed_active_members",
                new List<string>() { "accountCode" },
                new List<object>() { accountCode }
                );


            if (disposeDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the report legacy active dependents.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable LegacyActiveDependent.</returns>
        public static IEnumerable<LegacyActiveDependent> GetReportLegacyActiveDependents(LegacyDataContext legacyDb, string accountCode)
        {
            var disposeDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeDb = true;
            }

            var returnValue = ExecuteSp<LegacyActiveDependent>(legacyDb, "emed_active_member_dependent",
                new List<string>() { "accountCode" },
                new List<object>() { accountCode }
                );

            if (disposeDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the report legacy resigned principals.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable LegacyResignedPrincipal.</returns>
        public static IEnumerable<LegacyResignedPrincipal> GetReportLegacyResignedPrincipals(LegacyDataContext legacyDb, string accountCode)
        {
            var disposeDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeDb = true;
            }

            var returnValue = ExecuteSp<LegacyResignedPrincipal>(legacyDb, "emed_resigned_members_principal",
                new List<string>() { "accountCode" },
                new List<object>() { accountCode }
                );


            if (disposeDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the report legacy resigned dependents.
        /// </summary>
        /// <param name="legacyDb">The legacy database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable LegacyResignedDependent.</returns>
        public static IEnumerable<LegacyResignedDependent> GetReportLegacyResignedDependents(LegacyDataContext legacyDb, string accountCode)
        {
            var disposeDb = false;

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeDb = true;
            }

            var returnValue = ExecuteSp<LegacyResignedDependent>(legacyDb, "emed_resigned_members_dependent",
                new List<string>() { "accountCode" },
                new List<object>() { accountCode }
                );

            if (disposeDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }
        
        #endregion

        #region -- Private Functions --

        /// <summary>
        /// Executes the stored procedure and retrieve the data.
        /// </summary>
        /// <typeparam name="T">generic class</typeparam>
        /// <param name="db">The database context.</param>
        /// <param name="sp">The name of stored procedure.</param>
        /// <param name="parameterNames">The store procedure parameter names.</param>
        /// <param name="parameterValues">The store procedure parameter values.</param>
        /// <param name="readerIndex">reads the index/table from the stored procedure.</param>
        /// <returns>IEnumerable of the generic class.</returns>
        private static IEnumerable<T> ExecuteSp<T>(DbContext db, string sp, List<string> parameterNames, List<object> parameterValues, int readerIndex = 0) where T : new()
        {
            var returnValue = new List<T>();

            var connection = db.Database.Connection as SqlConnection;
            using (var command = new SqlCommand(sp, connection))
            {
                connection.Open();
                command.CommandType = CommandType.StoredProcedure;

                for (var index = 0; index <= parameterNames.Count - 1; index++)
                {
                    command.Parameters.Add(new SqlParameter(parameterNames[index], parameterValues[index]));
                }

                using (var reader = command.ExecuteReader())
                {
                    for (int index = 0; index < readerIndex; index++)
                    {
                        if (!reader.NextResult()) return returnValue;
                    }

                    MapSpResult<T>(returnValue, reader);

                    reader.Close();
                }
                connection.Close();
            }

            return returnValue;
        }

        /// <summary>
        /// Maps the result of stored procedure.
        /// </summary>
        /// <typeparam name="T">generic class</typeparam>
        /// <param name="list">The list of generic class.</param>
        /// <param name="reader">The sql data reader.</param>
        private static void MapSpResult<T>(List<T> list, SqlDataReader reader) where T : new()
        {
            while (reader.Read())
            {
                var entity = new T();
                foreach (var property in entity.GetType().GetProperties())
                {
                    string columnName;
                    var attribute = property.CustomAttributes.FirstOrDefault(t => t.AttributeType == typeof(ColumnAttribute));
                    if (attribute != null)
                    {
                        columnName = attribute.ConstructorArguments[0].Value.ToString();
                    }
                    else
                    {
                        columnName = property.Name;
                    }

                    if (HasColumn(reader, columnName))
                    {
                        if (reader[columnName].GetType() != typeof(DBNull)) property.SetValue(entity, reader[columnName]);
                    }
                }
                list.Add(entity);
            }

        }

        /// <summary>
        /// Determines whether the specified dr has column.
        /// </summary>
        /// <param name="dr">The data reader.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns><c>true</c> if the specified dr has column; otherwise, <c>false</c>.</returns>
        public static bool HasColumn(IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase)) return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the type of the action memo.
        /// </summary>
        /// <param name="actionMemo">The action memo interface.</param>
        /// <returns>ActionMemoType.</returns>
        public static ActionMemoType GetActionMemoType(IActionMemo actionMemo)
        {
            var resignAmendmentDate = DateTime.Now.AddDays(Constants.RESIGNED_AMENDMENT_DAYS);
            var resignResignedDate = DateTime.Now.AddDays(Constants.RESIGNED_RESIGNED_DAYS);
            if (
                Constants.RESIGNED_STAT_CODES.Contains(actionMemo.StatCode)
                &&
                !String.IsNullOrEmpty(actionMemo.StatusRemarks)
                &&
                (
                    actionMemo.StatusRemarks.ToUpper().Contains("LETTER")
                    || actionMemo.StatusRemarks.ToUpper().Contains("RCV")
                    || actionMemo.StatusRemarks.ToUpper().Contains("ATTACHMENT")
                    || actionMemo.StatusRemarks.ToUpper().Contains("PRE-TERMINATION")
                )
                &&
                (
                    actionMemo.AmmendmentDate >= resignAmendmentDate
                    || actionMemo.ResignedDate >= resignAmendmentDate
                )
            ) return ActionMemoType.Resigned;
            else if (
                !String.IsNullOrEmpty(actionMemo.ActionMemoRemarks)
                && !String.IsNullOrEmpty(actionMemo.ActionMemoRemarks.Trim())
                && !actionMemo.ActionMemoRemarks.ToUpper().Contains("CANCEL")
                &&
                (
                    (Constants.DISAPPROVED_STAT_CODES_CONTRACT_YEAR.Contains(actionMemo.StatCode) && actionMemo.ActionMemoRemarks.Contains("Contract Year"))
                    || (actionMemo.StatCode == 1 && actionMemo.Posted == 1)
                    || (actionMemo.StatCode == 9 &&
                        (
                            actionMemo.ActionMemoRemarks.ToUpper().Contains("CHANGE")
                            || actionMemo.ActionMemoRemarks.ToUpper().Contains("OVER")
                            || actionMemo.ActionMemoRemarks.ToUpper().Contains("NO EXTENDED")
                            || actionMemo.ActionMemoRemarks.ToUpper().Contains("NOT")
                        )
                    )
                )
            ) return ActionMemoType.Disapproved;
            else if (
                actionMemo.StatCode == 2
                && actionMemo.Posted == 1
            ) return ActionMemoType.Encoding;
            else if (
                actionMemo.StatCode == 3
                && actionMemo.Posted == 1
            ) return ActionMemoType.MedicalExamination;
            else if (
                (
                    (
                        actionMemo.StatCode == 1
                        && actionMemo.Posted == 1
                    )
                    || actionMemo.StatCode == 9
                )
                && actionMemo.ActionMemoRemarks.ToUpper().Contains("CANCEL")
            ) return ActionMemoType.Cancellation;
            else if (
                (
                    actionMemo.StatCode == 9
                    || actionMemo.StatCode == 0
                )
                && actionMemo.ActionMemoRemarks.ToUpper().Contains("AUTOMATICALLY DELETED")
                &&
                (
                    actionMemo.AmmendmentDate >= resignAmendmentDate
                    || actionMemo.ResignedDate >= resignAmendmentDate
                )
            ) return ActionMemoType.ResignedAutomatically;
            else if (
                actionMemo.StatCode == 4
                && (
                    !actionMemo.ActionMemoRemarks.ToUpper().Contains("LESS THAN")
                    || actionMemo.ActionMemoRemarks.ToUpper().Contains("LESS THAN 10")
                    || actionMemo.ActionMemoRemarks.ToUpper().Contains("LESS THAN 5")
                )
                && !String.IsNullOrEmpty(actionMemo.ActionMemoRemarks)
                && !String.IsNullOrEmpty(actionMemo.ActionMemoRemarks.Trim())
                && String.IsNullOrEmpty(actionMemo.PrincipalCode)
            ) return ActionMemoType.OnHoldOthers;
            else if (
                actionMemo.StatCode == 4
                && (
                    !actionMemo.ActionMemoRemarks.ToUpper().Contains("LESS THAN")
                    || actionMemo.ActionMemoRemarks.ToUpper().Contains("LESS THAN 10")
                    || actionMemo.ActionMemoRemarks.ToUpper().Contains("LESS THAN 5")
                )
                && !String.IsNullOrEmpty(actionMemo.ActionMemoRemarks)
                && !String.IsNullOrEmpty(actionMemo.ActionMemoRemarks.Trim())
                && !String.IsNullOrEmpty(actionMemo.PrincipalCode)
            ) return ActionMemoType.OnHoldActive;
            else if (
                actionMemo.StatCode == 9
                && !String.IsNullOrEmpty(actionMemo.ActionMemoRemarks)
                && actionMemo.ActionMemoRemarks.ToUpper().Contains("AUTOMATIC")
                &&
                (
                    actionMemo.AmmendmentDate >= resignAmendmentDate
                    || actionMemo.ResignedDate >= resignAmendmentDate
                )
            ) return ActionMemoType.Overage;

            return ActionMemoType.Undefined;
        }

        /// <summary>
        /// Sets the action memo where condition.
        /// </summary>
        /// <param name="query">The ienumerable of action memo interface.</param>
        /// <returns>IEnumerable IActionMemo.</returns>
        private static IEnumerable<IActionMemo> SetActionMemoWhereCondition(IEnumerable<IActionMemo> query)
        {
            var resignAmendmentDate = DateTime.Now.AddDays(Constants.RESIGNED_AMENDMENT_DAYS);
            var resignResignedDate = DateTime.Now.AddDays(Constants.RESIGNED_RESIGNED_DAYS);

            var returnValue = query.Where(t =>
                // Resigned Condition
                (
                    Constants.RESIGNED_STAT_CODES.Contains(t.StatCode)
                    &&
                    !String.IsNullOrEmpty(t.StatusRemarks)
                    &&
                    (
                        t.StatusRemarks.Contains("LETTER")
                        || t.StatusRemarks.Contains("RCV")
                        || t.StatusRemarks.Contains("ATTACHMENT")
                        || t.StatusRemarks.Contains("PRE-TERMINATION")
                    )
                    &&
                    (
                        t.AmmendmentDate >= resignAmendmentDate
                        || t.ResignedDate >= resignAmendmentDate
                    )
                )
                ||
                    // Disapproved Condition
                (
                    !String.IsNullOrEmpty(t.ActionMemoRemarks)
                    && !String.IsNullOrEmpty(t.ActionMemoRemarks.Trim())
                    && !t.ActionMemoRemarks.Contains("CANCEL")
                    &&
                    (
                        (Constants.DISAPPROVED_STAT_CODES_CONTRACT_YEAR.Contains(t.StatCode) && t.ActionMemoRemarks.Contains("Contract Year"))
                        || (t.StatCode == 1 && t.Posted == 1)
                        || (t.StatCode == 9 &&
                            (
                                t.ActionMemoRemarks.Contains("CHANGE")
                                || t.ActionMemoRemarks.Contains("OVER")
                                || t.ActionMemoRemarks.Contains("NO EXTENDED")
                                || t.ActionMemoRemarks.Contains("NOT")
                            )
                        )
                    )
                )
                ||
                    // Encoding Condition
                (
                    t.StatCode == 2
                    && t.Posted == 1
                )
                ||
                    // Medical Examination Condition
                (
                    t.StatCode == 3
                    && t.Posted == 1
                )
                ||
                    // Cancellation Condition
                (
                    (
                        (
                            t.StatCode == 1
                            && t.Posted == 1
                        )
                        || t.StatCode == 9
                    )
                    && !String.IsNullOrEmpty(t.ActionMemoRemarks)
                    && t.ActionMemoRemarks.Contains("CANCEL")
                )
                ||
                    // Resigned Automatically
                (
                    (
                        t.StatCode == 9
                        || t.StatCode == 9
                    )
                    && !String.IsNullOrEmpty(t.ActionMemoRemarks)
                    && t.ActionMemoRemarks.Contains("AUTOMATICALLY DELETED")
                    &&
                    (
                        t.AmmendmentDate >= resignAmendmentDate
                        || t.ResignedDate >= resignAmendmentDate
                    )
                )
                ||
                    // On Hold Others
                (
                    t.StatCode == 4
                    && !String.IsNullOrEmpty(t.ActionMemoRemarks)
                    && !String.IsNullOrEmpty(t.ActionMemoRemarks.Trim())
                    && (
                            !t.ActionMemoRemarks.Contains("LESS THAN")
                            || t.ActionMemoRemarks.Contains("LESS THAN 10")
                            || t.ActionMemoRemarks.Contains("LESS THAN 5")
                    )
                    && String.IsNullOrEmpty(t.PrincipalCode)
                )
                ||
                    // On Hold Active
                (
                    t.StatCode == 4
                    && !String.IsNullOrEmpty(t.ActionMemoRemarks)
                    && !String.IsNullOrEmpty(t.ActionMemoRemarks.Trim())
                    && (
                            !t.ActionMemoRemarks.Contains("LESS THAN")
                            || t.ActionMemoRemarks.Contains("LESS THAN 10")
                            || t.ActionMemoRemarks.Contains("LESS THAN 5")
                    )
                    && !String.IsNullOrEmpty(t.PrincipalCode)
                )
                ||
                    // Overage
                (
                    t.StatCode == 9
                    && !String.IsNullOrEmpty(t.ActionMemoRemarks)
                    && t.ActionMemoRemarks.Contains("AUTOMATIC")
                    &&
                    (
                        t.AmmendmentDate >= resignAmendmentDate
                        || t.ResignedDate >= resignAmendmentDate
                    )
                )
            );

            return returnValue;
        }

        #endregion
    }
}
