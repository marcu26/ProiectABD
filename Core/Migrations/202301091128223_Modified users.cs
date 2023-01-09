namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modifiedusers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "Timestamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Timestamp");
            DropColumn("dbo.Users", "IsDeleted");
        }
    }
}
