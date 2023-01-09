using Core.Database.Entities;
using System.Data.Entity;
using System.Security.Permissions;

namespace Core.Database.Context
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext() : base("name=ProjectABD") { }

        public DbSet<Author> Authors {get; set;}
        public DbSet<Article> Articles {get; set;}
        public DbSet<Book> Books {get; set;}
        public DbSet<Journal> Journals {get; set;}
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Volume> Volumes { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().Property(b => b.ISBN).HasMaxLength(13);
            modelBuilder.Entity<Journal>().Property(b => b.ISSN).HasMaxLength(13);
            modelBuilder.Entity<Article>().Property(a => a.Title).IsRequired();
            modelBuilder.Entity<Article>().Property(a => a.Abstract).IsRequired();
            modelBuilder.Entity<Author>().Property(a => a.FullName).IsRequired();
            modelBuilder.Entity<Author>().Property(a => a.Email).IsRequired();
            modelBuilder.Entity<Book>().Property(b => b.Description).IsRequired();
            modelBuilder.Entity<Book>().Property(b => b.Title).IsRequired();
            modelBuilder.Entity<Book>().Property(b => b.ISBN).IsRequired();
            modelBuilder.Entity<Journal>().Property(j => j.ISSN).IsRequired();
            modelBuilder.Entity<Journal>().Property(j => j.Name).IsRequired();
            modelBuilder.Entity<Keyword>().Property(k => k.Word).IsRequired();
            modelBuilder.Entity<Publication>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.ResetPasswordCode).IsOptional();

        }
    }
}
