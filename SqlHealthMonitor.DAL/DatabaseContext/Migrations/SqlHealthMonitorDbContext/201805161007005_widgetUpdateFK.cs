namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations.SqlHealthMonitorDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class widgetUpdateFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("widget.CpuWidget", "SqlServerDataId", "dbo.SqlServerDatas");
            DropIndex("widget.CpuWidget", new[] { "SqlServerDataId" });
            AlterColumn("widget.CpuWidget", "SqlServerDataId", c => c.Int());
            CreateIndex("widget.CpuWidget", "SqlServerDataId");
            AddForeignKey("widget.CpuWidget", "SqlServerDataId", "dbo.SqlServerDatas", "SqlServerDataId");
        }
        
        public override void Down()
        {
            DropForeignKey("widget.CpuWidget", "SqlServerDataId", "dbo.SqlServerDatas");
            DropIndex("widget.CpuWidget", new[] { "SqlServerDataId" });
            AlterColumn("widget.CpuWidget", "SqlServerDataId", c => c.Int(nullable: false));
            CreateIndex("widget.CpuWidget", "SqlServerDataId");
            AddForeignKey("widget.CpuWidget", "SqlServerDataId", "dbo.SqlServerDatas", "SqlServerDataId", cascadeDelete: true);
        }
    }
}
