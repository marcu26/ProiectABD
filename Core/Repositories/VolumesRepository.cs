using Core.Database.Context;
using Core.Database.Entities;
using Core.Dtos.Volumes;
using Infrastructure.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class VolumesRepository : BaseRepository<Volume>
    {
        public ProjectDbContext _dbContext;

        public VolumesRepository(ProjectDbContext dbContext) : base(dbContext)
        {
        _dbContext = dbContext;
        }
        public async Task<List<VolumesDto>> GetVolumesDtoByFiltersAsync(int number, int startYear, int endYear, int pageNumber) 
        {
            var list = await _dbContext.Volumes
                .Include(v => v.Articles)
                .Where(v => !v.IsDeleted)
                .Select(v => new VolumesDto
                {
                    VolumeId = v.Id,
                    VolumeNumber =v.Number,
                    NumberOfArticles = v.Articles.Count,
                    PublishedDate = v.PublishedDate
                })
                .Skip((pageNumber - 1) * 50)
                .Take(50)
                .ToListAsync();

            if (number > 0)
                list = list.FindAll(v => v.VolumeNumber == number);

            if (startYear != 0 && endYear != 0)
            {
                if (startYear <= endYear)
                    list = list.FindAll(v => v.PublishedDate.Year >= startYear && v.PublishedDate.Year <= endYear);
            }
            return list;
        }

        public async Task<List<VolumesDto>> GetVolumesDtoByJournalId(int journalId, int pageNumber) 
        {
            return await _dbContext.Volumes
               .Include(v => v.Articles)
               .Where(v => !v.IsDeleted && v.JournalId == journalId)
               .Select(v => new VolumesDto
               {
                   VolumeId = v.Id,
                   VolumeNumber = v.Number,
                   NumberOfArticles = v.Articles.Count,
                   PublishedDate = v.PublishedDate
               })
               .OrderBy(v=>v.PublishedDate) 
               .Skip((pageNumber - 1) * 50)
               .Take(50)
               .ToListAsync();
        }



    }
}
