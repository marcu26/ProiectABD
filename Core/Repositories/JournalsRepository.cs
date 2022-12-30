using Core.Database.Context;
using Core.Database.Entities;
using Core.Dtos.Journal;
using Infrastructure.Base;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class JournalsRepository: BaseRepository<Journal>
    {
        public ProjectDbContext _dbContext { get; set; }

        public JournalsRepository(ProjectDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<JournalsDto>> GetJournalsDtoByFiltersAsync(string name, string ISSN, int pageNumber) 
        {
            var list = await _dbContext.Journals
                .Include(j => j.Volumes)
                .Where(j => !j.IsDeleted)
                .Select(j => new JournalsDto
                {
                    ISSN = j.ISSN,
                    JournalName = j.Name,
                    JournalId = j.Id,
                    NumberOfVolumes = j.Volumes.Count
                })
                .OrderBy(j=>j.JournalName)
                .Skip((pageNumber - 1) * 50)
                .Take(50)
                .ToListAsync();

            if(name != null) 
            {
                list = list.FindAll(j => j.JournalName.Contains(name));
            }

            if(ISSN != null) 
            {
                list = list.FindAll(j => j.ISSN.Contains(ISSN));
            }

            return list;
        }

        public async Task<List<JournalsDto>> GetJournalsDtoByPublicationIdAsync(int publicationId, int pageNumber) 
        {
            return await _dbContext.Journals
               .Include(j => j.Volumes)
               .Where(j => !j.IsDeleted && j.PublicationId == publicationId)
               .Select(j => new JournalsDto
               {
                   ISSN = j.ISSN,
                   JournalName = j.Name,
                   JournalId = j.Id,
                   NumberOfVolumes = j.Volumes.Count
               })
               .OrderBy(j=>j.JournalName)
               .Skip((pageNumber - 1) * 50)
               .Take(50)
               .ToListAsync();
        }
    }
}
