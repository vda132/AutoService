using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBMasterManipulationViewModel
{
    class MasterReportViewModel:BaseViewModel
    {
        List<AutoServiceAutoPart> autoServiceAutoParts;
        List<AutoServiceAutoPart> displayAutoServiceAutoParts;
        string numberService;
        RelayCommand findCommand;
        RelayCommand resetCommand;
        int number;
        bool isEnable;
        public string NumberService
        {
            get => numberService;
            set
            {
                numberService = value;
                OnPropertyChanged(nameof(NumberService));
                if (numberService != null)
                    IsEnable = true;
            }
        }
        public List<AutoServiceAutoPart> DisplayAutoServiceAutoParts
        {
            get => displayAutoServiceAutoParts;
            set
            {
                displayAutoServiceAutoParts = value;
                OnPropertyChanged(nameof(DisplayAutoServiceAutoParts));
            }
        }
        private void ResetAll()
        {
            NumberService = null;
            DisplayAutoServiceAutoParts = null;
            IsEnable = false;
        }
        public bool IsEnable
        {
            get => isEnable;
            set
            {
                isEnable = value;
                OnPropertyChanged(nameof(IsEnable));
            }
        }
        public RelayCommand FindCommand
        {
            get
            {
                return findCommand ??
                     (findCommand = new RelayCommand((o) =>
                     {
                         try
                         {
                             if (Int32.TryParse(numberService, out number)) 
                             { 
                                 if (AutoServiceContext.GetContext().AutoServiceAutoParts.FirstOrDefault(A => A.IdautoService == number) == null)
                                 {
                                     MessageBox.Show("Такого ремонта не существует!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                     return;
                                 }
                                 else
                                 {
                                     autoServiceAutoParts = AutoServiceContext.GetContext().AutoServiceAutoParts.Where(A => A.IdautoService == number).ToList();
                                 }
                             }
                             else throw new Exception();
                             foreach (var tmp in autoServiceAutoParts)
                             {
                                 tmp.IdautoPartNavigation = AutoServiceContext.GetContext().AutoParts.FirstOrDefault(A => A.IdautoPart == tmp.IdautoPart);
                                 tmp.IdautoServiceNavigation = AutoServiceContext.GetContext().AutoServices.FirstOrDefault(A => A.IdautoService == tmp.IdautoService);
                                 tmp.IdautoServiceNavigation.StateNumberNavigation = AutoServiceContext.GetContext().Cars.FirstOrDefault(A => A.StateNumber == tmp.IdautoServiceNavigation.StateNumber);
                                 tmp.IdautoServiceNavigation.StateNumberNavigation.IdclientNavigation = AutoServiceContext.GetContext().Clients.FirstOrDefault(A => A.Idclient == tmp.IdautoServiceNavigation.StateNumberNavigation.Idclient);
                                 tmp.IdautoServiceNavigation.StateNumberNavigation.IdmodelNavigation = AutoServiceContext.GetContext().Models.FirstOrDefault(A => A.Idmodel == tmp.IdautoServiceNavigation.StateNumberNavigation.Idmodel);
                                 tmp.IdautoServiceNavigation.StateNumberNavigation.IdmodelNavigation.IdcarBrandNavigation = AutoServiceContext.GetContext().CarBrands.FirstOrDefault(A => A.IdcarBrand == tmp.IdautoServiceNavigation.StateNumberNavigation.IdmodelNavigation.IdcarBrand);
                             }
                             DisplayAutoServiceAutoParts = autoServiceAutoParts;
                         }
                         catch
                         {
                             MessageBox.Show("Выввели некоректные данные!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                         }
                     }));
            }
        }
        public RelayCommand ResetCommand
        {
            get
            {
                return resetCommand ??
                     (resetCommand = new RelayCommand((o) =>
                     {
                         ResetAll();
                     }));
            }
        }
    }
}
