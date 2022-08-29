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
    public class PublisherTableModel : IModifiedModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Visibility AdminMenu { get; set; }
        public ICommand BusketCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand EditCommand { get; set; }
    }
}
