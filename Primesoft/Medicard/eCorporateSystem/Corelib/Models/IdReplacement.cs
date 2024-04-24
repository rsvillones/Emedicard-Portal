﻿using Corelib.Enums;
using System;
using System.Web;
using System.Data.Entity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Corelib.Models
{
    public class IdReplacement : BaseModel, IValidatableObject
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

        [Required]
        [Display(Name = "Member Name")]
        [Index]
        [StringLength(25)]
        public string MemberCode { get; set; }

        [Display(Name = "Dependent Name")]
        [Index]
        [StringLength(25)]
        public string DependentCode { get; set; }

        public string DependentName { get; set; }

        public string MemberName { get; set; }

        #endregion
        
        #region -- Foreign Keys --

        [ForeignKey("Reason")]
        [Display(Name = "Request Reason")]
        public int ReasonId { get; set; }
        public Reason Reason { get; set; }

        [ForeignKey("EndorsementBatch")]
        public int EndorsementBatchId { get; set; }
        public EndorsementBatch EndorsementBatch { get; set; }

        [ForeignKey("DocumentType")]
        [Display(Name = "Document Type")]
        public int? DocumentTypeId { get; set; }
        public DocumentType DocumentType { get; set; }

        #endregion

        #region -- IValidatableObject Members --

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (var db = new IdentityDataContext())
            {
                if (RequestFor == Enums.RequestFor.Dependent && string.IsNullOrEmpty(DependentCode))
                {
                    yield return new ValidationResult("Dependent Name is required.", new List<string>() { "DependentCode" });
                }
            }
            if (this.FileWrapper != null)
            {
                var allowedExtensions = new[] { ".doc", ".docx", ".jpeg", ".pdf", ".png", ".jpg" };
                var extension = Path.GetExtension(this.FileWrapper.FileName);
                if (!allowedExtensions.Contains(extension))
                {
                    yield return new ValidationResult("Invalid file extension.", new List<string>() { "FileWrapper" });
                }
            }
        }

        #endregion
    }
}
