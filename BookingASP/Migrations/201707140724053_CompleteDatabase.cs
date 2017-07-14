namespace BookingASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompleteDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(),
                        Duration = c.Int(nullable: false),
                        Spaces = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Availability = c.String(),
                        CompanyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Companies", t => t.CompanyID, cascadeDelete: true)
                .Index(t => t.CompanyID);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 50),
                        Phone = c.String(nullable: false),
                        BookingTime = c.String(),
                        ServiceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Services", t => t.ServiceID, cascadeDelete: true)
                .Index(t => t.ServiceID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Services", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.Bookings", "ServiceID", "dbo.Services");
            DropIndex("dbo.Bookings", new[] { "ServiceID" });
            DropIndex("dbo.Services", new[] { "CompanyID" });
            DropTable("dbo.Bookings");
            DropTable("dbo.Services");
        }
    }
}
