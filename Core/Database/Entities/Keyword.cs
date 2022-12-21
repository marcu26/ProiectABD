using Infrastructure.Base;
using System.Collections.Generic;

namespace Core.Database.Entities
{
    public class Keyword : BaseEntity
    {
        public string Word { get; set; }
        public List<Article> Articles { get; set; }
    }
}
