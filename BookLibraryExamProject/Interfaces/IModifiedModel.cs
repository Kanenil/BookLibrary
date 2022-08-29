using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookLibraryExamProject.Interfaces
{
    public interface IModifiedModel
    {
        Visibility AdminMenu { get; set; }
        ICommand BusketCommand { get; set; }
        ICommand RemoveCommand { get; set; }
        ICommand EditCommand { get; set; }
    }
}
