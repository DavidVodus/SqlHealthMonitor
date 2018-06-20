namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations.SqlHealthMonitorDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class widgets : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "widget.WidgetBases",
                c => new
                    {
                        WidgetId = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.WidgetId)
                .ForeignKey("identity.Users", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("widget.WidgetBases", "ApplicationUserId", "identity.Users");
            DropIndex("widget.WidgetBases", new[] { "ApplicationUserId" });
            DropTable("widget.WidgetBases");
        }
    }
}
