using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel;
using WpfApp1.ViewModel.Abstract;
using WpfApp1.ViewModel.DBManipulationViewModel.DBMasterManipulationViewModel;
using WpfApp1.ViewModel.MenuViewModel;

namespace WpfApp1.Navigation
{
    static class MasterNavigation
    {
        public static event Action StateChanged;
        public static Stack<BaseViewModel> previousViewmModel = new Stack<BaseViewModel>();
        private static BaseViewModel currentViewModel = new MasterViewModel();
        private static int userId;
        public static int UserId
        {
            get => userId;
            set
            {
                userId = value;
            }
        }
        public static BaseViewModel CurrentViewModel
        {
            get => currentViewModel;
        }

        public static void ToClientMaster()
        {
            Navigate(new MasterClientViewModel());
        }
        public static void ToMasterAutoService()
        {
            Navigate(new MasterAutoServiceViewModel());
        }
        public static void ToMasterReport()
        {
            Navigate(new MasterReportViewModel());
        }
        private static void Navigate(BaseViewModel viewModel)
        {
            previousViewmModel.Push(currentViewModel);
            currentViewModel?.Dispose();
            currentViewModel = viewModel;
            StateChanged?.Invoke();
        }
        public static BaseViewModel ToPreviuosViewModel()
        {
            currentViewModel?.Dispose();
            currentViewModel = previousViewmModel.Pop();
            StateChanged?.Invoke();
            return currentViewModel;
        }
    }
}
