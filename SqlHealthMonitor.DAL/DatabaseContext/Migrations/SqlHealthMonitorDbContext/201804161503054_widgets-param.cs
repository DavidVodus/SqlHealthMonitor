namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations.SqlHealthMonitorDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class widgetsparam : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "widget.CpuWidget",
                c => new
                    {
                        WidgetId = c.Int(nullable: false),
                        ConnectionString = c.String(),
                    })
                .PrimaryKey(t => t.WidgetId)
                .ForeignKey("widget.WidgetBases", t => t.WidgetId)
                .Index(t => t.WidgetId);
            
            AddColumn("widget.WidgetBases", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("widget.CpuWidget", "WidgetId", "widget.WidgetBases");
            DropIndex("widget.CpuWidget", new[] { "WidgetId" });
            DropColumn("widget.WidgetBases", "Name");
            DropTable("widget.CpuWidget");
        }
    }
}
