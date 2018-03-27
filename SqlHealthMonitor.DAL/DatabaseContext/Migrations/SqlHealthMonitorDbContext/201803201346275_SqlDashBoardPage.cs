namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations.SqlHealthMonitorDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SqlDashBoardPage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "webpage.SqlDashBoardPage",
                c => new
                    {
                        PageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PageId)
                .ForeignKey("webpage.PageBases", t => t.PageId)
                .Index(t => t.PageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("webpage.SqlDashBoardPage", "PageId", "webpage.PageBases");
            DropIndex("webpage.SqlDashBoardPage", new[] { "PageId" });
            DropTable("webpage.SqlDashBoardPage");
        }
    }
}
