using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    [Table("SYS_AGENT_LTBL")]
    public class LegacyAgent
    {
        [Column("AGENT_CODE")]
        [Key]
        public string Code { get; set; }

        [Column("AGENT_ALIAS_CODE")]
        public string AliasCode { get; set; }

        [Column("AGENT_TYPECODE")]
        public string TypeCode { get; set; }

        [Column("AGENT_FNAME")]
        public string FirstName { get; set; }

        [Column("AGENT_MI")]
        public string MiddleInitial { get; set; }

        [Column("AGENT_LNAME")]
        public string LastName { get; set; }

        [Column("AGENT_EFFDATE")]
        public DateTime? EffectivityDate { get; set; }

        [Column("AGENT_FAX")]
        public string Fax { get; set; }

        [Column("AGENT_PHONE")]
        public string Phone { get; set; }

        [Column("STREET")]
        public string Street { get; set; }

        [Column("CITY_CODE")]
        public string City { get; set; }

        [Column("STATUS")]
        public string Status { get; set; }

        [Column("STATUS_DATE")]
        public DateTime? StatusDate { get; set; }

        [Column("AM_CODE")]
        public string AmCode { get; set; }

        [Column("GA_CODE")]
        public string GaCode { get; set; }

        [Column("EXP_QUOTA")]
        public string Quota { get; set; }

        [Column("PROVINCE_CODE")]
        public string Province { get; set; }

        [Column("UPDATED_BY")]
        public string ModBy { get; set; }

        [Column("UPDATED_DATE")]
        public DateTime? ModDate { get; set; }

        [Column("ACCT_LIMIT")]
        public int AccountLimit { get; set; }

        [Column("AGENT_DESIGNATION")]
        public string Designation { get; set; }

        [Column("AGENT_BROK")]
        public bool Broker { get; set; }

        [Column("Email_add")]
        public string Email { get; set; }

        [Column("rowguid")]
        public Guid? RowGuid { get; set; }

        [Column("SALES_GROUP")]
        public string SalesGroup { get; set; }

        [Column("SUB_GROUP")]
        public string SalesSubgroup { get; set; }
    }
}
