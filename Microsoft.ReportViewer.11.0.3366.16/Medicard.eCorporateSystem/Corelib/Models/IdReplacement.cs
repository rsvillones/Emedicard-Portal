﻿using Corelib.Enums;
using System;
using System.Web;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class IdReplacement : BaseModel,IValidatableObject
    {
        #region -- Constructor --

        public IdReplacement()
        {
            RequestDate = DateTime.Now;
        }

        #endregion

        #region -- Properties --

        [StringLength(25)]
        [Index]
        public string ControlNumber { get; set; }

        [DisplayFormat(DataFormatString = BaseModel.DateFormat, ApplyFormatInEditMode = true)]
        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; }
       
        [Display(Name = "Request For")]
        public RequestFor RequestFor { get; set; }

        [Display(Name = "Status")]
        public RequestStatus Status { get; set; }

        public string Remarks { get; set; }

        public string AccountCode { get; set; }

        public byte[] DocumentFile { get; set; }

        public string DocumentContentType { get; set; }

        public string DocumentFileName { get; set; }

        [NotMapped]
        public HttpPostedFileBase FileWrapper { get; set; }

        #endregion      

        #region -- Foreign Keys --

        [ForeignKey("Reason")]
        [Display(Name = "Request Reason")]
        public int ReasonId { get; set; }
        public Reason Reason { get; set; }

        [ForeignKey("EndorsementBatch")]
        public int EndorsementBatchId { get; set; }
        public EndorsementBatch EndorsementBatch { get; set; }

        [Index]
        [Display(Name = "Name of Dependent")]
        public int? DependentId { get; set; }

        [Index]
        [Display(Name = "Member Name")]
        public int MemberId { get; set; }

        #endregion

        #region -- IValidatableObject Members --

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (var db = new IdentityDataContext())
            {
                if (RequestFor == Enums.RequestFor.Dependent && DependentId == null)
                {
                    yield return new ValidationResult("Dependent Name is required.", new List<string>() { "DependentId" });
                }
            }   
        }

        #endregion
    }
}
