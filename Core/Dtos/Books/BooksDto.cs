using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dtos.Books
{
    public class BooksDto
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string Authors { get; set; }


    }
}
