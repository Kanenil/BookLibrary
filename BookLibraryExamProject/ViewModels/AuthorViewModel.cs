using BLL.Models;
using BLL.Services;
using BookLibraryExamProject.CustomCommands;
using BookLibraryExamProject.Interfaces;
using BookLibraryExamProject.Models;
using BookLibraryExamProject.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookLibraryExamProject.ViewModels
{
    public class AuthorViewModel : IViewModelBase
    {
        #region Fields
        private AuthorService _authorService;
        private AuthorTableModel _selected;
        private List<AuthorTableModel> _authors;
        private string _find;
        private string _message;
        private string _lastName;
        private string _name;
        private string _patronymic;
        #endregion
        #region Properties
        public List<AuthorTableModel> GetAuthors
        {
            get
            {
                return _authors;
            }

            set
            {
                _authors = value;
                OnPropertyChanged();
            }
        }
        public AuthorTableModel Selected
        {
            get
            {
                return _selected;
            }

            set
            {
                _selected = value;
                OnPropertyChanged();
            }
        }
        public string Find
        {
            get
            {
                return _find;
            }

            set
            {
                _find = value;
                if (String.IsNullOrEmpty(value))
                {
                    UpdateAuthors(_authorService.GetAll());
                }
                else
                {
                    UpdateAuthors(_authorService.FindAll(Find));
                }
                OnPropertyChanged();
            }
        }
        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }
        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }
        public string Patronymic
        {
            get
            {
                return _patronymic;
            }

            set
            {
                _patronymic = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Commands
        public ICommand AddCommand { get; }

        #endregion
        #region Constructor
        public AuthorViewModel()
        {
            InitializeComponent();
            AddCommand = new RelayCommand(AddCommandExecute);
        }
        #endregion
        #region Methods
        private void InitializeComponent()
        {
            _authorService = new AuthorService(_connectionString);
            UpdateAuthors(_authorService.GetAll());
        }
        public void UpdateAuthors(IEnumerable<AuthorDTO> authorDTOs = null)
        {
            if (authorDTOs == null)
            {
                GetAuthors = ConvertIEnumerableToObservableCollection(_authorService.GetAll());
            }
            else
            {
                GetAuthors = ConvertIEnumerableToObservableCollection(authorDTOs);
            }
        }
        private List<AuthorTableModel> ConvertIEnumerableToObservableCollection(IEnumerable<AuthorDTO> authorDTOs)
        {
            var temp = new List<AuthorTableModel>();
            if (authorDTOs != null)
            {
                foreach (var item in authorDTOs)
                    temp.Add(new AuthorTableModel()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        LastName = item.LastName,
                        Patronymic = item.Patronymic,
                        AdminMenu = CurrentUser.IsAdmin == true ? Visibility.Visible : Visibility.Collapsed,
                        EditCommand = new RelayCommand(EditCommandExecute),
                        RemoveCommand = new RelayCommand(RemoveCommandExecute)
                    }); ;
            }
            return temp;
        }
        private void AddCommandExecute(object obj)
        {
            var window = new AddEditAuthorWindow();
            window.btnLogin.Content = "ADD";
            window.windowName.Text = "ADD AUTHOR";
            window.btnLogin.Command = new RelayCommand((window.DataContext as AuthorViewModel).AddAuthorCommandExecute, (window.DataContext as AuthorViewModel).EditAuthorCommandCanExecute);
            window.ShowDialog();
            UpdateAuthors(_authorService.GetAll());
        }
        private void EditCommandExecute(object obj)
        {
            var window = new AddEditAuthorWindow();
            window.btnLogin.Content = "COMMIT";
            window.windowName.Text = "EDIT AUTHOR";
            window.btnLogin.Command = new RelayCommand((window.DataContext as AuthorViewModel).EditAuthorCommandExecute, (window.DataContext as AuthorViewModel).EditAuthorCommandCanExecute);
            (window.DataContext as AuthorViewModel).Name = Selected.Name;
            (window.DataContext as AuthorViewModel).LastName = Selected.LastName;
            (window.DataContext as AuthorViewModel).Patronymic = Selected.Patronymic;
            (window.DataContext as AuthorViewModel).Selected = Selected;
            window.ShowDialog();
            UpdateAuthors(_authorService.GetAll());
        }
        private void AddAuthorCommandExecute(object obj)
        {
            _authorService.Create(new AuthorDTO()
            {
                Name = Name,
                LastName = LastName,
                Patronymic = Patronymic
            });
            Message = "Succesfuly added";
        }
        private void EditAuthorCommandExecute(object obj)
        {
            _authorService.Update(new AuthorDTO()
            {
                Id = Selected.Id,
                Name = Name,
                LastName = LastName,
                Patronymic = Patronymic
            });
            Message = "Succesfuly edited";
        }
        private bool EditAuthorCommandCanExecute(object obj)
        {
            if (String.IsNullOrWhiteSpace(Name))
                return false;
            if (Name.Length > 50)
                return false;
            if (String.IsNullOrWhiteSpace(LastName))
                return false;
            if (LastName.Length > 50)
                return false;
            if (String.IsNullOrWhiteSpace(Patronymic))
                return false;
            if (LastName.Length > 50)
                return false;


            return true;
        }
        private void RemoveCommandExecute(object obj)
        {
            _authorService.Delete(Selected.Id);
            GetAuthors = ConvertIEnumerableToObservableCollection(_authorService.GetAll());
        }
        #endregion
    }
}
