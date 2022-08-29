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
    public class ActionViewModel : IViewModelBase
    {
        #region Fields
        private ActionService _actionService;
        private List<ActionTableModel> _actions;
        private ActionTableModel _selected;
        private string _message;
        private DateTime _start;
        private int _discount;
        private DateTime _end;
        #endregion
        #region Commands
        public ICommand AddCommand { get; }
        #endregion
        #region Propeties
        public List<ActionTableModel> GetActions
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
        public ActionTableModel Selected
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
        public DateTime Start
        {
            get
            {
                return _start;
            }

            set
            {
                _start = value;
                OnPropertyChanged();
            }
        }
        public DateTime End
        {
            get
            {
                return _end;
            }

            set
            {
                _end = value;
                OnPropertyChanged();
            }
        }
        public int Discount
        {
            get
            {
                return _discount;
            }

            set
            {
                _discount = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #region Constructor
        public ActionViewModel()
        {
            InitializeComponent();
            AddCommand = new RelayCommand(AddCommandExecute);
        }
        #endregion
        #region Methods
        private void InitializeComponent()
        {
            _actionService = new ActionService(_connectionString);
            UpdateActions();

            Start = DateTime.Now;
            End = DateTime.Now;
            Discount = 5;
        }
        public void UpdateActions()
        {
            GetActions = ConvertIEnumerableToObservableCollection(_actionService.GetAll());
        }
        private List<ActionTableModel> ConvertIEnumerableToObservableCollection(IEnumerable<ActionDTO> actions)
        {
            var temp = new List<ActionTableModel>();
            if (actions != null)
            {
                foreach (var item in actions)
                    temp.Add(new ActionTableModel()
                    {
                        Id = item.Id,
                        Start = item.Start,
                        End = item.End,
                        Discount = item.Discount,
                        AdminMenu = CurrentUser.IsAdmin == true ? Visibility.Visible : Visibility.Collapsed,
                        EditCommand = new RelayCommand(EditCommandExecute),
                        RemoveCommand = new RelayCommand(RemoveCommandExecute)
                    }); ;
            }
            return temp;
        }
        private void AddCommandExecute(object obj)
        {
            var window = new AddEditActionWindow();
            window.btnLogin.Content = "ADD";
            window.windowName.Text = "ADD ACTION";
            window.btnLogin.Command = new RelayCommand((window.DataContext as ActionViewModel).AddActionCommandExecute, (window.DataContext as ActionViewModel).EditActionCommandCanExecute);
            window.ShowDialog();
            UpdateActions();
        }
        private void EditCommandExecute(object obj)
        {
            var window = new AddEditActionWindow();
            window.btnLogin.Content = "COMMIT";
            window.windowName.Text = "EDIT ACTION";
            window.btnLogin.Command = new RelayCommand((window.DataContext as ActionViewModel).EditActionCommandExecute, (window.DataContext as ActionViewModel).EditActionCommandCanExecute);
            (window.DataContext as ActionViewModel).Start = Selected.Start;
            (window.DataContext as ActionViewModel).End = Selected.End;
            (window.DataContext as ActionViewModel).Discount = Selected.Discount;
            (window.DataContext as ActionViewModel).Selected = Selected;
            window.ShowDialog();
            UpdateActions();
        }
        private void RemoveCommandExecute(object obj)
        {
            _actionService.Delete(Selected.Id);
            UpdateActions();
        }
        private void AddActionCommandExecute(object obj)
        {
            _actionService.Create(new ActionDTO()
            {
                Start = Start,
                End = End,
                Discount = Discount
            });
            Message = "Succesfuly added";
        }
        private void EditActionCommandExecute(object obj)
        {
            _actionService.Update(new ActionDTO()
            {
                Id = Selected.Id,
                Start = Start,
                End = End,
                Discount = Discount
            });
            Message = "Succesfuly edited";
        }
        private bool EditActionCommandCanExecute(object obj)
        {
            if (Start >= End)
                return false;
            if (End <= DateTime.Now)
                return false;

            return true;
        }
        #endregion
    }
}
