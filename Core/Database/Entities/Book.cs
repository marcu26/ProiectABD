using Infrastructure.Base;
using System.Collections.Generic;

namespace Core.Database.Entities
{
    public class Book:BaseEntity
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Author> Authors { get; set; }
    }
}
