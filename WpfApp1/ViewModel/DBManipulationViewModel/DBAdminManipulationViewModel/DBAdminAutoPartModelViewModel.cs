using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        List<CarBrand> brands = AutoServiceContext.GetContext().CarBrands.ToList();
        List<Model> models;
        List<AutoPart> autoParts = AutoServiceContext.GetContext().AutoParts.ToList();
        CarBrand selectedCarBrand;
        Model selectedModel;
        Compatibility selectedAutoPartComp;
        RelayCommand addComp;
        AutoPart selectedAutoPart;
        private bool isEnable;
        private bool isEnableDelete;
        public bool IsEnable
        {
            get => isEnable;
            set
            {
                isEnable = value;
                OnPropertyChanged(nameof(IsEnable));
            }
        }
        public bool IsEnableDelete
        {
            get => isEnableDelete;
            set
            {
                isEnableDelete = value;
                OnPropertyChanged(nameof(IsEnableDelete));
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
                if(selectedCarBrand!=null)
                {
                    IsEnable = true;
                    Models = AutoServiceContext.GetContext().Models.Where(A => A.IdcarBrand == selectedCarBrand.IdcarBrand).ToList();
                }
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

        public Compatibility SelectedAutoPartComp
        {
            get => selectedAutoPartComp;
            set
            {
                selectedAutoPartComp = value;
                OnPropertyChanged(nameof(SelectedAutoPartComp));
                IsEnableDelete = true;
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

        public AutoPart SelectedAutoPart
        {
            get => selectedAutoPart;
            set
            {
                selectedAutoPart = value;
                OnPropertyChanged(nameof(SelectedAutoPart));
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
                          if (SelectedAutoPart == null)
                              errors.AppendLine("Выберете запчасть.");
                          if (SelectedModel == null)
                              errors.AppendLine("Выберете модель.");
                          if (SelectedCarBrand == null)
                              errors.AppendLine("Выберете марку.");
                          if(errors.Length>0)
                          {
                              MessageBox.Show(errors.ToString(),"Error",MessageBoxButton.OK,MessageBoxImage.Error);
                              return;
                          }
                          Compatibility tmp = new Compatibility()
                          {
                              Idmodel = SelectedModel.Idmodel,
                              IdautoPart = selectedAutoPart.IdautoPart
                          };
                          if (AutoServiceContext.GetContext().Compatibilities.FirstOrDefault(A => A == tmp) != null)
                          {
                              MessageBox.Show("Такая сходимость уже есть.", "Error", MessageBoxButton.OK);
                              return;
                          }
                              
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
                          SetProperties();
                          Compatibilities = displayCompatibilities;
                          IsEnable = false;
                          SelectedAutoPart = null;
                      }
                       ));
            }
        }
    }
}
