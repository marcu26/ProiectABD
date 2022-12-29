using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Volumes
{
    public class VolumesDto
    {
        public int VolumeId { get; set; }
        public int VolumeNumber { get; set; }
        public DateTime PublishedDate { get; set; }
        public int NumberOfArticles { get; set; }
    }
}
