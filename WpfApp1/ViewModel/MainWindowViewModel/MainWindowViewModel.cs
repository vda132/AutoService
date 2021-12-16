using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel
{
    class MainWindowViewModel:BaseViewModel
    {
        private BaseViewModel currentMenuViewModel;
        public BaseViewModel CurrentMenuViewModel
        {
            get => currentMenuViewModel;
            set
            {
                currentMenuViewModel = value;
                OnPropertyChanged(nameof(CurrentMenuViewModel));
            }
        }
        public MainWindowViewModel()
        {
            Navigation.Navigation.StateChanged += NavigationStateChanged;
            currentMenuViewModel = Navigation.Navigation.CurrentViewModel;
        }
        private void NavigationStateChanged()
        {
            CurrentMenuViewModel = Navigation.Navigation.CurrentViewModel;
        }
        public override void Dispose()
        {
            Navigation.Navigation.StateChanged -= NavigationStateChanged;
        }
    }
}
