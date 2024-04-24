using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Corelib.Models
{
    public class LegacyActiveDependent
    {
        #region -- Properties --

        [Column("AREA_DESC")]
        public string Area { get; set; }

        [Column("PRINCIPAL")]
        public string Principal { get; set; }

        [Column("DEP_CODE")]
        public string DependentCode { get; set; }

        [Column("PRIN_CODE")]
        public string PrincipalCode { get; set; }

        [Column("MEM_LNAME")]
        public string LastName { get; set; }

        [Column("MEM_FNAME")]
        public string FirstName { get; set; }

        [Column("MEM_MI")]
        public string MiddleName { get; set; }

        [Column("MEM_BDAY")]
        public DateTime? DateOfBirth { get; set; }

        [Column("MEM_AGE")]
        public Single Age { get; set; }

        [Column("SEX_DESC")]
        public string Gender { get; set; }

        [Column("MEMCSTAT_DESC")]
        public string CivilStatus { get; set; }

        [Column("EFF_DATE")]
        public DateTime? EffectivityDate { get; set; }

        [Column("VAL_DATE")]
        public DateTime? ValidityDate { get; set; }

        [Column("RSPROOMRATE_ID")]
        public int? PlanId { get; set; }

        [Column("PLAN_DESC")]
        public string PlanDescription { get; set; }

        [Column("PRIN_COMPID")]
        public string EmployeeNumber { get; set; }

        [Column("RELATION_CODE")]
        public string RalationshipCode { get; set; }

        [Column("DEP_DESCRIPTION")]
        public string Ralationship { get; set; }

        [Column("LIMIT")]
        public decimal Limit { get; set; }

        [NotMapped]
        public string PlanDescriptionForReport
        {
            get
            {
                return String.Format("{0} - ({1:Php 0,0.00} Limit)", this.PlanDescription, this.Limit);
            }
        }

        #endregion
    }
}
