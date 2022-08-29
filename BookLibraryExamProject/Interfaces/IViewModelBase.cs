using BLL.Models;
using BLL.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BookLibraryExamProject.Interfaces
{
    public abstract class IViewModelBase : INotifyPropertyChanged
    {
        #region Fields
        protected UserService _userService;
        protected string _connectionString;
        private UserDTO _currentUser;
        #endregion
        #region Propeties
        public UserDTO CurrentUser
        {
            get
            {
                return _currentUser;
            }

            set
            {
                _currentUser = value;
                OnPropertyChanged();
            }
        }
        public Visibility IsAdmin { get; }
        #endregion
        #region Constructor
        public IViewModelBase()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            _userService = new UserService(_connectionString);
            CurrentUser = new UserDTO();
            LoadCurrentUserData();
            IsAdmin = CurrentUser.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion
        #region Methods
        public void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private void LoadCurrentUserData()
        {
            var user = _userService.Find(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                CurrentUser.Login = user.Login;
                CurrentUser.Password = user.Password;
                CurrentUser.UserInfo = user.UserInfo;
                CurrentUser.IsAdmin = user.IsAdmin;
                CurrentUser.Id = user.Id;
                CurrentUser.Books = user.Books;
            }
            else
            {
                CurrentUser.Login = "Invalid user, not logged in";
                CurrentUser.UserInfo = null;
                CurrentUser.IsAdmin = false;
            }
        }
        #endregion
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
