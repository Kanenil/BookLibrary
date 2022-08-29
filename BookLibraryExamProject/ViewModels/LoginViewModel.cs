using BLL.Interfaces;
using BLL.Services;
using BookLibraryExamProject.CustomCommands;
using BookLibraryExamProject.Interfaces;
using BookLibraryExamProject.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookLibraryExamProject.ViewModels
{
    public class LoginViewModel : IViewModelBase
    {
        #region Field
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible;
        #endregion
        #region Properties
        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }
        public SecureString Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }
        public bool IsViewVisible
        {
            get
            {
                return _isViewVisible;
            }

            set
            {
                _isViewVisible = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Commands
        public ICommand LoginCommand { get; }
        public ICommand RegistrateCommand { get; }
        #endregion
        #region Constructor
        public LoginViewModel()
        {
            _isViewVisible = true;
            LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RegistrateCommand = new RelayCommand(ExecuteRegistrateCommand);
        }
        #endregion
        #region Methods
        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3)
                validData = false;
            else
                validData = true;
            return validData;
        }
        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = _userService.AuthenticateUser(new NetworkCredential(Username, Password));
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(Username), null);
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "* Invalid username or password";
            }
        }
        private void ExecuteRegistrateCommand(object obj)
        {
            var window = new RegistrateView();
            App.Current.MainWindow.Visibility = Visibility.Hidden;
            window.Top = App.Current.MainWindow.Top;
            window.Left = App.Current.MainWindow.Left;
            window.ShowDialog();
            App.Current.MainWindow.Top = window.Top;
            App.Current.MainWindow.Left = window.Left;
            App.Current.MainWindow.Visibility = Visibility.Visible;
        }
        #endregion
    }
}
