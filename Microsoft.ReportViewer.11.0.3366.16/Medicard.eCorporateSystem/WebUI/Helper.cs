﻿using System.Data.Entity;
using Corelib;
using Corelib.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Mvc.Mailer;
using RazorEngine;
using WebUI.Mailers;
using Corelib.Classes;
using System.Data.SqlClient;
using System.Text;
using Corelib.Enums;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace WebUI
{
    public class SortParameter
    {
        public string PropertyName { get; set; }
    }

    public static class Helper
    {
        private static int _controlNumberCounter;

        #region -- Functions --

        public static void SetSortParameters<T>(Controller controller, ref IQueryable<T> entities, string sortOrder, string currentFilter, SortParameter defaultParameter, IEnumerable<SortParameter> parameters)
        {
            controller.ViewBag.CurrentFilter = currentFilter;
            controller.ViewBag.CurrentSort = sortOrder;

            controller.ViewData.Add(String.Format("{0}SortParam", defaultParameter.PropertyName), String.IsNullOrEmpty(sortOrder) || sortOrder == defaultParameter.PropertyName.ToLower() ? String.Format("{0}_desc", defaultParameter.PropertyName.ToLower()) : defaultParameter.PropertyName.ToLower());
            if (String.IsNullOrEmpty(sortOrder) || sortOrder == defaultParameter.PropertyName.ToLower())
            {
                entities = entities.OrderBy(defaultParameter.PropertyName);
            }
            else if (sortOrder == String.Format("{0}_desc", defaultParameter.PropertyName.ToLower()))
            {
                entities = entities.OrderByDescending(defaultParameter.PropertyName);
            }

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    controller.ViewData.Add(String.Format("{0}SortParam", parameter.PropertyName), String.IsNullOrEmpty(sortOrder) || sortOrder == parameter.PropertyName.ToLower() ? String.Format("{0}_desc", parameter.PropertyName.ToLower()) : parameter.PropertyName.ToLower());
                    if (sortOrder == parameter.PropertyName.ToLower())
                    {
                        entities = entities.OrderBy(parameter.PropertyName);
                    }
                    else if (sortOrder == String.Format("{0}_desc", parameter.PropertyName.ToLower()))
                    {
                        entities = entities.OrderByDescending(parameter.PropertyName);
                    }
                }
            }
        }

        public static ApplicationUser AddUser(IdentityDataContext db, ApplicationUserViewModel applicationUser, bool saveChanges = true)
        {
            ApplicationUser returnValue = null;
            bool disposeDb = false;
            if (db == null)
            {
                db = new IdentityDataContext();
                db.User = db.Users.FirstOrDefault(t => t.UserName == HttpContext.Current.User.Identity.Name);
                disposeDb = true;
            }

            if (!db.Users.Any(t => t.UserName == applicationUser.UserName))
            {
                var userGroups = new Collection<Group>();
                var groupIds = applicationUser.Groups.Select(x => x.Id).ToList();
                var groups = db.Groups.Where(t => groupIds.Contains(t.Id)).ToList();
                foreach (var group in groups)
                {
                    userGroups.Add(group);
                }
                var user = new ApplicationUser();
                MapProperties(applicationUser, user);
                user.Groups = userGroups;

                var store = new UserStore<ApplicationUser>(db);
                var manager = new UserManager<ApplicationUser>(store);
                manager.Create(user, applicationUser.Password);

                if (saveChanges)
                {
                    db.SaveChanges();
                    returnValue = user;
                }
            }
            else
            {
                throw new Exception("UserName already exists.");
            }

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return returnValue;

        }

        public static bool ValidateOldPassword(string id, string oldPassword)
        {
            using (var context = new IdentityDataContext())
            {
                if (context.Users.Any(t => t.Id == id))
                {

                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    //string hashedPassword = manager.PasswordHasher..HashPassword(oldPassword);
                    var user = manager.FindById(id);
                    //return user.PasswordHash == hashedPassword;
                    return manager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, oldPassword) == PasswordVerificationResult.Success;
                }
            }

            return false;
        }

        public static async Task<bool> LoginUser(string id)
        {
            using (var context = new IdentityDataContext())
            {
                if (context.Users.Any(t => t.Id == id))
                {

                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    var user = manager.FindById(id);
                    var am = HttpContext.Current.GetOwinContext().Authentication;
                    am.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    am.SignIn(new AuthenticationProperties() { IsPersistent = false }, await user.GenerateUserIdentityAsync(manager));
                }
            }

            return true;
        }

        public static async Task<bool> UpdateUserPasswordAsync(string id, string username, string password)
        {
            using (var context = new IdentityDataContext())
            {
                if (context.Users.Any(t => t.Id == id))
                {

                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    string hashedPassword = manager.PasswordHasher.HashPassword(password);
                    var user = manager.FindById(id);
                    await store.SetPasswordHashAsync(user, hashedPassword);
                    await store.UpdateAsync(user);
                    await manager.UpdateSecurityStampAsync(id);
                }
            }

            return true;
        }

        public static async Task<bool> UpdateName(string id, string name)
        {
            using (var context = new IdentityDataContext())
            {
                if (context.Users.Any(t => t.Id == id))
                {

                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    var user = manager.FindById(id);
                    if (user != null)
                    {
                        user.Name = name;
                        await store.UpdateAsync(user);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static void SetUserGroup(IdentityDataContext context, UserStore<ApplicationUser> store, UserManager<ApplicationUser> manager, ApplicationUser applicationUser)
        {
            bool disposeContext = false;
            if (context == null)
            {
                context = new IdentityDataContext();
                disposeContext = true;
            }

            if (store == null)
            {
                store = new UserStore<ApplicationUser>(context);
            }

            if (manager == null)
            {
                manager = new UserManager<ApplicationUser>(store);
            }

            foreach (var group in context.Groups.Include(t => t.Roles).ToList())
            {
                if (applicationUser.Groups == null || !applicationUser.Groups.Any(t => t.Id == group.Id))
                {
                    foreach (var role in group.Roles)
                    {
                        if (manager.IsInRole(applicationUser.Id, role.Name))
                        {
                            manager.RemoveFromRole(applicationUser.Id, role.Name);
                        }
                    }
                }
            }

            if (applicationUser.Groups != null)
            {
                foreach (var group in applicationUser.Groups)
                {
                    var currentGroup = context.Groups.Include(t => t.Roles).FirstOrDefault(t => t.Id == group.Id);
                    if (currentGroup != null)
                    {
                        foreach (var role in currentGroup.Roles)
                        {
                            if (!manager.IsInRole(applicationUser.Id, role.Name))
                            {
                                manager.AddToRole(applicationUser.Id, role.Name);
                            }
                        }
                    }
                }
            }

            context.SaveChanges();

            if (disposeContext)
            {
                context.Dispose();
            }
        }

        public static ApplicationUser AddUser(Corelib.IdentityDataContext context, UserStore<ApplicationUser> store, UserManager<ApplicationUser> manager, string name, string username, string password, string email, string phone, bool isMember, params string[] roles)
        {
            bool disposeContext = false;
            if (context == null)
            {
                context = new IdentityDataContext();
                disposeContext = true;
            }

            if (store == null)
            {
                store = new UserStore<ApplicationUser>(context);
            }

            if (manager == null)
            {
                manager = new UserManager<ApplicationUser>(store);
            }

            var user = context.Users.FirstOrDefault(t => t.UserName == username);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Name = name,
                    UserName = username,
                    Email = email,
                    PhoneNumber = phone,
                    IsMember = isMember
                };

                manager.Create(user, password);
            }

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    if (!manager.IsInRole(user.Id, role))
                    {
                        manager.AddToRole(user.Id, role);
                    }
                }
            }

            if (disposeContext)
            {
                context.Dispose();
            }

            return user;
        }

        public static void MapProperties(object source, object destination, bool mapId = true, params string[] propertiesToExclude)
        {
            foreach (var pi in destination.GetType().GetProperties())
            {
                if (!pi.CanWrite || source.GetType().GetProperty(pi.Name) == null || (!mapId && pi.Name == "Id") || (propertiesToExclude != null && propertiesToExclude.Contains(pi.Name)))
                    continue;

                destination.GetType().GetProperty(pi.Name).SetValue(destination, source.GetType().GetProperty(pi.Name).GetValue(source));
            }
        }

        public static IEnumerable<Group> GetGroups(IdentityDataContext db)
        {
            var disposeDb = false;
            if (db == null)
            {
                db = new IdentityDataContext();
                disposeDb = true;
            }

            IEnumerable<Group> returnValue;

            var userContext = HttpContext.Current.User;
            if (userContext.IsInRole("SysAd"))
            {
                returnValue = db.Groups.Where(t => !t.Deleted).OrderBy(t => t.Name).ToList();
            }
            else
            {
                var currentUser = db.Users.FirstOrDefault(t => t.UserName == userContext.Identity.Name);
                var groups = db.Groups.Include(t => t.AccessibleGroups).Where(t => t.Users.Select(user => user.UserName).Contains(userContext.Identity.Name) && !t.Deleted).ToList();
                var groupGuids = groups.SelectMany(t => t.AccessibleGroups).Select(t => t.GroupGuid).Distinct().ToList();
                var createdGroupGuids = db.Groups.Where(t => t.CrById == currentUser.Id && !t.Deleted).Select(t => t.Guid).ToList();

                returnValue = db.Groups.Where(t => (groupGuids.Contains(t.Guid) || createdGroupGuids.Contains(t.Guid)) && !t.Deleted).ToList();
            }

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return returnValue;
        }

        public static IEnumerable<LegacyAccount> GetLegacyAccounts(LegacyDataContext legacyDb, string agentCode)
        {
            var disposeLegacyDb = false;
            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            var returnValue = !String.IsNullOrEmpty(agentCode) ? legacyDb.LegacyAccounts.Where(t => t.AgentCode == agentCode).OrderBy(t => t.Name).ToList() : new List<LegacyAccount>();

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        public static IEnumerable<LegacyAccount> GetLegacyAccounts(IdentityDataContext db, LegacyDataContext legacyDb)
        {
            var disposeDb = false;
            if (db == null)
            {
                db = new IdentityDataContext();
                disposeDb = true;
            }

            var disposeLegacyDb = false;
            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            IEnumerable<LegacyAccount> returnValue;

            var userContext = HttpContext.Current.User;
            var activeStatus = "ASC0000003";

            if (HttpContext.Current.User.IsInRole("SysAd") || userContext.IsInRole("CanAssignAllAccounts"))
            {
                returnValue = legacyDb.LegacyAccounts.Where(t => t.AccountStatusCode == activeStatus).OrderBy(t => t.Name).ToList();
            }
            else
            {
                var user = db.Users.FirstOrDefault(t => t.UserName == userContext.Identity.Name);
                var accountCodes = new List<string>();
                if (user != null && !string.IsNullOrEmpty(user.AgentCode))
                {
                    returnValue = legacyDb.LegacyAccounts.Where(t => t.AgentCode == user.AgentCode && t.AccountStatusCode == activeStatus).OrderBy(t => t.Name).ToList();
                }
                else
                {
                    accountCodes = db.Accounts.Where(t => t.ApplicationUser.UserName == HttpContext.Current.User.Identity.Name).Select(t => t.Code).ToList();
                    returnValue = legacyDb.LegacyAccounts.Where(t => accountCodes.Contains(t.Code)).OrderBy(t => t.Name).ToList();
                }
            }



            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        public static IEnumerable<string> GetLegacyAccountCodes(IdentityDataContext db, LegacyDataContext legacyDb)
        {
            var disposeDb = false;
            if (db == null)
            {
                db = new IdentityDataContext();
                disposeDb = true;
            }

            var disposeLegacyDb = false;
            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            IEnumerable<string> returnValue;

            var userContext = HttpContext.Current.User;

            if (HttpContext.Current.User.IsInRole("SysAd") || userContext.IsInRole("CanAssignAllAccounts"))
            {
                returnValue = legacyDb.LegacyAccounts.Select(t => t.Code).ToList();
            }
            else
            {
                var user = db.Users.FirstOrDefault(t => t.UserName == userContext.Identity.Name);
                var accountCodes = new List<string>();
                if (user != null && !string.IsNullOrEmpty(user.AgentCode))
                {
                    returnValue = legacyDb.LegacyAccounts.Where(t => t.AgentCode == user.AgentCode).Select(t => t.Code).ToList();
                }
                else
                {
                    returnValue = db.Accounts.Where(t => t.ApplicationUser.UserName == HttpContext.Current.User.Identity.Name).Select(t => t.Code).ToList();
                }
            }

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        public static bool CreateUserForBatch(int endorsementBatchId)
        {
            using (var db = new IdentityDataContext())
            {
                db.User = db.Users.FirstOrDefault(t => t.UserName == System.Web.HttpContext.Current.User.Identity.Name);

                var endorsementBatch = db.EndorsementBatches.Include(t => t.Members).FirstOrDefault(t => t.Id == endorsementBatchId);
                if (endorsementBatch == null) throw new Exception("Invalid batch id passed.");
                Account account;
                if (HttpContext.Current.User.IsInRole("SysAd"))
                {
                    account = db.Accounts.FirstOrDefault(t => t.Code == endorsementBatch.AccountCode);
                }
                else
                {
                    account = db.Accounts.FirstOrDefault(t => t.Code == endorsementBatch.AccountCode && t.ApplicationUser.UserName == HttpContext.Current.User.Identity.Name);
                }
                if (account == null) throw new Exception("Account selected was not found.");
                var accountSetting = db.AccountSettings.FirstOrDefault(t => t.AccountCode == account.Code) ?? new AccountSetting();

                foreach (var member in endorsementBatch.Members)
                {
                    var username = "";
                    var originalUsername = "";
                    var counter = 1;
                    if (accountSetting.UseEmailAsLogin && !string.IsNullOrEmpty(member.EmailAddress))
                    {
                        username = member.EmailAddress;
                    }
                    else
                    {
                        username = string.Format("{0}{1}{2:MMddyyyy}", member.FirstName.Substring(0, 1).ToLower(), member.LastName.ToLower(), member.DateOfBirth);
                        username = RemoveSpecialCharacters(username);
                    }
                    originalUsername = username;
                    while (true)
                    {
                        if (db.Users.Any(t => t.UserName == username))
                        {
                            username = string.Format("{0}{1}", originalUsername, counter);
                            counter++;
                            continue;
                        }
                        break;
                    }
                    var password = "";
                    if (accountSetting.UseRandomGeneratedPassword)
                    {
                        password = System.Web.Security.Membership.GeneratePassword(6, 2);
                    }
                    else
                    {
                        password = string.Format("{0}{1:MMddyyyy}", member.LastName.ToLower(), member.DateOfBirth);
                    }

                    var applicationUser = new ApplicationUserViewModel()
                    {
                        Name = String.Format("{0} {1} {2}", member.FirstName, member.MiddleName, member.LastName),
                        Email = member.EmailAddress,
                        Address = member.Area,
                        IsMember = true,
                        UserName = username,
                        Password = password
                    };

                    var user = AddUser(db, applicationUser, true);
                    member.UserId = user.Id;
                    member.UserName = username;
                    var store = new UserStore<ApplicationUser>(db);
                    var manager = new UserManager<ApplicationUser>(store);
                    manager.AddToRole(user.Id, "Member");

                    MemberEmailNotification(System.Web.HttpContext.Current, member, username, password);
                }

                db.SaveChanges();
            }

            return true;
        }

        public static bool CreateMembersForRenewalBatch(IdentityDataContext db, int endorsementBatchId)
        {
            var renewalMembers = db.RenewalMembers.Where(t => t.EndorsementBatchId == endorsementBatchId && t.Type == "Principal").ToList();

            foreach (var renewalMember in renewalMembers)
            {
                var member = db.Members.FirstOrDefault(t => t.LastName == renewalMember.LastName && t.FirstName == renewalMember.FirstName && t.MiddleName == renewalMember.MiddleName && t.DateOfBirth == renewalMember.DateOfBirth);
                if (member != null)
                {
                    Helper.MapProperties(renewalMember, member, false);
                    member.Status = MembershipStatus.New;
                    db.Entry(member).State = EntityState.Modified;
                    db.Dependents.RemoveRange(db.Dependents.Where(t => t.MemberId == member.Id));
                }
                else
                {
                    member = new Member();
                    Helper.MapProperties(renewalMember, member);
                    member.Status = MembershipStatus.New;
                    member.LegacyMapCode = Helper.GenerateLegacyMapCode(db);
                    db.Members.Add(member);
                }
            }

            db.SaveChanges();

            var renewalDependents = db.RenewalMembers.Where(t => t.EndorsementBatchId == endorsementBatchId && t.Type == "Dependent").ToList();
            foreach (var renewalDependent in renewalDependents)
            {
                var dependent = db.Dependents.FirstOrDefault(t => t.LastName == renewalDependent.LastName && t.FirstName == renewalDependent.FirstName && t.MiddleName == renewalDependent.MiddleName && t.DateOfBirth == renewalDependent.DateOfBirth);
                var member = db.Members.FirstOrDefault(t => t.Code == renewalDependent.PrincipalMemberCode);
                if (member != null)
                {
                    if (dependent != null)
                    {
                        Helper.MapProperties(renewalDependent, dependent, false);
                        dependent.Status = MembershipStatus.New;
                        dependent.MemberId = member.Id;
                        db.Entry(dependent).State = EntityState.Modified;
                    }
                    else
                    {
                        dependent = new Dependent();
                        Helper.MapProperties(renewalDependent, dependent);
                        dependent.Status = MembershipStatus.New;
                        dependent.MemberId = member.Id;
                        db.Dependents.Add(dependent);
                    }
                    member.Dependent++;
                    db.SaveChanges();
                }
            }

            return true;
        }

        public static bool IsPlanValid(int legacyRoomRateId, string accountCode)
        {
            using (var legacyDb = new LegacyDataContext())
            {
                return legacyDb.LegacyRoomRates.Any(t => t.AccountCode == accountCode && t.Id == legacyRoomRateId);
            }
        }

        public static IEnumerable<LegacyRoomRate> GetLegacyRoomRates(string accountCode, bool principal)
        {
            var paymentMode = (principal ? 0 : 1);
            using (var legacyDb = new LegacyDataContext())
            {
                if (principal)
                {
                    return legacyDb.LegacyRoomRates
                        .Include(t => t.LegacyPlan)
                        .Include(t => t.LegacyPaymode)
                            .Where(t => t.AccountCode == accountCode && (t.LegacyPaymode.Id == 0 || t.LegacyPaymode.Id == 5))
                            .ToList();
                }
                else
                {
                    return legacyDb.LegacyRoomRates
                        .Include(t => t.LegacyPlan)
                        .Include(t => t.LegacyPaymode)
                        .Where(t => t.AccountCode == accountCode && (t.LegacyPaymode.Id == 1 || t.LegacyPaymode.Id == 5))
                    .ToList();
                }
            }
        }

        public static bool IsMedicalHistoryComplete(IdentityDataContext db, Member member)
        {
            var disposeDb = false;
            if (db == null)
            {
                db = new IdentityDataContext();
                disposeDb = true;
            }

            var accountSetting = db.AccountSettings.FirstOrDefault(t => t.AccountCode == member.AccountCode) ?? new AccountSetting();
            var returnValue = false;
            if (!accountSetting.BypassMedicalHistory)
            {
                db.Database.ExecuteSqlCommand("SpCore_ProcessMedicalHistoryQuestions @MemberId=@p0", member.Id);

                returnValue = !db.MedicalHistories.Any(t => t.MemberId == member.Id && !t.Answer.HasValue);
            }
            else
            {
                returnValue = true;
            }

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return returnValue;
        }

        public static bool IsDependentMedicalHistoryComplete(IdentityDataContext db, Member member)
        {
            var disposeDb = false;
            if (db == null)
            {
                db = new IdentityDataContext();
                disposeDb = true;
            }

            var accountSetting = db.AccountSettings.FirstOrDefault(t => t.AccountCode == member.AccountCode) ?? new AccountSetting();
            var returnValue = false;

            if (!accountSetting.BypassMedicalHistory)
            {
                var dependents = db.Dependents.Where(t => t.MemberId == member.Id && !t.Deleted).ToList();
                foreach (var dependent in dependents)
                {
                    db.Database.ExecuteSqlCommand("SpCore_ProcessDependentMedicalHistoryQuestions @DependentId=@p0", dependent.Id);
                }

                returnValue = !db.DependentMedicalHistories.Any(t => t.Dependent.MemberId == member.Id && !t.Answer.HasValue);
            }
            else
            {
                returnValue = true;
            }


            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return returnValue;
        }

        public static bool IsDependentMedicalHistoryComplete(IdentityDataContext db, Dependent dependent)
        {
            var disposeDb = false;
            if (db == null)
            {
                db = new IdentityDataContext();
                disposeDb = true;
            }

            var accountSetting = db.AccountSettings.FirstOrDefault(t => t.AccountCode == dependent.Member.AccountCode) ?? new AccountSetting();
            var returnValue = false;

            if (!accountSetting.BypassMedicalHistory)
            {
                db.Database.ExecuteSqlCommand("SpCore_ProcessDependentMedicalHistoryQuestions @DependentId=@p0", dependent.Id);

                returnValue = !db.DependentMedicalHistories.Any(t => t.DependentId == dependent.Id && !t.Answer.HasValue);
            }
            else
            {
                returnValue = true;
            }


            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return returnValue;
        }

        public static bool IsAdditionalDependentMedicalHistoryComplete(IdentityDataContext db, Member member)
        {
            var disposeDb = false;
            if (db == null)
            {
                db = new IdentityDataContext();
                disposeDb = true;
            }

            var accountSetting = db.AccountSettings.FirstOrDefault(t => t.AccountCode == member.AccountCode) ?? new AccountSetting();
            var returnValue = false;

            if (!accountSetting.BypassMedicalHistory)
            {
                var additionalDependents = db.AdditionalDependents.Where(t => t.MemberId == member.Id && !t.Deleted).ToList();
                foreach (var additionalDependent in additionalDependents)
                {
                    db.Database.ExecuteSqlCommand("SpCore_ProcessAdditionalDependentMedicalHistoryQuestions @AdditionalDependentId=@p0", additionalDependent.Id);
                }

                returnValue = !db.AdditionalDependentMedicalHistories.Any(t => t.AdditionalDependent.MemberId == member.Id && !t.Answer.HasValue);
            }
            else
            {
                returnValue = true;
            }


            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return returnValue;
        }

        public static int ComputeAge(DateTime dateOfBirth)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age)) age--;

            return age;
        }

        private static string RandomString(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            var sb = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                sb.Append(ch);
            }

            return sb.ToString();
        }

        public static string GenerateRandomEndorsementBatchReferenceNumber()
        {
            // get 1st random string 
            var randomString = System.Web.Security.Membership.GeneratePassword(4, 0).ToUpper();
            randomString = System.Text.RegularExpressions.Regex.Replace(randomString, @"[^a-zA-Z0-9]", m => "9");
            string rand1 = randomString;
            // get 2nd random string 
            randomString = System.Web.Security.Membership.GeneratePassword(4, 0).ToUpper();
            randomString = System.Text.RegularExpressions.Regex.Replace(randomString, @"[^a-zA-Z0-9]", m => "9");
            string rand2 = randomString;
            // creat full rand string
            return String.Format("{0}-{1}", rand1, rand2);
        }

        public static string GenerateReceivingEntryControlNumber()
        {
            var controlNo = new SqlParameter()
            {
                ParameterName = "control_no",
                SqlDbType = SqlDbType.NVarChar,
                Direction = ParameterDirection.Output,
                Size = 25
            };
            using (var legacyDb = new LegacyDataContext())
            {
                legacyDb.Database.ExecuteSqlCommand("dbo.getURGID @control_no OUTPUT", controlNo);
                return Convert.ToString(controlNo.Value);
            }
        }

        public static string GenerateMemberCode()
        {
            var randomString = System.Web.Security.Membership.GeneratePassword(10, 0).ToUpper();
            randomString = System.Text.RegularExpressions.Regex.Replace(randomString, @"[^a-zA-Z0-9]", m => "9");
            return String.Format("{0}", randomString);
        }

        public static string GenerateLegacyMapCode(IdentityDataContext db)
        {
            var disposeDb = false;

            if (db == null)
            {
                db = new IdentityDataContext();
                disposeDb = true;
            }

            var randomString = "EMED" + System.Web.Security.Membership.GeneratePassword(21, 0).ToUpper();
            randomString = System.Text.RegularExpressions.Regex.Replace(randomString, @"[^a-zA-Z0-9]", m => "9");
            while (db.Members.Any(t => t.LegacyMapCode == randomString))
            {
                randomString = "EMED" + System.Web.Security.Membership.GeneratePassword(21, 0).ToUpper();
                randomString = System.Text.RegularExpressions.Regex.Replace(randomString, @"[^a-zA-Z0-9]", m => "9");
            }

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            return String.Format("{0}", randomString);
       }

        private static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static bool IsMember()
        {
            using (var db = new IdentityDataContext())
            {
                var user = db.Users.FirstOrDefault(t => t.UserName == HttpContext.Current.User.Identity.Name);
                return user != null && user.IsMember;
            }
        }

        public static bool IsCorporateAdmin()
        {
            using (var db = new IdentityDataContext())
            {
                return db.Accounts.Any(t => t.ApplicationUser.UserName == HttpContext.Current.User.Identity.Name && t.IsCorporateAdmin);
            }
        }

        public static IEnumerable<LegacyMember> GetActiveMembers(IdentityDataContext db, LegacyDataContext legacyDb, string accountCode)
        {
            var disposeDb = false;
            var disposeLegacyDb = false;

            if (db == null)
            {
                db = new IdentityDataContext();
                disposeDb = true;
            }

            if (legacyDb == null)
            {
                legacyDb = new LegacyDataContext();
                disposeLegacyDb = true;
            }

            var returnValue = new List<LegacyMember>();
            var members = db.Members.Include(t => t.Dependents).Where(t => t.AccountCode == accountCode);
            var counter = 1;
            foreach (var member in members)
            {
                var lm = new LegacyMember();
                Helper.MapProperties(member, lm);
                lm.Code = String.Format("MEM{0:00000}", counter);
                lm.Type = "Principal";
                returnValue.Add(lm);
                counter++;
                var principalMemberCode = lm.Code;
                foreach (var dependent in member.Dependents)
                {
                    lm = new LegacyMember();
                    Helper.MapProperties(dependent, lm);
                    lm.Code = String.Format("MEM{0:00000}", counter);
                    lm.Type = "Dependent";
                    lm.PrincipalMemberCode = principalMemberCode;
                    returnValue.Add(lm);
                    counter++;
                }
            }

            if (disposeDb)
            {
                db.Dispose();
                db = null;
            }

            if (disposeLegacyDb)
            {
                legacyDb.Dispose();
                legacyDb = null;
            }

            return returnValue;
        }

        #endregion

        #region -- Extension Methods --

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, params object[] values)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));

            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string ordering, params object[] values)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));

            return source.Provider.CreateQuery<T>(resultExp);
        }

        #endregion

        #region -- Email Helpers --

        public static string SetEmailTemplate(EmailViewModel model, string mapTemplate)
        {
            var template = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath(mapTemplate));

            return Razor.Parse(template, model);
        }

        #region -- Member Profile --

        public static void MemberEmailNotification(HttpContext context, Member member, string userName, string password)
        {
            using (var db = new IdentityDataContext())
            {
                if (!string.IsNullOrEmpty(member.EmailAddress))
                {
                    var client = new SmtpClientWrapper();
                    client.SendCompleted += client_SendCompleted;

                    MvcMailMessage emailMessage = null;
                    emailMessage = UserMailer.SendMemberNotification(member, userName, password);
                    emailMessage.To.Add(member.EmailAddress);
                    emailMessage.SendEmailAsync(context);
                }
            }
        }

        public static void BatchSummaryNotification(HttpContext context, List<Member> members, string accountCode)
        {
            var emails = HrEmailAddresses(accountCode);
            foreach (var email in emails)
            {
                if (string.IsNullOrEmpty(email)) continue;

                var client = new SmtpClientWrapper();
                client.SendCompleted += client_SendCompleted;

                MvcMailMessage emailMessage = null;
                emailMessage = UserMailer.SendBatchSummaryNotification(members);
                emailMessage.To.Add(email);
                emailMessage.SendEmailAsync(context);
            }
        }

        public static void CorpAdminMemberProfileNotification(HttpContext context, Member member, string accountCode)
        {
            var emails = HrEmailAddresses(accountCode);
            foreach (var email in emails)
            {
                var client = new SmtpClientWrapper();
                client.SendCompleted += client_SendCompleted;

                MvcMailMessage emailMessage = null;
                emailMessage = UserMailer.SendCorpAdminMemberProfileNotification(member);
                emailMessage.To.Add(email);
                emailMessage.SendEmailAsync(context);
            }
        }

        public static void UrgMemberProfileNotification(HttpContext context, Member member, string accountCode)
        {
            var emails = UrgEmailAddresses(accountCode);
            var client = new SmtpClientWrapper();
            client.SendCompleted += client_SendCompleted;

            foreach (var email in emails)
            {
                MvcMailMessage emailMessage = null;
                emailMessage = UserMailer.SendUrgMemberProfileNotification(member);
                emailMessage.To.Add(email);
                emailMessage.SendEmailAsync(context);
            }
        }

        #endregion

        #region -- Amendment --

        public static void MemberAmendment(HttpContext context, Amendment amendment, bool bypassHrApproval)
        {
            var client = new SmtpClientWrapper();
            client.SendCompleted += client_SendCompleted;
            if (amendment == null) throw new Exception("Amendment not found.");

            var emails = bypassHrApproval ? UrgEmailAddresses(amendment.AccountCode) : HrEmailAddresses(amendment.AccountCode);
            var ccEmails = HrEmailAddresses(amendment.AccountCode);
            foreach (var email in emails)
            {
                MvcMailMessage emailMessage = null;
                emailMessage = UserMailer.SendMemberAmendment(amendment, bypassHrApproval);
                emailMessage.To.Add(email);
                if (bypassHrApproval) ccEmails.ForEach(ccEmail => emailMessage.CC.Add(ccEmail));
                emailMessage.SendEmailAsync(context);
            }
        }

        public static void UrgAmendment(HttpContext context, Amendment amendment)
        {
            using (var db = new IdentityDataContext())
            {
                var client = new SmtpClientWrapper();
                client.SendCompleted += client_SendCompleted;

                if (amendment == null) throw new Exception("Amendment not found.");
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == amendment.MemberId) ?? new Member();

                if (!string.IsNullOrEmpty(member.EmailAddress))
                {
                    MvcMailMessage emailMessage = null;
                    emailMessage = UserMailer.SendUrgAmendment(amendment);
                    emailMessage.To.Add(member.EmailAddress);
                    emailMessage.SendEmailAsync(context);
                }
            }
        }

        public static void CorpAdminAmendment(HttpContext context, Amendment amendment)
        {
            var client = new SmtpClientWrapper();
            client.SendCompleted += client_SendCompleted;
            if (amendment == null) throw new Exception("Amendment not found.");
            var emails = UrgEmailAddresses(amendment.AccountCode);
            foreach (var email in emails)
            {
                MvcMailMessage emailMessage = null;
                emailMessage = UserMailer.SendCorpAdminAmendment(amendment);
                emailMessage.To.Add(email);
                emailMessage.SendEmailAsync(context);
            }
        }

        public static void CorpAdminCancelAmendment(HttpContext context, Amendment amendment)
        {
            using (var db = new IdentityDataContext())
            {
                var client = new SmtpClientWrapper();
                client.SendCompleted += client_SendCompleted;
                if (amendment == null) throw new Exception("Amendment not found.");
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == amendment.MemberId);

                if (!string.IsNullOrEmpty(member.EmailAddress))
                {
                    MvcMailMessage emailMessage = null;
                    emailMessage = UserMailer.SendCorpAdminCancelAmendment(amendment);
                    emailMessage.To.Add(member.EmailAddress);
                    emailMessage.SendEmailAsync(context);
                }
            }
        }

        #endregion

        #region -- Action Memo --

        public static void MemberActionMemo(HttpContext context, ActionMemo actionMemo, string accountCode, bool bypassHrApproval)
        {
            var emails = bypassHrApproval ? UrgEmailAddresses(accountCode) : HrEmailAddresses(accountCode);
            var ccEmails = HrEmailAddresses(accountCode);
            var client = new SmtpClientWrapper();
            client.SendCompleted += client_SendCompleted;

            if (actionMemo == null) throw new Exception("Action Memo not found.");

            foreach (var email in emails)
            {
                MvcMailMessage emailMessage = null;
                emailMessage = UserMailer.SendMemberActionMemo(actionMemo, bypassHrApproval);
                emailMessage.To.Add(email);
                if (bypassHrApproval) ccEmails.ForEach(ccEmail => emailMessage.CC.Add(ccEmail));
                emailMessage.SendEmailAsync(context);
            }
        }

        public static void CorpAdminActionMemo(HttpContext context, ActionMemo actionMemo, string accountCode)
        {
            var emails = UrgEmailAddresses(accountCode);
            var client = new SmtpClientWrapper();
            client.SendCompleted += client_SendCompleted;

            if (actionMemo == null) throw new Exception("Action Memo not found.");

            //foreach (var email in emails)
            //{
            MvcMailMessage emailMessage = null;
            emailMessage = UserMailer.SendCorpAdminActionMemo(actionMemo);
            emailMessage.To.Add("prince_louie00@yahoo.com");
            emailMessage.SendEmailAsync(context);
            //}
        }

        public static void UrgActionMemo(HttpContext context, ActionMemo actionMemo, string accountCode)
        {
            using (var db = new IdentityDataContext())
            {
                var client = new SmtpClientWrapper();
                client.SendCompleted += client_SendCompleted;

                if (actionMemo == null) throw new Exception("Action Memo not found.");
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == actionMemo.MemberId) ?? new Member();

                var ccEmails = HrEmailAddresses(accountCode);
                if (!string.IsNullOrEmpty(member.EmailAddress))
                {
                    MvcMailMessage emailMessage = null;
                    emailMessage = UserMailer.SendUrgActionMemo(actionMemo);
                    emailMessage.To.Add(member.EmailAddress);
                    ccEmails.ForEach(ccEmail => emailMessage.CC.Add(ccEmail));
                    emailMessage.SendEmailAsync(context);
                }
            }
        }

        #endregion

        #region -- ID Replacement --

        public static void MemberIdReplacement(HttpContext context, IdReplacement idReplacement, bool bypassHrApproval)
        {
            var client = new SmtpClientWrapper();
            client.SendCompleted += client_SendCompleted;
            if (idReplacement == null) throw new Exception("Id Replacement not found.");

            var emails = bypassHrApproval ? UrgEmailAddresses(idReplacement.AccountCode) : HrEmailAddresses(idReplacement.AccountCode);
            var ccEmails = HrEmailAddresses(idReplacement.AccountCode);
            foreach (var email in emails)
            {
                MvcMailMessage emailMessage = null;
                emailMessage = UserMailer.SendMemberIdReplacement(idReplacement, bypassHrApproval);
                emailMessage.To.Add(email);
                if (bypassHrApproval) ccEmails.ForEach(ccEmail => emailMessage.CC.Add(ccEmail));
                emailMessage.SendEmailAsync(context);
            }
        }

        public static void CorpAdminIdReplacement(HttpContext context, IdReplacement idReplacement)
        {
            var client = new SmtpClientWrapper();
            client.SendCompleted += client_SendCompleted;
            if (idReplacement == null) throw new Exception("ID Replacement not found.");
            var emails = UrgEmailAddresses(idReplacement.AccountCode);
            foreach (var email in emails)
            {
                MvcMailMessage emailMessage = null;
                emailMessage = UserMailer.SendCorpAdminIdReplacement(idReplacement);
                emailMessage.To.Add(email);
                emailMessage.SendEmailAsync(context);
            }
        }

        public static void CorpAdminCancelIdReplacement(HttpContext context, IdReplacement idReplacement)
        {
            using (var db = new IdentityDataContext())
            {
                var client = new SmtpClientWrapper();
                client.SendCompleted += client_SendCompleted;
                if (idReplacement == null) throw new Exception("ID Replacement not found.");
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == idReplacement.MemberId);

                if (!string.IsNullOrEmpty(member.EmailAddress))
                {
                    MvcMailMessage emailMessage = null;
                    emailMessage = UserMailer.SendCorpAdminCancelIdReplacement(idReplacement);
                    emailMessage.To.Add(member.EmailAddress);
                    emailMessage.SendEmailAsync(context);
                }
            }
        }

        public static void UrgIdReplacement(HttpContext context, IdReplacement idReplacement)
        {
            using (var db = new IdentityDataContext())
            {
                var client = new SmtpClientWrapper();
                client.SendCompleted += client_SendCompleted;

                if (idReplacement == null) throw new Exception("ID Replacement not found.");
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == idReplacement.MemberId) ?? new Member();

                if (!string.IsNullOrEmpty(member.EmailAddress))
                {
                    MvcMailMessage emailMessage = null;
                    emailMessage = UserMailer.SendUrgIdReplacement(idReplacement);
                    emailMessage.To.Add(member.EmailAddress);
                    emailMessage.SendEmailAsync(context);
                }
            }
        }

        #endregion

        #region -- Additional Dependent --

        public static void MemberAdditionDependent(HttpContext context, AdditionalDependent additionalDependent, bool bypassHrApproval)
        {
            var client = new SmtpClientWrapper();
            client.SendCompleted += client_SendCompleted;
            if (additionalDependent == null) throw new Exception("Additional Dependent not found.");

            var emails = bypassHrApproval ? UrgEmailAddresses(additionalDependent.AccountCode) : HrEmailAddresses(additionalDependent.AccountCode);
            var ccEmails = HrEmailAddresses(additionalDependent.AccountCode);
            foreach (var email in emails)
            {
                MvcMailMessage emailMessage = null;
                emailMessage = UserMailer.SendMemberAdditionalDependent(additionalDependent, bypassHrApproval);
                emailMessage.To.Add(email);
                if (bypassHrApproval) ccEmails.ForEach(ccEmail => emailMessage.CC.Add(ccEmail));
                emailMessage.SendEmailAsync(context);
            }
        }

        public static void CorpAdminAdditionalDependent(HttpContext context, AdditionalDependent additionalDependent)
        {
            var client = new SmtpClientWrapper();
            client.SendCompleted += client_SendCompleted;
            if (additionalDependent == null) throw new Exception("Additional Dependent not found.");
            var emails = UrgEmailAddresses(additionalDependent.AccountCode);
            foreach (var email in emails)
            {
                MvcMailMessage emailMessage = null;
                emailMessage = UserMailer.SendCorpAdminAdditionalDependent(additionalDependent);
                emailMessage.To.Add(email);
                emailMessage.SendEmailAsync(context);
            }
        }

        public static void CorpAdminCancelAdditionalDependent(HttpContext context, AdditionalDependent additionalDependent)
        {
            using (var db = new IdentityDataContext())
            {
                var client = new SmtpClientWrapper();
                client.SendCompleted += client_SendCompleted;
                if (additionalDependent == null) throw new Exception("Additional Dependent not found.");
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == additionalDependent.MemberId);

                if (!string.IsNullOrEmpty(member.EmailAddress))
                {
                    MvcMailMessage emailMessage = null;
                    emailMessage = UserMailer.SendCorpAdminCancelAdditionalDependent(additionalDependent);
                    emailMessage.To.Add(member.EmailAddress);
                    emailMessage.SendEmailAsync(context);
                }
            }
        }

        #endregion

        #region -- Dependent Cancellation --

        public static void MemberDependentCancellation(HttpContext context, DependentCancellation dependentCancellation, bool bypassHrApproval)
        {
            var client = new SmtpClientWrapper();
            client.SendCompleted += client_SendCompleted;
            if (dependentCancellation == null) throw new Exception("Dependent Cancellation not found.");

            var emails = bypassHrApproval ? UrgEmailAddresses(dependentCancellation.AccountCode) : HrEmailAddresses(dependentCancellation.AccountCode);
            var ccEmails = HrEmailAddresses(dependentCancellation.AccountCode);
            foreach (var email in emails)
            {
                MvcMailMessage emailMessage = null;
                emailMessage = UserMailer.SendMemberDependentCancellation(dependentCancellation, bypassHrApproval);
                emailMessage.To.Add(email);
                if (bypassHrApproval) ccEmails.ForEach(ccEmail => emailMessage.CC.Add(ccEmail));
                emailMessage.SendEmailAsync(context);
            }
        }

        public static void CorpAdminDependentCancellation(HttpContext context, DependentCancellation dependentCancellation)
        {
            var client = new SmtpClientWrapper();
            client.SendCompleted += client_SendCompleted;
            if (dependentCancellation == null) throw new Exception("Dependent Cancellation not found.");
            var emails = UrgEmailAddresses(dependentCancellation.AccountCode);
            foreach (var email in emails)
            {
                MvcMailMessage emailMessage = null;
                emailMessage = UserMailer.SendCorpAdminDependentCancellation(dependentCancellation);
                emailMessage.To.Add(email);
                emailMessage.SendEmailAsync(context);
            }
        }

        public static void CorpAdminCancelDependentCancellation(HttpContext context, DependentCancellation dependentCancellation)
        {
            using (var db = new IdentityDataContext())
            {
                var client = new SmtpClientWrapper();
                client.SendCompleted += client_SendCompleted;
                if (dependentCancellation == null) throw new Exception("Dependent Cancellation not found.");
                var member = db.Members.FirstOrDefault(t => !t.Deleted && t.Id == dependentCancellation.MemberId);

                if (!string.IsNullOrEmpty(member.EmailAddress))
                {
                    MvcMailMessage emailMessage = null;
                    emailMessage = UserMailer.SendCorpAdminCancelDependentCancellation(dependentCancellation);
                    emailMessage.To.Add(member.EmailAddress);
                    emailMessage.SendEmailAsync(context);
                }
            }
        }

        #endregion

        #region -- Cancellation of Membership --

        public static void MembershipCancellation(HttpContext context, List<CancelledMember> cancelledMembers)
        {
            using (var db = new IdentityDataContext())
            {
                foreach (var member in cancelledMembers){
                    if (!string.IsNullOrEmpty(member.EmailAddress)){
                        var client = new SmtpClientWrapper();
                        client.SendCompleted += client_SendCompleted;

                        MvcMailMessage emailMessage = null;
                        emailMessage = UserMailer.SendMembershipCancellation(member);
                        emailMessage.To.Add(member.EmailAddress);
                        emailMessage.SendEmailAsync(context);
                    }
                }
            }
        }

        public static void CorpAdminMembershipCancellationBatchSummary(HttpContext context, List<CancelledMember> cancelledMembers, string accountCode)
        {
            var emails = UrgEmailAddresses(accountCode);
            var corpAdminEmails = HrEmailAddresses(accountCode);
            foreach (var email in emails)
            {
                var client = new SmtpClientWrapper();
                client.SendCompleted += client_SendCompleted;

                MvcMailMessage emailMessage = null;
                emailMessage = UserMailer.SendCorpAdminMembershipCancellation(cancelledMembers);
                emailMessage.To.Add(email);
                corpAdminEmails.ForEach(corpAdminEmail => emailMessage.CC.Add(corpAdminEmail));
                emailMessage.SendEmailAsync(context);
            }
        }

        #endregion

        #region -- Renewal of Membership --

        public static void MembershipRenewal(HttpContext context, IEnumerable<RenewalMember> renewalMember)
        {
            using (var db = new IdentityDataContext()){
                foreach (var member in renewalMember){
                    if (!string.IsNullOrEmpty(member.EmailAddress)){
                        var client = new SmtpClientWrapper();
                        client.SendCompleted += client_SendCompleted;
                        MvcMailMessage emailMessage = null;
                        emailMessage = UserMailer.SendMembershipRenewal(member);
                        emailMessage.To.Add(member.EmailAddress);
                        emailMessage.SendEmailAsync(context);
                    }
                }
            }
        }

        public static void CorpAdminMembershipRenewalBatchSummary(HttpContext context, IEnumerable<RenewalMember> renewalMember, string accountCode)
        {
            var emails = UrgEmailAddresses(accountCode);
            var corpAdminEmails = HrEmailAddresses(accountCode);
            foreach (var email in emails){
                var client = new SmtpClientWrapper();
                client.SendCompleted += client_SendCompleted;
                MvcMailMessage emailMessage = null;
                emailMessage = UserMailer.SendCorpAdminMembershipRenewal(renewalMember);
                emailMessage.To.Add(email);
                corpAdminEmails.ForEach(corpAdminEmail => emailMessage.CC.Add(corpAdminEmail));
                emailMessage.SendEmailAsync(context);
            }
        }

        #endregion

        static void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                throw new Exception(e.Error.Message);
            }
        }

        public static string AbsoluteAction(string actionName, string controllerName, object routeValues = null)
        {
            var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string scheme = url.RequestContext.HttpContext.Request.Url.Scheme;
            var returnValue = url.Action(actionName, controllerName, routeValues, scheme);
            return returnValue;
        }

        public static List<string> HrEmailAddresses(string accountCode)
        {
            using (var db = new IdentityDataContext())
            {
                return db.Accounts.Include(t => t.ApplicationUser).Where(t => !t.Deleted && t.Code == accountCode && t.IsCorporateAdmin).Select(t => t.ApplicationUser.Email).ToList();
            }
        }

        public static List<string> UrgEmailAddresses(string accountCode)
        {
            using (var db = new IdentityDataContext())
            {
                return db.Accounts.Include(t => t.ApplicationUser).Where(t => !t.Deleted && t.Code == accountCode && t.IsUnderWriter).Select(t => t.ApplicationUser.Email).ToList();
            }
        }

        #region -- UserMailer --

        private static IUserMailer _userMailer = new UserMailer();

        public static IUserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }

        #endregion

        #endregion

    }

    #region -- Enum Helper --

    public static class EnumHelper<T>
    {
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }
    }

    #endregion

}