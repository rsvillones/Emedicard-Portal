﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corelib.Models;

namespace Corelib
{
    public class LegacyDataContext : DbContext
    {
        #region -- Constructor --

        public LegacyDataContext() : base ("name=Medicard.LegacySystem")
        {

        }

        #endregion

        #region -- DbSets --

        public DbSet<LegacyAgent> LegacyAgents { get; set; }
        public DbSet<LegacyAccount> LegacyAccounts { get; set; }
        public DbSet<LegacyRoomRate> LegacyRoomRates { get; set; }
        public DbSet<LegacyPlan> LegacyPlans { get; set; }
        public DbSet<LegacyPaymode> LegacyPaymodes { get; set; }
        public DbSet<LegacyMember> LegacyMembers { get; set; }
        public DbSet<LegacyDependent> LegacyDependents { get; set; }
        public DbSet<LegacyHoliday> LegacyHolidays { get; set; }
        public DbSet<LegacySob> LegacySobs { get; set; }
        public DbSet<LegacyCity> LegacyCities { get; set; }
        public DbSet<LegacyProvince> LegacyProvinces { get; set; }
        public DbSet<LegacyPrincipalActionMemo> LegacyPrincipalActionMemos { get; set; }
        public DbSet<LegacyDependentActionMemo> LegacyDependentActionMemos { get; set; }
        public DbSet<LegacyPrincipalProcess> LegacyPrincipalProcesses { get; set; }
        public DbSet<LegacyDependentProcess> LegacyDependentProcesses { get; set; }
        public DbSet<LegacyArea> LegacyAreas { get; set; }
        public DbSet<LegacyCivilStatus> LegacyCivilStatuses { get; set; }


        #endregion
    }
}
