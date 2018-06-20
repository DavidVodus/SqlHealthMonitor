namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations.SqlHealthMonitorDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datid : DbMigration
    {
        public override void Up()
        {
            AddColumn("widget.DatabasesSizeWidgets", "DatabaseIdsStorage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("widget.DatabasesSizeWidgets", "DatabaseIdsStorage");
        }
    }
}
