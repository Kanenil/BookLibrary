using BLL.Models;
using BookLibraryExamProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookLibraryExamProject.Models
{
    public class BookTableModel : IModifiedModel
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string Style { get; set; }
        public int CountPage { get; set; }
        public int Count { get; set; }
        public int Year { get; set; }
        public decimal OldPrice { get; set; }
        public decimal PriceForSale { get; set; }
        public virtual ActionDTO ActionBook { get; set; }
        public virtual AuthorDTO AutorBook { get; set; }
        public virtual PublisherDTO PublisherBook { get; set; }
        public TextDecorationCollection IsDiscount { get; set; }
        public Visibility Discount { get; set; }
        public Visibility AdminMenu { get; set; }
        public ICommand BusketCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand EditCommand { get; set; }
    }
}
