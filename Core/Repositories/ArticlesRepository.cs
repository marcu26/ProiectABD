using Core.Database.Context;
using Core.Database.Entities;
using Core.Dtos.Articles;
using Infrastructure.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{



    public class ArticlesRepository : BaseRepository<Article>
    {
        public ProjectDbContext _dbContext;

        public ArticlesRepository(ProjectDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ArticlesDto>> GetArticlesDtoByFiltersAsync(string title, string _abstract, List<string> authors, List<string> keywords) 
        {
            List<Article> intermediaryList = await _dbContext.Articles
                .Include(ar => ar.Authors)
                .Include(ar => ar.Keywords)
                .Where(ar => !ar.IsDeleted)
                .ToListAsync();

            List<ArticlesDto> articles = intermediaryList.Select(ar => new ArticlesDto
            {
                Abstract = ar.Abstract,
                ArticleId = ar.Id,
                ArticleTitle = ar.Title,
                Authors = string.Join(", ", ar.Authors.Select(au => au.FullName)),
                Keywords = string.Join(", ", ar.Keywords.Select(au => au.Word))

            }).ToList();

         

            if (title != null) 
            {
                articles = articles.FindAll(ar => ar.ArticleTitle.Contains(title));
            }

            if (_abstract != null) 
            {
                articles = articles.FindAll(ar => ar.Abstract.Contains(_abstract));
            }

            if(authors != null) 
            {
                List<ArticlesDto> aux = new List<ArticlesDto>();
                List<ArticlesDto> aux2 = new List<ArticlesDto>();

                foreach (string author in authors) 
                {
                    aux = articles.FindAll(ar => ar.Authors.Contains(author));

                    foreach(ArticlesDto art in aux) 
                    {
                       if(!aux2.Any(ax=>ax==art))
                        aux2.Add(art);
                    }
                }

                articles = aux2;
               
            }

            if (keywords != null)
            {
                List<ArticlesDto> aux = new List<ArticlesDto>();
                List<ArticlesDto> aux2 = new List<ArticlesDto>();

                foreach (string keyword in keywords)
                {
                    aux = articles.FindAll(ar => ar.Keywords.Contains(keyword));

                    foreach (ArticlesDto art in aux)
                    {
                        if (!aux2.Any(ax => ax == art))
                            aux2.Add(art);
                    }
                }

                articles = aux2;

            }


            return articles;
        }

        public async Task<List<ArticlesDto>> GetArticlesDtoByVolumeId(int volumeId) 
        {
            List<Article> articles = await _dbContext.Articles
                .Include(ar => ar.Authors)
                .Include(ar=>ar.Keywords)
                .Where(ar => ar.VolumeId == volumeId && !ar.IsDeleted)
                .ToListAsync();


            return articles.Select(ar => new ArticlesDto
            {
                Abstract = ar.Abstract,
                ArticleId = ar.Id,
                ArticleTitle=ar.Title,
                Authors=string.Join(", ",ar.Authors.Select(au=>au.FullName)),
                Keywords=string.Join(", ",ar.Keywords.Select(au=>au.Word))


            }).ToList();
        }
    }
}
