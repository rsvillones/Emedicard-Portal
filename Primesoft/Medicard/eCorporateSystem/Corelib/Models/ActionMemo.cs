﻿using Corelib.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    [Table("ActionMemos")]
    public class ActionMemo : BaseModel
    {
        #region -- Constructor --

        public ActionMemo()
        {
            this.DateIssued = DateTime.Now;
            this.IsNew = true;
        }

        #endregion

        #region -- Properties --

        [StringLength(25)]
        [Index]
        public string ControlNumber { get; set; }

        [Display(Name = "Date Issued")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateIssued { get; set; }

        [Display(Name = "Due Date to Comply")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

        public ActionMemoType Type { get; set; }

        public int? MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member Member { get; set; }

        public int? DependentId { get; set; }
        [ForeignKey("DependentId")]
        public Dependent Dependent { get; set; }

        public int EndorsementBatchId { get; set; }
        [ForeignKey("EndorsementBatchId")]
        public EndorsementBatch EndorsementBatch { get; set; }

        public string Details { get; set; }

        [Display(Name = "Member Reply")]
        public string MemberReply { get; set; }

        public bool IsNew { get; set; }

        public ICollection<Document> Documents { get; set; }

        public ActionMemoStatus Status { get; set; }

        [Index]
        public DateTime? DateSubmittedToCorporateAdmin { get; set; }

        [Index]
        public DateTime? DateSubmittedToUrg { get; set; }

        [NotMapped]
        [Display(Name = "Member Type")]
        public string MemberType
        {
            get
            {
                if (this.Dependent != null)
                {
                    return "Dependent";
                }
                else
                {
                    return "Principal";
                }
            }
        }

        [NotMapped]
        [Display(Name = "Membership Status")]
        public string UrgStatus
        {
            get
            {
                var returnValue = "";
                switch (this.Type)
                {
                    case ActionMemoType.Disapproved:
                        returnValue = "DISAPPROVED";
                        break;
                    case ActionMemoType.Resigned:
                        returnValue = "RESIGNED";
                        break;
                    case ActionMemoType.Encoding:
                        returnValue = "FOR ENCODING";
                        break;
                    case ActionMemoType.MedicalExamination:
                        returnValue = "FOR MEDICAL EXAMINATION";
                        break;
                    case ActionMemoType.Cancellation:
                        returnValue = "CANCELLED";
                        break;
                    case ActionMemoType.ResignedAutomatically:
                        returnValue = "RESIGNED";
                        break;
                    case ActionMemoType.OnHoldOthers:
                        returnValue = "ON HOLD";
                        break;
                    case ActionMemoType.OnHoldActive:
                        returnValue = "ACTIVE";
                        break;
                    default:
                        returnValue = "";
                        break;
                }
                return returnValue;
            }
        }

        [NotMapped]
        [Display(Name="Membership Status")]
        public string MembershipStatus
        {
            get
            {
                var returnValue = "";
                switch (this.Type)
                {
                    case ActionMemoType.Disapproved:
                        returnValue = "DISAPPROVED";
                        break;
                    case ActionMemoType.Resigned:
                        returnValue = "RESIGNED";
                        break;
                    case ActionMemoType.Encoding:
                        returnValue = "ON HOLD";
                        break;
                    case ActionMemoType.MedicalExamination:
                        returnValue = "ON HOLD";
                        break;
                    case ActionMemoType.Cancellation:
                        returnValue = "CANCELLED";
                        break;
                    case ActionMemoType.ResignedAutomatically:
                        returnValue = "RESIGNED";
                        break;
                    case ActionMemoType.OnHoldOthers:
                        returnValue = "ON HOLD";
                        break;
                    case ActionMemoType.OnHoldActive:
                        returnValue = "ON HOLD";
                        break;
                    default:
                        returnValue = "";
                        break;
                }
                return returnValue;
            }
        }

        [NotMapped]
        [Display(Name = "Company Name")]
        public string CompanyName
        {
            get
            {                
                return this.EndorsementBatch != null ? this.EndorsementBatch.CompanyName : "";
            }
        }

        #endregion
    }
}
