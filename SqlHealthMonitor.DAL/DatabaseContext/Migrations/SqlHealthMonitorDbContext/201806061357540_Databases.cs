namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations.SqlHealthMonitorDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Databases : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "widget.DatabasesSizeWidgets",
                c => new
                    {
                        WidgetId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WidgetId)
                .ForeignKey("widget.WidgetBases", t => t.WidgetId)
                .Index(t => t.WidgetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("widget.DatabasesSizeWidgets", "WidgetId", "widget.WidgetBases");
            DropIndex("widget.DatabasesSizeWidgets", new[] { "WidgetId" });
            DropTable("widget.DatabasesSizeWidgets");
        }
    }
}
