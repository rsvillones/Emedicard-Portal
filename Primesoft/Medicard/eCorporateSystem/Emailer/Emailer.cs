using Corelib;
using Corelib.Models;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Threading;
using Corelib.Enums;

namespace Emailer
{
    public static class Emailer
    {
        #region -- Functions --

        #region -- New --

        /// <summary>
        /// Instantly sends an email of newly endorsed members.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        public static void SendProcessNewEmailInstant(EndorsementBatch endorsementBatch)
        {
            using (var db = new IdentityDataContext())
            {
                var accountSetting = db.AccountSettings.FirstOrDefault(t => t.AccountCode == endorsementBatch.AccountCode);
                if (accountSetting.DirectlySubmitToUrg)
                {
                    SendProcessNewEndorsementBatchByCorporateAdminDirectlyToUrg(endorsementBatch);
                    SendProcessNewEndorsementBatchByCorporateAdminToCorporateAdmin(endorsementBatch);
                }
                else
                {
                    SendProcessNewEndorsementBatchByCorporateAdminToCorporateAdmin(endorsementBatch);
                }
            }
        }

        /// <summary>
        /// instantly sends an email of new user accounts for newly endorsed members.
        /// </summary>
        /// <param name="member">The member entity.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public static void SendNewAccessInstant(Member member, string username, string password)
        {
            SendNewAccess(member, username, password);
        }

        /// <summary>
        /// instantly sends an email of submitted member's profile to Corporate Admin.
        /// </summary>
        /// <param name="member">The member entity.</param>
        /// <param name="bypassHRManagerApproval">if set to <c>true</c> [bypass hr manager approval].</param>
        public static void SendSubmitNewEmailInstant(Member member, bool bypassHRManagerApproval)
        {
            SendSubmitNewMemberByMemberToMember(member, bypassHRManagerApproval);
            SendSubmitNewMemberByMemberToCorporateAdmin(member, bypassHRManagerApproval);
        }

        /// <summary>
        /// instantly sends an email of submitted member's profile to URG.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        public static void SendSubmitNewEmailToUrgInstant(EndorsementBatch endorsementBatch)
        {
            SendSubmitNewMemberByMemberToUrg(endorsementBatch);
        }

        /// <summary>
        /// instantly sends an email of endorsement to URG from Corporate Admin.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        public static void SendApprovedNewEndorsementByCorpAdminToUrg(EndorsementBatch endorsementBatch)
        {
            SendProcessNewEndorsementBatchByCorporateAdminDirectlyToUrg(endorsementBatch);
        }

        public static void SendSubmitNewEmailEndOfDay()
        {
            //DateTime from, to;
            //SetEndOfDayDates(out from, out to);

            //using (var db = new IdentityDataContext())
            //{
            //    var endorsementBatchesToCorporateAdmin = db.EndorsementBatches
            //        .Include(t => t.Members)
            //        .Where(t => t.Members.Any(m => m.DateSubmittedToCorporateAdmin >= from && m.DateSubmittedToCorporateAdmin <= to)).ToList();

            //    foreach (var endorsementBatch in endorsementBatchesToCorporateAdmin)
            //    {
            //        SendSubmitNewMemberByMemberToCorporateAdmin(endorsementBatch, from, to, db);
            //    }

            //    var endorsementBatchesToUrg = db.EndorsementBatches
            //        .Include(t => t.Members)
            //        .Where(t => t.Members.Any(m => m.DateSubmittedToUrg >= from && m.DateSubmittedToUrg <= to)).ToList();

            //    foreach (var endorsementBatch in endorsementBatchesToUrg)
            //    {
            //        //SendSubmitNewMemberByMemberToUrg(endorsementBatch, from, to);
            //    }
            //}
        }

        #endregion

        #region -- Renewal --

        /// <summary>
        /// Instantly sends and email of renewed memberships.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        public static void SendProcessRenewalEmailInstant(EndorsementBatch endorsementBatch)
        {
            SendProcessRenewalByCorporateAdminToCorporateAdmin(endorsementBatch);
            //SendProcessRenewalByCorporateAdminToAllAccess(endorsementBatch);
            SendProcessRenewalByCorporateAdminToMember(endorsementBatch);
        }

        /// <summary>
        /// Sends the process renewal corporate admin to urg email instant.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        public static void SendProcessRenewalCorporateAdminToUrgEmailInstant(EndorsementBatch endorsementBatch)
        {
            SendProcessRenewalByCorporateAdminToUrg(endorsementBatch);
        }

        #endregion

        #region -- Member Cancellation --

        /// <summary>
        /// instantly sends an email of cancelled member/s to URG
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        public static void SendProcessCancelledMemberInstant(EndorsementBatch endorsementBatch)
        {
            SendProcessCancelledMemberByCorporateAdminToUrg(endorsementBatch);
            SendProcessCancelledMemberByCorporateAdminToCorporateAdmin(endorsementBatch);
        }

        #endregion

        /// <summary>
        /// Instantly sends an email of approved members to URG.
        /// </summary>
        /// <param name="member">The member entity.</param>
        public static void SendApproveNewMemberInstant(Member member)
        {
            SendApproveNewMemberByUrgToMember(member);
        }

        /// <summary>
        /// Sends an email of processed action memo to members and corporate admin
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        public static void SendActionMemoInstant(EndorsementBatch endorsementBatch)
        {
            var memberIds = new List<int>();
            SendActionMemoByUrgToCorporateAdmin(endorsementBatch);
            SendActionMemoByUrgToAllAccess(endorsementBatch);
            foreach (var actionMemo in endorsementBatch.ActionMemos)
            {
                if (actionMemo.MemberId.HasValue && memberIds.Contains(actionMemo.MemberId.Value)) continue;
                SendActionMemoByUrgToMember(actionMemo);
                memberIds.Add(actionMemo.MemberId.Value);
            }
        }

        /// <summary>
        /// Sends an email of action memo for cancellation.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        public static void SendActionMemoForCancellationInstant(EndorsementBatch endorsementBatch)
        {
            SendActionMemoForCancellationByUrgToCorporateAdmin(endorsementBatch);
            SendActionMemoForCancellationByUrgToAllAccess(endorsementBatch);
        }

        /// <summary>
        /// Sends an email notification memo action memo replied by member.
        /// </summary>
        /// <param name="actionMemo">The action memo entity.</param>
        public static void SendActionMemoReplyInstant(ActionMemo actionMemo)
        {
            SendActionMemoReplyByCorporateAdminToMember(actionMemo);
            SendActionMemoReplyCorporateAdminToCorporateAdmin(actionMemo);
        }

        /// <summary>
        /// Sends an email notification of action memo replied by corporate admin.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        public static void SendActionMemoCorporateAdminReplyInstant(EndorsementBatch endorsementBatch)
        {
            SendActionMemoReplyByCorporateAdminToUrg(endorsementBatch);
        }

        /// <summary>
        /// Sends an email of action memo replied by member.
        /// </summary>
        /// <param name="actionMemo">The action memo entity.</param>
        /// <param name="bypassHrApproval">if set to <c>true</c> [bypass hr approval].</param>
        public static void SendActionMemoReplyByMemberInstant(ActionMemo actionMemo, bool bypassHrApproval)
        {
            SendActionMemoReplyByMemberToMember(actionMemo, bypassHrApproval);
            SendActionMemoReplyByMemberToCorporateAdmin(actionMemo, bypassHrApproval);
        }

        /// <summary>
        /// Sends an email of action memo by the end of day.
        /// </summary>
        public static void SentActionMemoReplyEndOfDay()
        {
            DateTime from, to;
            SetEndOfDayDates(out from, out to);

            using (var db = new IdentityDataContext())
            {
                var actionMemosToCorporateAdmin = db.ActionMemos
                    .Include(t => t.Member)
                    .Include(t => t.Dependent)
                    .Include(t => t.EndorsementBatch)
                    .Where(t => t.DateSubmittedToCorporateAdmin >= from && t.DateSubmittedToCorporateAdmin <= to).ToList();

                var accountCodes = actionMemosToCorporateAdmin.Select(t => t.EndorsementBatch.AccountCode).Distinct();
                foreach (var accountCode in accountCodes)
                {
                    SendActionMemoReplyByMemberToCorporateAdmin(accountCode, actionMemosToCorporateAdmin.Where(t => t.EndorsementBatch.AccountCode == accountCode).ToList());
                }

                var actionMemosToUrg = db.ActionMemos
                    .Include(t => t.Member)
                    .Include(t => t.Dependent)
                    .Include(t => t.EndorsementBatch)
                    .Where(t => t.DateSubmittedToUrg >= from && t.DateSubmittedToUrg <= to).ToList();

                accountCodes = actionMemosToUrg.Select(t => t.EndorsementBatch.AccountCode).Distinct();
                foreach (var accountCode in accountCodes)
                {
                    SendActionMemoReplyByMemberToUrg(accountCode, actionMemosToUrg.Where(t => t.EndorsementBatch.AccountCode == accountCode).ToList());
                }

            }
        }

        /// <summary>
        /// Sends an email of transmittal.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch.</param>
        public static void SendTransmittalInstant(EndorsementBatch endorsementBatch)
        {
            SendTransmittalByUrgToCorporateAdmin(endorsementBatch);
            SendTransmittalByUrgToAllAccess(endorsementBatch);
            foreach (var member in endorsementBatch.Members)
            {
                if (member.Status != Corelib.Enums.MembershipStatus.Approved) continue;

                SendTransmittalByUrgToMember(member);
            }
        }

        #region -- ID Replacement --

        /// <summary>
        /// Sends an email of submitted Id replacement.
        /// </summary>
        /// <param name="idReplacement">The IdReplacement entity.</param>
        /// <param name="bypassHRManagerApproval">if set to <c>true</c> [bypass hr manager approval].</param>
        public static void SendSubmitIdReplacementEmailInstant(IdReplacement idReplacement, bool bypassHRManagerApproval)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == idReplacement.MemberCode) ?? new Member();
                idReplacement.EndorsementBatch = db.EndorsementBatches.FirstOrDefault(t => t.Id == idReplacement.EndorsementBatchId);
                idReplacement.Reason = db.Reasons.FirstOrDefault(t => t.Id == idReplacement.ReasonId);
                if (idReplacement.Status == Corelib.Enums.RequestStatus.CorporateAdminApproved || bypassHRManagerApproval)
                {
                    SendSubmitIdReplacementByMemberToUrg(idReplacement, member);
                }
                else if (idReplacement.Status == Corelib.Enums.RequestStatus.Submitted)
                {
                    SendSubmitIdReplacementByMemberToCorporateAdmin(idReplacement, member);
                }
                SendSubmitIdReplacementByMemberToMember(member, bypassHRManagerApproval);
            }
        }

        /// <summary>
        /// Sends an email of submitted id replacement approved by corporate admin.
        /// </summary>
        /// <param name="idReplacement">The identifier replacement entity.</param>
        public static void SendSubmitIdReplacementCorporateAdminEmailInstant(IdReplacement idReplacement)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == idReplacement.MemberCode) ?? new Member();
                idReplacement.EndorsementBatch = db.EndorsementBatches.FirstOrDefault(t => t.Id == idReplacement.EndorsementBatchId);
                idReplacement.Reason = db.Reasons.FirstOrDefault(t => t.Id == idReplacement.ReasonId);
                //SendSubmitIdReplacementByCorporateAdminToMember(member);
                SendSubmitIdReplacementByCorporateAdminToUrg(idReplacement);
            }
        }

        /// <summary>
        /// Sends an email of id replacement cancelled by corporate admin.
        /// </summary>
        /// <param name="idReplacement">The id replacement entity.</param>
        public static void SendCancelledIdReplacementCorporateAdminToMemberEmailInstant(IdReplacement idReplacement)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == idReplacement.MemberCode) ?? new Member();
                SendCancelledIdReplacementByCorporateAdminToMember(member);
            }
        }

        /// <summary>
        /// Sends an email of id replacement disapproved by corporate admin.
        /// </summary>
        /// <param name="idReplacement">The id replacement entity.</param>
        public static void SendDisapproveIdReplacementCorporateAdminToMemberEmailInstant(IdReplacement idReplacement)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == idReplacement.MemberCode) ?? new Member();
                SendDisapproveIdReplacementByCorporateAdminToMember(member);
            }
        }

        /// <summary>
        /// Sends an email of id replacement declined by corporate admin.
        /// </summary>
        /// <param name="idReplacement">The id replacement entity.</param>
        /// <param name="urgRemark">The urg remark.</param>
        public static void SendDeclinedIdReplacementUrgEmailInstant(IdReplacement idReplacement, string urgRemark)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == idReplacement.MemberCode) ?? new Member();
                idReplacement.Reason = db.Reasons.FirstOrDefault(t => t.Id == idReplacement.ReasonId);
                SendDisapproveIdReplacementByUrgToMember(idReplacement, urgRemark, member.EmailAddress);
                SendDisapproveIdReplacementByUrgToCorporateAdmin(idReplacement, urgRemark);
            }
        }

        #endregion

        #region -- Amendment --

        /// <summary>
        /// Sends an email of submitted amendment.
        /// </summary>
        /// <param name="amendment">The amendment entity.</param>
        /// <param name="bypassHRManagerApproval">if set to <c>true</c> [bypass hr manager approval].</param>
        public static void SendSubmitAmendmentEmailInstant(Amendment amendment, bool bypassHRManagerApproval)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == amendment.MemberCode) ?? new Member();
                amendment.EndorsementBatch = db.EndorsementBatches.FirstOrDefault(t => t.Id == amendment.EndorsementBatchId);
                amendment.Reason = db.Reasons.FirstOrDefault(t => t.Id == amendment.ReasonId);
                if (amendment.Status == Corelib.Enums.RequestStatus.CorporateAdminApproved || bypassHRManagerApproval)
                {
                    SendSubmitAmendmentByMemberToUrg(amendment, member);
                }
                else if (amendment.Status == Corelib.Enums.RequestStatus.Submitted)
                {
                    SendSubmitAmendmentByMemberToCorporateAdmin(amendment, member);
                }
                SendSubmitAmendmentByMemberToMember(member, bypassHRManagerApproval);
            }
        }

        /// <summary>
        /// Sends an email of submitted amendment by corporate admin.
        /// </summary>
        /// <param name="amendment">The amendment entity.</param>
        public static void SendSubmitAmendmentCorporateAdminEmailInstant(Amendment amendment)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == amendment.MemberCode) ?? new Member();
                amendment.EndorsementBatch = db.EndorsementBatches.FirstOrDefault(t => t.Id == amendment.EndorsementBatchId);
                amendment.Reason = db.Reasons.FirstOrDefault(t => t.Id == amendment.ReasonId);
                //SendSubmitAmendmentByCorporateAdminToMember(member);
                SendSubmitAmendmentByCorporateAdminToUrg(amendment);
            }
        }

        /// <summary>
        /// Sends an email of cancelled amendment by corporate admin.
        /// </summary>
        /// <param name="amendment">The amendment entity.</param>
        public static void SendCancelledAmendmentCorporateAdminToMemberEmailInstant(Amendment amendment)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == amendment.MemberCode) ?? new Member();
                SendCancelledAmendmentByCorporateAdminToMember(member);
            }
        }

        /// <summary>
        /// Sends an email of disapproved amendment by corporate admin.
        /// </summary>
        /// <param name="amendment">The amendment entity.</param>
        public static void SendDisapproveAmendmentCorporateAdminToMemberEmailInstant(Amendment amendment)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == amendment.MemberCode) ?? new Member();
                SendDisapproveAmendmentByCorporateAdminToMember(member);
            }
        }

        /// <summary>
        /// Sends an email of declained amendment by URG.
        /// </summary>
        /// <param name="amendment">The amendment.</param>
        /// <param name="urgRemark">The urg remark.</param>
        public static void SendDeclainedAmendmentUrgEmailInstant(Amendment amendment, string urgRemark)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == amendment.MemberCode) ?? new Member();
                amendment.Reason = db.Reasons.FirstOrDefault(t => t.Id == amendment.ReasonId);
                SendDeclinedAmendmentByUrgToMember(amendment, urgRemark, member.EmailAddress);
                SendDeclinedAmendmentByUrgToCorporateAdmin(amendment, urgRemark);
            }
        }

        #endregion

        #region -- Additionl Dependent --

        /// <summary>
        /// Sends an email of submitted Additional Dependent.
        /// </summary>
        /// <param name="additionalDependent">The additional dependent entity.</param>
        /// <param name="bypassHRManagerApproval">if set to <c>true</c> [bypass hr manager approval].</param>
        public static void SendSubmitAdditionalDependentEmailInstant(AdditionalDependent additionalDependent, bool bypassHRManagerApproval)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == additionalDependent.MemberCode) ?? new Member();
                additionalDependent.EndorsementBatch = db.EndorsementBatches.FirstOrDefault(t => t.Id == additionalDependent.EndorsementBatchId);
                additionalDependent.Relationship = db.Relationships.FirstOrDefault(t => t.Code == additionalDependent.RelationshipCode);
                if (additionalDependent.Status == Corelib.Enums.RequestStatus.CorporateAdminApproved || bypassHRManagerApproval)
                {
                    SendSubmitAdditionalDependentByMemberToUrg(additionalDependent, member);
                }
                else if (additionalDependent.Status == Corelib.Enums.RequestStatus.Submitted)
                {
                    SendSubmitAdditionalDependentByMemberToCorporateAdmin(additionalDependent, member);
                }
                SendSubmitAdditionalDependentByMemberToMember(member, bypassHRManagerApproval);
            }
        }

        /// <summary>
        /// Sends an email of submitted Additional Dependent by corporate admin.
        /// </summary>
        /// <param name="additionalDependent">The additional dependent entity.</param>
        public static void SendSubmitAdditionalDependentCorporateAdminEmailInstant(AdditionalDependent additionalDependent)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == additionalDependent.MemberCode) ?? new Member();
                additionalDependent.EndorsementBatch = db.EndorsementBatches.FirstOrDefault(t => t.Id == additionalDependent.EndorsementBatchId);
                additionalDependent.Relationship = db.Relationships.FirstOrDefault(t => t.Code == additionalDependent.RelationshipCode);
                //SendSubmitAdditionalDependentByCorporateAdminToMember(member);
                SendSubmitAdditionalDependentByCorporateAdminToUrg(additionalDependent);
            }
        }

        /// <summary>
        /// Sends an email of cancelled Additional Dependent by corporate admin.
        /// </summary>
        /// <param name="additionalDependent">The additional dependent entity.</param>
        public static void SendCancelledAdditionalDependentCorporateAdminToMemberEmailInstant(AdditionalDependent additionalDependent)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == additionalDependent.MemberCode) ?? new Member();
                SendCancelledAdditionalDependentByCorporateAdminToMember(member);
            }
        }

        /// <summary>
        /// Sends an email of disapproved Additional Dependent by corporate admin.
        /// </summary>
        /// <param name="additionalDependent">The additional dependent entity.</param>
        public static void SendDisapproveAdditionalDependentCorporateAdminToMemberEmailInstant(AdditionalDependent additionalDependent)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == additionalDependent.MemberCode) ?? new Member();
                SendDisapproveAdditionalDependentByCorporateAdminToMember(member);
            }
        }

        #endregion

        #region -- Dependent Cancellation --

        /// <summary>
        /// Sends an email of submitted Dependent Cancellation.
        /// </summary>
        /// <param name="dependentCancellation">The dependent cancellation entity.</param>
        /// <param name="bypassHRManagerApproval">if set to <c>true</c> [bypass hr manager approval].</param>
        public static void SendSubmitDependentCancellationEmailInstant(DependentCancellation dependentCancellation, bool bypassHRManagerApproval)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == dependentCancellation.MemberCode) ?? new Member();
                dependentCancellation.EndorsementBatch = db.EndorsementBatches.FirstOrDefault(t => t.Id == dependentCancellation.EndorsementBatchId);
                dependentCancellation.Reason = db.Reasons.FirstOrDefault(t => t.Id == dependentCancellation.ReasonId);

                if (dependentCancellation.Status == Corelib.Enums.RequestStatus.CorporateAdminApproved || bypassHRManagerApproval)
                {
                    SendSubmitDependentCancellationByMemberToUrg(dependentCancellation, member);
                }
                else if (dependentCancellation.Status == Corelib.Enums.RequestStatus.Submitted)
                {
                    SendSubmitDependentCancellationByMemberToCorporateAdmin(dependentCancellation, member);
                }
                SendSubmitDependentCancellationByMemberToMember(member, bypassHRManagerApproval);
            }
        }

        /// <summary>
        /// Sends an email of submitted Dependent Cancellation by corporate admin.
        /// </summary>
        /// <param name="dependentCancellation">The dependent cancellation entity.</param>
        public static void SendSubmitDependentCancellationCorporateAdminEmailInstant(DependentCancellation dependentCancellation)
        {
            using (var db = new IdentityDataContext())
            {
                var member = db.Members.FirstOrDefault(t => t.Code == dependentCancellation.MemberCode) ?? new Member();
                dependentCancellation.EndorsementBatch = db.EndorsementBatches.FirstOrDefault(t => t.Id == dependentCancellation.EndorsementBatchId);
                dependentCancellation.Reason = db.Reasons.FirstOrDefault(t => t.Id == dependentCancellation.ReasonId);
                //SendSubmitDependentCancellationByCorporateAdminToMember(member);
                SendSubmitDependentCancellationByCorporateAdminToUrg(dependentCancellation);
            }
        }

        /// <summary>
        /// Sends an email of cancelled Dependent Cancellation by corporate admin.
        /// </summary>
        /// <param name="dependentCancellation">The dependent cancellation entity.</param>
        public static void SendCancelledDependentCancellationCorporateAdminToMemberEmailInstant(DependentCancellation dependentCancellation)
        {
            //using (var db = new IdentityDataContext())
            //{
            //    var member = db.Members.FirstOrDefault(t => t.Code == dependentCancellation.MemberCode) ?? new Member();
            //    SendCancelledDependentCancellationByCorporateAdminToMember(member);
            //}
        }

        /// <summary>
        /// Sends an email of disapproved Dependent Cancellation by corporate admin.
        /// </summary>
        /// <param name="dependentCancellation">The dependent cancellation.</param>
        public static void SendDisapproveDependentCancellationCorporateAdminToMemberEmailInstant(DependentCancellation dependentCancellation)
        {
            //using (var db = new IdentityDataContext())
            //{
            //    var member = db.Members.FirstOrDefault(t => t.Code == dependentCancellation.MemberCode) ?? new Member();
            //    SendDisapproveDependentCancellationByCorporateAdminToMember(member);
            //}
        }

        #endregion

        #region -- Endorsement Listing --

        /// <summary>
        /// Sends an email of cancelled membership by corporate admin.
        /// </summary>
        /// <param name="member">The member entity.</param>
        public static void SendCancelMembershipCorporateAdminToMemberEmailInstant(Member member)
        {
            SendCancelMembershipByCorporateAdminToMember(member);
        }

        /// <summary>
        /// Sends an email of returned membership by corporate admin.
        /// </summary>
        /// <param name="member">The member entity.</param>
        public static void SendReturnMembershipCorporateAdminToMemberEmailInstant(Member member)
        {
            SendReturnMembershipByCorporateAdminToMember(member);
        }

        /// <summary>
        /// Sends an email of approved membership by corporate admin.
        /// </summary>
        /// <param name="member">The member entity.</param>
        public static void SendApproveMembershipCorporateAdminToMemberEmailInstant(Member member)
        {
            SendApproveMembershipByCorporateAdminToMember(member);
        }

        #endregion

        #region -- Receiving Entry --

        /// <summary>
        /// Sends an email of receiving entries.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        public static void SendReceivingEntryInstant(EndorsementBatch endorsementBatch)
        {
            SendReceivingEntryUrgToCorporateAdmin(endorsementBatch);
            SendReceivingEntryUrgToAllAccess(endorsementBatch);
        }

        /// <summary>
        /// Sends an email of receiving enties by URG to Assigned User.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batchentity.</param>
        /// <param name="assignUser">The assign user.</param>
        public static void SendReceivingEntryUrgToAssignUserInstant(EndorsementBatch endorsementBatch, string assignUser)
        {
            SendReceivingEntryUrgToUrg(endorsementBatch, assignUser);
        }

        #endregion

        #region -- Submittion Deadline --

        /// <summary>
        /// Sends an email of submission deadline of membership.
        /// </summary>
        public static void SendMemberSubmittionDeadline()
        {
            using (var db = new IdentityDataContext())
            {
                var deadline = DateTime.Now.AddDays(Config.SendEmailNotificationBeforeDeadline).Date;
                var endorsementBatches = db.EndorsementBatches
                    .Include(t => t.Members)
                    .Where(t => (int)t.Status <= (int)EndorsementBatchStatus.PartiallyForProcessing &&
                        t.Deadline == deadline &&
                        t.Members.Any(a => (int)a.Status <= (int)MembershipStatus.Saved) &&
                        (t.EndorsementType == Constants.RENEWAL_ENDORSEMENT_TYPE || t.EndorsementType == Constants.NEW_ENDORSEMENT_TYPE))
                        .ToList();

                foreach (var endorsement in endorsementBatches)
                {
                    foreach (var member in endorsement.Members)
                    {
                        if (member.Status != MembershipStatus.Saved && member.Status != MembershipStatus.New) continue;
                        SendSubmittionDeadlineNotification(member, endorsement.Deadline);
                    }
                }
            }
        }

        #endregion

        #region -- Member Migration --

        /// <summary>
        /// Sends an email of created user accounts to corporate admin.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <param name="members">The members entity.</param>
        public static void SendCreatedUserAccounts(string accountCode, List<Member> members)
        {
            SendCreatedUserAccountToCorporateAdmin(members, accountCode);
        }

        #endregion

        #endregion

        #region -- Sending Emails --

        #region -- New Access --

        /// <summary>
        /// sets the content of email for new access
        /// </summary>
        /// <param name="member">The member entity.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        private static void SendNewAccess(Member member, string username, string password)
        {
            if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrEmpty(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.NewAccess, new EmailViewModel()
            {
                Member = member,
                Username = username,
                Password = password
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: New Member Access",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(member.EmailAddress);
            SendMailMessage(mailMessage);
        }

        #endregion

        #region -- Process New Endorsement --

        /// <summary>
        /// set the content of email for new endorsement batch processed by corporate admin to itself.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendProcessNewEndorsementBatchByCorporateAdminToCorporateAdmin(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.ProcessNewEndorsementBatchByCorporateAdminToCorporateAdmin, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMedicard: Additional Member",
                Body = body,
                IsBodyHtml = true
            };
            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail) SendMailMessage(mailMessage);
        }

        /// <summary>   
        /// set the content of email for new endorsement batch processed by corporate admin to members.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendProcessNewEndorsementBatchByCorporateAdminToMember(EndorsementBatch endorsementBatch)
        {
            foreach (var member in endorsementBatch.Members)
            {
                if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrEmpty(member.EmailAddress)) continue;

                var body = ParseEmailTemplate(Config.ProcessNewEndorsementBatchByCorporateAdminToMember, new EmailViewModel()
                {
                    Member = member
                });
                var mailMessage = new MailMessage()
                {
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "eMediCard: New Member Access",
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(member.EmailAddress);
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// set the content of email for new endorsement batch processed by corporate admin to URG.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendProcessNewEndorsementBatchByCorporateAdminDirectlyToUrg(EndorsementBatch endorsementBatch)
        {
            var title = "ADDITIONAL MEMBER";
            if (endorsementBatch.EndorsementType == Corelib.Constants.RENEWAL_ENDORSEMENT_TYPE)
            {
                title = "MEMBERS FOR RENEWAL";
            }

            var body = ParseEmailTemplate(Config.ProcessNewDirectlyToUrgByCorporateAdminToUrg, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch,
                Count = endorsementBatch.Members.Where(t => t.Status == MembershipStatus.CorporateAdminApproved).ToList().Count()
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard: {0} - {1}", endorsementBatch.CompanyName, title),
                Body = body,
                IsBodyHtml = true
            };
            var sendEmail = false;
            foreach (var email in GetUnderwriterEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            //var attachmentPath = String.Format(@"{0}{1}", Config.UploadedFilesPath, endorsementBatch.GuidFilename);
            //if (!String.IsNullOrEmpty(endorsementBatch.GuidFilename) && File.Exists(attachmentPath))
            //{
            //    mailMessage.Attachments.Add(new Attachment(attachmentPath));
            //}

            if (sendEmail)
            {
                mailMessage.CC.Add(new MailAddress(string.Format("{0}@medicardphils.com", LegacyHelper.GetAssignedProcessor(endorsementBatch.AccountCode))));
                var accountSetting = GetAccountSetting(endorsementBatch.AccountCode);
                if (accountSetting.BypassHRManagerApproval)
                {
                    foreach (var email in GetCorporateAdminEmails(endorsementBatch.AccountCode))
                    {
                        mailMessage.CC.Add(new MailAddress(email));
                        sendEmail = true;
                    }
                }
                SendMailMessage(mailMessage);
            };
        }

        #endregion

        #region -- Submit New Endorsement --

        /// <summary>
        /// set the content of email for submitted membership profile by member to itself.
        /// </summary>
        /// <param name="member">The member entity.</param>
        /// <param name="bypassCorporateAdminApproval">if set to <c>true</c> [bypass corporate admin approval].</param>
        private static void SendSubmitNewMemberByMemberToMember(Member member, bool bypassCorporateAdminApproval)
        {
            if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.SubmitNewMemberByMemberToMember, new EmailViewModel()
            {
                Member = member,
                BypassCorporateAdminApproval = bypassCorporateAdminApproval
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Membership Information Submitted",
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(new MailAddress(member.EmailAddress));

            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// set the content of email for submitted membership profile by member to corporate admin.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        /// <param name="from">date From.</param>
        /// <param name="to">date To.</param>
        /// <param name="db">The database context.</param>
        private static void SendSubmitNewMemberByMemberToCorporateAdmin(EndorsementBatch endorsementBatch, DateTime from, DateTime to, IdentityDataContext db)
        {
            var accountSetting = db.AccountSettings.FirstOrDefault(t => t.AccountCode == endorsementBatch.AccountCode) ?? new AccountSetting();
            var body = ParseEmailTemplate(Config.SubmitNewMemberByMemberToCorporateAdmin, new EmailViewModel()
            {
                From = from,
                To = to,
                EndorsementBatch = endorsementBatch,
                BypassCorporateAdminApproval = accountSetting.BypassHRManagerApproval
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = accountSetting.BypassHRManagerApproval ? "eMediCard: Membership Information Submitted to MediCard" : "eMediCard: Membership Information Submitted for Evaluation",
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail) SendMailMessage(mailMessage);
        }

        /// <summary>
        /// set the content of submitted membership profile by member to corporate admin.
        /// </summary>
        /// <param name="member">The member entity.</param>
        /// <param name="bypassHRManagerApproval">if set to <c>true</c> [bypass hr manager approval].</param>
        private static void SendSubmitNewMemberByMemberToCorporateAdmin(Member member, bool bypassHRManagerApproval)
        {
            var body = ParseEmailTemplate(Config.SubmitNewMemberByMemberToCorporateAdmin, new EmailViewModel()
            {
                Member = member,
                BypassCorporateAdminApproval = bypassHRManagerApproval
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = bypassHRManagerApproval ? "eMediCard: Membership Information Submitted to MediCard" : "eMediCard: Membership Information Submitted for Evaluation",
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(member.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail) SendMailMessage(mailMessage);
        }

        /// <summary>
        /// set the content of email for submitted membership profile by member to URG.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendSubmitNewMemberByMemberToUrg(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.SubmitNewMemberByMemberToUrg, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch,
                Count = endorsementBatch.Members.Where(t => t.Status == MembershipStatus.CorporateAdminApproved).ToList().Count()
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} Membership Information Submitted to MediCard", endorsementBatch.CompanyName),
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetUnderwriterEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                mailMessage.CC.Add(new MailAddress(string.Format("{0}@medicardphils.com", LegacyHelper.GetAssignedProcessor(endorsementBatch.AccountCode))));
                var accountSetting = GetAccountSetting(endorsementBatch.AccountCode);
                if (accountSetting.BypassHRManagerApproval)
                {
                    foreach (var email in GetCorporateAdminEmails(endorsementBatch.AccountCode))
                    {
                        mailMessage.CC.Add(new MailAddress(email));
                    }
                }
                SendMailMessage(mailMessage);
            }
        }

        #endregion

        #region -- Approve New Endorsement --

        /// <summary>
        /// set the content of email for approved membership by URG to corporate admin.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendApproveNewMemberByUrgToCorporateAdmin(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.ApproveNewMemberByUrgToCorporateAdmin, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - APPROVED MEMBERS", Config.GetCompanyName(endorsementBatch.AccountCode)),
                Body = body,
                IsBodyHtml = true
            };
            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                foreach (var email in GetUnderwriterEmails(endorsementBatch.AccountCode))
                {
                    mailMessage.CC.Add(new MailAddress(email));
                    sendEmail = true;
                }
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// set the content of email for approved membership by URG to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendApproveNewMemberByUrgToMember(Member member)
        {
            if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.ApproveNewMemberByUrgToMember, new EmailViewModel()
            {
                Member = member
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - APPROVED MEMBERS", Config.GetCompanyName(member.AccountCode)),
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));

            SendMailMessage(mailMessage);
        }

        #endregion

        #region -- Action Memo --

        /// <summary>
        /// set the content of email for action memo by URG to corporate admin.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendActionMemoByUrgToCorporateAdmin(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.ActionMemoByUrgToCorporateAdmin, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Members with Action Memo",
                Body = body,
                IsBodyHtml = true
            };

            var di = new DirectoryInfo(Config.ActionMemoDocumentsPath);
            foreach (var fi in di.GetFiles(String.Format("A_{0}*.pdf", endorsementBatch.ReplyTo)))
            {
                mailMessage.Attachments.Add(new System.Net.Mail.Attachment(fi.FullName));
            }

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                foreach (var email in GetUnderwriterEmails(endorsementBatch.AccountCode))
                {
                    mailMessage.CC.Add(new MailAddress(email));
                }
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of email for action memo for cancellation by urg to corporate admin.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendActionMemoForCancellationByUrgToCorporateAdmin(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.ActionMemoForCancellationByUrgToCorporateAdmin, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Action Memo",
                Body = body,
                IsBodyHtml = true
            };

            var di = new DirectoryInfo(Config.ActionMemoDocumentsPath);
            foreach (var fi in di.GetFiles(String.Format("A_{0}*.pdf", endorsementBatch.ControlNumber)))
            {
                mailMessage.Attachments.Add(new System.Net.Mail.Attachment(fi.FullName));
            }

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                foreach (var email in GetUnderwriterEmails(endorsementBatch.AccountCode))
                {
                    mailMessage.CC.Add(new MailAddress(email));
                }
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of email for action memo for cancellation by urg to all access.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendActionMemoForCancellationByUrgToAllAccess(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.ActionMemoForCancellationByUrgToCorporateAdmin, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Action Memo (All Access)",
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetOtherEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of action memo by urg to all access.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendActionMemoByUrgToAllAccess(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.ActionMemoByUrgToCorporateAdmin, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: MEMBERS WITH ACTION MEMO",
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetOtherEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail) SendMailMessage(mailMessage);
        }   

        /// <summary>
        /// sets the content of email for the action memo by urg to member.
        /// </summary>
        /// <param name="actionMemo">The action memo entity.</param>
        private static void SendActionMemoByUrgToMember(ActionMemo actionMemo)
        {
            if (actionMemo.Member == null || string.IsNullOrEmpty(actionMemo.Member.EmailAddress) || string.IsNullOrWhiteSpace(actionMemo.Member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.ActionMemoByUrgToMember, new EmailViewModel()
            {
                ActionMemo = actionMemo
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Action Memo",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(actionMemo.Member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the action memo reply by member to corporate admin.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <param name="actionMemos">The collection of action memos entity.</param>
        private static void SendActionMemoReplyByMemberToCorporateAdmin(string accountCode, IEnumerable<ActionMemo> actionMemos)
        {
            var body = ParseEmailTemplate(Config.ActionMemoReplyByMemberToCorporateAdmin, new EmailViewModel()
            {
                ActionMemos = actionMemos
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Action Memo Reply Submitted",
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(accountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail) SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the action memo reply by member to corporate admin.
        /// </summary>
        /// <param name="actionMemo">The action memo entity.</param>
        /// <param name="bypassHrApproval">if set to <c>true</c> [bypass hr approval].</param>
        private static void SendActionMemoReplyByMemberToCorporateAdmin(ActionMemo actionMemo, bool bypassHrApproval)
        {
            var companyName = Config.GetCompanyName(actionMemo.EndorsementBatch.AccountCode);
            var body = ParseEmailTemplate(Config.ActionMemoReplyByMemberToCorporateAdmin, new EmailViewModel()
            {
                ActionMemo = actionMemo,
                CompanyName = companyName,
                BypassCorporateAdminApproval = bypassHrApproval
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Action Memo Reply for Evaluation",
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(actionMemo.EndorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail) SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the action memo reply by member to member.
        /// </summary>
        /// <param name="actionMemo">The action memo entity.</param>
        /// <param name="bypassHrApproval">if set to <c>true</c> [bypass hr approval].</param>
        private static void SendActionMemoReplyByMemberToMember(ActionMemo actionMemo, bool bypassHrApproval)
        {
            if (actionMemo.Member == null || string.IsNullOrEmpty(actionMemo.Member.EmailAddress) || string.IsNullOrWhiteSpace(actionMemo.Member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.ActionMemoReplyByMemberToMember, new EmailViewModel()
            {
                ActionMemo = actionMemo,
                BypassCorporateAdminApproval = bypassHrApproval
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Action Memo Reply Submitted to MediCard",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(actionMemo.Member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the action memo reply by member to urg.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <param name="actionMemos">The collection of action memos entity.</param>
        private static void SendActionMemoReplyByMemberToUrg(string accountCode, IEnumerable<ActionMemo> actionMemos)
        {
            var companyName = Config.GetCompanyName(accountCode);
            var body = ParseEmailTemplate(Config.ActionMemoReplyByMemberToUrg, new EmailViewModel()
            {
                ActionMemos = actionMemos,
                CompanyName = companyName
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - REPLY TO ACTION MEMO", companyName),
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetUnderwriterEmails(accountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                mailMessage.CC.Add(new MailAddress(string.Format("{0}@medicardphils.com", LegacyHelper.GetAssignedProcessor(accountCode))));
                var accountSetting = GetAccountSetting(accountCode);
                if (accountSetting.BypassHRManagerApproval)
                {
                    foreach (var email in GetCorporateAdminEmails(accountCode))
                    {
                        mailMessage.CC.Add(new MailAddress(email));
                    }
                }
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of email for the action memo reply by corporate admin to member.
        /// </summary>
        /// <param name="actionMemo">The action memo entity.</param>
        private static void SendActionMemoReplyByCorporateAdminToMember(ActionMemo actionMemo)
        {
            if (string.IsNullOrEmpty(actionMemo.Member.EmailAddress) || string.IsNullOrWhiteSpace(actionMemo.Member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.ActionMemoReplyByCorporateAdminToMember, new EmailViewModel()
            {
                ActionMemo = actionMemo
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Action Memo Reply Submitted to MediCard",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(actionMemo.Member.EmailAddress));

            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the action memo reply corporate admin to corporate admin.
        /// </summary>
        /// <param name="actionMemo">The action memo entity.</param>
        private static void SendActionMemoReplyCorporateAdminToCorporateAdmin(ActionMemo actionMemo)
        {
            var companyName = Config.GetCompanyName(actionMemo.EndorsementBatch.AccountCode);
            var body = ParseEmailTemplate(Config.ActionMemoReplyByCorporateAdminToCorporateAdmin, new EmailViewModel()
            {
                ActionMemo = actionMemo,
                CompanyName = companyName
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Action Memo Reply Submitted",
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(actionMemo.EndorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail) SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the action memo reply by corporate admin to urg.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendActionMemoReplyByCorporateAdminToUrg(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.ActionMemoReplyByCorporateAdminToUrg, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch,
                Count = endorsementBatch.ActionMemos.Where(t => t.Status == ActionMemoStatus.Replied).ToList().Count()
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - REPLY TO ACTION MEMO", Config.GetCompanyName(endorsementBatch.AccountCode)),
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetUnderwriterEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                mailMessage.CC.Add(new MailAddress(string.Format("{0}@medicardphils.com", LegacyHelper.GetAssignedProcessor(endorsementBatch.AccountCode))));
                var accountSetting = GetAccountSetting(endorsementBatch.AccountCode);
                if (accountSetting.BypassHRManagerApproval)
                {
                    foreach (var email in GetCorporateAdminEmails(endorsementBatch.AccountCode))
                    {
                        mailMessage.CC.Add(new MailAddress(email));
                    }
                }
                SendMailMessage(mailMessage);
            }
        }

        #endregion

        #region -- Transmittal --

        /// <summary>
        /// sets the content of email for the transmittal by urg to corporate admin.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendTransmittalByUrgToCorporateAdmin(EndorsementBatch endorsementBatch)
        {
            var count = endorsementBatch.Members.Where(t => t.Status == MembershipStatus.Approved || t.Status == MembershipStatus.VerifyMembership).ToList().Count();

            var body = ParseEmailTemplate(Config.TransmittalByUrgToCorporateAdmin, new EmailViewModel()
            {
                Count = count,
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Approved Members",
                Body = body,
                IsBodyHtml = true
            };

            var di = new DirectoryInfo(Config.TransmittalDocumentsPath);
            foreach (var fi in di.GetFiles(String.Format("T_{0}*.pdf", endorsementBatch.ControlNumber)))
            {
                mailMessage.Attachments.Add(new System.Net.Mail.Attachment(fi.FullName));
            }

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                foreach (var email in GetUnderwriterEmails(endorsementBatch.AccountCode))
                {
                    mailMessage.CC.Add(new MailAddress(email));
                }
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of email for the transmittal by urg to all access.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendTransmittalByUrgToAllAccess(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.TransmittalByUrgToCorporateAdmin, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Approved Members",
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetOtherEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail) SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the transmittal by urg to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendTransmittalByUrgToMember(Member member)
        {
            if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.TransmittalByUrgToMember, new EmailViewModel()
            {
                Member = member
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Approved Member",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        #endregion

        #region -- Process Renewal --

        /// <summary>
        /// sets the content of email for the process renewal by corporate admin to corporate admin.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendProcessRenewalByCorporateAdminToCorporateAdmin(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.ProcessRenewalByCorporateAdminToCorporateAdmin, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMedicard: Members for Renewal",
                Body = body,
                IsBodyHtml = true
            };
            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail) SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the process renewal by corporate admin to all access.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendProcessRenewalByCorporateAdminToAllAccess(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.ProcessRenewalByCorporateAdminToCorporateAdmin, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMedicard: Members for Renewal (All Access)",
                Body = body,
                IsBodyHtml = true
            };
            var sendEmail = false;
            foreach (var email in GetOtherEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail) SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the process renewal by corporate admin to member.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendProcessRenewalByCorporateAdminToMember(EndorsementBatch endorsementBatch)
        {
            foreach (var member in endorsementBatch.Members)
            {
                if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrEmpty(member.EmailAddress)) continue;

                var body = ParseEmailTemplate(Config.ProcessRenewalByCorporateAdminToMember, new EmailViewModel()
                {
                    Member = member
                });
                var mailMessage = new MailMessage()
                {
                    From = new MailAddress(Config.NotificationFromEmail),
                    Subject = "eMediCard: Membership Renewal",
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(member.EmailAddress);
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of email for the process renewal by corporate admin to urg.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendProcessRenewalByCorporateAdminToUrg(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.ProcessRenewalByCorporateAdminToUrg, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - MEMBERS FOR RENEWAL", endorsementBatch.CompanyName),
                Body = body,
                IsBodyHtml = true
            };
            var sendEmail = false;
            foreach (var email in GetUnderwriterEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                mailMessage.CC.Add(new MailAddress(string.Format("{0}@medicardphils.com", LegacyHelper.GetAssignedProcessor(endorsementBatch.AccountCode))));
                var accountSetting = GetAccountSetting(endorsementBatch.AccountCode);
                if (accountSetting.BypassHRManagerApproval)
                {
                    foreach (var email in GetCorporateAdminEmails(endorsementBatch.AccountCode))
                    {
                        mailMessage.CC.Add(new MailAddress(email));
                    }
                }
                SendMailMessage(mailMessage);
            }
        }

        #endregion

        #region -- Process Member Cancellation --

        /// <summary>
        /// sets the content of email for the process cancelled member by corporate admin to urg.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendProcessCancelledMemberByCorporateAdminToUrg(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.ProcessCancelledMemberByCorporateAdminToUrg, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - MEMBER FOR DELETION", endorsementBatch.CompanyName),
                Body = body,
                IsBodyHtml = true
            };
            var sendEmail = false;
            foreach (var email in GetUnderwriterEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                mailMessage.CC.Add(new MailAddress(string.Format("{0}@medicardphils.com", LegacyHelper.GetAssignedProcessor(endorsementBatch.AccountCode))));
                var accountSetting = GetAccountSetting(endorsementBatch.AccountCode);
                if (accountSetting.BypassHRManagerApproval)
                {
                    foreach (var email in GetCorporateAdminEmails(endorsementBatch.AccountCode))
                    {
                        mailMessage.CC.Add(new MailAddress(email));
                    }
                }
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of email for the process cancelled member by corporate admin to corporate admin.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendProcessCancelledMemberByCorporateAdminToCorporateAdmin(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.ProcessCancelledMemberByCorporateAdminToCorporateAdmin, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: MEMBER FOR DELETION",
                Body = body,
                IsBodyHtml = true
            };
            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(endorsementBatch.AccountCode))
            {
                mailMessage.CC.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                SendMailMessage(mailMessage);
            }
        }


        #endregion

        #region -- ID Replacement --

        /// <summary>
        /// sets the content of email for the submit identifier replacement by member to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        /// <param name="bypassCorporateAdminApproval">if set to <c>true</c> [bypass corporate admin approval].</param>
        private static void SendSubmitIdReplacementByMemberToMember(Member member, bool bypassCorporateAdminApproval)
        {
            if (string.IsNullOrWhiteSpace(member.EmailAddress) || string.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.SubmitIdReplacementByMemberToMember, new EmailViewModel()
            {
                Member = member,
                BypassCorporateAdminApproval = bypassCorporateAdminApproval
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: ID Replacement Request Submitted",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));

            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the submit identifier replacement by member to corporate admin.
        /// </summary>
        /// <param name="idReplacement">The identifier replacement.</param>
        /// <param name="member">The member entity.</param>
        private static void SendSubmitIdReplacementByMemberToCorporateAdmin(IdReplacement idReplacement, Member member)
        {
            var companyName = Config.GetCompanyName(idReplacement.AccountCode);
            var body = ParseEmailTemplate(Config.SubmitIdReplacementByMemberToCorporateAdmin, new EmailViewModel()
            {
                IdReplacement = idReplacement,
                CompanyName = companyName
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: ID Replacement Request for Evaluation",
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(idReplacement.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }
            if (sendEmail) SendMailMessage(mailMessage);

        }

        /// <summary>
        /// sets the content of email for the submit identifier replacement by member to urg.
        /// </summary>
        /// <param name="idReplacement">The identifier replacement.</param>
        /// <param name="member">The member entity.</param>
        private static void SendSubmitIdReplacementByMemberToUrg(IdReplacement idReplacement, Member member)
        {
            var body = ParseEmailTemplate(Config.SubmitIdReplacementByMemberToUrg, new EmailViewModel()
            {
                IdReplacement = idReplacement
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - REQUEST FOR LOST ID", Config.GetCompanyName(idReplacement.AccountCode)),
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetUnderwriterEmails(idReplacement.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                mailMessage.CC.Add(new MailAddress(string.Format("{0}@medicardphils.com", LegacyHelper.GetAssignedProcessor(idReplacement.AccountCode))));
                var accountSetting = GetAccountSetting(idReplacement.AccountCode);
                if (accountSetting.BypassHRManagerApproval)
                {
                    foreach (var email in GetCorporateAdminEmails(idReplacement.AccountCode))
                    {
                        mailMessage.CC.Add(new MailAddress(email));
                    }
                }
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of email for the submit identifier replacement by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendSubmitIdReplacementByCorporateAdminToMember(Member member)
        {
            if (String.IsNullOrEmpty(member.EmailAddress) || String.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.SubmitIdReplacementByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: ID Replacement Accepted For Evaluation",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the submit identifier replacement by corporate admin to urg.
        /// </summary>
        /// <param name="idReplacement">The id replacement entity.</param>
        private static void SendSubmitIdReplacementByCorporateAdminToUrg(IdReplacement idReplacement)
        {
            var body = ParseEmailTemplate(Config.SubmitIdReplacementByCorporateAdminToUrg, new EmailViewModel()
            {
                IdReplacement = idReplacement
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - REQUEST FOR LOST ID", Config.GetCompanyName(idReplacement.AccountCode)),
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetUnderwriterEmails(idReplacement.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                mailMessage.CC.Add(new MailAddress(string.Format("{0}@medicardphils.com", LegacyHelper.GetAssignedProcessor(idReplacement.AccountCode))));
                var accountSetting = GetAccountSetting(idReplacement.AccountCode);
                if (accountSetting.BypassHRManagerApproval)
                {
                    foreach (var email in GetCorporateAdminEmails(idReplacement.AccountCode))
                    {
                        mailMessage.CC.Add(new MailAddress(email));
                    }
                }
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of email for the cancelled identifier replacement by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendCancelledIdReplacementByCorporateAdminToMember(Member member)
        {
            if (String.IsNullOrEmpty(member.EmailAddress) || String.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.CancelledIdReplacementByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: ID Replacement Request Cancelled",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the disapprove identifier replacement by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendDisapproveIdReplacementByCorporateAdminToMember(Member member)
        {
            if (String.IsNullOrEmpty(member.EmailAddress) || String.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.DisapproveIdReplacementByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: ID Replacement Request Disapproved",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the disapprove identifier replacement by urg to member.
        /// </summary>
        /// <param name="idReplacement">The id replacement entity.</param>
        /// <param name="urgRemark">The urg remark.</param>
        /// <param name="email">The email.</param>
        private static void SendDisapproveIdReplacementByUrgToMember(IdReplacement idReplacement, string urgRemark, string email)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrWhiteSpace(email)) return;

            var body = ParseEmailTemplate(Config.DisapproveIdReplacementByUrgToMember, new EmailViewModel()
            {
                IdReplacement = idReplacement,
                Remarks = urgRemark
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: ID Replacement Request Declined",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(email));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the disapprove identifier replacement by urg to corporate admin.
        /// </summary>
        /// <param name="idReplacement">The id replacement entity.</param>
        /// <param name="urgRemark">The urg remark.</param>
        private static void SendDisapproveIdReplacementByUrgToCorporateAdmin(IdReplacement idReplacement, string urgRemark)
        {
            var body = ParseEmailTemplate(Config.DisapproveIdReplacementByUrgToCorporateAdmin, new EmailViewModel()
            {
                IdReplacement = idReplacement,
                Remarks = urgRemark
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: ID Replacement Request Declined",
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(idReplacement.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }


            if (sendEmail)
            {
                foreach (var email in GetUnderwriterEmails(idReplacement.AccountCode))
                {
                    mailMessage.CC.Add(new MailAddress(email));
                }
                SendMailMessage(mailMessage);
            }
        }

        #endregion

        #region -- Amendment --

        /// <summary>
        /// sets the content of email for the submit amendment by member to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        /// <param name="bypassCorporateAdminApproval">if set to <c>true</c> [bypass corporate admin approval].</param>
        private static void SendSubmitAmendmentByMemberToMember(Member member, bool bypassCorporateAdminApproval)
        {
            if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.SubmitAmendmentByMemberToMember, new EmailViewModel()
            {
                Member = member,
                BypassCorporateAdminApproval = bypassCorporateAdminApproval
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Amendment Request Submitted",
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(new MailAddress(member.EmailAddress));

            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the submit amendment by member to corporate admin.
        /// </summary>
        /// <param name="amendment">The amendment entity.</param>
        /// <param name="member">The member entity.</param>
        private static void SendSubmitAmendmentByMemberToCorporateAdmin(Amendment amendment, Member member)
        {
            var companyName = Config.GetCompanyName(amendment.AccountCode);
            var body = ParseEmailTemplate(Config.SubmitAmendmentByMemberToCorporateAdmin, new EmailViewModel()
            {
                Amendment = amendment,
                CompanyName = companyName
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Amendment Request for Evaluation",
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(amendment.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }
            if (sendEmail) SendMailMessage(mailMessage);

        }

        /// <summary>
        /// sets the content of email for the submit amendment by member to urg.
        /// </summary>
        /// <param name="amendment">The amendment entity.</param>
        /// <param name="member">The member entity.</param>
        private static void SendSubmitAmendmentByMemberToUrg(Amendment amendment, Member member)
        {
            var subjectAmendment = SubjectAmendment(amendment.DataType);
            var body = ParseEmailTemplate(Config.SubmitAmendmentByMemberToUrg, new EmailViewModel()
            {
                Amendment = amendment,
                SubjectAmendment = subjectAmendment
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - FOR CORRECTION {1}", Config.GetCompanyName(amendment.AccountCode), subjectAmendment),
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetUnderwriterEmails(amendment.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                mailMessage.CC.Add(new MailAddress(string.Format("{0}@medicardphils.com", LegacyHelper.GetAssignedProcessor(amendment.AccountCode))));
                var accountSetting = GetAccountSetting(amendment.AccountCode);
                if (accountSetting.BypassHRManagerApproval)
                {
                    foreach (var email in GetCorporateAdminEmails(amendment.AccountCode))
                    {
                        mailMessage.CC.Add(new MailAddress(email));
                    }
                }
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of email for the submit amendment by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendSubmitAmendmentByCorporateAdminToMember(Member member)
        {
            if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.SubmitAmendmentByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Notification of Request for Amendment by Corporate Administrator to Member",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the submit amendment by corporate admin to urg.
        /// </summary>
        /// <param name="amendment">The amendment entity.</param>
        private static void SendSubmitAmendmentByCorporateAdminToUrg(Amendment amendment)
        {
            var subjectAmendment = SubjectAmendment(amendment.DataType);
            var body = ParseEmailTemplate(Config.SubmitAmendmentByCorporateAdminToUrg, new EmailViewModel()
            {
                Amendment = amendment,
                SubjectAmendment = subjectAmendment
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - FOR CORRECTION {1}", Config.GetCompanyName(amendment.AccountCode), subjectAmendment),
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetUnderwriterEmails(amendment.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                mailMessage.CC.Add(new MailAddress(string.Format("{0}@medicardphils.com", LegacyHelper.GetAssignedProcessor(amendment.AccountCode))));
                var accountSetting = GetAccountSetting(amendment.AccountCode);
                if (accountSetting.BypassHRManagerApproval)
                {
                    foreach (var email in GetCorporateAdminEmails(amendment.AccountCode))
                    {
                        mailMessage.CC.Add(new MailAddress(email));
                    }
                }
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of email for the cancelled amendment by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendCancelledAmendmentByCorporateAdminToMember(Member member)
        {
            if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.CancelledAmendmentByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Amendment Request Cancelled",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the disapprove amendment by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendDisapproveAmendmentByCorporateAdminToMember(Member member)
        {
            if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.DisapproveAmendmentByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Amendment Request Disapproved",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the declined amendment by urg to member.
        /// </summary>
        /// <param name="amendment">The amendment entity.</param>
        /// <param name="urgRemark">The urg remark.</param>
        /// <param name="email">The email.</param>
        private static void SendDeclinedAmendmentByUrgToMember(Amendment amendment, string urgRemark, string email)
        {
            if (String.IsNullOrEmpty(email) || String.IsNullOrWhiteSpace(email)) return;

            var body = ParseEmailTemplate(Config.DeclinedAmendmentByUrgToMember, new EmailViewModel()
            {
                Amendment = amendment,
                Remarks = urgRemark
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Amendment Request Declined",
                Body = body,
                IsBodyHtml = true
            };

            //if (!string.IsNullOrEmpty(amendment.DocumentFileName))
            //{
            //    Stream stream = new MemoryStream(amendment.DocumentFile);
            //    mailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream,amendment.DocumentFileName,amendment.DocumentContentType));
            //}

            mailMessage.To.Add(new MailAddress(email));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the declined amendment by urg to corporate admin.
        /// </summary>
        /// <param name="amendment">The amendment entity.</param>
        /// <param name="urgRemark">The urg remark.</param>
        private static void SendDeclinedAmendmentByUrgToCorporateAdmin(Amendment amendment, string urgRemark)
        {
            var body = ParseEmailTemplate(Config.DeclinedAmendmentByUrgToCorporateAdmin, new EmailViewModel()
            {
                Amendment = amendment,
                Remarks = urgRemark
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Amendment Request Declined",
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(amendment.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }


            if (sendEmail)
            {
                foreach (var email in GetUnderwriterEmails(amendment.AccountCode))
                {
                    mailMessage.CC.Add(new MailAddress(email));
                }
                SendMailMessage(mailMessage);
            }
        }

        #endregion

        #region -- Additional Dependent --

        /// <summary>
        /// sets the content of email for the submit additional dependent by member to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        /// <param name="bypassCorporateAdminApproval">if set to <c>true</c> [bypass corporate admin approval].</param>
        private static void SendSubmitAdditionalDependentByMemberToMember(Member member, bool bypassCorporateAdminApproval)
        {
            if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.SubmitAdditionalDependentByMemberToMember, new EmailViewModel()
            {
                Member = member,
                BypassCorporateAdminApproval = bypassCorporateAdminApproval
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Additional Dependent Request Submitted",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));

            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the submit additional dependent by member to corporate admin.
        /// </summary>
        /// <param name="additionalDependent">The additional dependent entity.</param>
        /// <param name="member">The member.</param>
        private static void SendSubmitAdditionalDependentByMemberToCorporateAdmin(AdditionalDependent additionalDependent, Member member)
        {
            var companyName = Config.GetCompanyName(additionalDependent.AccountCode);
            var body = ParseEmailTemplate(Config.SubmitAdditionalDependentByMemberToCorporateAdmin, new EmailViewModel()
            {
                AdditionalDependent = additionalDependent,
                CompanyName = companyName
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Additional Dependent Request for Evaluation",
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(additionalDependent.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }
            if (sendEmail) SendMailMessage(mailMessage);

        }

        /// <summary>
        /// sets the content of email for the submit additional dependent by member to urg.
        /// </summary>
        /// <param name="additionalDependent">The additional dependent entity.</param>
        /// <param name="member">The member.</param>
        private static void SendSubmitAdditionalDependentByMemberToUrg(AdditionalDependent additionalDependent, Member member)
        {
            var body = ParseEmailTemplate(Config.SubmitAdditionalDependentByMemberToUrg, new EmailViewModel()
            {
                AdditionalDependent = additionalDependent
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - ADDITIONAL DEPENDENT", Config.GetCompanyName(additionalDependent.AccountCode)),
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetUnderwriterEmails(additionalDependent.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                mailMessage.CC.Add(new MailAddress(string.Format("{0}@medicardphils.com", LegacyHelper.GetAssignedProcessor(additionalDependent.AccountCode))));
                var accountSetting = GetAccountSetting(additionalDependent.AccountCode);
                if (accountSetting.BypassHRManagerApproval)
                {
                    foreach (var email in GetCorporateAdminEmails(additionalDependent.AccountCode))
                    {
                        mailMessage.CC.Add(new MailAddress(email));
                    }
                }
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of email for the submit additional dependent by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendSubmitAdditionalDependentByCorporateAdminToMember(Member member)
        {
            if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.SubmitAdditionalDependentByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Notification of Application for Additional Dependent by Corporate Administrator to Member",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the submit additional dependent by corporate admin to urg.
        /// </summary>
        /// <param name="additionalDependent">The additional dependent entity.</param>
        private static void SendSubmitAdditionalDependentByCorporateAdminToUrg(AdditionalDependent additionalDependent)
        {
            var body = ParseEmailTemplate(Config.SubmitAdditionalDependentByCorporateAdminToUrg, new EmailViewModel()
            {
                AdditionalDependent = additionalDependent
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - ADDITIONAL DEPENDENT", Config.GetCompanyName(additionalDependent.AccountCode)),
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetUnderwriterEmails(additionalDependent.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                mailMessage.CC.Add(new MailAddress(string.Format("{0}@medicardphils.com", LegacyHelper.GetAssignedProcessor(additionalDependent.AccountCode))));
                var accountSetting = GetAccountSetting(additionalDependent.AccountCode);
                if (accountSetting.BypassHRManagerApproval)
                {
                    foreach (var email in GetCorporateAdminEmails(additionalDependent.AccountCode))
                    {
                        mailMessage.CC.Add(new MailAddress(email));
                    }
                }
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of email for the cancelled additional dependent by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendCancelledAdditionalDependentByCorporateAdminToMember(Member member)
        {
            if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.CancelledAdditionalDependentByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Notification of Application for Additional Dependent by Corporate Administrator to Member",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the disapprove additional dependent by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendDisapproveAdditionalDependentByCorporateAdminToMember(Member member)
        {
            if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.DisapproveAdditionalDependentByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Additional Dependent Request Disapproved",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        #endregion

        #region -- Dependent Cancellation --

        /// <summary>
        /// sets the content of email for the submit dependent cancellation by member to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        /// <param name="bypassCorporateAdminApproval">if set to <c>true</c> [bypass corporate admin approval].</param>
        private static void SendSubmitDependentCancellationByMemberToMember(Member member, bool bypassCorporateAdminApproval)
        {
            if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.SubmitDependentCancellationByMemberToMember, new EmailViewModel()
            {
                Member = member,
                BypassCorporateAdminApproval = bypassCorporateAdminApproval
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Submit Cancellation of Dependent's Membership By Member To Member",
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(new MailAddress(member.EmailAddress));

            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the submit dependent cancellation by member to corporate admin.
        /// </summary>
        /// <param name="dependentCancellation">The dependent cancellation.</param>
        /// <param name="member">The member entity.</param>
        private static void SendSubmitDependentCancellationByMemberToCorporateAdmin(DependentCancellation dependentCancellation, Member member)
        {
            var companyName = Config.GetCompanyName(dependentCancellation.AccountCode);
            var body = ParseEmailTemplate(Config.SubmitDependentCancellationByMemberToCorporateAdmin, new EmailViewModel()
            {
                DependentCancellation = dependentCancellation,
                CompanyName = companyName
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Submit Cancellation of Dependent's Membership by Member to Corporate Admin",
                Body = body,
                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(dependentCancellation.DocumentFileName))
            {
                Stream stream = new MemoryStream(dependentCancellation.DocumentFile);
                mailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, dependentCancellation.DocumentFileName, dependentCancellation.DocumentContentType));
            }

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(dependentCancellation.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }
            if (sendEmail) SendMailMessage(mailMessage);

        }

        /// <summary>
        /// sets the content of email for the submit dependent cancellation by member to urg.
        /// </summary>
        /// <param name="dependentCancellation">The dependent cancellation.</param>
        /// <param name="member">The member entity.</param>
        private static void SendSubmitDependentCancellationByMemberToUrg(DependentCancellation dependentCancellation, Member member)
        {
            var body = ParseEmailTemplate(Config.SubmitDependentCancellationByMemberToUrg, new EmailViewModel()
            {
                DependentCancellation = dependentCancellation
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - DEPENDENT FOR DELETION", Config.GetCompanyName(dependentCancellation.AccountCode)),
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetUnderwriterEmails(dependentCancellation.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                mailMessage.CC.Add(new MailAddress(string.Format("{0}@medicardphils.com", LegacyHelper.GetAssignedProcessor(dependentCancellation.AccountCode))));
                var accountSetting = GetAccountSetting(dependentCancellation.AccountCode);
                if (accountSetting.BypassHRManagerApproval)
                {
                    foreach (var email in GetCorporateAdminEmails(dependentCancellation.AccountCode))
                    {
                        mailMessage.CC.Add(new MailAddress(email));
                    }
                }
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of email for the submit dependent cancellation by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendSubmitDependentCancellationByCorporateAdminToMember(Member member)
        {
            if (String.IsNullOrEmpty(member.EmailAddress) || String.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.SubmitDependentCancellationByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Notification of Cancellation of Dependent's Membership by Corporate Administrator to Member",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the submit dependent cancellation by corporate admin to urg.
        /// </summary>
        /// <param name="dependentCancellation">The dependent cancellation entity.</param>
        private static void SendSubmitDependentCancellationByCorporateAdminToUrg(DependentCancellation dependentCancellation)
        {
            var body = ParseEmailTemplate(Config.SubmitDependentCancellationByCorporateAdminToUrg, new EmailViewModel()
            {
                DependentCancellation = dependentCancellation
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - DEPENDENT FOR DELETION", Config.GetCompanyName(dependentCancellation.AccountCode)),
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetUnderwriterEmails(dependentCancellation.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail)
            {
                mailMessage.CC.Add(new MailAddress(string.Format("{0}@medicardphils.com", LegacyHelper.GetAssignedProcessor(dependentCancellation.AccountCode))));
                var accountSetting = GetAccountSetting(dependentCancellation.AccountCode);
                if (accountSetting.BypassHRManagerApproval)
                {
                    foreach (var email in GetCorporateAdminEmails(dependentCancellation.AccountCode))
                    {
                        mailMessage.CC.Add(new MailAddress(email));
                    }
                }
                SendMailMessage(mailMessage);
            }
        }

        /// <summary>
        /// sets the content of email for the cancelled dependent cancellation by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendCancelledDependentCancellationByCorporateAdminToMember(Member member)
        {
            if (String.IsNullOrEmpty(member.EmailAddress) || String.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.CancelledDependentCancellationByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Notification of Cancellation of Dependent's Membership by Corporate Administrator to Member",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the disapprove dependent cancellation by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendDisapproveDependentCancellationByCorporateAdminToMember(Member member)
        {
            if (String.IsNullOrEmpty(member.EmailAddress) || String.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.DisapproveDependentCancellationByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Notification of Cancellation of Dependent's Membership by Corporate Administrator to Member",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        #endregion

        #region -- Endorsement Listing

        /// <summary>
        /// sets the content of email for the cancel membership by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendCancelMembershipByCorporateAdminToMember(Member member)
        {
            if (String.IsNullOrEmpty(member.EmailAddress) || String.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.CancelMembershipByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Cancelled Membership",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the approve membership by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendApproveMembershipByCorporateAdminToMember(Member member)
        {
            if (String.IsNullOrEmpty(member.EmailAddress) || String.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.ApproveMembershipByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Approved Membership",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the return membership by corporate admin to member.
        /// </summary>
        /// <param name="member">The member entity.</param>
        private static void SendReturnMembershipByCorporateAdminToMember(Member member)
        {
            if (String.IsNullOrEmpty(member.EmailAddress) || String.IsNullOrWhiteSpace(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.ReturnMembershipByCorporateAdminToMember, new EmailViewModel()
            {
                Member = member
            });

            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Cancelled Membership",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(member.EmailAddress));
            SendMailMessage(mailMessage);
        }

        #endregion

        #region -- Receiving Entry --

        /// <summary>
        /// sets the content of email for the receiving entry urg to urg.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        /// <param name="email">The email.</param>
        private static void SendReceivingEntryUrgToUrg(EndorsementBatch endorsementBatch, string email)
        {
            if (string.IsNullOrEmpty(email)) return;

            var count = 1;
            switch (endorsementBatch.EndorsementType)
            {
                case Constants.NEW_ENDORSEMENT_TYPE:
                    count = endorsementBatch.Members.Where(t => t.Status == MembershipStatus.CorporateAdminApproved).ToList().Count();
                    break;
                case Constants.RENEWAL_ENDORSEMENT_TYPE:
                    count = endorsementBatch.RenewalMembers.Where(t => t.RenewalStatus == RenewalStatus.ForRenewal).ToList().Count();
                    break;
                case Constants.ACTION_MEMO_ENDORSEMENT_TYPE:
                    count = endorsementBatch.ActionMemos.Where(t => t.Status == ActionMemoStatus.Replied).ToList().Count();
                    break;
                case Constants.CANCEL_MEMBERSHIP_ENDORSEMENT_TYPE:
                    count = endorsementBatch.CancelledMembers.Where(t => t.Status == CancelledMembershipStatus.CorporateAdminApproved).ToList().Count();
                    break;
            }

            var body = ParseEmailTemplate(Config.ReceiveEntryUrgToUrg, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch,
                Count = count
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - {1} Received by URG to Processor", endorsementBatch.CompanyName, endorsementBatch.EndorsementType),
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(string.Format("{0}@medicardphils.com", email)));
            SendMailMessage(mailMessage);

        }

        /// <summary>
        /// sets the content of email for the receiving entry urg to corporate admin.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendReceivingEntryUrgToCorporateAdmin(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.ReceiveEntryUrgToCorporateAdmin, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard: Endorsement Received for Review and Processing({0})", endorsementBatch.EndorsementType),
                Body = body,
                IsBodyHtml = true
            };
            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail) SendMailMessage(mailMessage);
        }

        /// <summary>
        /// sets the content of email for the receiving entry urg to all access.
        /// </summary>
        /// <param name="endorsementBatch">The endorsement batch entity.</param>
        private static void SendReceivingEntryUrgToAllAccess(EndorsementBatch endorsementBatch)
        {
            var body = ParseEmailTemplate(Config.ReceiveEntryUrgToCorporateAdmin, new EmailViewModel()
            {
                EndorsementBatch = endorsementBatch
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = string.Format("eMediCard:{0} - Endorsement Received By URG", endorsementBatch.EndorsementType),
                Body = body,
                IsBodyHtml = true
            };
            var sendEmail = false;

            foreach (var email in GetOtherEmails(endorsementBatch.AccountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail) SendMailMessage(mailMessage);
        }

        #endregion

        #region -- Submittion Deadline --

        /// <summary>
        /// sets the content of email for the submittion deadline notification.
        /// </summary>
        /// <param name="member">The member entity.</param>
        /// <param name="submittionDeadline">The submittion deadline.</param>
        private static void SendSubmittionDeadlineNotification(Member member, DateTime submittionDeadline)
        {
            if (string.IsNullOrEmpty(member.EmailAddress) || string.IsNullOrEmpty(member.EmailAddress)) return;

            var body = ParseEmailTemplate(Config.SubmittionDeadlineNotification, new EmailViewModel()
            {
                Member = member,
                SubmittionDeadline = submittionDeadline
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: Submission Deadline",
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(member.EmailAddress);
            SendMailMessage(mailMessage);
        }

        #endregion

        #region -- Member Migration --

        /// <summary>
        /// sets the content of email for the created user account to corporate admin.
        /// </summary>
        /// <param name="members">The members entity.</param>
        /// <param name="accountCode">The account code.</param>
        private static void SendCreatedUserAccountToCorporateAdmin(List<Member> members, string accountCode)
        {
            var body = ParseEmailTemplate(Config.CreatedUserAccountToCorporateAdmin, new EmailViewModel()
            {
                Members = members,
                CompanyName = Config.GetCompanyName(accountCode)
            });
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(Config.NotificationFromEmail),
                Subject = "eMediCard: CREATED USER ACCOUNTS",
                Body = body,
                IsBodyHtml = true
            };

            var sendEmail = false;
            foreach (var email in GetCorporateAdminEmails(accountCode))
            {
                mailMessage.To.Add(new MailAddress(email));
                sendEmail = true;
            }

            if (sendEmail) SendMailMessage(mailMessage);
        }

        #endregion

        #endregion

        #region  -- Functions --

        /// <summary>
        /// Parses the email template.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="model">The model.</param>
        /// <returns>System.String.</returns>
        private static string ParseEmailTemplate(string path, object model)
        {
            var templateConfig = new TemplateServiceConfiguration();
            templateConfig.Resolver = new DelegateTemplateResolver(name =>
            {
                return System.IO.File.ReadAllText(name);
            });

            Razor.SetTemplateService(new TemplateService(templateConfig));
            var template = Razor.Resolve(path, model);
            var returnValue = template.Run(new ExecuteContext());
            return returnValue;
        }

        /// <summary>
        /// Sends the mail message.
        /// </summary>
        /// <param name="mailMessage">The mail message.</param>
        private static void SendMailMessage(MailMessage mailMessage)
        {
            var thread = new Thread(new ParameterizedThreadStart(SendMailMessageAsync));
            thread.Start(mailMessage);

            //var smtpClient = new SmtpClient(Config.SmtpServer)
            //{
            //    Port = Config.SmtpPort,
            //    Credentials = new NetworkCredential(Config.SmtpUsername, Config.SmtpPassword)
            //};

            //try
            //{
            //    //smtpClient.Timeout = 5;
            //    smtpClient.Send(mailMessage);
            //    Thread.Sleep(5000);
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine(ex.Message);
            //}
        }

        /// <summary>
        /// Sends the mail message asynchronous.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private static void SendMailMessageAsync(object parameter)
        {
            MailMessage mailMessage = parameter as MailMessage;

            var smtpClient = new SmtpClient(Config.SmtpServer)
            {
                Port = Config.SmtpPort,
                Credentials = new NetworkCredential(Config.SmtpUsername, Config.SmtpPassword)
            };

            smtpClient.SendMailAsync(mailMessage);
        }

        /// <summary>
        /// Gets the corporate admin emails.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        private static IEnumerable<string> GetCorporateAdminEmails(string accountCode)
        {
            using (var db = new IdentityDataContext())
            {
                return db.Accounts.Where(t => t.Code == accountCode && t.IsCorporateAdmin && !string.IsNullOrEmpty(t.ApplicationUser.Email) && t.ApplicationUser.IsActive).Select(t => t.ApplicationUser.Email).ToList();
            }
        }

        /// <summary>
        /// Gets the underwriter emails.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        private static IEnumerable<string> GetUnderwriterEmails(string accountCode)
        {
            using (var db = new IdentityDataContext())
            {
                return db.Accounts.Where(t => t.Code == accountCode && t.IsUnderWriter && !string.IsNullOrEmpty(t.ApplicationUser.Email) && t.ApplicationUser.IsActive).Select(t => t.ApplicationUser.Email).ToList();
            }
        }

        /// <summary>
        /// Gets the other emails.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        private static IEnumerable<string> GetOtherEmails(string accountCode)
        {
            using (var db = new IdentityDataContext())
            {
                return db.Accounts.Where(t => t.Code == accountCode && !t.IsCorporateAdmin && !t.IsUnderWriter && !string.IsNullOrEmpty(t.ApplicationUser.Email) && t.ApplicationUser.IsActive).Select(t => t.ApplicationUser.Email).ToList();
            }
        }

        /// <summary>
        /// Gets the account setting.
        /// </summary>
        /// <param name="accountCode">The account code.</param>
        /// <returns>AccountSetting.</returns>
        private static AccountSetting GetAccountSetting(string accountCode)
        {
            using (var db = new IdentityDataContext())
            {
                return db.AccountSettings.FirstOrDefault(t => t.AccountCode == accountCode && !t.Deleted) ?? new AccountSetting();
            }
        }

        /// <summary>
        /// Sets the end of day dates.
        /// </summary>
        /// <param name="from">date From.</param>
        /// <param name="to">date BTo.</param>
        private static void SetEndOfDayDates(out DateTime from, out DateTime to)
        {
            var now = DateTime.Now;
            from = new DateTime(now.Year, now.Month, now.Day);
            to = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
        }

        /// <summary>
        /// Subjects the amendment.
        /// </summary>
        /// <param name="requestDataType">Type of the request data.</param>
        /// <returns>System.String.</returns>
        private static string SubjectAmendment(RequestDataType requestDataType)
        {
            var simpleAmendment = "(Simple amendment)";

            if (requestDataType == RequestDataType.LastName ||
                requestDataType == RequestDataType.FirstName ||
                requestDataType == RequestDataType.MiddleName ||
                requestDataType == RequestDataType.DateOfBirth ||
                requestDataType == RequestDataType.Gender ||
                requestDataType == RequestDataType.Address ||
                requestDataType == RequestDataType.Telephone)
            {
                return simpleAmendment;
            }

            return "(Company wide amendment)";
        }

        #endregion
    }
}
