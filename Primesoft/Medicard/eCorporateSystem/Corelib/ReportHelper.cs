using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corelib.Models;
using System.Data;

namespace Corelib
{
    public class ReportHelper
    {
        #region -- Active Members --

        /// <summary>
        /// Get active principal members.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable&lt;emed_active_membersResult&gt;.</returns>
        public static IEnumerable<emed_active_membersResult> ActivePrincipalMembers(string accountCode)
        {
            using (var db = new StoredProceduresDataContext(Config.LegacyConnectionString))
            {
                return db.emed_active_members(accountCode).ToList();
            }
        }

        /// <summary>
        /// Get Active dependent members.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable&lt;emed_active_member_dependentResult&gt;.</returns>
        public static IEnumerable<emed_active_member_dependentResult> ActiveDependentMembers(string accountCode)
        {
            using (var db = new StoredProceduresDataContext(Config.LegacyConnectionString))
            {
                return db.emed_active_member_dependent(accountCode).ToList();
            }
        }

        /// <summary>
        /// Get active dependents.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable&lt;LegacyActiveDependent&gt;.</returns>
        public static IEnumerable<LegacyActiveDependent> LegacyActiveDependents(string accountCode)
        {
            using (var db = new LegacyDataContext())
            {
                return LegacyHelper.GetReportLegacyActiveDependents(db, accountCode);
            }
        }

        /// <summary>
        /// Get active principals.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable&lt;LegacyActivePrincipal&gt;.</returns>
        public static IEnumerable<LegacyActivePrincipal> LegacyActivePrincipals(string accountCode)
        {
            using (var db = new LegacyDataContext())
            {
                return LegacyHelper.GetReportLegacyActivePrincipals(db, accountCode);
            }
        }

        #endregion

        #region -- Resigned Members --

        /// <summary>
        /// Get resigned principal members.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable&lt;emed_resigned_members_principalResult&gt;.</returns>
        public static IEnumerable<emed_resigned_members_principalResult> ResignedMembersPrincipal(string accountCode)
        {
            using (var db = new StoredProceduresDataContext(Config.LegacyConnectionString))
            {
                return db.emed_resigned_members_principal(accountCode).ToList();
            }
        }

        /// <summary>
        /// Get resigned dependent members.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable&lt;emed_resigned_members_dependentResult&gt;.</returns>
        public static IEnumerable<emed_resigned_members_dependentResult> ResignedMembersDependent(string accountCode)
        {
            using (var db = new StoredProceduresDataContext(Config.LegacyConnectionString))
            {
                return db.emed_resigned_members_dependent(accountCode).ToList();
            }
        }

        /// <summary>
        /// Get resigned dependent members.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable&lt;LegacyResignedDependent&gt;.</returns>
        public static IEnumerable<LegacyResignedDependent> LegacyResignedDependents(string accountCode)
        {
            using (var db = new LegacyDataContext())
            {
                return LegacyHelper.GetReportLegacyResignedDependents(db, accountCode);
            }
        }

        /// <summary>
        /// Get resigned principal members.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable&lt;LegacyResignedPrincipal&gt;.</returns>
        public static IEnumerable<LegacyResignedPrincipal> LegacyResignedPrincipals(string accountCode)
        {
            using (var db = new LegacyDataContext())
            {
                return LegacyHelper.GetReportLegacyResignedPrincipals(db, accountCode);
            }
        }


        #endregion

        #region -- Utilization Reporting --

        #region -- Per Member All Services --

        /// <summary>
        /// Get Utilizations report member all service.
        /// </summary>
        /// <param name="memberCode">The member code.</param>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable&lt;UtilizationMemberAllService&gt;.</returns>
        public static IEnumerable<UtilizationMemberAllService> UtilizationReportMemberAllService(string memberCode,string accountCode)
        {
            using (var db = new UtilizationDataContext())
            {
                return UtilizationHelper.GetUtilizationMemberAllServices(db, memberCode, accountCode);
            }
        }

        #endregion

        #region -- In-Patient --

        /// <summary>
        /// Get utilization report in-patient.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;UtilizationInPatient&gt;.</returns>
        public static IEnumerable<UtilizationInPatient> UtilizationReportInPatient(string accountCode, DateTime startDate, DateTime endDate, string memberCode)
        {
            using (var db = new UtilizationDataContext())
            {
                return UtilizationHelper.GetUtilizationInPatients(db, accountCode, startDate, endDate, memberCode);
            }
        }

        #endregion

        #region -- Out-Patient --

        /// <summary>
        /// Get utilization report out-patient.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;UtilizationOutPatient&gt;.</returns>
        public static IEnumerable<UtilizationOutPatient> UtilizationReportOutPatient(string accountCode, DateTime startDate, DateTime endDate, string memberCode)
        {
            using (var db = new UtilizationDataContext())
            {
                return UtilizationHelper.GetUtilizationOutPatients(db, accountCode, startDate, endDate, memberCode);
            }
        }

        #endregion

        #region -- Medical Services --

        /// <summary>
        /// Get utilization report medical service.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;UtilizationMedicalService&gt;.</returns>
        public static IEnumerable<UtilizationMedicalService> UtilizationReportMedicalService(string accountCode, DateTime startDate, DateTime endDate, string memberCode)
        {
            using (var db = new UtilizationDataContext())
            {
                return UtilizationHelper.GetUtilizationMedicalServices(db, accountCode, startDate, endDate, memberCode);
            }
        }

        #endregion

        #region -- Reimbursement --

        /// <summary>
        /// Get utilization report reimbursement.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;UtilizationReimbursement&gt;.</returns>
        public static IEnumerable<UtilizationReimbursement> UtilizationReportReimbursement(string service, string accountCode, DateTime startDate, DateTime endDate, string memberCode)
        {
            using (var db = new UtilizationDataContext())
            {
                return UtilizationHelper.GetUtilizationReimbursements(db,service, accountCode, startDate, endDate, memberCode);
            }
        }
                
        #endregion

        #region -- Dental --

        /// <summary>
        /// Get utilization report dental.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;UtilizationDental&gt;.</returns>
        public static IEnumerable<UtilizationDental> UtilizationReportDental(string accountCode, DateTime startDate, DateTime endDate, string memberCode)
        {
            using (var db = new UtilizationDataContext())
            {
                return UtilizationHelper.GetUtilizationDentals(db, accountCode, startDate, endDate, memberCode);
            }
        }

        #endregion

        #region -- Report by not yet billed (Out Patient / Medical Services)

        /// <summary>
        /// Get utilization report unbilled report out-patient / medical sevice.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;UtilizationUnBilledReportOutPatientMedSevice&gt;.</returns>
        public static IEnumerable<UtilizationUnBilledReportOutPatientMedSevice> UtilizationReportUnBilledReportOutPatientMedSevice(string accountCode, DateTime startDate, DateTime endDate, string memberCode)
        {
            using (var db = new UtilizationDataContext())
            {
                return UtilizationHelper.GetUtilizationUnBilledReportOutPatientMedSevices(db, accountCode, startDate, endDate, memberCode);
            }
        }

        #endregion

        #region -- Report by not yet billed (In-Patient)

        /// <summary>
        /// Get tilization report unbilled report in-patient.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;UtilizationUnBilledReportInPatient&gt;.</returns>
        public static IEnumerable<UtilizationUnBilledReportInPatient> UtilizationReportUnBilledReportInPatient(string accountCode, DateTime startDate, DateTime endDate, string memberCode)
        {
            using (var db = new UtilizationDataContext())
            {
                return UtilizationHelper.GetUtilizationUnBilledReportInPatients(db, accountCode, startDate, endDate, memberCode);
            }
        }

        #endregion

        #endregion

    }
}
