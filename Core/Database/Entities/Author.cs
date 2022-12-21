using Infrastructure.Base;
using System.Collections.Generic;

namespace Core.Database.Entities
{
    public class Author : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<Book> Books { get; set; }
        public List<Article> Articles { get; set; }
    }
}
