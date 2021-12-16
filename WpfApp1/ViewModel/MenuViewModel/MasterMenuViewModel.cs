using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.MenuViewModel
{

    class MasterMenuViewModel : BaseViewModel
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
                          if (CurrentViewModel.GetType() != new MasterViewModel().GetType())
                          {
                              CurrentViewModel = Navigation.MasterNavigation.ToPreviuosViewModel();
                          }
                          else Navigation.Navigation.ToLogin();
                      }));
            }
        }
        public MasterMenuViewModel()
        {
            Navigation.MasterNavigation.StateChanged += NavigationStateChanged;
            currentViewModel = Navigation.MasterNavigation.CurrentViewModel;
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
