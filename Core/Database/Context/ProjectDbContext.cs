using Core.Database.Entities;
using System.Data.Entity;


namespace Core.Database.Context
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext() : base("name=ProjectABD") { }

        public DbSet<Author> Authors {get; set;}

      

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
