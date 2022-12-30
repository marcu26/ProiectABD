using Core.Database.Context;
using Core.Database.Entities;
using Core.Dtos.Journal;
using Core.Dtos.Publications;
using Infrastructure.Base;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class PublicationsRepository : BaseRepository<Publication>
    {
        public ProjectDbContext _dbContext { get; set; }

        public PublicationsRepository(ProjectDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PublicationsDto>> GetPublicationsDtoByFiltersAsync(string name, int startYear, int endYear, int pageNumber)
        {
            var list = await _dbContext.Publications
                .Include(p => p.Journals)
                .Where(p => !p.IsDeleted)
                .Select(p => new PublicationsDto 
            {
                PublicationId = p.Id,
                PublishedDate = p.PublishedDate,
                PublicationName = p.Name,
                NumberOfJournals = p.Journals.Count
            })
                .OrderBy(p=>p.PublicationName)
                .Skip((pageNumber - 1) * 50)
                .Take(50)
                .ToListAsync();

            if(name != null) 
            {
                list = list.FindAll(p => p.PublicationName.Contains(name));
            }

            if (startYear != 0 && endYear != 0)
            {
                if (startYear <= endYear)
                {
                    list = list.FindAll(p => p.PublishedDate.Year >= startYear && p.PublishedDate.Year <= endYear);
                }
            }

            return list;
        }

       
    }
}
