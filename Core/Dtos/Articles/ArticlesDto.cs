using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Articles
{
    public class ArticlesDto
    {
        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public string Abstract { get; set; }
        public string Authors { get; set; }
        public string Keywords { get; set; }
    }
}
