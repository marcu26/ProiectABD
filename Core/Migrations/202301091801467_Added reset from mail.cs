namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedresetfrommail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ResetPasswordCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ResetPasswordCode");
        }
    }
}
