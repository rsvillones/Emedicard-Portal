using Corelib.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Corelib
{
    public static class UtilizationHelper
    {
        #region -- Public Functions --

        /// <summary>
        /// Gets the utilization dentals.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;UtilizationDental&gt;.</returns>
        public static IEnumerable<UtilizationDental> GetUtilizationDentals(UtilizationDataContext db, string accountCode, DateTime? startDate, DateTime? endDate, string memberCode)
        {
            var disposeDb = false;

            if (db == null)
            {
                db = new UtilizationDataContext();
                disposeDb = true;
            }

            var startDateString = ParameterDateTime(startDate);
            var endDateString = !string.IsNullOrEmpty(startDateString) ? ParameterDateTime(endDate) : "";
            if (string.IsNullOrEmpty(startDateString) && string.IsNullOrEmpty(endDateString))
            {
                if (disposeDb)
                {
                    db.Dispose();
                    db = null;
                }
                return new List<UtilizationDental>();
            }
            IEnumerable<UtilizationDental> returnValue = new List<UtilizationDental>();

            if (!string.IsNullOrEmpty(memberCode))
            {
                returnValue = ExecuteSp<UtilizationDental>(db, "SP_Intra_ActiveAccount_Utilization_ByMemCode_DT",
                    new List<string>() { "ComCode", "startDate", "endDate", "Member_Code" },
                    new List<object>() { accountCode, startDateString, endDateString, memberCode }
                );
            }
            else
            {
                returnValue = ExecuteSp<UtilizationDental>(db, "SP_Intra_ActiveAccount_Utilization_DT",
                       new List<string>() { "ComCode", "startDate", "endDate", "lastName" },
                       new List<object>() { accountCode, startDateString, endDateString, "" }
                   );
            }

            var memberCodeExceptions = GetMemberCodeExceptions(accountCode);
            var planIdExceptions = GetPlanIdExceptions(accountCode);

            returnValue = returnValue.Where(t => !memberCodeExceptions.Contains(t.AccountNumber));
            returnValue = returnValue.Where(t => !planIdExceptions.Contains(t.AppliedPlan));

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the utilization medical services.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;UtilizationMedicalService&gt;.</returns>
        public static IEnumerable<UtilizationMedicalService> GetUtilizationMedicalServices(UtilizationDataContext db, string accountCode, DateTime? startDate, DateTime? endDate, string memberCode)
        {
            var disposeDb = false;

            if (db == null)
            {
                db = new UtilizationDataContext();
                disposeDb = true;
            }

            var startDateString = ParameterDateTime(startDate);
            var endDateString = !string.IsNullOrEmpty(startDateString) ? ParameterDateTime(endDate) : "";
            if (string.IsNullOrEmpty(startDateString) && string.IsNullOrEmpty(endDateString))
            {
                if (disposeDb)
                {
                    db.Dispose();
                    db = null;
                }
                return new List<UtilizationMedicalService>();
            }

            IEnumerable<UtilizationMedicalService> returnValue = new List<UtilizationMedicalService>();

            if (!string.IsNullOrEmpty(memberCode))
            {
                returnValue = ExecuteSp<UtilizationMedicalService>(db, "SP_Intra_ActiveAccount_Utilization_ByMemCode_ER",
                    new List<string>() { "ComCode", "startDate", "endDate", "Member_Code" },
                    new List<object>() { accountCode, startDateString, endDateString, memberCode }
                    );

            }
            else
            {
                returnValue = ExecuteSp<UtilizationMedicalService>(db, "SP_Intra_ActiveAccount_Utilization_ER",
                    new List<string>() { "ComCode", "startDate", "endDate", "lastName" },
                    new List<object>() { accountCode, startDateString, endDateString, "" }
                    );
            }

            var memberCodeExceptions = GetMemberCodeExceptions(accountCode);
            var planIdExceptions = GetPlanIdExceptions(accountCode);

            returnValue = returnValue.Where(t => !memberCodeExceptions.Contains(t.AccountNumber));
            returnValue = returnValue.Where(t => !planIdExceptions.Contains(t.AppliedPlan));

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the utilization out patients.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;UtilizationOutPatient&gt;.</returns>
        public static IEnumerable<UtilizationOutPatient> GetUtilizationOutPatients(UtilizationDataContext db, string accountCode, DateTime? startDate, DateTime? endDate, string memberCode)
        {
            var disposeDb = false;

            if (db == null)
            {
                db = new UtilizationDataContext();
                disposeDb = true;
            }
            var startDateString = ParameterDateTime(startDate);
            var endDateString = !string.IsNullOrEmpty(startDateString) ? ParameterDateTime(endDate) : "";
            if (string.IsNullOrEmpty(startDateString) && string.IsNullOrEmpty(endDateString))
            {
                if (disposeDb)
                {
                    db.Dispose();
                    db = null;
                }
                return new List<UtilizationOutPatient>();
            }
            IEnumerable<UtilizationOutPatient> returnValue = new List<UtilizationOutPatient>();
            if (!string.IsNullOrEmpty(memberCode))
            {
                returnValue = ExecuteSp<UtilizationOutPatient>(db, "SP_Intra_ActiveAccount_Utilization_ByMemCode_OP",
                    new List<string>() { "ComCode", "startDate", "endDate", "Member_Code" },
                    new List<object>() { accountCode, startDateString, endDateString, memberCode }
                    );
            }
            else
            {
                returnValue = ExecuteSp<UtilizationOutPatient>(db, "SP_Intra_ActiveAccount_Utilization_OP",
                    new List<string>() { "ComCode", "startDate", "endDate", "lastName" },
                    new List<object>() { accountCode, startDateString, endDateString, "" }
                    );
            }

            var memberCodeExceptions = GetMemberCodeExceptions(accountCode);
            var planIdExceptions = GetPlanIdExceptions(accountCode);

            returnValue = returnValue.Where(t => !memberCodeExceptions.Contains(t.AccountNumber));
            returnValue = returnValue.Where(t => !planIdExceptions.Contains(t.AppliedPlan));

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the utilization in patients.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;UtilizationInPatient&gt;.</returns>
        public static IEnumerable<UtilizationInPatient> GetUtilizationInPatients(UtilizationDataContext db, string accountCode, DateTime? startDate, DateTime? endDate, string memberCode)
        {
            var disposeDb = false;

            if (db == null)
            {
                db = new UtilizationDataContext();
                disposeDb = true;
            }

            var startDateString = ParameterDateTime(startDate);
            var endDateString = !string.IsNullOrEmpty(startDateString) ? ParameterDateTime(endDate) : "";
            if (string.IsNullOrEmpty(startDateString) && string.IsNullOrEmpty(endDateString))
            {
                if (disposeDb)
                {
                    db.Dispose();
                    db = null;
                }
                return new List<UtilizationInPatient>();
            }

            IEnumerable<UtilizationInPatient> returnValue = new List<UtilizationInPatient>();

            if (!string.IsNullOrEmpty(memberCode))
            {
                returnValue = ExecuteSp<UtilizationInPatient>(db, "SP_Intra_ActiveAccount_Utilization_ByMemCode_IP",
                           new List<string>() { "ComCode", "startDate", "endDate", "Member_Code" },
                           new List<object>() { accountCode, startDateString, endDateString, memberCode }
                       );
            }
            else
            {
                returnValue = ExecuteSp<UtilizationInPatient>(db, "SP_Intra_ActiveAccount_Utilization_IP",
                           new List<string>() { "ComCode", "startDate", "endDate", "lastName" },
                           new List<object>() { accountCode, startDateString, endDateString, "" }
                       );
            }

            var memberCodeExceptions = GetMemberCodeExceptions(accountCode);
            var planIdExceptions = GetPlanIdExceptions(accountCode);

            returnValue = returnValue.Where(t => !memberCodeExceptions.Contains(t.AccountNumber));
            returnValue = returnValue.Where(t => !planIdExceptions.Contains(t.AppliedPlan));

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the utilization member all services.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="memberCode">The member code.</param>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable&lt;UtilizationMemberAllService&gt;.</returns>
        public static IEnumerable<UtilizationMemberAllService> GetUtilizationMemberAllServices(UtilizationDataContext db, string memberCode, string accountCode)
        {
            var disposeDb = false;

            if (db == null)
            {
                db = new UtilizationDataContext();
                disposeDb = true;
            }

            if (string.IsNullOrEmpty(memberCode))
            {

                if (disposeDb)
                {
                    db.Dispose();
                    db = null;
                }

                return new List<UtilizationMemberAllService>();
            }

            var returnValue = ExecuteSp<UtilizationMemberAllService>(db, "CP_CLMS_REP_UTILIZATION_ALL_EMED",
                new List<string>() { "MEMCODE" },
                new List<object>() { memberCode }
            );

            var memberCodeExceptions = GetMemberCodeExceptions(accountCode);
            returnValue = returnValue.Where(t => !memberCodeExceptions.Contains(t.MemberCode));

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the utilization reimbursements.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="service">The service.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;UtilizationReimbursement&gt;.</returns>
        public static IEnumerable<UtilizationReimbursement> GetUtilizationReimbursements(UtilizationDataContext db, string service, string accountCode, DateTime? startDate, DateTime? endDate, string memberCode)
        {
            var disposeDb = false;

            if (db == null)
            {
                db = new UtilizationDataContext();
                disposeDb = true;
            }

            var startDateString = ParameterDateTime(startDate);
            var endDateString = !string.IsNullOrEmpty(startDateString) ? ParameterDateTime(endDate) : "";
            if (string.IsNullOrEmpty(startDateString) && string.IsNullOrEmpty(endDateString))
            {
                if (disposeDb)
                {
                    db.Dispose();
                    db = null;
                }
                return new List<UtilizationReimbursement>();
            }

            IEnumerable<UtilizationReimbursement> returnValue = new List<UtilizationReimbursement>();
            if (!string.IsNullOrEmpty(memberCode))
            {
                returnValue = ExecuteSp<UtilizationReimbursement>(db, "SP_Intra_ActiveAccount_Utilization_ByMemCode_REIM",
                    new List<string>() { "SERVICE", "ACCT_CODE", "StartDate", "EndDate", "Member_Code" },
                    new List<object>() { service, accountCode, startDateString, endDateString, memberCode }
                    );
            }
            else
            {
                returnValue = ExecuteSp<UtilizationReimbursement>(db, "SP_EMED_REIM_UTIL",
                    new List<string>() { "SERVICE", "ACCT_CODE", "StartDate", "EndDate", "LastName" },
                    new List<object>() { service, accountCode, startDateString, endDateString, "" }
                    );
            }

            var memberCodeExceptions = GetMemberCodeExceptions(accountCode);
            var planIdExceptions = GetPlanIdExceptions(accountCode);

            returnValue = returnValue.Where(t => !memberCodeExceptions.Contains(t.MemberCode));
            returnValue = returnValue.Where(t => !planIdExceptions.Contains(t.AppliedPlan != null ? t.AppliedPlan.Value : -1));

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the utilization un billed report out patient med sevices.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;UtilizationUnBilledReportOutPatientMedSevice&gt;.</returns>
        public static IEnumerable<UtilizationUnBilledReportOutPatientMedSevice> GetUtilizationUnBilledReportOutPatientMedSevices(UtilizationDataContext db, string accountCode, DateTime? startDate, DateTime? endDate, string memberCode)
        {
            var disposeDb = false;

            if (db == null)
            {
                db = new UtilizationDataContext();
                disposeDb = true;
            }

            var startDateString = ParameterDateTime(startDate);
            var endDateString = !string.IsNullOrEmpty(startDateString) ? ParameterDateTime(endDate) : "";
            if (string.IsNullOrEmpty(startDateString) && string.IsNullOrEmpty(endDateString))
            {
                if (disposeDb)
                {
                    db.Dispose();
                    db = null;
                }
                return new List<UtilizationUnBilledReportOutPatientMedSevice>();
            }
            IEnumerable<UtilizationUnBilledReportOutPatientMedSevice> returnValue = new List<UtilizationUnBilledReportOutPatientMedSevice>();

            if (!string.IsNullOrEmpty(memberCode))
            {
                returnValue = ExecuteSp<UtilizationUnBilledReportOutPatientMedSevice>(db, "SP_Intra_ActiveAccount_Utilization_CALLLOG_ByMemCode",
                    new List<string>() { "ComCode", "StartDate", "EndDate", "Memcode" },
                    new List<object>() { accountCode, startDateString, endDateString, memberCode }
                );
            }
            else
            {
                returnValue = ExecuteSp<UtilizationUnBilledReportOutPatientMedSevice>(db, "SP_Intra_ActiveAccount_Utilization_CALLLOG",
                    new List<string>() { "ComCode", "StartDate", "EndDate", "LastName" },
                    new List<object>() { accountCode, startDateString, endDateString, "" }
                );
            }

            var memberCodeExceptions = GetMemberCodeExceptions(accountCode);
            var planIdExceptions = GetPlanIdExceptions(accountCode);

            returnValue = returnValue.Where(t => !memberCodeExceptions.Contains(t.AccountNumber));
            returnValue = returnValue.Where(t => !planIdExceptions.Contains(t.AppliedPlan));

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the utilization un billed report in patients.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="accountCode">The account code.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="memberCode">The member code.</param>
        /// <returns>IEnumerable&lt;UtilizationUnBilledReportInPatient&gt;.</returns>
        public static IEnumerable<UtilizationUnBilledReportInPatient> GetUtilizationUnBilledReportInPatients(UtilizationDataContext db, string accountCode, DateTime? startDate, DateTime? endDate, string memberCode)
        {
            var disposeDb = false;

            if (db == null)
            {
                db = new UtilizationDataContext();
                disposeDb = true;
            }

            var startDateString = ParameterDateTime(startDate);
            var endDateString = !string.IsNullOrEmpty(startDateString) ? ParameterDateTime(endDate) : "";
            if (string.IsNullOrEmpty(startDateString) && string.IsNullOrEmpty(endDateString))
            {
                if (disposeDb)
                {
                    db.Dispose();
                    db = null;
                }
                return new List<UtilizationUnBilledReportInPatient>();
            }

            IEnumerable<UtilizationUnBilledReportInPatient> returnValue = new List<UtilizationUnBilledReportInPatient>();

            if (!string.IsNullOrEmpty(memberCode))
            {
                returnValue = ExecuteSp<UtilizationUnBilledReportInPatient>(db, "SP_Intra_ActiveAccount_Utilization_DCR_ByMemCode",
                    new List<string>() { "ComCode", "StartDate", "EndDate", "MemCode" },
                    new List<object>() { accountCode, startDateString, endDateString, memberCode }
                    );
            }
            else
            {
                returnValue = ExecuteSp<UtilizationUnBilledReportInPatient>(db, "SP_Intra_ActiveAccount_Utilization_DCR",
                    new List<string>() { "ComCode", "StartDate", "EndDate", "LastName" },
                    new List<object>() { accountCode, startDateString, endDateString, "" }
                    );
            }

            var memberCodeExceptions = GetMemberCodeExceptions(accountCode);
            var planIdExceptions = GetPlanIdExceptions(accountCode);

            returnValue = returnValue.Where(t => !memberCodeExceptions.Contains(t.AccountNumber));
            returnValue = returnValue.Where(t => !planIdExceptions.Contains(t.AppliedPlan));

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return returnValue;
        }

        /// <summary>
        /// Gets the principal codes.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> GetPrincipalCodes(string accountCode)
        {
            using(var legacyDb = new LegacyDataContext()){
                var returnValue = LegacyHelper.GetAllLegacyPrincipalProcesses(legacyDb, accountCode).Select(t => t.PrincipalCode).ToList();
                return (returnValue != null && returnValue.Any()) ? returnValue : new List<string>();
            }             
        }

        /// <summary>
        /// Gets the dependent codes.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> GetDependentCodes(string accountCode)
        {
            using (var legacyDb = new LegacyDataContext())
            {
                var returnValue = LegacyHelper.GetAllLegacyDependentProcesses(legacyDb, accountCode).Select(t => t.DependentCode).ToList();
                return (returnValue != null && returnValue.Any()) ? returnValue : new List<string>();
            }
        }
        
        #endregion

        #region -- Functions --

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
        private static IEnumerable<T> ExecuteSp<T>(UtilizationDataContext db, string sp, List<string> parameterNames, List<object> parameterValues) where T : new()
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
                    MapSpResult<T>(returnValue, reader);

                    reader.Close();
                }

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
                    var attribute = property.CustomAttributes.FirstOrDefault(t => t.AttributeType == typeof(System.ComponentModel.DataAnnotations.Schema.ColumnAttribute));
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
        /// Set string parameter to datetime.
        /// </summary>
        /// <param name="parameterDate">The string parameter date.</param>
        /// <returns>DateTime.</returns>
        public static DateTime ParameterDateTime(string parameterDate)
        {
            if (!string.IsNullOrEmpty(parameterDate)) return Convert.ToDateTime(parameterDate);
            return DateTime.Now;
        }

        /// <summary>
        /// Reports the parameter date time.
        /// </summary>
        /// <param name="parameterDate">The parameter date.</param>
        /// <returns>System.Nullable&lt;DateTime&gt;.</returns>
        public static DateTime? ReportParameterDateTime(string parameterDate)
        {
            DateTime dateValue;
            if (DateTime.TryParse(parameterDate, out dateValue)) return Convert.ToDateTime(parameterDate);
            return null;
        }

        /// <summary>
        /// Parameters the date time.
        /// </summary>
        /// <param name="parameterDate">The parameter date.</param>
        /// <returns>System.String.</returns>
        public static string ParameterDateTime(DateTime? parameterDate)
        {
            if (parameterDate != null) return parameterDate.Value.ToString("MM/dd/yyyy");
            return "";
        }

        /// <summary>
        /// Gets the member code exceptions.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static List<string> GetMemberCodeExceptions(string accountCode)
        {
            using (var db = new IdentityDataContext())
            {
                var user = db.Users.FirstOrDefault(t => t.UserName == System.Web.HttpContext.Current.User.Identity.Name);
                return db.UtilizationReportExceptionForUsers.Where(t => !t.Deleted && t.UserIds.Contains(user.Id) && t.AccountCode == accountCode).Select(t => t.MemberCode).ToList();
            }
        }

        /// <summary>
        /// Gets the plan id exceptions.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>List&lt;System.Int32&gt;.</returns>
        public static List<int> GetPlanIdExceptions(string accountCode)
        {
            using (var db = new IdentityDataContext())
            {
                var user = db.Users.FirstOrDefault(t => t.UserName == System.Web.HttpContext.Current.User.Identity.Name);
                return db.UtilizationReportExceptionForPlans.Where(t => !t.Deleted && t.UserIds.Contains(user.Id) && t.AccountCode == accountCode).Select(t => t.PlanId).ToList();
            }
        }

        #endregion
    }
}
