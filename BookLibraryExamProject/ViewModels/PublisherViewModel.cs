using BLL.Models;
using BLL.Services;
using BookLibraryExamProject.CustomCommands;
using BookLibraryExamProject.Interfaces;
using BookLibraryExamProject.Models;
using BookLibraryExamProject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookLibraryExamProject.ViewModels
{
    public class PublisherViewModel : IViewModelBase
    {
        #region Field
        private PublisherService _publisherService;
        private string _name;
        private string _message;
        private string _find;
        private List<PublisherTableModel> _publishers;
        private PublisherTableModel _selected;
        #endregion
        #region Properties
        public List<PublisherTableModel> GetPublishers
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
        public PublisherTableModel Selected
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
                if (String.IsNullOrEmpty(_find))
                    UpdatePublishers();
                else
                    UpdatePublishers(_publisherService.FindAll(_find));
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
        #endregion
        #region Commands
        public ICommand AddCommand { get; }
        #endregion
        #region Constructor
        public PublisherViewModel()
        {
            _publisherService = new PublisherService(_connectionString);
            AddCommand = new RelayCommand(AddCommandExecute);
            UpdatePublishers();
        }
        #endregion
        #region Methods
        public void UpdatePublishers(IEnumerable<PublisherDTO> publishers = null)
        {
            if (publishers == null)
                GetPublishers = ConvertIEnumerableToObservableCollection(_publisherService.GetAll());
            else
                GetPublishers = ConvertIEnumerableToObservableCollection(publishers);
        }
        private List<PublisherTableModel> ConvertIEnumerableToObservableCollection(IEnumerable<PublisherDTO> publishers)
        {
            var temp = new List<PublisherTableModel>();
            if (publishers != null)
            {
                foreach (var item in publishers)
                    temp.Add(new PublisherTableModel()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        AdminMenu = CurrentUser.IsAdmin == true ? Visibility.Visible : Visibility.Collapsed,
                        EditCommand = new RelayCommand(EditCommandExecute),
                        RemoveCommand = new RelayCommand(RemoveCommandExecute)
                    }); ;
            }
            return temp;
        }
        private void AddCommandExecute(object obj)
        {
            var window = new AddEditPublisherWindow();
            window.btnLogin.Content = "ADD";
            window.windowName.Text = "ADD PUBLISHER";
            window.btnLogin.Command = new RelayCommand((window.DataContext as PublisherViewModel).AddPublisherCommandExecute, (window.DataContext as PublisherViewModel).EditPublisherCommandCanExecute);
            window.ShowDialog();
            UpdatePublishers();
        }
        private void EditCommandExecute(object obj)
        {
            var window = new AddEditPublisherWindow();
            window.btnLogin.Content = "COMMIT";
            window.windowName.Text = "EDIT PUBLISHER";
            window.btnLogin.Command = new RelayCommand((window.DataContext as PublisherViewModel).EditPublisherCommandExecute, (window.DataContext as PublisherViewModel).EditPublisherCommandCanExecute);
            (window.DataContext as PublisherViewModel).Name = Selected.Name;
            (window.DataContext as PublisherViewModel).Selected = Selected;
            window.ShowDialog();
            UpdatePublishers();
        }
        private void RemoveCommandExecute(object obj)
        {
            _publisherService.Delete(Selected.Id);
            UpdatePublishers();
        }
        private void AddPublisherCommandExecute(object obj)
        {
            _publisherService.Create(new PublisherDTO()
            {
                Name = Name
            });
            Message = "Succesfuly added";
        }
        private void EditPublisherCommandExecute(object obj)
        {
            _publisherService.Update(new PublisherDTO()
            {
                Id = Selected.Id,
                Name = Name
            });
            Message = "Succesfuly edited";
        }
        private bool EditPublisherCommandCanExecute(object obj)
        {
            if (String.IsNullOrWhiteSpace(Name))
                return false;
            if (Name.Length > 50)
                return false;

            return true;
        }
        #endregion
    }
}
