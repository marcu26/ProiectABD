using Infrastructure.Base;
using System.Collections.Generic;

namespace Core.Database.Entities
{
    public class Journal : BaseEntity
    {
        public string Name { get; set; }
        public string ISSN { get; set; }
        public int PublicationId { get; set; }
        public Publication Publication { get; set; }
        public List<Volume> Volumes { get; set; }
    }
}
