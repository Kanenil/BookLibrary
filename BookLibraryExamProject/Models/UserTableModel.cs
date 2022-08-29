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
    public class UserTableModel : IModifiedModel
    {
        public UserTableModel() { Books = new List<BookTableModel>(); }
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public virtual PeopleDTO UserInfo { get; set; }
        public BookTableModel Selected { get; set; }
        public virtual IList<BookTableModel> Books { get; set; }
        public Visibility AddAdmin { get; set; }
        public Visibility AdminMenu { get; set; }
        public ICommand BusketCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand EditCommand { get; set; }
    }
}
