using Core.Database.Context;
using Core.Database.Entities;
using Infrastructure.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Core.Repositories
{
    public class KeywordsRepository : BaseRepository<Keyword>
    {
        public ProjectDbContext _dbContext { get; set; }

        public KeywordsRepository(ProjectDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> GetKeywordsAsync() 
        {
            return await _dbContext.Keywords.Select(k => k.Word).ToListAsync();
        }


    }
}
