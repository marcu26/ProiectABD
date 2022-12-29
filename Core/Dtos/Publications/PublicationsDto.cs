using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Publications
{
    public class PublicationsDto
    {
        public int PublicationId { get; set; }
        public string PublicationName { get; set; }
        public DateTime PublishedDate { get; set; }
        public int NumberOfJournals { get; set; }
    }
}
