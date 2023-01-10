using Infrastructure.Base;
using System.Collections.Generic;

namespace Core.Database.Entities
{
    public class Article : BaseEntity
    {
        public string Title { get; set; }
        public string Abstract { get; set; }
        public int VolumeId { get; set; }
        public Volume Volume { get; set; }
        public List<Author> Authors { get; set; }
        public List<Keyword> Keywords { get; set; }
        public bool IsPublic { get; set; }
    }
}
