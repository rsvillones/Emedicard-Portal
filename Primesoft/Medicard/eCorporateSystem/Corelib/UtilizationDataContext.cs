using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corelib.Models;

namespace Corelib
{
    public class UtilizationDataContext : DbContext
    {
        #region -- Constructor --

        public UtilizationDataContext()
            : base("name=Medicard.UtilizationSystem")
        {

        }

        #endregion

        #region -- DbSets --

        public DbSet<UtilizationDental> UtilizationDentals { get; set; }
        public DbSet<UtilizationMedicalService> UtilizationMedicalServices { get; set; }
        public DbSet<UtilizationOutPatient> UtilizationOutPatients { get; set; }
        public DbSet<UtilizationInPatient> UtilizationInPatients { get; set; }
        public DbSet<UtilizationMemberAllService> UtilizationMemberAllServices { get; set; }
        public DbSet<UtilizationReimbursement> UtilizationReimbursements { get; set; }
        public DbSet<UtilizationUnBilledReportOutPatientMedSevice> UtilizationUnBilledReportOutPatientMedSevices { get; set; }

        #endregion

    }
}
