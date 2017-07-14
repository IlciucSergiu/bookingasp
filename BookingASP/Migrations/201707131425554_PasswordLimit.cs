namespace BookingASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordLimit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "Password", c => c.String(nullable: false, maxLength: 18));
        }
    }
}
