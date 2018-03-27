using System.Data.Entity.Migrations;

namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext.SqlHealthMonitorDbContext>
    {
        public Configuration()
        {
            CommandTimeout = 100000; // migration timeout
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DatabaseContext.SqlHealthMonitorDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
