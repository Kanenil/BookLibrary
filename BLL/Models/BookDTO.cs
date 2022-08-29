using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string Style { get; set; }
        public int CountPage { get; set; }
        public int Count { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public virtual ActionDTO ActionBook { get; set; }
        public virtual AuthorDTO AutorBook { get; set; }
        public virtual PublisherDTO PublisherBook { get; set; }
    }
}
