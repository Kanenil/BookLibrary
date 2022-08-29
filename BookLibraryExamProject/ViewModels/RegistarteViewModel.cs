using BLL.Interfaces;
using BLL.Models;
using BLL.Services;
using BookLibraryExamProject.CustomCommands;
using BookLibraryExamProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookLibraryExamProject.ViewModels
{
    public class RegistarteViewModel : IViewModelBase
    {
        #region Field
        private string _username;
        private SecureString _password;
        private SecureString _commitPassword;
        private string _name;
        private string _lastName;
        private string _email;
        private string _errorMessage;
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
        public SecureString CommitPassword
        {
            get
            {
                return _commitPassword;
            }

            set
            {
                _commitPassword = value;
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
        public string Email
        {
            get
            {
                return _email;
            }

            set
            {
                _email = value;
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

        #endregion
        #region Commands
        public ICommand RegistrateCommand { get; }
        #endregion
        #region Constructor
        public RegistarteViewModel()
        {
            RegistrateCommand = new RelayCommand(ExecuteRegistrateCommand, CanExecuteRegistrateCommand);
        }
        #endregion
        #region Methods
        private bool CanExecuteRegistrateCommand(object obj)
        {
            if (String.IsNullOrWhiteSpace(Username) ||
                Password == null ||
                CommitPassword == null ||
                String.IsNullOrWhiteSpace(Name) ||
                String.IsNullOrWhiteSpace(LastName) ||
                String.IsNullOrWhiteSpace(Email))
            {
                ErrorMessage = "* You must fill all fields";
                return false;
            }
            if (_userService.Find(Username) != null)
            {
                ErrorMessage = "* This login is already registered";
                return false;
            }
            if (Username.Length < 3 || Username.Length > 20)
            {
                ErrorMessage = "* Login must be at least 3 symbols and not more than 20 symbols";
                return false;
            }

            string password = new System.Net.NetworkCredential(string.Empty, Password).Password;
            string commitPassword = new System.Net.NetworkCredential(string.Empty, CommitPassword).Password;

            if (password != commitPassword)
            {
                ErrorMessage = "* Password and Commit Password aren't match";
                return false;
            }

            if (password.Length < 3 || password.Length > 20)
            {
                ErrorMessage = "* Password must be more than 3 symbols and not more than 20 symbols";
                return false;
            }

            if (Name.Length > 40)
            {
                ErrorMessage = "* First Name cann't be more than 40 symbols";
                return false;
            }

            if (LastName.Length > 40)
            {
                ErrorMessage = "* Last Name cann't be more than 40 symbols";
                return false;
            }

            if (!Regex.IsMatch(Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                ErrorMessage = "* Email must looks like [some_thing]@Email_domain]";
                return false;
            }

            ErrorMessage = "";
            return true;
        }
        private void ExecuteRegistrateCommand(object obj)
        {
            var credential = new NetworkCredential(Username, Password);
            _userService.Create(new UserDTO
            {
                Login = credential.UserName,
                Password = credential.Password,
                UserInfo = new PeopleDTO()
                {
                    Name = Name,
                    LastName = LastName,
                    Email = Email
                }
            });
            (obj as Window).Close();
        }
        #endregion
    }
}
