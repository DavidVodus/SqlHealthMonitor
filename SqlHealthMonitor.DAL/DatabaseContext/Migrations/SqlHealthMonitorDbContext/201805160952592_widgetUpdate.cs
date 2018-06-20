namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations.SqlHealthMonitorDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class widgetUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("widget.CpuWidget", "SqlServerDataId", c => c.Int(nullable: false));
            CreateIndex("widget.CpuWidget", "SqlServerDataId");
            AddForeignKey("widget.CpuWidget", "SqlServerDataId", "dbo.SqlServerDatas", "SqlServerDataId", cascadeDelete: true);
            DropColumn("widget.CpuWidget", "ConnectionString");
        }
        
        public override void Down()
        {
            AddColumn("widget.CpuWidget", "ConnectionString", c => c.String());
            DropForeignKey("widget.CpuWidget", "SqlServerDataId", "dbo.SqlServerDatas");
            DropIndex("widget.CpuWidget", new[] { "SqlServerDataId" });
            DropColumn("widget.CpuWidget", "SqlServerDataId");
        }
    }
}
