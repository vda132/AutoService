using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel;
using WpfApp1.ViewModel.Abstract;
using WpfApp1.ViewModel.MenuViewModel;

namespace WpfApp1.Navigation
{
    static class Navigation
    {
        public static event Action StateChanged;
        public static Stack<BaseViewModel> previousViewmModel = new Stack<BaseViewModel>();
        private static BaseViewModel currentViewModel = new LoginViewModel();
        public static BaseViewModel CurrentViewModel
        {
            get => currentViewModel;
        }
        public static void ToDirector()
        {
            Navigate(new DirectorMenuViewModel());
        }
        public static void ToMaster()
        {
            Navigate(new MasterMenuViewModel());
        }
        public static void ToDBAdmin()
        {
            Navigate(new DBAdminMenuViewModel());
        }
        public static void ToLogin()
        {
            currentViewModel?.Dispose();
            currentViewModel = previousViewmModel.Pop();
            StateChanged?.Invoke();
        }
        private static void Navigate(BaseViewModel viewModel)
        {
            previousViewmModel.Push(currentViewModel);
            currentViewModel?.Dispose();
            currentViewModel = viewModel;
            StateChanged?.Invoke();
        }

    }
}
