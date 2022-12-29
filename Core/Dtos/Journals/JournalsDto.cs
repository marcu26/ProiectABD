using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Journal
{
    public class JournalsDto
    {
        public int JournalId { get; set; }
        public string JournalName { get; set; }
        public string ISSN { get; set; }
        public int NumberOfVolumes { get; set; }
    }
}
