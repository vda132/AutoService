using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel
{
    class DBAdminBrandViewModel:BaseViewModel
    {
        List<CarBrand> carBrands;
        List<CarBrand> displaycarBrands;
        public DBAdminBrandViewModel()
        {
            SetProperties();
        }
        private void SetProperties()
        {
            carBrands = AutoServiceContext.GetContext().CarBrands.ToList();
            var autocincerns = AutoServiceContext.GetContext().AutoConcerns.ToList();
            foreach (var concern in carBrands)
            {
                concern.IdautoConcernNavigation = autocincerns.FirstOrDefault(A => A.IdautoConcern == concern.IdautoConcern);
            }
            displaycarBrands = carBrands;
        }
        public List<CarBrand> CarBrands
        {
            get => displaycarBrands;
            set
            {
                displaycarBrands = value;
                OnPropertyChanged(nameof(CarBrands));
            }
        }
        RelayCommand toAddingBrand;
        public RelayCommand ToAddingBrand
        {
            get
            {
                return toAddingBrand ??
                      (toAddingBrand = new RelayCommand((o) =>
                      {
                          Navigation.DBAdminNavigation.ToAddingBrand();
                      }
                       ));
            }
        }
    }
}
