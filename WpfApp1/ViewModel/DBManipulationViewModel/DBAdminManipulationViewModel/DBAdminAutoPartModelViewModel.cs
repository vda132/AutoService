using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel
{
    class DBAdminAutoPartModelViewModel:BaseViewModel
    {
        List<Compatibility> compatibilities;
        List<Compatibility> displayCompatibilities;

        public DBAdminAutoPartModelViewModel()
        {
            SetProperties();
        }

        private void SetProperties()
        {
            compatibilities = AutoServiceContext.GetContext().Compatibilities.ToList();
            var models = AutoServiceContext.GetContext().Models.ToList();
            var brands = AutoServiceContext.GetContext().CarBrands.ToList();
            var autoParts = AutoServiceContext.GetContext().AutoParts.ToList();
            foreach (var model in compatibilities)
            {
                model.IdmodelNavigation = models.FirstOrDefault(A => A.Idmodel == model.Idmodel);
                model.IdmodelNavigation.IdcarBrandNavigation = brands.FirstOrDefault(A => A.IdcarBrand == model.IdmodelNavigation.IdcarBrand);
                model.IdautoPartNavigation = autoParts.FirstOrDefault(A=>A.IdautoPart==model.IdautoPart);
            }
            displayCompatibilities = compatibilities;
        }

        public List<Compatibility> Compatibilities
        {
            get => displayCompatibilities;
            set
            {
                displayCompatibilities = value;
                OnPropertyChanged(nameof(Compatibilities));
            }
        }
        RelayCommand toAddingAutoPartModel;
        public RelayCommand ToAddingAutoPartModel
        {
            get
            {
                return toAddingAutoPartModel ??
                      (toAddingAutoPartModel = new RelayCommand((o) =>
                      {
                          Navigation.DBAdminNavigation.ToAddingAutoPartModel();
                      }
                       ));
            }
        }
    }
}
