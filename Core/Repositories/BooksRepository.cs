using Core.Database.Context;
using Core.Database.Entities;
using Core.Dtos.Articles;
using Core.Dtos.Books;
using Infrastructure.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public class BooksRepository : BaseRepository<Book>
    {
        public ProjectDbContext _dbContext;

        public BooksRepository(ProjectDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<BooksDto>> GetBooksDtoByFilters(string _ISBN, string title, string description, List<string> authors, int pageNumber) 
        {
            List<Book> intermediaryList = await _dbContext.Books
                .Include(b => b.Authors)
                .ToListAsync();

            List<BooksDto> books = intermediaryList.Select(b => new BooksDto
            {
                BookTitle=b.Title,
                BookId =b.Id,
                Description=b.Description,
                ISBN=b.ISBN,
                Authors = string.Join(", ", b.Authors.Select(au => au.FullName))

            }).ToList();



            if (title != null)
            {
                books = books.FindAll(b => b.BookTitle.Contains(title));
            }

            if (description != null)
            {
                books = books.FindAll(b => b.Description.Contains(description));
            }

            if (_ISBN != null)
            {
                books = books.FindAll(b => b.ISBN.Contains(_ISBN));
            }

            if (authors != null)
            {
                List<BooksDto> aux = new List<BooksDto>();
                List<BooksDto> aux2 = new List<BooksDto>();

                foreach (string author in authors)
                {
                    aux = books.FindAll(b => b.Authors.Contains(author));

                    foreach (BooksDto bok in aux)
                    {
                        if (!aux2.Any(ax => ax == bok))
                            aux2.Add(bok);
                    }
                }

                books = aux2;
            }

        
           

            return books
                 .OrderBy(b => b.BookTitle)
                 .Skip((pageNumber - 1) * 50)
                 .Take(50)
                 .ToList();
        }


        public async Task<int> GetNumberOfBookPagesByFilter(string _ISBN, string title, string description, List<string> authors)
        {
            List<Book> intermediaryList = await _dbContext.Books
                .Include(b => b.Authors)
                .ToListAsync();

            List<BooksDto> books = intermediaryList.Select(b => new BooksDto
            {
                BookTitle = b.Title,
                BookId = b.Id,
                Description = b.Description,
                ISBN = b.ISBN,
                Authors = string.Join(", ", b.Authors.Select(au => au.FullName))

            }).ToList();



            if (title != null)
            {
                books = books.FindAll(b => b.BookTitle.Contains(title));
            }

            if (description != null)
            {
                books = books.FindAll(b => b.Description.Contains(description));
            }

            if (_ISBN != null)
            {
                books = books.FindAll(b => b.ISBN.Contains(_ISBN));
            }

            if (authors != null)
            {
                List<BooksDto> aux = new List<BooksDto>();
                List<BooksDto> aux2 = new List<BooksDto>();

                foreach (string author in authors)
                {
                    aux = books.FindAll(b => b.Authors.Contains(author));

                    foreach (BooksDto bok in aux)
                    {
                        if (!aux2.Any(ax => ax == bok))
                            aux2.Add(bok);
                    }
                }

                books = aux2;
            }

            return books.Count()/50+1;
        }
    }
}




