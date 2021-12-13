using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel
{
    class DBAdminModelViewModel:BaseViewModel
    {
        List<Model> carModels;
        List<Model> displaycarModels;
        public DBAdminModelViewModel()
        {
            SetProperties();
        }
        private void SetProperties()
        {
            carModels = AutoServiceContext.GetContext().Models.ToList();
            var autoBrands = AutoServiceContext.GetContext().CarBrands.ToList();
            foreach (var brand in carModels)
            {
                brand.IdcarBrandNavigation = autoBrands.FirstOrDefault(A => A.IdcarBrand == brand.IdcarBrand);
            }
            displaycarModels = carModels;
        }
        public List<Model> CarModels
        {
            get => displaycarModels;
            set
            {
                displaycarModels = value;
                OnPropertyChanged(nameof(CarModels));
            }
        }
        RelayCommand toAddingModel;
        public RelayCommand ToAddingModel
        {
            get
            {
                return toAddingModel ??
                      (toAddingModel = new RelayCommand((o) =>
                      {
                          Navigation.DBAdminNavigation.ToAddingModel();
                      }
                       ));
            }
        }
    }
}
