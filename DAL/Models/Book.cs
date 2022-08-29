using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Book
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string BookName { get; set; }
        [MaxLength(60)]
        public string Style { get; set; }
        public int CountPage { get; set; }
        public int Count { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
        public virtual Action ActionBook { get; set; }
        public virtual Author AutorBook { get; set; }
        public virtual Publisher PublisherBook { get; set; }
    }
}
