namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations.SqlHealthMonitorDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cpusettings : DbMigration
    {
        public override void Up()
        {
            AddColumn("widget.CpuWidget", "NumberOfRecords", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("widget.CpuWidget", "NumberOfRecords");
        }
    }
}
