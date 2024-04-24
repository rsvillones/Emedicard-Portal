using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class UtilizationReimbursement
    {
        #region -- Properties --

        [Column("MEMBER_CODE")]
        public string MemberCode { get; set; }

        [Column("DNAME")]
        public string DName { get; set; }

        [Column("COMPANY_CODE")]
        public string AccountCode { get; set; }

        [Column("CONTROL_CODE")]
        public string ControlCode { get; set; }

        [Column("DATE_RCVD")]
        public DateTime RecievedDate { get; set; }

        [Column("DATE_DUE")]
        public DateTime? DueDate { get; set; }

        [Column("RELEASED_DATE")]
        public DateTime? ReleaseDate { get; set; }

        [Column("HOSPITAL_NAME")]
        public string HospitalName { get; set; }

        [Column("DIAG_DESC")]
        public string DiagnosisDescription { get; set; }

        [Column("AMOUNT")]
        public decimal Amount { get; set; }

        [Column("TOTAL_APPROVED")]
        public decimal TotalApproved { get; set; }

        [Column("RM_CODE")]
        public int? AppliedPlan { get; set; }

        [Column("PROCESSED_DATE")]
        public DateTime? ProcessDate { get; set; }

        [Column("ACTION_MEMO")]
        public string ActionMemo { get; set; }

        [Column("ACTION_MEMO_REM")]
        public string ActionMemoRemark { get; set; }

        [Column("DISAPPROVED")]
        public string Disapproved { get; set; }

        [Column("DISAPPROVED_REM")]
        public string DisapprovedRemarks { get; set; }

        [Column("PRIN_COMPID")]
        public string EmployeeNumber { get; set; }

        #endregion
    }
}
