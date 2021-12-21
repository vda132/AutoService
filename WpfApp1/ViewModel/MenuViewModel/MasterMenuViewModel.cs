using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;
using WpfApp1.ViewModel.DBManipulationViewModel.DBDirectorManipulationViewModel;
using WpfApp1.ViewModel.DBManipulationViewModel.DBMasterManipulationViewModel;

namespace WpfApp1.ViewModel.MenuViewModel
{

    class MasterMenuViewModel : BaseViewModel
    {

        RelayCommand backButtonCommand;
        private BaseViewModel currentViewModel;
        Visibility visible;
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
                          if (CurrentViewModel.GetType() == new MasterViewModel().GetType() && MessageBox.Show($"Вы точно хотите вернуться на страницу авторизации? ", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                          {
                              Navigation.Navigation.ToLogin();
                          }
                          else if(CurrentViewModel.GetType() != new MasterViewModel().GetType())
                          {  
                               Navigation.MasterNavigation.ToPreviuosViewModel();
                          }
                          else
                          {
                              return;
                          }
                          
                      }));
            }
        }
        public Visibility Visible
        {
            get => visible;
            set
            {
                visible = value;
                OnPropertyChanged(nameof(Visible));
            }
        }
        public MasterMenuViewModel()
        {
            visible = Visibility.Visible;
            Navigation.MasterNavigation.StateChanged += NavigationStateChanged;
            currentViewModel = Navigation.MasterNavigation.CurrentViewModel;
            Visible = visible;
        }
        private void NavigationStateChanged()
        {
            CurrentViewModel = Navigation.MasterNavigation.CurrentViewModel;
        }
        public override void Dispose()
        {
            Navigation.MasterNavigation.StateChanged -= NavigationStateChanged;
        }
    }
}
