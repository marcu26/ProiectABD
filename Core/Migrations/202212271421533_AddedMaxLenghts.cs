namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMaxLenghts : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ArticleAuthors", newName: "AuthorArticles");
            DropPrimaryKey("dbo.AuthorArticles");
            AlterColumn("dbo.Authors", "FullName", c => c.String(nullable: false));
            AlterColumn("dbo.Authors", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Articles", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Articles", "Abstract", c => c.String(nullable: false));
            AlterColumn("dbo.Keywords", "Word", c => c.String(nullable: false));
            AlterColumn("dbo.Journals", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Journals", "ISSN", c => c.String(nullable: false, maxLength: 13));
            AlterColumn("dbo.Publications", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Books", "ISBN", c => c.String(nullable: false, maxLength: 13));
            AlterColumn("dbo.Books", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Books", "Description", c => c.String(nullable: false));
            AddPrimaryKey("dbo.AuthorArticles", new[] { "Author_Id", "Article_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.AuthorArticles");
            AlterColumn("dbo.Books", "Description", c => c.String());
            AlterColumn("dbo.Books", "Title", c => c.String());
            AlterColumn("dbo.Books", "ISBN", c => c.String());
            AlterColumn("dbo.Publications", "Name", c => c.String());
            AlterColumn("dbo.Journals", "ISSN", c => c.String());
            AlterColumn("dbo.Journals", "Name", c => c.String());
            AlterColumn("dbo.Keywords", "Word", c => c.String());
            AlterColumn("dbo.Articles", "Abstract", c => c.String());
            AlterColumn("dbo.Articles", "Title", c => c.String());
            AlterColumn("dbo.Authors", "Email", c => c.String());
            AlterColumn("dbo.Authors", "FullName", c => c.String());
            AddPrimaryKey("dbo.AuthorArticles", new[] { "Article_Id", "Author_Id" });
            RenameTable(name: "dbo.AuthorArticles", newName: "ArticleAuthors");
        }
    }
}
