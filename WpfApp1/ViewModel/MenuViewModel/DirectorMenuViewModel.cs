using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.MenuViewModel
{

    class DirectorMenuViewModel : BaseViewModel
    {
        RelayCommand backButtonCommand;
        bool isBackButtonEnable=true;
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

        public DirectorMenuViewModel()
        {
            Navigation.DirectorNavigation.StateChanged += NavigationStateChanged;
            currentViewModel = Navigation.DirectorNavigation.CurrentViewModel;
        }
        public RelayCommand BackButtonCommand
        {
            get
            {
                return backButtonCommand ??
                      (backButtonCommand = new RelayCommand((o) =>
                      {
                          if (CurrentViewModel.GetType() == new DirectorViewModel().GetType() && MessageBox.Show($"Вы точно хотите вернуться на страницу авторизации? ", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                          {
                              Navigation.Navigation.ToLogin();
                          }
                          else if (CurrentViewModel.GetType() != new DirectorViewModel().GetType())
                          {
                              CurrentViewModel = Navigation.DirectorNavigation.ToPreviuosViewModel();
                          }
                          else
                          {
                              return;
                          }
                      }));
            }
        }
        private void NavigationStateChanged()
        {
            CurrentViewModel = Navigation.DirectorNavigation.CurrentViewModel;
        }
        public override void Dispose()
        {
            Navigation.DirectorNavigation.StateChanged -= NavigationStateChanged;
        }
    }
}
