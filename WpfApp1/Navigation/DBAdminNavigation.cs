using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel;
using WpfApp1.ViewModel.Abstract;
using WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel;
using WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel.AddingViewModel;

namespace WpfApp1.Navigation
{
    class DBAdminNavigation
    {
        public static event Action StateChanged;
        public static Stack<BaseViewModel> previousViewmModel = new Stack<BaseViewModel>();
        private static BaseViewModel currentViewModel = new DBAdministratorViewModel();
        public static BaseViewModel CurrentViewModel
        {
            get => currentViewModel;
        }

        public static void ToAutoConcern()
        {
            Navigate(new DBAdminAutoConcernViewModel());
        }

        public static void ToModel()
        {
            Navigate(new DBAdminModelViewModel());
        }
        public static void ToAutoPart()
        {
            Navigate(new DBAdminAutoPartViewModel());
        }

        public static void ToAutoPartPrice()
        {
            Navigate(new DBAdminAutoPartPriceViewModel());
        }

        public static void ToAutoPartModel()
        {
            Navigate(new DBAdminAutoPartModelViewModel());
        }

        public static void ToBrand()
        {
            Navigate(new DBAdminBrandViewModel());
        }

        public static void ToAddingAutoConcern()
        {
            Navigate(new AddingAutoConcernViewModel());
        }

        public static void ToAddingBrand()
        {
            Navigate(new AddingBrandViewModel());
        }

        public static void ToAddingModel()
        {
            Navigate(new AddingModelViewModel());
        }
        public static void ToAddingAutoPart()
        {
            Navigate(new AddingAutoPartViewModel());
        }

        public static void ToAddingAutoPartModel()
        {
            Navigate(new AddingAutoPartModelViewModel());
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
