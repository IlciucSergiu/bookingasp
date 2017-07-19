namespace BookingASP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogoPath : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "Logo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "Logo", c => c.Binary());
        }
    }
}
