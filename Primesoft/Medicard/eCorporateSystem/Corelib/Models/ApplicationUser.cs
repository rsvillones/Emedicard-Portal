using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Corelib.Models
{
    public class ApplicationUser : IdentityUser, IValidatableObject
    {
        #region -- Constructor --

        public ApplicationUser()
        {
            this.Groups = new Collection<Group>();
            this.Accounts = new Collection<Account>();
            this.Guid = Guid.NewGuid();
            this.IsActive = true;
        }

        #endregion

        #region -- Properties --

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Index]
        public Guid Guid { get; set; }

        public bool IsMember { get; set; }

        [Display(Name = "Roles")]
        public ICollection<Group> Groups { get; set; }

        [StringLength(20)]
        [Index]
        [Display(Name = "Agent Code")]
        public string AgentCode { get; set; }

        [StringLength(128)]
        [Index]
        public string CrById { get; set; }

        [StringLength(64)]
        public string Designation { get; set; }

        [StringLength(512)]
        public string Address { get; set; }

        [StringLength(32)]
        public string Mobile { get; set; }

        [StringLength(32)]
        public string Fax { get; set; }

        public bool AcceptedMemorandumOfAgreement { get; set; }

        public ICollection<Account> Accounts { get; set; }

        [Display(Name = "Use Active Directory to Authenticate User")]
        public bool UseActiveDirectory { get; set; }

        [StringLength(25)]
        [Display(Name = "Member Code")]
        public string MemberCode { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Deactivated By")]
        [StringLength(256)]
        public string DeactivatedBy { get; set; }

        [Display(Name = "Deactivated Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DeactivatedDate { get; set; }

        public DateTime? LastPasswordChangedDate { get; set; }

        public string CompanyName { get; set; }

        #endregion

        #region -- Functions --

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        #endregion

        #region -- IValidatableObject Members --

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (String.IsNullOrEmpty(this.UserName))
            {
                yield return new ValidationResult("UserName is required", new List<string>() { "UserName" });
            }

            var rgxUrl = new Regex("[^A-Za-z0-9]");
            if (!String.IsNullOrEmpty(this.UserName) && rgxUrl.IsMatch(this.UserName))
            {
                yield return new ValidationResult("UserName should not contains spaces and special characters.", new List<string>() { "UserName" });
            }

            using (var legacyDb = new LegacyDataContext())
            {
                if (this.Accounts != null)
                {
                    var duplicates = this.Accounts.Where(t => t != null).GroupBy(t => new { t.Code }).SelectMany(group => group.Skip(1));
                    if (duplicates != null && duplicates.Count() > 0)
                    {
                        foreach (var duplicate in duplicates)
                        {
                            var legacyAccount = legacyDb.LegacyAccounts.FirstOrDefault(t => t.Code == duplicate.Code) ?? new LegacyAccount();
                            yield return new ValidationResult(String.Format("Duplicate Account Association found for {0} - {1}", duplicate.Code, legacyAccount.Name));
                        }
                    }
                }
            }

            using (var db = new IdentityDataContext())
            {
                if (db.Users.Any(t => t.UserName == this.UserName && t.Id != this.Id))
                {
                    yield return new ValidationResult("Username already in use.", new List<string>() { "UserName" });
                }
                string errorMessage;
                if (!LegacyHelper.CheckAssociateMember(db, this.Id, this.MemberCode, out errorMessage))
                {
                    yield return new ValidationResult(errorMessage, new List<string>() { "MemberCode" });
                }
            }
        }

        #endregion
    }
}
