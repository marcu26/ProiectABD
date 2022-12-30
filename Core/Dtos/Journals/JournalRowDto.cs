using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Journals
{
    public class JournalRowDto
    {
        public string JournalId { get; set; }
        public string JournalName { get; set; }
        public string ISSN { get; set; }
        public string NumberOfVolumes { get; set; }
    }
}
