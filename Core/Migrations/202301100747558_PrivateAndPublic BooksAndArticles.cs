namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrivateAndPublicBooksAndArticles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "IsPublic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Books", "IsPublic", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "IsPublic");
            DropColumn("dbo.Articles", "IsPublic");
        }
    }
}
