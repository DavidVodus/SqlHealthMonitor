namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations.SqlHealthMonitorDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class widthheight : DbMigration
    {
        public override void Up()
        {
            AddColumn("widget.WidgetBases", "Width", c => c.Int(nullable: false));
            AddColumn("widget.WidgetBases", "Height", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("widget.WidgetBases", "Height");
            DropColumn("widget.WidgetBases", "Width");
        }
    }
}
