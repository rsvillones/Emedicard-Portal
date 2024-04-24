﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corelib.Enums;

namespace Corelib.Models
{
    public class AccountSetting : BaseModel
    {
        #region -- Constructor --

        public AccountSetting()
        {
            this.DomainEmail = "medicardphils.com";
            this.DirectlySubmitToUrg = false;
            this.BypassMedicalHistory = true;
            this.ActionMemoDueDateDays = Config.ActionMemoDueDateDays;
        }

        #endregion

        #region -- Properties --

        [StringLength(25)]
        [Required]
        [Index]
        public string AccountCode { get; set; }

        public string AccountName { get; set; }

        [Display(Name = "Use Email as Login")]
        public bool UseEmailAsLogin { get; set; }

        [Display(Name = "Use Random Generated Password")]
        public bool UseRandomGeneratedPassword { get; set; }

        [Display(Name = "Bypass HR Approval")]
        public bool BypassHRManagerApproval { get; set; }

        [Display(Name = "Bypass Medical History")]
        public bool BypassMedicalHistory { get; set; }

        [Display(Name = "Directly Submit Endorsements to Urg")]
        public bool DirectlySubmitToUrg { get; set; }

        [Display(Name = "URG Setting")]
        public UrgSetting UrgSetting { get; set; }

        [Display(Name = "Domain Email Address")]
        public string DomainEmail { get; set; }

        [Display(Name = "Action Memo Due Date Days")]
        public int ActionMemoDueDateDays { get; set; }

        [Display(Name = "Show Action Memo Due Date")]
        public bool ShowActionMemoDueDate { get; set; }

        #endregion
    }
}
