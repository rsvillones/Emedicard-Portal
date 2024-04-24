using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Corelib
{
    public static class ReportHelper
    {
        public static IEnumerable<SpReport_ActivePrincipalMembersResult> ActivePrincipalMembers(string accountCode)
        {
            using (var db = new StoredProceduresDataContext(Config.ConnectionString))
            {
                return db.ActivePrincipalMembers(accountCode).ToList();
            }
        }
        public static IEnumerable<SpReport_ActiveDependentMembersResult> ActiveDependentMembers(string accountCode)
        {
            using (var db = new StoredProceduresDataContext(Config.ConnectionString))
            {
                return db.ActiveDependentMembers(accountCode).ToList();
            }
        }
    }
}
