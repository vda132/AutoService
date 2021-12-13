using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel.AddingViewModel
{
    class AddingAutoPartModelViewModel:BaseViewModel
    {
        List<CarBrand> brands=AutoServiceContext.GetContext().CarBrands.ToList();
        List<Model> models;
        List<AutoPart> autoParts = AutoServiceContext.GetContext().AutoParts.ToList();
        CarBrand selectedCarBrand;
        Model selectedModel;
        AutoPart selectedAutoPart;
        RelayCommand addComp;
        private bool isEnable;
        public bool IsEnable
        {
            get => isEnable;
            set
            {
                isEnable = value;
                OnPropertyChanged(nameof(IsEnable));
            }
        }
        public List<CarBrand> Brands
        {
            get => brands;
            set
            {
                brands = value;
                OnPropertyChanged(nameof(Brands));
            }
        }
        public CarBrand SelectedCarBrand
        {
            get => selectedCarBrand;
            set
            {
                selectedCarBrand = value;
                OnPropertyChanged(nameof(SelectedCarBrand));
                IsEnable = true;
                Models = AutoServiceContext.GetContext().Models.Where(A => A.IdcarBrand == selectedCarBrand.IdcarBrand).ToList();
            }
        }
        public Model SelectedModel
        {
            get => selectedModel;
            set
            {
                selectedModel = value;
                OnPropertyChanged(nameof(SelectedCarBrand));
            }
        }
        public List<AutoPart> AutoParts
        {
            get => autoParts;
            set
            {
                autoParts = value;
                OnPropertyChanged(nameof(AutoParts));
            }
        }

        public AutoPart SelectedAutoPart
        {
            get => selectedAutoPart;
            set
            {
                selectedAutoPart = value;
                OnPropertyChanged(nameof(SelectedAutoPart));
            }
        }
        public List<Model> Models
        {
            get => models;
            set
            {
                models = value;
                OnPropertyChanged(nameof(Models));
            }
        }
        public RelayCommand AddComp
        {
            get
            {
                return addComp ??
                      (addComp = new RelayCommand((o) =>
                      {
                          StringBuilder errors = new StringBuilder();
                          Compatibility tmp = new Compatibility()
                          {
                              Idmodel = SelectedModel.Idmodel,
                              IdautoPart = selectedAutoPart.IdautoPart
                          };
                          AutoServiceContext.GetContext().Compatibilities.Add(tmp);
                          try
                          {
                              AutoServiceContext.GetContext().SaveChanges();
                              MessageBox.Show("Информация сохранена!");

                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message.ToString());
                          }

                      }
                       ));
            }
        }
    }
}
