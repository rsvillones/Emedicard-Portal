﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Corelib.Models
{
    [Table("SYS_ACCOUNT_MTBL")]
    public class LegacyAccount
    {
        #region -- Properties --

        [Column("ACCOUNT_CODE")]
        [StringLength(25)]
        [Key]
        public string Code { get; set; }

        [Column("ACCOUNT_NAME")]
        [StringLength(150)]
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        [Column("PROPOSAL_NAME")]
        [StringLength(100)]
        public string ProposalName { get; set; }

        [Column("EFF_DATE")]
        [DisplayFormat(DataFormatString="MM/dd/yyyy")]
        public DateTime? EffectivityDate { get; set; }

        [Column("AGENT_CODE")]
        [StringLength(20)]
        public string AgentCode { get; set; }

        [Column("SERIAL_NO")]
        [StringLength(25)]
        public string SerialNumber { get; set; }

        [Column("ACCTTYPE_CODE")]
        [StringLength(10)]
        public string AccountTypeCode { get; set; }

        [Column("BRANCHES")]
        [StringLength(250)]
        public string Branches { get; set; }

        [Column("MEMBERSHIP_CODE")]
        [StringLength(10)]
        public string MembershipCode { get; set; }

        [Column("ACCTSTATUS_CODE")]
        [StringLength(10)]
        public string AccountStatusCode { get; set; }

        [Column("MEMSIZE_CODE")]
        [StringLength(10)]
        public string MemSizeCode { get; set; }

        [Column("PACKAGE_CODE")]
        [StringLength(10)]
        public string PackageCode { get; set; }

        [Column("SETUP_CODE")]
        [StringLength(10)]
        public string SetupCode { get; set; }

        [Column("STREET")]
        [StringLength(150)]
        public string Street { get; set; }

        [Column("CITY_CODE")]
        [StringLength(10)]
        public string City { get; set; }

        [Column("PROVINCE_CODE")]
        [StringLength(10)]
        public string Province { get; set; }

        [Column("PHONE_NO")]
        [StringLength(20)]
        public string Phone { get; set; }

        [Column("FAX_NO")]
        [StringLength(20)]
        public string Fax { get; set; }

        [Column("CONTACT_SALUTATION")]
        [StringLength(20)]
        public string ContactSalutation { get; set; }

        [Column("CONTACT_FNAME")]
        [StringLength(30)]
        public string ContactFirstName { get; set; }

        [Column("CONTACT_LNAME")]
        [StringLength(30)]
        public string ContactLastName { get; set; }

        [Column("CONTACT_MNAME")]
        [StringLength(30)]
        public string ContactMiddleName { get; set; }

        [Column("CONTACT_POSITION")]
        [StringLength(100)]
        public string ContactPosition { get; set; }

        [Column("FRANCHISE_DATE")]
        public DateTime? FranchiseDate { get; set; }

        [Column("FRANCHISE_EXPIRY")]
        public DateTime? FranchiseExpiryDate { get; set; }

        [Column("EXTEND60")]
        public bool? Extend60 { get; set; }

        [Column("EXTEND150")]
        public bool? Extend150 { get; set; }

        [Column("INDUSTRY_CODE")]
        [StringLength(10)]
        public string IndustryCode { get; set; }

        [Column("MINDUSTRY_CODE")]
        [StringLength(10)]
        public string MIndustryCode { get; set; }

        [Column("OLD_HMO_CODE")]
        [StringLength(10)]
        public string OldHMOCode { get; set; }

        [Column("OLD_HMO_EXPIRY")]
        public DateTime? OldHMOExpiryDate { get; set; }

        [Column("ADD_SALUTATION")]
        [StringLength(20)]
        public string AddresseeSalutation { get; set; }

        [Column("ADDRESSEE_FNAME")]
        [StringLength(30)]
        public string AddresseeFirstName { get; set; }

        [Column("ADDRESSEE_MNAME")]
        [StringLength(30)]
        public string AddresseeMiddleName { get; set; }

        [Column("ADDRESSEE_LNAME")]
        [StringLength(30)]
        public string AddresseeLastName { get; set; }

        [Column("ADDRESSEE_POSITION")]
        [StringLength(100)]
        public string AddresseePosition { get; set; }

        [Column("REQUIRE_REMARKS")]
        [StringLength(4000)]
        public string RequireRemarks { get; set; }

        [Column("WITH_RC")]
        public bool? WithRC { get; set; }

        [Column("WITH_MASTERLIST")]
        public bool? WithMasterList { get; set; }

        [Column("SUBMIT_DATE_ML")]
        public DateTime? MasterListSubmissionDate { get; set; }

        [Column("WITH_SURVEY_FORM")]
        public bool? WithSurveyForm { get; set; }

        [Column("SUBMIT_DATE_SF")]
        public DateTime? SurveyFormSubmissionDate { get; set; }

        [Column("EXTENDED_FROM")]
        public DateTime? ExtendedFrom { get; set; }

        [Column("EXTENDED_TO")]
        public DateTime? ExtendedTo { get; set; }

        [Column("PROGRESS_REM")]
        [StringLength(4000)]
        public string ProgressRemarks { get; set; }

        [Column("DATE_CLOSED")]
        public DateTime? DateClosed { get; set; }

        [Column("WITH_PROPOSAL")]
        [StringLength(3)]
        public string WithProposal { get; set; }

        [Column("UPDATED_DATE")]
        public DateTime? ModDate { get; set; }

        [Column("UPDATED_BY")]
        [StringLength(10)]
        public string ModBy { get; set; }

        [Column("SOB_APPROVED")]
        public bool? SOBApproved { get; set; }

        [Column("MOTHER_CODE")]
        [StringLength(25)]
        public string MotherCode { get; set; }

        [Column("MOTHER_STATUS")]
        public bool? MotherStatus { get; set; }

        [Column("PROPOSAL_TYPE")]
        public int? ProposalType { get; set; }

        [Column("ACCT_CATEGORY")]
        [StringLength(15)]
        public string AccountCategory { get; set; }

        [Column("AGENT_REASON")]
        [StringLength(300)]
        public string AgentReason { get; set; }

        [Column("ACCOUNT_DISPLAY")]
        [StringLength(40)]
        public string AccountDisplay { get; set; }

        [Column("SPLCASEID")]
        public int? SpecialCaseId { get; set; }

        [Column("ID_TYPE")]
        public int? IdType { get; set; }

        [Column("TRANSMITTAL_TYPE")]
        public int? TransmittalType { get; set; }

        [Column("BILLING_CTR_MO")]
        public int? BillingControlMonth { get; set; }

        [Column("BILLING_CTR_QTR")]
        public int? BillingControlQuarter { get; set; }

        [Column("BILLING_CTR_SA")]
        public int? BillingControlSemiAnnual { get; set; }

        [Column("BILLING_CTR_AN")]
        public int? BillingControlAnnual { get; set; }

        [Column("Other_Remarks")]
        [StringLength(500)]
        public string OtherRemarks { get; set; }

        [Column("GRACE_PERIOD")]
        public int? GracePeriod { get; set; }

        [Column("GROUP_TOP")]
        [StringLength(25)]
        public string GroupTop { get; set; }

        [Column("TAG_TOP")]
        [StringLength(1)]
        public string TagTop { get; set; }

        [Column("DATE_VALIDITY")]
        [DisplayFormat(DataFormatString = "MM/dd/yyyy")]
        public DateTime? ValidityDate { get; set; }

        [Column("ENCODE_TYPE")]
        public bool? EncodeType { get; set; }

        [Column("TAG")]
        public bool? Tag { get; set; }

        [Column("FOR_RENEWAL")]
        public bool? ForRenewal { get; set; }

        [Column("REN_UPDATE_DATE")]
        public DateTime? RenewalUpdateDate { get; set; }

        [Column("LOGIN")]
        [StringLength(12)]
        public string UserName { get; set; }

        [Column("USER_ASSIGN")]
        [StringLength(15)]
        public string AssignedUser { get; set; }

        [Column("TIN_NO")]
        [StringLength(30)]
        public string TIN { get; set; }

        [Column("FROM_INTRA")]
        public bool? FromIntra { get; set; }

        [Column("WITH_RSP")]
        public bool? WithRSP { get; set; }

        [Column("VATTYPEID")]
        public int? VatTypeId { get; set; }

        [Column("rowguid")]
        public Guid Guid { get; set; }

        [Column("LAST_COVERED_DATE")]
        public DateTime? LastCoveredDate { get; set; }

        [Column("EMAIL_ADD_1")]
        [StringLength(50)]
        public string EmailAddress1 { get; set; }

        [Column("EMAIL_ADD_2")]
        [StringLength(50)]
        public string EmailAddress2 { get; set; }

        [Column("corr_days")]
        public int? CorrDays { get; set; }

        public ICollection<LegacyRoomRate> LegacyRoomRates { get; set; }

        private LegacySob _legacySob = null;
        [NotMapped]
        public LegacySob LegacySob {
            get
            {
                if (_legacySob == null) GetLegacySob();

                return _legacySob;
            }
            set
            {
                _legacySob = value;
            }
        }

        #endregion

        #region -- Functions --

        private void GetLegacySob()
        {
            using (var legacyDb = new LegacyDataContext())
            {
                switch (this.AccountCategory)
                {
                    case "SOLO":
                        this.LegacySob = legacyDb.LegacySobs.FirstOrDefault(t => t.AccountCode == this.Code) ?? new LegacySob();
                        break;
                    case "SISTER":
                        this.LegacySob = legacyDb.LegacySobs.FirstOrDefault(t => t.AccountCode == this.MotherCode) ?? new LegacySob();
                        break;
                    case "MOTHER":
                        this.LegacySob = legacyDb.LegacySobs.FirstOrDefault(t => t.AccountCode == this.Code) ?? new LegacySob();
                        break;
                }
            }
        }

        #endregion

    }
}
