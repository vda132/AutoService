using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.MenuViewModel
{
    class DBAdminMenuViewModel : BaseViewModel
    {
        RelayCommand backButtonCommand;
        private BaseViewModel currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }
        public RelayCommand BackButtonCommand
        {
            get
            {
                return backButtonCommand ??
                      (backButtonCommand = new RelayCommand((o) =>
                      {
                          if (CurrentViewModel.GetType() == new DBAdministratorViewModel().GetType() && MessageBox.Show($"Вы точно хотите вернуться на страницу авторизации? ", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                          {
                              Navigation.Navigation.ToLogin();
                          }
                          else if (CurrentViewModel.GetType() != new DBAdministratorViewModel().GetType())
                          {
                              CurrentViewModel = Navigation.DBAdminNavigation.ToPreviuosViewModel();
                          }
                          else
                          {
                              return;
                          }
                      }));
            }
        }
        public DBAdminMenuViewModel()
        {
            Navigation.DBAdminNavigation.StateChanged += NavigationStateChanged;
            currentViewModel = Navigation.DBAdminNavigation.CurrentViewModel;
        }
        private void NavigationStateChanged()
        {
            CurrentViewModel = Navigation.DBAdminNavigation.CurrentViewModel;
        }
        public override void Dispose()
        {
            Navigation.DBAdminNavigation.StateChanged -= NavigationStateChanged;
        }
    }
}

