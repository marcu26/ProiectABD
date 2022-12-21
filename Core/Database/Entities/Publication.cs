using Infrastructure.Base;
using System;
using System.Collections.Generic;

namespace Core.Database.Entities
{
    public class Publication : BaseEntity
    {
        public string Name { get; set; }
        public DateTime PublishedDate { get; set; }
        public List<Journal> Journals { get; set; }
    }
}
