﻿
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Corelib.Enums
{
    public enum Gender
    {
        Male,
        Female
    }

    public enum EndorsementBatchStatus
    {
        New,
        ForEmployeeUpdating,
        PartiallyForProcessing,
        Processing,
        Completed,
        Cancelled
    }

    public enum MembershipStatus
    {
        New,
        Saved,
        SubmittedToCorporateAdmin,
        CorporateAdminApproved,
        CorporateAdminDisapproved,
        ForProcessing,
        Approved,
        Cancelled,
        WithActionMemo
    }

    public enum UrgSetting
    {
        All,
        Partial,
        Principal
    }

    public enum RequestFor
    {
        Principal,
        Dependent,
        Account
    }

    public enum RequestDataType
    {
        [Display(Name = "Last Name")]
        LastName,
        [Display(Name = "First Name")]
        FirstName,
        [Display(Name = "Middle Name")]
        MiddleName,
        [Display(Name = "Civil Status")]
        CivilStatus,
        [Display(Name = "Address (Street, City, Province, Area Code)")]
        Address,
        Position,
        [Display(Name = "Telephone Number")]
        Telephone,
        [Display(Name = "Cost Centre")]
        CostCenter,
        Area,
        [Display(Name = "Hiring Date")]
        DateHired,
        [Display(Name = "Effectivity Date")]
        EffectivityDate,
        [Display(Name = "Company Name")]
        CompanyName
    }

    public enum RequestStatus
    {
        Saved,
        Submitted,
        [Display(Name = "For Processing")]
        ForProcessing,
        [Display(Name = "Corporate Admin Approved")]
        CorporateAdminApproved,
        [Display(Name = "Corporate Admin Disapproved")]
        CorporateAdminDisapproved,
        Approved,
        Disapproved,
        [Display(Name = "Cancelled Request")]
        CancelledRequest
    }

    public enum ActionMemoType
    {
        Disapproved,
        Resigned,
        Encoding,
        [Display(Name="Medical Examination")]
        MedicalExamination,
        Cancellation,
        [Display(Name = "Resigned - Automatically")]
        ResignedAutomatically,
        [Display(Name = "On Hold - Others")]
        OnHoldOthers,
        [Display(Name = "On Hold - Active")]
        OnHoldActive
    }

    public enum ActionMemoStatus
    {
        New,
        ForCorporateAdminApproval,
        Replied,
        ForProcessing,
        Issued
    }

    public enum CancelledMembershipStatus
    {
        [Display(Name = "Corporate Admin Approved")]
        CorporateAdminApproved,
        [Display(Name = "For Processing")]
        ForProcessing,
        Approved,
        Disapproved
    }

    public enum QuestionType
    {
        [Display(Name = "Answerable by Yes or No")]
        YesNo,
        [Display(Name = "Select Single Item From Available Answers")]
        SingleSelect,
        [Display(Name = "Select Multiple Items From Available Answers")]
        MultipleSelect,
        [Display(Name = "Remarks Only")]
        RemarksOnly
    }

}