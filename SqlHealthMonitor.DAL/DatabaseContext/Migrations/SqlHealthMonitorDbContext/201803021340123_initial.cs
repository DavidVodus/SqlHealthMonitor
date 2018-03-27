namespace SqlHealthMonitor.DAL.DatabaseContext.Migrations.SqlHealthMonitorDbContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "webpage.PageBases",
                c => new
                    {
                        PageId = c.Int(nullable: false, identity: true),
                        ControllerName = c.String(),
                        PageName = c.String(),
                        StartActionName = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PageId)
                .ForeignKey("identity.Users", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "identity.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Language = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "identity.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("identity.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "identity.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("identity.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "identity.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("identity.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("identity.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "identity.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "webpage.HomePages",
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
            DropForeignKey("webpage.HomePages", "PageId", "webpage.PageBases");
            DropForeignKey("identity.UserRoles", "RoleId", "identity.Roles");
            DropForeignKey("identity.UserRoles", "UserId", "identity.Users");
            DropForeignKey("webpage.PageBases", "ApplicationUserId", "identity.Users");
            DropForeignKey("identity.UserLogins", "UserId", "identity.Users");
            DropForeignKey("identity.UserClaims", "UserId", "identity.Users");
            DropIndex("webpage.HomePages", new[] { "PageId" });
            DropIndex("identity.Roles", "RoleNameIndex");
            DropIndex("identity.UserRoles", new[] { "RoleId" });
            DropIndex("identity.UserRoles", new[] { "UserId" });
            DropIndex("identity.UserLogins", new[] { "UserId" });
            DropIndex("identity.UserClaims", new[] { "UserId" });
            DropIndex("identity.Users", "UserNameIndex");
            DropIndex("webpage.PageBases", new[] { "ApplicationUserId" });
            DropTable("webpage.HomePages");
            DropTable("identity.Roles");
            DropTable("identity.UserRoles");
            DropTable("identity.UserLogins");
            DropTable("identity.UserClaims");
            DropTable("identity.Users");
            DropTable("webpage.PageBases");
        }
    }
}
