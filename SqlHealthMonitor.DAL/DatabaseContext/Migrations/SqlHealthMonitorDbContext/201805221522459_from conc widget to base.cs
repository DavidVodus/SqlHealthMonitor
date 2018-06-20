namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations.SqlHealthMonitorDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fromconcwidgettobase : DbMigration
    {
        public override void Up()
        {
            DropIndex("widget.CpuWidget", new[] { "SqlServerDataId" });
            AddColumn("widget.WidgetBases", "SqlServerDataId", c => c.Int());
            CreateIndex("widget.WidgetBases", "SqlServerDataId");
            DropColumn("widget.CpuWidget", "SqlServerDataId");
        }
        
        public override void Down()
        {
            AddColumn("widget.CpuWidget", "SqlServerDataId", c => c.Int());
            DropIndex("widget.WidgetBases", new[] { "SqlServerDataId" });
            DropColumn("widget.WidgetBases", "SqlServerDataId");
            CreateIndex("widget.CpuWidget", "SqlServerDataId");
        }
    }
}
