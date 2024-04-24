﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Corelib.Models;
namespace WebUI.Models
{
    public class EmailViewModel
    {
        public IEnumerable<Member> Members { get; set; }

        public IEnumerable<CancelledMember> CancelledMembers { get; set; }

        public IEnumerable<RenewalMember> RenewalMembers { get; set; }

        public Member Member { get; set; }

        public Amendment Amendment { get; set; }

        public IdReplacement IdReplacement { get; set; }

        public EndorsementBatch EndorsementBatch { get; set; }

        public ActionMemo ActionMemo { get; set; }

        public Dependent Dependent { get; set; }

        public AdditionalDependent AdditionalDependent { get; set; }

        public DependentCancellation DependentCancellation { get; set; }

        public CancelledMember CancelledMember { get; set; }

        public RenewalMember RenewalMember { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Url { get; set; }
    }
}