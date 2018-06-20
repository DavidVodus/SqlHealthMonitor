namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations.SqlHealthMonitorDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fromconcwidgettobase2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "widget.JobsWidget",
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
            DropForeignKey("widget.JobsWidget", "WidgetId", "widget.WidgetBases");
            DropIndex("widget.JobsWidget", new[] { "WidgetId" });
            DropTable("widget.JobsWidget");
        }
    }
}
