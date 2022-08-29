using BLL.Models;
using BLL.Services;
using BookLibraryExamProject.CustomCommands;
using BookLibraryExamProject.Interfaces;
using BookLibraryExamProject.Models;
using BookLibraryExamProject.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookLibraryExamProject.ViewModels
{
    public class UserViewModel : IViewModelBase
    {
        #region Field
        private BookService _bookService;
        private PeopleService _peopleService;
        private List<UserTableModel> _users;
        private UserTableModel _selectedUser;
        private List<BookTableModel> _books;
        private BookTableModel _selected;
        private bool _isValueEditChanged;
        private string _find;
        private string _buyText;
        private SecureString _oldPassword;
        private SecureString _password;
        private SecureString _confirmPassword;
        private string _name;
        private string _lastName;
        private string _email;
        #endregion
        #region Properties
        public List<UserTableModel> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }
        public UserTableModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }
        public List<BookTableModel> Books
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged();
            }
        }
        public BookTableModel Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged();
            }
        }
        public string Find
        {
            get => _find;
            set
            {
                _find = value;
                if (String.IsNullOrEmpty(value))
                {
                    UpdateTable();
                }
                else
                {
                    UpdateTable(_userService.Find(CurrentUser.Id).Books
                                                                 .Where(b => b.BookName.ToUpper().Contains(value.ToUpper()) ||
                                                                             b.AutorBook.Name.ToUpper().Contains(value.ToUpper()) ||
                                                                             b.AutorBook.LastName.ToUpper().Contains(value.ToUpper()) ||
                                                                             b.AutorBook.Patronymic.ToUpper().Contains(value.ToUpper()) ||
                                                                             b.Style.ToUpper().Contains(value.ToUpper()) ||
                                                                             b.PublisherBook.Name.ToUpper().Contains(value.ToUpper())));
                }
                OnPropertyChanged();
            }
        }
        public string BuyText
        {
            get => _buyText;
            set
            {
                _buyText = value;
                OnPropertyChanged();
            }
        }
        public SecureString OldPassword
        {
            get => _oldPassword;
            set
            {
                _oldPassword = value;
                OnPropertyChanged();
            }
        }
        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        public SecureString ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                _isValueEditChanged = true;
                OnPropertyChanged();
            }
        }
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                _isValueEditChanged = true;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                _isValueEditChanged = true;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Commands
        public ICommand BuyCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand DeleteAccountCommand { get; }
        public ICommand EditInformationCommand { get; }
        public ICommand ChangePasswordCommand { get; }
        #endregion
        #region Constructor
        public UserViewModel()
        {
            InitializeComponent();
            BuyCommand = new RelayCommand(BuyExecute, b => CurrentUser.Books.Count != 0);
            LogOutCommand = new RelayCommand(LogOutExecute);
            EditInformationCommand = new RelayCommand(EditInformationExecute, EditInformationCanExecute);
            ChangePasswordCommand = new RelayCommand(ChangePasswordExecute, ChangePasswordCanExecute);
            DeleteAccountCommand = new RelayCommand(DeleteAccountExecute);
        }
        #endregion
        #region Methods
        private void InitializeComponent()
        {
            UpdateTable();
            _bookService = new BookService(_connectionString);
            _peopleService = new PeopleService(_connectionString);
            UpdateUsers();
            _isValueEditChanged = false;
            _name = CurrentUser.UserInfo.Name;
            _lastName = CurrentUser.UserInfo.LastName;
            _email = CurrentUser.UserInfo.Email;

        }
        public void UpdateTable(IEnumerable<BookDTO> bookDTOs = null)
        {
            CurrentUser = _userService.Find(CurrentUser.Id);
            if (bookDTOs == null)
            {
                Books = ConvertIEnumerableToObservableCollection(_userService.Find(CurrentUser.Id).Books);
                decimal sum = 0;
                foreach (var book in Books)
                    sum += book.PriceForSale * book.Count;
                BuyText = $"🛒 {decimal.Round(sum, 2, MidpointRounding.AwayFromZero)}";
            }
            else
                Books = ConvertIEnumerableToObservableCollection(bookDTOs);
        }
        public void UpdateUsers(IEnumerable<UserDTO> users = null)
        {
            if (users == null)
            {
                var tempUsers = new List<UserTableModel>();
                foreach (var item in _userService.GetAll())
                {
                    if (item.Id == CurrentUser.Id)
                        continue;

                    var tempUser = new UserTableModel()
                    {
                        Id = item.Id,
                        Login = item.Login,
                        Password = item.Password,
                        UserInfo = item.UserInfo,
                        AddAdmin = item.IsAdmin ? Visibility.Collapsed : Visibility.Visible,
                        AdminMenu = item.Books.Count > 0 && !item.IsAdmin ? Visibility.Visible : Visibility.Collapsed,
                        EditCommand = new RelayCommand(AddAdminCommandExecute),
                        RemoveCommand = new RelayCommand(RemoveUserExecute)
                    };
                    var books = new List<BookTableModel>();
                    foreach (var book in item.Books)
                    {
                        books.Add(new BookTableModel()
                        {
                            Id = book.Id,
                            BookName = book.BookName,
                            Style = book.Style,
                            AutorBook = book.AutorBook,
                            CountPage = book.CountPage,
                            PriceForSale = book.ActionBook == null ? book.Price : book.Price - (book.Price * book.ActionBook.Discount / 100),
                            PublisherBook = book.PublisherBook,
                            ActionBook = book.ActionBook,
                            Discount = book.ActionBook != null ? Visibility.Visible : Visibility.Hidden,
                            IsDiscount = book.ActionBook != null ? TextDecorations.Strikethrough : null,
                            OldPrice = book.Price,
                            Year = book.Year,
                            Count = book.Count,
                            RemoveCommand = new RelayCommand(RemoveBookCommandExecute)
                        });
                    }
                    tempUser.Books = books;
                    tempUsers.Add(tempUser);
                }
                Users = tempUsers;
            }
            else
            {
                var tempUsers = new List<UserTableModel>();
                foreach (var item in users)
                {
                    if (item.Id == CurrentUser.Id)
                        continue;

                    var tempUser = new UserTableModel()
                    {
                        Id = item.Id,
                        Login = item.Login,
                        Password = item.Password,
                        UserInfo = item.UserInfo,
                        AddAdmin = item.IsAdmin ? Visibility.Collapsed : Visibility.Visible,
                        AdminMenu = item.Books.Count > 0 && !item.IsAdmin ? Visibility.Visible : Visibility.Collapsed,
                        EditCommand = new RelayCommand(AddAdminCommandExecute),
                        RemoveCommand = new RelayCommand(RemoveUserExecute)
                    };
                    var books = new List<BookTableModel>();
                    foreach (var book in item.Books)
                    {
                        books.Add(new BookTableModel()
                        {
                            Id = book.Id,
                            BookName = book.BookName,
                            Style = book.Style,
                            AutorBook = book.AutorBook,
                            CountPage = book.CountPage,
                            PriceForSale = book.ActionBook == null ? book.Price : book.Price - (book.Price * book.ActionBook.Discount / 100),
                            PublisherBook = book.PublisherBook,
                            ActionBook = book.ActionBook,
                            Discount = book.ActionBook != null ? Visibility.Visible : Visibility.Hidden,
                            IsDiscount = book.ActionBook != null ? TextDecorations.Strikethrough : null,
                            OldPrice = book.Price,
                            Year = book.Year,
                            Count = book.Count,
                            RemoveCommand = new RelayCommand(RemoveBookCommandExecute)
                        });
                    }
                    tempUser.Books = books;
                    tempUsers.Add(tempUser);
                }
                Users = tempUsers;
            }
        }
        private List<BookTableModel> ConvertIEnumerableToObservableCollection(IEnumerable<BookDTO> bookDTOs)
        {
            var temp = new List<BookTableModel>();
            if (bookDTOs != null)
            {
                foreach (var item in bookDTOs)
                {
                    var book = new BookTableModel()
                    {
                        Id = item.Id,
                        BookName = item.BookName,
                        Style = item.Style,
                        AutorBook = item.AutorBook,
                        CountPage = item.CountPage,
                        PriceForSale = item.ActionBook == null ? item.Price : item.Price - (item.Price * item.ActionBook.Discount / 100),
                        PublisherBook = item.PublisherBook,
                        ActionBook = item.ActionBook,
                        Discount = item.ActionBook != null ? Visibility.Visible : Visibility.Hidden,
                        IsDiscount = item.ActionBook != null ? TextDecorations.Strikethrough : null,
                        OldPrice = item.Price,
                        Year = item.Year,
                        Count = item.Count,
                        RemoveCommand = new RelayCommand(RemoveCommandExecute)
                    };
                    temp.Add(book);
                }
            }
            return temp;
        }
        private void RemoveCommandExecute(object obj)
        {
            CurrentUser = _userService.Find(CurrentUser.Id);
            var book = CurrentUser.Books.First(b => b.Id == Selected.Id);
            if (Selected.Count - Convert.ToInt32(obj) == 0)
            {
                CurrentUser.Books.Remove(book);
                _userService.Update(CurrentUser);
                _bookService.Delete(book.Id);
                var newBook = _bookService.FindAll(book.BookName)
                                          .Where(x => x.AutorBook.Name == book.AutorBook.Name &&
                                                      x.AutorBook.LastName == book.AutorBook.LastName &&
                                                      x.AutorBook.Patronymic == book.AutorBook.Patronymic &&
                                                      x.Price == book.Price &&
                                                      x.Style == book.Style &&
                                                      x.CountPage == book.CountPage)
                                          .First();
                if (newBook != null)
                {
                    newBook.Count += Convert.ToInt32(obj.ToString());
                    _bookService.Update(newBook);
                }
                else
                {
                    _bookService.Create(new BookDTO()
                    {
                        BookName = book.BookName,
                        ActionBook = book.ActionBook,
                        Count = Convert.ToInt32(obj),
                        AutorBook = book.AutorBook,
                        CountPage = book.CountPage,
                        Price = book.Price,
                        PublisherBook = book.PublisherBook,
                        Style = book.Style,
                        Year = book.Year
                    });
                }
            }
            else
            {
                book.Count -= Convert.ToInt32(obj.ToString());
                _bookService.Update(book);
                var newBook = _bookService.FindAll(book.BookName)
                                          .Where(x => x.AutorBook.Name == book.AutorBook.Name &&
                                                      x.AutorBook.LastName == book.AutorBook.LastName &&
                                                      x.AutorBook.Patronymic == book.AutorBook.Patronymic &&
                                                      x.Price == book.Price &&
                                                      x.Style == book.Style &&
                                                      x.CountPage == book.CountPage)
                                          .First();
                if (newBook != null)
                {
                    newBook.Count += Convert.ToInt32(obj.ToString());
                    _bookService.Update(newBook);
                }
                else
                {
                    _bookService.Create(new BookDTO()
                    {
                        BookName = book.BookName,
                        ActionBook = book.ActionBook,
                        Count = Convert.ToInt32(obj.ToString()),
                        AutorBook = book.AutorBook,
                        CountPage = book.CountPage,
                        Price = book.Price,
                        PublisherBook = book.PublisherBook,
                        Style = book.Style,
                        Year = book.Year
                    });
                }
            }
            CurrentUser = _userService.Find(CurrentUser.Id);
            UpdateTable();
        }
        private void BuyExecute(object obj)
        {
            CurrentUser = _userService.Find(CurrentUser.Id);

            for (int i = CurrentUser.Books.Count - 1; i >= 0; i--)
                _bookService.Delete(CurrentUser.Books[i].Id);

            CurrentUser.Books.Clear();
            UpdateTable();
        }
        private void LogOutExecute(object obj)
        {
            var loginView = new LoginView();
            loginView.Show();

            foreach (var item in App.Current.Windows)
                if (item is MainView)
                    (item as MainView).Close();

            loginView.IsVisibleChanged += (s, ev) =>
            {
                if (loginView.IsVisible == false && loginView.IsLoaded && Application.Current.Windows.Count == 2)
                {
                    var mainView = new MainView();
                    mainView.Show();
                    loginView.Close();
                }
            };

            foreach (var item in App.Current.Windows)
                if (item is MainView)
                    (item as MainView).Close();
        }
        private void EditInformationExecute(object obj)
        {
            var people = _peopleService.Find(CurrentUser.UserInfo.Id);
            people.Name = Name;
            people.LastName = LastName;
            people.Email = Email;
            _peopleService.Update(people);
            CurrentUser = _userService.Find(CurrentUser.Id);
            CurrentUser.UserInfo = people;
        }
        private bool EditInformationCanExecute(object obj)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (!regex.IsMatch(Email))
                return false;
            if (String.IsNullOrWhiteSpace(Name))
                return false;
            if (Name.Length > 40)
                return false;
            if (String.IsNullOrWhiteSpace(LastName))
                return false;
            if (LastName.Length > 40)
                return false;
            if (!_isValueEditChanged)
                return false;

            return true;
        }
        private void ChangePasswordExecute(object obj)
        {
            string password = new System.Net.NetworkCredential(string.Empty, Password).Password;
            CurrentUser.Password = password;
            _userService.Update(CurrentUser);
            CurrentUser = _userService.Find(CurrentUser.Id);
        }
        private bool ChangePasswordCanExecute(object obj)
        {
            string oldPassword = new System.Net.NetworkCredential(string.Empty, OldPassword).Password;
            string password = new System.Net.NetworkCredential(string.Empty, Password).Password;
            string confirmPassword = new System.Net.NetworkCredential(string.Empty, ConfirmPassword).Password;

            if (oldPassword != CurrentUser.Password)
                return false;
            if (password != confirmPassword)
                return false;
            if (password.Length < 3 || password.Length > 20)
                return false;
            if (password == oldPassword)
                return false;

            return true;
        }
        private void RemoveUserExecute(object obj)
        {
            var rez = MessageBox.Show("Delete selected user?", "Delete", MessageBoxButton.YesNo);
            if (rez == MessageBoxResult.Yes)
            {
                var user = _userService.Find(SelectedUser.Id);
                foreach (var item in user.Books)
                {
                    _bookService.Delete(item.Id);
                    var newBook = _bookService.FindAll(item.BookName)
                                              .Where(x => x.AutorBook.Name == item.AutorBook.Name &&
                                                          x.AutorBook.LastName == item.AutorBook.LastName &&
                                                          x.AutorBook.Patronymic == item.AutorBook.Patronymic &&
                                                          x.Price == item.Price &&
                                                          x.Style == item.Style &&
                                                          x.CountPage == item.CountPage)
                                              .First();
                    if (newBook != null)
                    {
                        newBook.Count += item.Count;
                        _bookService.Update(newBook);
                    }
                    else
                    {
                        _bookService.Create(new BookDTO()
                        {
                            BookName = item.BookName,
                            ActionBook = item.ActionBook,
                            Count = item.Count,
                            AutorBook = item.AutorBook,
                            CountPage = item.CountPage,
                            Price = item.Price,
                            PublisherBook = item.PublisherBook,
                            Style = item.Style,
                            Year = item.Year
                        });
                    }
                }
                user.Books.Clear();
                _userService.Delete(user.Id);
            }
            UpdateUsers();
        }
        private void RemoveBookCommandExecute(object obj)
        {
            var user = _userService.Find(SelectedUser.Id);
            var book = _bookService.Find(SelectedUser.Books.First(b => b.Id == SelectedUser.Selected.Id).Id);
            if (SelectedUser.Selected.Count - Convert.ToInt32(obj) == 0)
            {
                user.Books.Remove(user.Books.First(x => x.Id == book.Id));
                _userService.Update(user);
                _bookService.Delete(book.Id);
                var newBook = _bookService.FindAll(book.BookName)
                                          .Where(x => x.AutorBook.Name == book.AutorBook.Name &&
                                                      x.AutorBook.LastName == book.AutorBook.LastName &&
                                                      x.AutorBook.Patronymic == book.AutorBook.Patronymic &&
                                                      x.Price == book.Price &&
                                                      x.Style == book.Style &&
                                                      x.CountPage == book.CountPage)
                                          .First();
                if (newBook != null)
                {
                    newBook.Count += Convert.ToInt32(obj.ToString());
                    _bookService.Update(newBook);
                }
                else
                {
                    _bookService.Create(new BookDTO()
                    {
                        BookName = book.BookName,
                        ActionBook = book.ActionBook,
                        Count = Convert.ToInt32(obj),
                        AutorBook = book.AutorBook,
                        CountPage = book.CountPage,
                        Price = book.Price,
                        PublisherBook = book.PublisherBook,
                        Style = book.Style,
                        Year = book.Year
                    });
                }
            }
            else
            {
                book.Count -= Convert.ToInt32(obj.ToString());
                _bookService.Update(book);
                var newBook = _bookService.FindAll(book.BookName)
                                          .Where(x => x.AutorBook.Name == book.AutorBook.Name &&
                                                      x.AutorBook.LastName == book.AutorBook.LastName &&
                                                      x.AutorBook.Patronymic == book.AutorBook.Patronymic &&
                                                      x.Price == book.Price &&
                                                      x.Style == book.Style &&
                                                      x.CountPage == book.CountPage)
                                          .First();
                if (newBook != null)
                {
                    newBook.Count += Convert.ToInt32(obj.ToString());
                    _bookService.Update(newBook);
                }
                else
                {
                    _bookService.Create(new BookDTO()
                    {
                        BookName = book.BookName,
                        ActionBook = book.ActionBook,
                        Count = Convert.ToInt32(obj.ToString()),
                        AutorBook = book.AutorBook,
                        CountPage = book.CountPage,
                        Price = book.Price,
                        PublisherBook = book.PublisherBook,
                        Style = book.Style,
                        Year = book.Year
                    });
                }
            }
            UpdateUsers();
        }
        private void AddAdminCommandExecute(object obj)
        {
            var rez = MessageBox.Show("Add selected user to admins?\nAfter adding you won't have permision to delete this user and remove from admins!", "Add to admins", MessageBoxButton.YesNo);
            if (rez == MessageBoxResult.Yes)
            {
                var user = _userService.Find(SelectedUser.Id);
                user.IsAdmin = true;
                _userService.Update(user);
            }
            UpdateUsers();
        }
        private void DeleteAccountExecute(object obj)
        {
            var rez = MessageBox.Show("Delete account?\nAfter deleting you won't recover information!", "Delete account", MessageBoxButton.YesNo);
            if (rez == MessageBoxResult.Yes)
            {
                var user = _userService.Find(CurrentUser.Id);
                foreach (var item in user.Books)
                {
                    _bookService.Delete(item.Id);
                    var newBook = _bookService.FindAll(item.BookName)
                                              .Where(x => x.AutorBook.Name == item.AutorBook.Name &&
                                                          x.AutorBook.LastName == item.AutorBook.LastName &&
                                                          x.AutorBook.Patronymic == item.AutorBook.Patronymic &&
                                                          x.Price == item.Price &&
                                                          x.Style == item.Style &&
                                                          x.CountPage == item.CountPage)
                                              .First();
                    if (newBook != null)
                    {
                        newBook.Count += item.Count;
                        _bookService.Update(newBook);
                    }
                    else
                    {
                        _bookService.Create(new BookDTO()
                        {
                            BookName = item.BookName,
                            ActionBook = item.ActionBook,
                            Count = item.Count,
                            AutorBook = item.AutorBook,
                            CountPage = item.CountPage,
                            Price = item.Price,
                            PublisherBook = item.PublisherBook,
                            Style = item.Style,
                            Year = item.Year
                        });
                    }
                }
                user.Books.Clear();
                _userService.Delete(user.Id);
                LogOutExecute(null);
            }
        }
        #endregion
    }
}
