using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corelib
{
    public class Constants
    {
        public static readonly IEnumerable<int?> RESIGNED_STAT_CODES = new List<int?> { 0, 9, 20 };
        public static readonly IEnumerable<int?> DISAPPROVED_STAT_CODES_CONTRACT_YEAR = new List<int?> { 0, 20, 21 };
        public static readonly IEnumerable<int?> ACTIVE_OSTAT_CODES = new List<int?> { 0, 20, 21, 22 };
        public static readonly int? RESIGNED_OSTAT_CODE_PRINCIPAL = 9;

        public const int RESIGNED_AMENDMENT_DAYS = -120;
        public const int RESIGNED_RESIGNED_DAYS = -120;

        public const string NEW_ENDORSEMENT_TYPE = "New";
        public const string RENEWAL_ENDORSEMENT_TYPE = "Renewal";
        public const string ACTION_MEMO_ENDORSEMENT_TYPE = "Action Memo";
        public const string AMENDMENT_ENDORSEMENT_TYPE = "Amendment";
        public const string ID_REPLACEMENT_ENDORSEMENT_TYPE = "ID Replacement";
        public const string ADDITIONAL_DEPENDENT_ENDORSEMENT_TYPE = "Additional Dependent";
        public const string DEPENDENT_CANCELLATION_ENDORSEMENT_TYPE = "Dependent Cancellation";
        public const string CANCEL_MEMBERSHIP_ENDORSEMENT_TYPE = "Cancel Membership";
    }
}