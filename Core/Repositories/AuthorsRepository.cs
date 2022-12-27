using Core.Database.Context;
using Core.Database.Entities;
using Infrastructure.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class AuthorsRepository : BaseRepository<Author>
    {
        public ProjectDbContext _dbContext;

        public AuthorsRepository(ProjectDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Author> GetAuthors() 
        {
            return _dbContext.Authors.ToList();
        }
    }
}
