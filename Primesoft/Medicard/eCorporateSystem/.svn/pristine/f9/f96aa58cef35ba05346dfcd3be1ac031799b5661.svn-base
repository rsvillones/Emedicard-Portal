using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class CancelledMemberWrapper : BaseModel
    {
        #region -- Properties --

        public string MemberCode { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string EmailAddress { get; set; }

        public string DateOfBirth { get; set; }

        public string Area { get; set; }

        public string EmployeeNumber { get; set; }

        public string AppliedPlan { get; set; }

        [NotMapped]
        public string AppliedPlanFromExcel
        {
            set
            {
                if (!String.IsNullOrEmpty(value) && value.Contains("|"))
                {
                    this.AppliedPlan = value.Split('|')[1];
                }
                else
                {
                    this.AppliedPlan = "";
                }
            }
        }

        public string Type { get; set; }

        public string Relationship { get; set; }

        public string Gender { get; set; }

        public string CivilStatus { get; set; }

        public string Waiver { get; set; }

        public string EffectivityDate { get; set; }

        public string ValidityDate { get; set; }

        public string Remarks { get; set; }

        public string EndorsementBatchId { get; set; }

        #endregion
    }
}
