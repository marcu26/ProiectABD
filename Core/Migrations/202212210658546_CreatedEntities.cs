namespace Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Abstract = c.String(),
                        VolumeId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Volumes", t => t.VolumeId, cascadeDelete: true)
                .Index(t => t.VolumeId);
            
            CreateTable(
                "dbo.Keywords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Word = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Volumes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        PublishedDate = c.DateTime(nullable: false),
                        JournalId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Journals", t => t.JournalId, cascadeDelete: true)
                .Index(t => t.JournalId);
            
            CreateTable(
                "dbo.Journals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ISSN = c.String(),
                        PublicationId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Publications", t => t.PublicationId, cascadeDelete: true)
                .Index(t => t.PublicationId);
            
            CreateTable(
                "dbo.Publications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PublishedDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ISBN = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ArticleAuthors",
                c => new
                    {
                        Article_Id = c.Int(nullable: false),
                        Author_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Article_Id, t.Author_Id })
                .ForeignKey("dbo.Articles", t => t.Article_Id, cascadeDelete: true)
                .ForeignKey("dbo.Authors", t => t.Author_Id, cascadeDelete: true)
                .Index(t => t.Article_Id)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.KeywordArticles",
                c => new
                    {
                        Keyword_Id = c.Int(nullable: false),
                        Article_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Keyword_Id, t.Article_Id })
                .ForeignKey("dbo.Keywords", t => t.Keyword_Id, cascadeDelete: true)
                .ForeignKey("dbo.Articles", t => t.Article_Id, cascadeDelete: true)
                .Index(t => t.Keyword_Id)
                .Index(t => t.Article_Id);
            
            CreateTable(
                "dbo.BookAuthors",
                c => new
                    {
                        Book_Id = c.Int(nullable: false),
                        Author_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Book_Id, t.Author_Id })
                .ForeignKey("dbo.Books", t => t.Book_Id, cascadeDelete: true)
                .ForeignKey("dbo.Authors", t => t.Author_Id, cascadeDelete: true)
                .Index(t => t.Book_Id)
                .Index(t => t.Author_Id);
            
            AddColumn("dbo.Authors", "FullName", c => c.String());
            AddColumn("dbo.Authors", "Email", c => c.String());
            DropColumn("dbo.Authors", "Nume");
            DropColumn("dbo.Authors", "Prenume");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Authors", "Prenume", c => c.String());
            AddColumn("dbo.Authors", "Nume", c => c.String());
            DropForeignKey("dbo.BookAuthors", "Author_Id", "dbo.Authors");
            DropForeignKey("dbo.BookAuthors", "Book_Id", "dbo.Books");
            DropForeignKey("dbo.Volumes", "JournalId", "dbo.Journals");
            DropForeignKey("dbo.Journals", "PublicationId", "dbo.Publications");
            DropForeignKey("dbo.Articles", "VolumeId", "dbo.Volumes");
            DropForeignKey("dbo.KeywordArticles", "Article_Id", "dbo.Articles");
            DropForeignKey("dbo.KeywordArticles", "Keyword_Id", "dbo.Keywords");
            DropForeignKey("dbo.ArticleAuthors", "Author_Id", "dbo.Authors");
            DropForeignKey("dbo.ArticleAuthors", "Article_Id", "dbo.Articles");
            DropIndex("dbo.BookAuthors", new[] { "Author_Id" });
            DropIndex("dbo.BookAuthors", new[] { "Book_Id" });
            DropIndex("dbo.KeywordArticles", new[] { "Article_Id" });
            DropIndex("dbo.KeywordArticles", new[] { "Keyword_Id" });
            DropIndex("dbo.ArticleAuthors", new[] { "Author_Id" });
            DropIndex("dbo.ArticleAuthors", new[] { "Article_Id" });
            DropIndex("dbo.Journals", new[] { "PublicationId" });
            DropIndex("dbo.Volumes", new[] { "JournalId" });
            DropIndex("dbo.Articles", new[] { "VolumeId" });
            DropColumn("dbo.Authors", "Email");
            DropColumn("dbo.Authors", "FullName");
            DropTable("dbo.BookAuthors");
            DropTable("dbo.KeywordArticles");
            DropTable("dbo.ArticleAuthors");
            DropTable("dbo.Books");
            DropTable("dbo.Publications");
            DropTable("dbo.Journals");
            DropTable("dbo.Volumes");
            DropTable("dbo.Keywords");
            DropTable("dbo.Articles");
        }
    }
}
