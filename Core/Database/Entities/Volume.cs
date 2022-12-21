using Infrastructure.Base;
using System;
using System.Collections.Generic;

namespace Core.Database.Entities
{
    public class Volume : BaseEntity
    {
        public int Number { get; set; }
        public DateTime PublishedDate { get; set; }
        public int JournalId { get; set; }
        public Journal Journal { get; set; }
        public List<Article> Articles { get; set; }
    }
}
