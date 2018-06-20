namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations.SqlHealthMonitorDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class necopridano : DbMigration
    {
        public override void Up()
        {
            AddColumn("widget.WidgetBases", "UpdateInterval", c => c.Long(nullable: false));
            AddColumn("widget.WidgetBases", "Order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("widget.WidgetBases", "Order");
            DropColumn("widget.WidgetBases", "UpdateInterval");
        }
    }
}
