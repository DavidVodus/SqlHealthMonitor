namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations.SqlHealthMonitorDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SqlServercollectiontouser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SqlServerDatas",
                c => new
                    {
                        SqlServerDataId = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        Name = c.String(),
                        ConnectionString = c.String(),
                    })
                .PrimaryKey(t => t.SqlServerDataId)
                .ForeignKey("identity.Users", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SqlServerDatas", "ApplicationUserId", "identity.Users");
            DropIndex("dbo.SqlServerDatas", new[] { "ApplicationUserId" });
            DropTable("dbo.SqlServerDatas");
        }
    }
}
