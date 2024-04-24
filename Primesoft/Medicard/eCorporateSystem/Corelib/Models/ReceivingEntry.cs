﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    [Table("SYS_UWRECEIVING_MTBL")]
    public class ReceivingEntry : BaseModel
    {
        #region -- Properties --

        [Index]
        [StringLength(25)]
        [Required]
        [Column("CONTROL_NO")]
        public string ControlNumber { get; set; }

        [StringLength(25)]
        [Column("ACCOUNT_CODE")]
        public string AccountCode { get; set; }

        [Column("DATE_RECEIVED")]
        public DateTime? DateReceived { get; set; }

        [Column("DATE_FORWARDED")]
        public DateTime? DateForwarded { get; set; }

        [Column("DATE_DUE")]
        public DateTime? DueDate { get; set; }

        public bool? Processed { get; set; }

        [Column("NewPrin")]
        public int? NewPrincipalCount { get; set; }

        [Column("NewDep")]
        public int? NewDependentCount { get; set; }

        [Column("RenewPrin")]
        public int? RenewPrincipalCount { get; set; }

        [Column("RenewDep")]
        public int? RenewDependentCount { get; set; }

        [Column("AddPrin")]
        public int? AdditionalPrincipalCount { get; set; }

        [Column("AddDep")]
        public int? AdditionalDependentCount { get; set; }

        [Column("Reply")]
        public int? ReplyCount { get; set; }

        [Column("apprv_med")]
        public int? ApprovedCount { get; set; }

        [Column("change_stat")]
        public int? ChangeStatus { get; set; }

        [Column("simple_amend")]
        public int? SimpleAmendment { get; set; }

        [Column("med_reeval")]
        public int? MedicardReevaluation { get; set; }

        [Column("deletions")]
        public int? DeletionCount { get; set; }

        [Column("corrections")]
        public int? CorrectionCount { get; set; }

        [Column("corr_days")]
        public int? CorrectionDays { get; set; }

        [Column("lost_id")]
        public int? LostIdCount { get; set; }

        [StringLength(15)]
        [Column("TIME_RECEIVED")]
        public string TimeReceived { get; set; }

        [Column("from_Intra")]
        public string FromIntra { get; set; }

        [Column("USER_ASSIGNED")]
        [StringLength(25)]
        public string AssignedUser { get; set; }

        [Index]
        public DateTime? LegacyDateReceived { get; set; }

        #endregion
    }
}
