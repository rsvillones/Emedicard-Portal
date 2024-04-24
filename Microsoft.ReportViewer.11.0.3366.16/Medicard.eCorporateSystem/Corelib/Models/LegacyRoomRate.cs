﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    [Table("SYS_ROOMRATE_MTBL")]
    public class LegacyRoomRate
    {
        #region -- Properties --

        [Column("RSPROOMRATE_ID")]
        public int Id { get; set; }

        [Column("PLAN_CODE")]
        public string PlanCode { get; set; }

        [Column("PLAN_SELECTED")]
        public bool Selected { get; set; }

        [Column("PREMIUM_PAYMENT_FOR")]
        public int PaymentFor { get; set; }

        [Column("DD_REG_LIMITS")]
        public decimal Limit { get; set; }

        [Column("RNB_FOR")]
        public string For { get; set; }

        [Column("ACCOUNT_CODE")]
        public string AccountCode { get; set; }

        [ForeignKey("AccountCode")]
        public LegacyAccount LegacyAccount { get; set; }

        [ForeignKey("PlanCode")]
        public LegacyPlan LegacyPlan { get; set; }

        [ForeignKey("PaymentFor")]
        public LegacyPaymode LegacyPaymode { get; set; }

        [NotMapped]
        public string LongDescription
        {
            get
            {
                return String.Format("{0} - {1} ({2:Php 0,0.00} Limit)", this.LegacyPlan.Description, this.For, this.Limit);
            }
        }

        [NotMapped]
        public string DescriptionForExcel
        {
            get
            {
                return String.Format("{1} - {2} ({3:Php 0,0.00} Limit)|{0}", this.Id, this.LegacyPlan.Description, this.For, this.Limit);
            }
        }

        #endregion
    }
}
