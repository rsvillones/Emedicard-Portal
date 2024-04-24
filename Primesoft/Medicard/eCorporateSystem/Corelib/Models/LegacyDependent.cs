﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    
    public class LegacyDependent
    {
        [Key]
        [Column("DEP_APPNUM")]
        public decimal? DepAppNum { get; set; }

        [Column("DEP_CODE")]
        public string Code { get; set; }

        [Column("ACCOUNT_CODE")]
        public string AccountCode { get; set; }

        [Column("MEM_LNAME")]
        public string LastName { get; set; }

        [Column("MEM_FNAME")]
        public string FirstName { get; set; }

        [Column("MEM_MI")]
        public string MiddleName { get; set; }

        [Column("MEM_BDAY")]
        public DateTime DateOfBirth { get; set; }

        [Column("MEM_SEX")]
        public int MEM_SEX { get; set; }

        [NotMapped]
        public string Gender
        {
            get
            {
                switch (this.MEM_SEX)
                {
                    case 0:
                        return "Female";
                        break;
                    default:
                        return "Male";
                        break;
                }
            }
        }

        [Column("MEM_CIVILSTAT")]
        public int MEM_CIVILSTAT { get; set; }

        [NotMapped]
        public string CivilStatus
        {
            get
            {
                switch (this.MEM_CIVILSTAT)
                {
                    case 1:
                        return "Married";
                        break;
                    default:
                        return "Single";
                        break;
                }
            }
        }

        [Column("EFF_DATE")]
        public DateTime EffectivityDate { get; set; }

        [Column("VAL_DATE")]
        public DateTime ValidityDate { get; set; }

        [Column("RSPROOMRATE_ID")]
        public int AppliedPlan { get; set; }

        [Column("DEP_RELCODE")]
        public string Relationship { get; set; }

        [Column("PRIN_APPNUM")]
        public decimal PrinAppNum { get; set; }
        [ForeignKey("PrinAppNum")]
        public LegacyMember LegacyMember { get; set; }

        [Column("DTACARD_PRINTED")]
        public DateTime? DataCardPrintedDate { get; set; }
    }
}
