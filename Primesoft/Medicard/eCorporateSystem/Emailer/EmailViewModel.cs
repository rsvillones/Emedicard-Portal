﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corelib.Models;

namespace Emailer
{
    public class EmailViewModel
    {
        #region -- Properties --

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string AccountCode { get; set; }

        public bool BypassCorporateAdminApproval { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public EndorsementBatch EndorsementBatch { get; set; }

        public Member Member { get; set; }

        public IEnumerable<Member> Members { get; set; }

        public RenewalMember RenewalMember { get; set; }

        public ActionMemo ActionMemo { get; set; }

        public IEnumerable<ActionMemo> ActionMemos { get; set; }

        public IEnumerable<IdReplacement> IdReplacements { get; set; }

        public IdReplacement IdReplacement { get; set; }

        public string SubjectAmendment { get; set; }

        public string CompanyName { get; set; }

        public Amendment Amendment { get; set; }

        public AdditionalDependent AdditionalDependent { get; set; }

        public DependentCancellation DependentCancellation { get; set; }

        public string Remarks { get; set; }

        public int Count { get; set; }

        public DateTime SubmittionDeadline { get; set; }

        public IEnumerable<LegacyMember> LegacyMembers { get; set; }

        #endregion
    }
}