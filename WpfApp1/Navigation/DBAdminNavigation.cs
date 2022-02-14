using System;
using System.Collections.Generic;
using WpfApp1.ViewModel;
using WpfApp1.ViewModel.Abstract;
using WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel;


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
