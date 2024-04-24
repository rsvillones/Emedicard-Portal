﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Corelib.Models
{
    public class RenewalMemberWrapper : BaseModel, IValueProvider
    {
        #region -- Properties --

        [Index]
        public Guid EndorsementBatchGuid { get; set; }

        [NotMapped]
        public string Identifier
        {
            get
            {
                return String.Format("{0} - {1}, {2} {3}", this.Row, this.LastName, this.FirstName, this.MiddleName);
            }
        }

        public int Row { get; set; }

        public string Code { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string EmailAddress { get; set; }

        public string DateOfBirth { get; set; }

        public string Area { get; set; }

        public string EmployeeNumber { get; set; }

        public string AppliedPlan { get; set; }

        public string AllowedPlans { get; set; }

        private string _AppliedPlanFromExcel;
        public string AppliedPlanFromExcel
        {
            get { return _AppliedPlanFromExcel; }
            set
            {
                _AppliedPlanFromExcel = value;

                if (!String.IsNullOrEmpty(value) && value.Contains("|"))
                {
                    this.AppliedPlan = value.Split('|')[1];
                }
                else
                {
                    this.AppliedPlan = null;
                }
            }
        }

        private string _AllowedPlansFromExcel;
        public string AllowedPlansFromExcel
        {
            get { return _AllowedPlansFromExcel; }
            set
            {
                _AllowedPlansFromExcel = value;

                if (!String.IsNullOrEmpty(value) && value.Contains("|"))
                {
                    var valueToAssign = string.Empty;
                    var values = value.Split('|');
                    for (var index = 1; index < values.Length; index++)
                    {
                        if (valueToAssign != "") valueToAssign += ",";
                        if (values[index].Contains(","))
                        {
                            valueToAssign += values[index].Substring(0, values[index].IndexOf(","));
                        }
                        else
                        {
                            valueToAssign += values[index];
                        }
                    }
                    this.AllowedPlans = valueToAssign;
                }
                else
                {
                    this.AllowedPlans = null;
                }
            }
        }

        public string Type { get; set; }

        public string PrincipalMemberCode { get; set; }

        public string Relationship { get; set; }

        public string Gender { get; set; }

        public string CivilStatus { get; set; }

        public string Waiver { get; set; }

        public string EffectivityDate { get; set; }

        public string ValidityDate { get; set; }

        public string Remarks { get; set; }

        public string AccountCode { get; set; }

        public string EndorsementBatchId { get; set; }

        public bool IsValid { get; set; }

        public string ErrorMessage { get; set; }

        public string EndorsementType { get; set; }

        public string DependentStringValue { get; set; }

        private string _RelationshipFromExcel;
        public string RelationshipExcel
        {
            get { return _RelationshipFromExcel; }
            set
            {
                _RelationshipFromExcel = value;

                if (!String.IsNullOrEmpty(value) && value.Contains("|"))
                {
                    this.Relationship = value.Split('|')[1];
                }
                else
                {
                    this.Relationship = null;
                }
            }
        }

        private string _AreaFromExcel;
        public string AreaExcel
        {
            get { return _AreaFromExcel; }
            set
            {
                _AreaFromExcel = value;

                if (!String.IsNullOrEmpty(value) && value.Contains("|"))
                {
                    this.Area = value.Split('|')[1];
                }
                else
                {
                    this.Area = null;
                }
            }
        }

        public string RenewalStatus { get; set; }

        [NotMapped]
        public string RenewalStatusRemark
        {
            get
            {
                return this.RenewalStatus == "0" ? "For Renewal" : "Not For Renewal";
            }
        }

        #endregion

        #region -- IValueProvider Members --

        public bool ContainsPrefix(string prefix)
        {
            var pi = this.GetType().GetProperty(prefix);
            return (pi != null && pi.CanRead);
        }

        public ValueProviderResult GetValue(string key)
        {
            var pi = this.GetType().GetProperty(key);
            if (pi == null || !pi.CanRead) return null;

            return new ValueProviderResult(Convert.ToString(pi.GetValue(this)), null, CultureInfo.CurrentCulture);
        }

        #endregion
    }
}