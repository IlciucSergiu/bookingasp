namespace BookingASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueEmail : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Companies", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Companies", new[] { "Email" });
        }
    }
}
