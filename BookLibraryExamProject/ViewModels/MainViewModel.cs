using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using BookLibraryExamProject.CustomCommands;
using BookLibraryExamProject.Interfaces;
using BookLibraryExamProject.Views;
using BookLibraryExamProject.Views.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace BookLibraryExamProject.ViewModels
{
    public class MainViewModel : IViewModelBase
    {
        #region Field
        private Page _booksPage;
        private Page _authorsPage;
        private Page _publishersPage;
        private Page _actionsPage;
        private Page _basketPage;
        private Page _outstockPage;
        private Page _settingsPage;
        private Page _usersPage;
        private Page _currentPage;
        #endregion
        #region Properties
        public Page CurrentPage
        {
            get
            {
                return _currentPage;
            }

            set
            {
                _currentPage = value;
                OnPropertyChanged();
            }
        }
        public Thickness IconMargin { get => new Thickness(0, 0, 0, 0); }
        #endregion
        #region Commands
        public ICommand BooksPageClickCommand { get; }
        public ICommand BooksMouseEnterCommand { get; }
        public ICommand BooksMouseLeaveCommand { get; }
        public ICommand AuthorsPageClickCommand { get; }
        public ICommand AuthorsMouseEnterCommand { get; }
        public ICommand AuthorsMouseLeaveCommand { get; }
        public ICommand PublishersPageClickCommand { get; }
        public ICommand PublishersMouseEnterCommand { get; }
        public ICommand PublishersMouseLeaveCommand { get; }
        public ICommand ActionsPageClickCommand { get; }
        public ICommand ActionsMouseEnterCommand { get; }
        public ICommand ActionsMouseLeaveCommand { get; }
        public ICommand BasketPageClickCommand { get; }
        public ICommand BasketMouseEnterCommand { get; }
        public ICommand BasketMouseLeaveCommand { get; }
        public ICommand OutStockPageClickCommand { get; }
        public ICommand OutStockMouseEnterCommand { get; }
        public ICommand OutStockMouseLeaveCommand { get; }
        public ICommand SettingsPageClickCommand { get; }
        public ICommand SettingsMouseEnterCommand { get; }
        public ICommand SettingsMouseLeaveCommand { get; }
        public ICommand UsersPageClickCommand { get; }
        public ICommand UsersMouseEnterCommand { get; }
        public ICommand UsersMouseLeaveCommand { get; }
        #endregion
        #region Constructor
        public MainViewModel()
        {
            InitializeComponent();
            BooksMouseEnterCommand = new DelegateCommand<MouseEventArgs>(BooksMouseEnter);
            BooksMouseLeaveCommand = new DelegateCommand<MouseEventArgs>(BooksMouseLeave);
            BooksPageClickCommand = new DelegateCommand<RoutedEventArgs>(BooksClick);
            AuthorsMouseEnterCommand = new DelegateCommand<MouseEventArgs>(AuthorsMouseEnter);
            AuthorsMouseLeaveCommand = new DelegateCommand<MouseEventArgs>(AuthorsMouseLeave);
            AuthorsPageClickCommand = new DelegateCommand<RoutedEventArgs>(AuthorsClick);
            PublishersMouseEnterCommand = new DelegateCommand<MouseEventArgs>(PublishersMouseEnter);
            PublishersMouseLeaveCommand = new DelegateCommand<MouseEventArgs>(PublishersMouseLeave);
            PublishersPageClickCommand = new DelegateCommand<RoutedEventArgs>(PublishersClick);
            ActionsMouseEnterCommand = new DelegateCommand<MouseEventArgs>(ActionsMouseEnter);
            ActionsMouseLeaveCommand = new DelegateCommand<MouseEventArgs>(ActionsMouseLeave);
            ActionsPageClickCommand = new DelegateCommand<RoutedEventArgs>(ActionsClick);
            BasketMouseEnterCommand = new DelegateCommand<MouseEventArgs>(BasketMouseEnter);
            BasketMouseLeaveCommand = new DelegateCommand<MouseEventArgs>(BasketMouseLeave);
            BasketPageClickCommand = new DelegateCommand<RoutedEventArgs>(BasketClick);
            OutStockMouseEnterCommand = new DelegateCommand<MouseEventArgs>(OutStockMouseEnter);
            OutStockMouseLeaveCommand = new DelegateCommand<MouseEventArgs>(OutStockMouseLeave);
            OutStockPageClickCommand = new DelegateCommand<RoutedEventArgs>(OutStockClick);
            SettingsMouseEnterCommand = new DelegateCommand<MouseEventArgs>(SettingsMouseEnter);
            SettingsMouseLeaveCommand = new DelegateCommand<MouseEventArgs>(SettingsMouseLeave);
            SettingsPageClickCommand = new DelegateCommand<RoutedEventArgs>(SettingsClick);
            UsersMouseEnterCommand = new DelegateCommand<MouseEventArgs>(UsersMouseEnter);
            UsersMouseLeaveCommand = new DelegateCommand<MouseEventArgs>(UsersMouseLeave);
            UsersPageClickCommand = new DelegateCommand<RoutedEventArgs>(UsersClick);
        }
        #endregion
        #region Methods
        private void InitializeComponent()
        {
            _booksPage = new BooksPage();
            _authorsPage = new AuthorsPage();
            _publishersPage = new PublishersPage();
            _actionsPage = new ActionsPage();
            _basketPage = new BusketPage();
            _outstockPage = new OutStockPage();
            _settingsPage = new SettingsPage();
            _usersPage = new UsersPage();
        }
        private void BooksMouseEnter(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            if (window.Tg_Btn.IsChecked == false)
            {
                window.Popup.PlacementTarget = window.btnBooks;
                window.Popup.Placement = PlacementMode.Right;
                window.Popup.IsOpen = true;
                window.Header.PopupText.Text = "Books";
            }
        }
        private void BooksMouseLeave(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            window.Popup.Visibility = Visibility.Collapsed;
            window.Popup.IsOpen = false;
        }
        private void BooksClick(RoutedEventArgs e)
        {
            CurrentPage = _booksPage;
            (_booksPage.DataContext as BookViewModel).UpdateBooks();
        }
        private void AuthorsMouseEnter(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            if (window.Tg_Btn.IsChecked == false)
            {
                window.Popup.PlacementTarget = window.btnAuthors;
                window.Popup.Placement = PlacementMode.Right;
                window.Popup.IsOpen = true;
                window.Header.PopupText.Text = "Authors";
            }
        }
        private void AuthorsMouseLeave(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            window.Popup.Visibility = Visibility.Collapsed;
            window.Popup.IsOpen = false;
        }
        private void AuthorsClick(RoutedEventArgs e)
        {
            CurrentPage = _authorsPage;
            (_authorsPage.DataContext as AuthorViewModel).UpdateAuthors();
        }
        private void PublishersMouseEnter(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            if (window.Tg_Btn.IsChecked == false)
            {
                window.Popup.PlacementTarget = window.btnPublishers;
                window.Popup.Placement = PlacementMode.Right;
                window.Popup.IsOpen = true;
                window.Header.PopupText.Text = "Publishers";
            }
        }
        private void PublishersMouseLeave(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            window.Popup.Visibility = Visibility.Collapsed;
            window.Popup.IsOpen = false;
        }
        private void PublishersClick(RoutedEventArgs e)
        {
            CurrentPage = _publishersPage;
            (_publishersPage.DataContext as PublisherViewModel).UpdatePublishers();
        }
        private void ActionsMouseEnter(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            if (window.Tg_Btn.IsChecked == false)
            {
                window.Popup.PlacementTarget = window.btnActions;
                window.Popup.Placement = PlacementMode.Right;
                window.Popup.IsOpen = true;
                window.Header.PopupText.Text = "Actions";
            }
        }
        private void ActionsMouseLeave(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            window.Popup.Visibility = Visibility.Collapsed;
            window.Popup.IsOpen = false;
        }
        private void ActionsClick(RoutedEventArgs e)
        {
            CurrentPage = _actionsPage;
            (_actionsPage.DataContext as ActionViewModel).UpdateActions();
        }
        private void BasketMouseEnter(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            if (window.Tg_Btn.IsChecked == false)
            {
                window.Popup.PlacementTarget = window.btnBasket;
                window.Popup.Placement = PlacementMode.Right;
                window.Popup.IsOpen = true;
                window.Header.PopupText.Text = "Basket";
            }
        }
        private void BasketMouseLeave(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            window.Popup.Visibility = Visibility.Collapsed;
            window.Popup.IsOpen = false;
        }
        private void BasketClick(RoutedEventArgs e)
        {
            CurrentPage = _basketPage;
            (_basketPage.DataContext as UserViewModel).UpdateTable();
        }
        private void OutStockMouseEnter(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            if (window.Tg_Btn.IsChecked == false)
            {
                window.Popup.PlacementTarget = window.btnOutstock;
                window.Popup.Placement = PlacementMode.Right;
                window.Popup.IsOpen = true;
                window.Header.PopupText.Text = "Out Stock";
            }
        }
        private void OutStockMouseLeave(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            window.Popup.Visibility = Visibility.Collapsed;
            window.Popup.IsOpen = false;
        }
        private void OutStockClick(RoutedEventArgs e)
        {
            CurrentPage = _outstockPage;
            (_outstockPage.DataContext as BookViewModel).UpdateOutStock();
        }
        private void SettingsMouseEnter(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            if (window.Tg_Btn.IsChecked == false)
            {
                window.Popup.PlacementTarget = window.btnSettings;
                window.Popup.Placement = PlacementMode.Right;
                window.Popup.IsOpen = true;
                window.Header.PopupText.Text = "Settings";
            }
        }
        private void SettingsMouseLeave(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            window.Popup.Visibility = Visibility.Collapsed;
            window.Popup.IsOpen = false;
        }
        private void SettingsClick(RoutedEventArgs e)
        {
            CurrentPage = _settingsPage;
        }
        private void UsersMouseEnter(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            if (window.Tg_Btn.IsChecked == false)
            {
                window.Popup.PlacementTarget = window.btnUsers;
                window.Popup.Placement = PlacementMode.Right;
                window.Popup.IsOpen = true;
                window.Header.PopupText.Text = "Users";
            }
        }
        private void UsersMouseLeave(MouseEventArgs e)
        {
            var window = (MainView)App.Current.Windows[0];
            window.Popup.Visibility = Visibility.Collapsed;
            window.Popup.IsOpen = false;
        }
        private void UsersClick(RoutedEventArgs e)
        {
            CurrentPage = _usersPage;
            (_usersPage.DataContext as UserViewModel).UpdateUsers();
        }
        #endregion
    }
}
