using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corelib.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Corelib
{
    public class AppDataContext : DbContext
    {
        #region -- Constructor --

        public AppDataContext() : base("name=Medicard.eCorporateSystem")
        {

        }

        #endregion

        #region -- DbSets --

        #endregion

        #region -- Overrides --

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entry in this.ChangeTracker.Entries().Where(t => t.Entity is BaseModel && (t.State == EntityState.Added || t.State == EntityState.Modified || t.State == EntityState.Deleted)))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        ((BaseModel)entry.Entity).CrBy = Config.CurrentUser;
                        ((BaseModel)entry.Entity).CrDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        ((BaseModel)entry.Entity).ModBy = Config.CurrentUser;
                        ((BaseModel)entry.Entity).ModDate = DateTime.Now;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        ((BaseModel)entry.Entity).Deleted = true;
                        ((BaseModel)entry.Entity).ModBy = Config.CurrentUser;
                        ((BaseModel)entry.Entity).ModDate = DateTime.Now;
                        break;
                }
            }

            try
            {
                return base.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        #endregion
    }
}
