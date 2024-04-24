﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class UtilizationDental
    {
        #region -- Properties --

        [Column("CTLNO")]
        public string ControlNumber { get; set; }

        [Column("HOSPITAL_CODE")]
        public string HospitalCode { get; set; }

        [Column("H_NAME")]
        public string HospitalName { get; set; }

        [Column("REC_DATE")]
        public DateTime RecordDate { get; set; }

        [Column("DUE_DATE")]
        public DateTime DueDate { get; set; }

        [Column("ACCOUNT_NO")]
        public string AccountNumber { get; set; }

        [Column("VISIT_DATE")]
        public DateTime VisitDate { get; set; }

        [Column("DX_DESC")]
        public string Diagnosis { get; set; }

        [Column("UTIL_AMT")]
        public decimal UtilizationAmount { get; set; }

        [Column("LAST_NAME")]
        public string LastName { get; set; }

        [Column("FIRST_NAME")]
        public string FirstName { get; set; }

        [Column("MI")]
        public string MiddleInitial { get; set; }

        [Column("MEM_TYPE")]
        public string MemberType { get; set; }

        [Column("PRO_BY")]
        public string ProcessBy { get; set; }

        [Column("SERVICE")]
        public string Service { get; set; }

        [Column("AREA")]
        public string Area { get; set; }

        [Column("PRO_DATE")]
        public DateTime ProcessDate { get; set; }

        [Column("EFF_DATE")]
        public DateTime? EffectivityDate { get; set; }

        [Column("VAL_DATE")]
        public DateTime? ValidityDate { get; set; }

        [Column("AGE")]
        public decimal Age { get; set; }

        [Column("CIVIL_STAT")]
        public int CivilStatus { get; set; }

        [Column("SEX")]
        public int Gender { get; set; }

        [Column("RM_CODE")]
        public int AppliedPlan { get; set; }

        [Column("PRINCIPAL")]
        public string PrincipalName { get; set; }

        [Column("PRIN_COMPID")]
        public string EmployeeNumber { get; set; }

        #endregion
    }
}
