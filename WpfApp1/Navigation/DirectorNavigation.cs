using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel;
using WpfApp1.ViewModel.Abstract;
using WpfApp1.ViewModel.DBManipulationViewModel.DBDirectorManipulationViewModel;


namespace WpfApp1.Navigation
{
    static class DirectorNavigation
    {
        public static event Action StateChanged;
        public static Stack<BaseViewModel> previousViewmModel = new Stack<BaseViewModel>();
        private static BaseViewModel currentViewModel = new DirectorViewModel();
        private static int userId;
        public static BaseViewModel CurrentViewModel
        {
            get => currentViewModel;
        }
        public static int UserId
        {
            get => userId;
            set
            {
                userId = value;
            }
        }
        public static void ToDirectorWorkers()
        {
            Navigate(new DirectorWorkersViewModel());
        }
      
        public static void ToDirectorService()
        {
            Navigate(new DirectorServiceViewModel());
        }
        public static void ToDirectorReport()
        {
            Navigate(new DirectorReportViewModel());
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
