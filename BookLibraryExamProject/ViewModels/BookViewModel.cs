using BLL.Models;
using BLL.Services;
using BookLibraryExamProject.CustomCommands;
using BookLibraryExamProject.Interfaces;
using BookLibraryExamProject.Models;
using BookLibraryExamProject.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookLibraryExamProject.ViewModels
{
    public class BookViewModel : IViewModelBase
    {
        #region Field
        private BookService _bookService;
        private PublisherService _publisherService;
        private ActionService _actionService;
        private List<BookTableModel> _outStock;
        private AuthorService _authorService;
        private List<AuthorDTO> _authors;
        private List<BookTableModel> _books;
        private List<PublisherDTO> _publishers;
        private List<ActionDTO> _actions;
        private BookTableModel _selected;
        private string _message;
        private string _find;
        private string _bookName;
        private decimal _price;
        private int _count;
        private string _style;
        private ActionDTO _action;
        private PublisherDTO _publisherBook;
        private AuthorDTO _authorBook;
        private int _year;
        private int _countPage;
        #endregion
        #region Properties
        public List<BookTableModel> OutStock
        {
            get
            {
                return _outStock;
            }

            set
            {
                _outStock = value;
                OnPropertyChanged();
            }
        }
        public List<BookTableModel> GetBooks
        {
            get
            {
                return _books;
            }

            set
            {
                _books = value;
                OnPropertyChanged();
            }
        }
        public List<AuthorDTO> GetAuthors
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
        public List<PublisherDTO> GetPublishers
        {
            get
            {
                return _publishers;
            }

            set
            {
                _publishers = value;
                OnPropertyChanged();
            }
        }
        public List<ActionDTO> GetActions
        {
            get
            {
                return _actions;
            }

            set
            {
                _actions = value;
                OnPropertyChanged();
            }
        }
        public BookTableModel Selected
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
                    UpdateBooks(_bookService.GetAll());
                    UpdateOutStock(_bookService.GetAll());
                }
                else
                {
                    UpdateBooks(_bookService.FindAll(Find));
                    UpdateOutStock(_bookService.FindAll(Find));
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
        public string BookName
        {
            get
            {
                return _bookName;
            }

            set
            {
                _bookName = value;
                OnPropertyChanged();
            }
        }
        public string Style
        {
            get
            {
                return _style;
            }

            set
            {
                _style = value;
                OnPropertyChanged();
            }
        }
        public int CountPage
        {
            get
            {
                return _countPage;
            }

            set
            {
                _countPage = value;
                OnPropertyChanged();
            }
        }
        public int Count
        {
            get
            {
                return _count;
            }

            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }
        public int Year
        {
            get
            {
                return _year;
            }

            set
            {
                _year = value;
                OnPropertyChanged();
            }
        }
        public decimal Price
        {
            get
            {
                return _price;
            }

            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }
        public AuthorDTO AuthorBook
        {
            get
            {
                return _authorBook;
            }

            set
            {
                _authorBook = value;
                OnPropertyChanged();
            }
        }
        public PublisherDTO PublisherBook
        {
            get
            {
                return _publisherBook;
            }

            set
            {
                _publisherBook = value;
                OnPropertyChanged();
            }
        }
        public ActionDTO Action
        {
            get
            {
                return _action;
            }

            set
            {
                _action = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Commands
        public ICommand AddCommand { get; }
        public ICommand RemoveAction { get; }
        public ICommand AddAction { get; }
        public ICommand AddActionsCommand { get; }
        #endregion
        #region Constructor
        public BookViewModel()
        {
            InitializeComponent();
            AddCommand = new RelayCommand(AddCommandExecute);
            RemoveAction = new RelayCommand(x => Action = null, x => Action != null);
            AddAction = new RelayCommand(AddActionWindowExecute, x => GetActions.Count > 0);
            AddActionsCommand = new RelayCommand(AddActionExecute, AddActionCanExecute);
        }
        #endregion
        #region Methods
        private void InitializeComponent()
        {
            _bookService = new BookService(_connectionString);
            _userService = new UserService(_connectionString);
            _publisherService = new PublisherService(_connectionString);
            _actionService = new ActionService(_connectionString);
            _authorService = new AuthorService(_connectionString);
            UpdateBooks(_bookService.GetAll());
            GetAuthors = _authorService.GetAll().ToList();
            GetPublishers = _publisherService.GetAll().ToList();
            GetActions = _actionService.GetAll().ToList();
            UpdateOutStock();
        }
        private List<BookTableModel> ConvertIEnumerableToObservableCollection(IEnumerable<BookDTO> bookDTOs)
        {
            var temp = new List<BookTableModel>();
            if (bookDTOs != null)
            {
                foreach (var item in bookDTOs)
                    temp.Add(new BookTableModel()
                    {
                        Id = item.Id,
                        BookName = item.BookName,
                        Style = item.Style,
                        AutorBook = item.AutorBook,
                        CountPage = item.CountPage,
                        PriceForSale = item.ActionBook == null ? item.Price : item.Price - (item.Price * item.ActionBook.Discount / 100),
                        PublisherBook = item.PublisherBook,
                        ActionBook = item.ActionBook,
                        Discount = item.ActionBook != null ? Visibility.Visible : Visibility.Collapsed,
                        IsDiscount = item.ActionBook != null ? TextDecorations.Strikethrough : null,
                        OldPrice = item.Price,
                        Year = item.Year,
                        Count = item.Count,
                        AdminMenu = CurrentUser.IsAdmin == true ? Visibility.Visible : Visibility.Collapsed,
                        BusketCommand = new RelayCommand(BusketCommandExecute),
                        EditCommand = new RelayCommand(EditCommandExecute),
                        RemoveCommand = new RelayCommand(RemoveCommandExecute)
                    });
            }
            return temp;
        }
        public void UpdateBooks(IEnumerable<BookDTO> bookDTOs = null)
        {
            GetActions = _actionService.GetAll().ToList();
            if (bookDTOs == null)
            {
                GetBooks = ConvertIEnumerableToObservableCollection(_bookService.GetAll());
            }
            else
            {
                GetBooks = ConvertIEnumerableToObservableCollection(bookDTOs);
            }
            foreach (var item in _userService.GetAll())
            {
                foreach (var item1 in item.Books)
                {
                    GetBooks.Remove(GetBooks.Find((x) => x.Id == item1.Id));
                }
            }
            for (int i = GetBooks.Count - 1; i >= 0; i--)
            {
                if (GetBooks[i].Count == 0)
                    GetBooks.RemoveAt(i);
            }
        }
        public void UpdateOutStock(IEnumerable<BookDTO> bookDTOs = null)
        {
            if (bookDTOs == null)
            {
                var temp = new List<BookTableModel>();
                foreach (var item in _bookService.GetAll())
                    temp.Add(new BookTableModel()
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
                        AdminMenu = CurrentUser.IsAdmin == true ? Visibility.Visible : Visibility.Collapsed,
                        EditCommand = new RelayCommand(AddOutStockExecute),
                        RemoveCommand = new RelayCommand(RemoveOutStockExecute)
                    });
                OutStock = temp;
            }
            else
            {
                var temp = new List<BookTableModel>();
                foreach (var item in bookDTOs)
                    temp.Add(new BookTableModel()
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
                        AdminMenu = CurrentUser.IsAdmin == true ? Visibility.Visible : Visibility.Collapsed,
                        EditCommand = new RelayCommand(AddOutStockExecute),
                        RemoveCommand = new RelayCommand(RemoveOutStockExecute)
                    });
                OutStock = temp;
            }
            for (int i = OutStock.Count - 1; i >= 0; i--)
            {
                if (OutStock[i].Count != 0)
                    OutStock.RemoveAt(i);
            }
        }
        private void AddCommandExecute(object obj)
        {
            var window = new AddEditBookWindow();
            window.btnLogin.Content = "ADD";
            window.windowName.Text = "ADD BOOK";
            window.btnLogin.Command = new RelayCommand((window.DataContext as BookViewModel).AddBookCommandExecute, (window.DataContext as BookViewModel).EditBookCommandCanExecute);
            window.ShowDialog();
            UpdateBooks(_bookService.GetAll());
        }
        private void BusketCommandExecute(object obj)
        {
            try
            {
                CurrentUser = _userService.Find(CurrentUser.Id);
                var temp = CurrentUser.Books.First((x) => x.BookName == Selected.BookName &&
                                                          x.Style == Selected.Style &&
                                                          x.CountPage == Selected.CountPage &&
                                                          x.Price == Selected.OldPrice);

                temp.Count += Convert.ToInt32(obj);
                _bookService.Update(temp);
                var tempBook = _bookService.Find(Selected.Id);
                tempBook.Count -= Convert.ToInt32(obj);
                _bookService.Update(tempBook);
            }
            catch
            {
                var book = new BookDTO()
                {
                    BookName = Selected.BookName,
                    Year = Selected.Year,
                    Count = Convert.ToInt32(obj),
                    Style = Selected.Style,
                    CountPage = Selected.CountPage,
                    Price = Selected.OldPrice
                };
                book.AutorBook = Selected.AutorBook;
                if (Selected.ActionBook != null)
                {
                    book.ActionBook = _actionService.FindAll(Selected.ActionBook.Discount)
                          .Where(x => x.Start.Day == Selected.ActionBook.Start.Day &&
                                      x.Start.Month == Selected.ActionBook.Start.Month &&
                                      x.End.Day == Selected.ActionBook.End.Day &&
                                      x.End.Month == Selected.ActionBook.End.Month)
                          .First();
                }
                book.PublisherBook = _publisherService.FindAll(Selected.PublisherBook.Name)
                                           .First();

                var user = _userService.Find(CurrentUser.Id);
                user.Books.Add(book);
                _userService.Update(user);
                CurrentUser = user;

                var bookFind = _bookService.Find(Selected.Id);
                bookFind.Count -= Convert.ToInt32(obj);
                _bookService.Update(bookFind);
            }
            UpdateBooks(_bookService.GetAll());
        }
        private void AddOutStockExecute(object obj)
        {
            var book = _bookService.Find(Selected.Id);
            book.Count += Convert.ToInt32(obj);
            _bookService.Update(book);
            UpdateOutStock();
        }
        private void RemoveOutStockExecute(object obj)
        {
            _bookService.Delete(Selected.Id);
            UpdateOutStock();
        }
        private void EditCommandExecute(object obj)
        {
            var window = new AddEditBookWindow();
            window.btnLogin.Content = "COMMIT";
            window.windowName.Text = "EDIT BOOK";
            window.btnLogin.Command = new RelayCommand((window.DataContext as BookViewModel).EditBookCommandExecute, (window.DataContext as BookViewModel).EditBookCommandCanExecute);
            (window.DataContext as BookViewModel).AuthorBook = Selected.AutorBook;
            (window.DataContext as BookViewModel).BookName = Selected.BookName;
            (window.DataContext as BookViewModel).Style = Selected.Style;
            (window.DataContext as BookViewModel).CountPage = Selected.CountPage;
            (window.DataContext as BookViewModel).Price = Selected.OldPrice;
            (window.DataContext as BookViewModel).Count = Selected.Count;
            (window.DataContext as BookViewModel).Action = Selected.ActionBook;
            (window.DataContext as BookViewModel).PublisherBook = Selected.PublisherBook;
            (window.DataContext as BookViewModel).Year = Selected.Year;
            (window.DataContext as BookViewModel).Selected = Selected;
            window.ShowDialog();
            UpdateBooks(_bookService.GetAll());
        }
        private void AddBookCommandExecute(object obj)
        {
            _bookService.Create(new BookDTO()
            {
                BookName = BookName,
                Style = Style,
                AutorBook = AuthorBook,
                CountPage = CountPage,
                Price = Price,
                Count = Count,
                Year = Year,
                ActionBook = Action,
                PublisherBook = PublisherBook
            });
            Message = "Succesfuly added";
        }
        private void EditBookCommandExecute(object obj)
        {
            var oldBook = _bookService.Find(Selected.Id);
            foreach (var item in _bookService.FindAll(oldBook.BookName)
                                                 .Where(x => x.AutorBook.Name == oldBook.AutorBook.Name &&
                                                             x.AutorBook.LastName == oldBook.AutorBook.LastName &&
                                                             x.AutorBook.Patronymic == oldBook.AutorBook.Patronymic &&
                                                             x.CountPage == oldBook.CountPage &&
                                                             x.Year == oldBook.Year &&
                                                             x.Style == oldBook.Style))
            {
                bool isCount = true;
                foreach (var item2 in _userService.GetAll().Where(b => b.Books.Count != 0))
                {
                    foreach (var item3 in item2.Books)
                    {
                        if (item3.Id == item.Id)
                        {
                            isCount = false;
                            break;
                        }
                    }
                }
                if (isCount)
                {
                    item.Count = Count;
                }
                item.BookName = BookName;
                item.Style = Style;
                item.AutorBook = AuthorBook;
                item.CountPage = CountPage;
                item.Price = Price;
                item.Year = Year;
                item.PublisherBook = PublisherBook;
                item.ActionBook = Action;
                _bookService.Update(item);
            }

            Message = "Succesfuly edited";
        }
        private bool EditBookCommandCanExecute(object obj)
        {
            if (String.IsNullOrWhiteSpace(BookName))
                return false;
            if (BookName.Length > 100)
                return false;
            if (String.IsNullOrWhiteSpace(Style))
                return false;
            if (Style.Length > 60)
                return false;
            if (AuthorBook == null)
                return false;
            if (CountPage < 0)
                return false;
            if (Count < 0)
                return false;
            if (Price < 0)
                return false;
            if (Year < 0)
                return false;
            if (PublisherBook == null)
                return false;

            return true;
        }
        private void RemoveCommandExecute(object obj)
        {
            var book = _bookService.Find(Selected.Id);
            book.Count -= Convert.ToInt32(obj);
            _bookService.Update(book);
            UpdateBooks();
        }
        private void AddActionWindowExecute(object obj)
        {
            var window = new AddActionToBooksWindow();
            for (int i = (window.DataContext as BookViewModel).GetBooks.Count - 1; i >= 0; i--)
                if ((window.DataContext as BookViewModel).GetBooks[i].ActionBook != null)
                    (window.DataContext as BookViewModel).GetBooks.RemoveAt(i);
            window.ShowDialog();
            UpdateBooks(_bookService.GetAll());
        }
        private void AddActionExecute(object obj)
        {
            DataGrid dataGrid = null;
            foreach (var item in App.Current.Windows)
                if (item is AddActionToBooksWindow)
                    dataGrid = (item as AddActionToBooksWindow).dataGrid;

            var selected = dataGrid.SelectedItems;
            foreach (BookTableModel item in selected)
            {
                var oldBook = _bookService.Find(item.Id);
                foreach (var item1 in _bookService.FindAll(oldBook.BookName)
                                                     .Where(x => x.AutorBook.Name == oldBook.AutorBook.Name &&
                                                                 x.AutorBook.LastName == oldBook.AutorBook.LastName &&
                                                                 x.AutorBook.Patronymic == oldBook.AutorBook.Patronymic &&
                                                                 x.CountPage == oldBook.CountPage &&
                                                                 x.Year == oldBook.Year &&
                                                                 x.Style == oldBook.Style))
                {
                    item1.ActionBook = Action;
                    _bookService.Update(item1);
                }
            }
            Message = "Succesfuly added actions";

            UpdateBooks(_bookService.GetAll());
            for (int i = GetBooks.Count - 1; i >= 0; i--)
                if (GetBooks[i].ActionBook != null)
                    GetBooks.RemoveAt(i);
            dataGrid.SelectedItems.Clear();
            Action = null;
        }
        private bool AddActionCanExecute(object obj)
        {
            if (Action == null)
                return false;
            foreach (var item in App.Current.Windows)
                if (item is AddActionToBooksWindow)
                    if ((item as AddActionToBooksWindow).dataGrid.SelectedItems.Count == 0)
                        return false;

            Message = "";
            return true;
        }
        #endregion
    }
}
