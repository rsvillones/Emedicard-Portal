using Corelib.Enums;
using System;
using System.Data.Entity;
using System.Web;
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
    public class DependentCancellation : BaseModel, IValidatableObject
    {
        #region -- Constructor --

        public DependentCancellation()
        {
            RequestDate = DateTime.Now;
            RequestEffectivityDate = DateTime.Now;
        }

        #endregion

        #region -- Properties --

        [StringLength(25)]
        [Index]
        public string ControlNumber { get; set; }

        [DisplayFormat(DataFormatString = BaseModel.DateFormat, ApplyFormatInEditMode = true)]
        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; }

        [DisplayFormat(DataFormatString = BaseModel.DateFormat, ApplyFormatInEditMode = true)]
        [Display(Name = "Request Effectivity Date")]
        public DateTime RequestEffectivityDate { get; set; }

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

        [Required]
        [Display(Name = "Dependent Name")]
        [Index]
        [StringLength(25)]
        public string DependentCode { get; set; }

        public string DependentName { get; set; }

        public string MemberName { get; set; }

        #endregion

        #region -- Not Mapped --

        [NotMapped]
        public string DependentRelationship
        {
            get
            { 
                using (var db = new LegacyDataContext())
                {
                    var dependent = db.LegacyDependentProcesses.FirstOrDefault(t => t.DependentCode == this.DependentCode);
                    return dependent != null ? dependent.Relationship : "";
                }
            }
        }

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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            using (var db = new IdentityDataContext())
            {
                if (db.DependentCancellations.Any(t => t.DependentCode == this.DependentCode && (t.Status != RequestStatus.Disapproved && t.Status != RequestStatus.CorporateAdminDisapproved && t.Status != RequestStatus.CancelledRequest) && t.Id != this.Id))
                {
                    yield return new ValidationResult("Dependent was already requested to be cancelled.", new List<string>() { "DependentCode" });
                }
                if (string.IsNullOrEmpty(this.Remarks) && db.Reasons.Any(t=>t.Id == this.ReasonId && t.Description.ToLower().Trim() == "others"))
                {
                    yield return new ValidationResult("Please specify reason in remarks field.", new List<string>() { "Remarks" });
                }
                if (db.Reasons.Any(t => t.Id == this.ReasonId && t.Description.ToLower().Trim() != "others") && string.IsNullOrEmpty(this.DocumentFileName))
                {
                    if (this.DocumentTypeId == null) yield return new ValidationResult("Please specify document type field.", new List<string>() { "DocumentTypeId" });
                    if (this.FileWrapper == null) yield return new ValidationResult("Please specify file attachment field.", new List<string>() { "FileWrapper" });                    
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
    }
}
