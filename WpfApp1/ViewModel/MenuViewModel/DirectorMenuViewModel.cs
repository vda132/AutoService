using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.MenuViewModel
{

    class DirectorMenuViewModel : BaseViewModel
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
                          if (CurrentViewModel.GetType() != new DirectorViewModel().GetType())
                          {
                              CurrentViewModel = Navigation.DirectorNavigation.ToPreviuosViewModel();
                          }
                          else Navigation.Navigation.ToLogin();
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
