using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using SqlHealthMonitor.DAL.Models.Identity.UserLogin;
using SqlHealthMonitor.DAL.Models.WebPages;
using SqlHealthMonitor.DAL.Models;
using SqlHealthMonitor.DAL.Models.Widgets;
using System.Linq;

namespace SqlHealthMonitor.DAL.DatabaseContext
{
  public class SqlHealthMonitorDbContextLocal : IdentityDbContext<ApplicationUser,
    ApplicationRole,
        string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public SqlHealthMonitorDbContextLocal() : base("name=SqlHealthMonitorContextLocal")
        {
            Configuration.ProxyCreationEnabled = false;
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 1800;
        }
        public DbSet<PageBase> PageBases { get; set; }

      
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users", "identity").HasMany(x => x.PagesPreferences)
                .WithOptional(x => x.ApplicationUser).WillCascadeOnDelete(true);
            modelBuilder.Entity<ApplicationRole>().ToTable("Roles", "identity");
            modelBuilder.Entity<ApplicationUserClaim>().ToTable("UserClaims", "identity");
            modelBuilder.Entity<ApplicationUserLogin>().ToTable("UserLogins", "identity");
            modelBuilder.Entity<ApplicationUserRole>().ToTable("UserRoles", "identity");
            modelBuilder.Entity<HomePage>().ToTable("HomePages", "webpage");
            modelBuilder.Entity<PageBase>().ToTable("PageBases", "webpage");
            modelBuilder.Entity<SqlDashBoardPage>().ToTable("SqlDashBoardPage", "webpage");
            modelBuilder.Entity<WidgetBase>().ToTable("WidgetBases", "widget");
            modelBuilder.Entity<CpuWidget>().ToTable("CpuWidget", "widget");
            modelBuilder.Entity<JobsWidget>().ToTable("JobsWidget", "widget");
            modelBuilder.Entity<DatabasesSizeWidget>().ToTable("DatabasesSizeWidgets", "widget");

        }
    }
}
