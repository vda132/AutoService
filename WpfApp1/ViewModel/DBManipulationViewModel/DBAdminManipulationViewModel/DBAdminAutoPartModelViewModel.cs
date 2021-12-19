using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel
{
    class DBAdminAutoPartModelViewModel : BaseViewModel
    {
        List<Compatibility> compatibilities;
        List<Compatibility> displayCompatibilities;
        RelayCommand resetAll;
        List<CarBrand> brands = new List<CarBrand>();
        List<Model> models;
        List<AutoPart> autoParts = new List<AutoPart>();
        CarBrand selectedCarBrand;
        Model selectedModel;
        Compatibility selectedAutoPartComp;
        RelayCommand addComp;
        AutoPart selectedAutoPart;
        bool isResetEnable = false;
        public DBAdminAutoPartModelViewModel()
        {
            SetProperties();
        }

        private void SetProperties()
        {
            using (var context = new AutoServiceContext())
            {
                autoParts = context.AutoParts.ToList();
                brands = context.CarBrands.ToList();
                compatibilities = context.Compatibilities.ToList();
                var _models = context.Models.ToList();
                var _brands = context.CarBrands.ToList();
                var _autoParts = context.AutoParts.ToList();
                foreach (var model in compatibilities)
                {
                    model.IdmodelNavigation = _models.FirstOrDefault(A => A.Idmodel == model.Idmodel);
                    model.IdmodelNavigation.IdcarBrandNavigation = _brands.FirstOrDefault(A => A.IdcarBrand == model.IdmodelNavigation.IdcarBrand);
                    model.IdautoPartNavigation = _autoParts.FirstOrDefault(A => A.IdautoPart == model.IdautoPart);
                }
                Compatibilities = compatibilities;
            }
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
                if (selectedCarBrand != null)
                {
                    using (var context = new AutoServiceContext())
                    {

                        IsResetEnable = true;
                        Models = context.Models.Where(A => A.IdcarBrand == selectedCarBrand.IdcarBrand).ToList();
                    }
                }
            }
        }
        public Model SelectedModel
        {
            get => selectedModel;
            set
            {
                selectedModel = value;
                OnPropertyChanged(nameof(SelectedModel));
                if (selectedModel != null)
                {
                    IsResetEnable = true;
                }
            }
        }
        public bool IsResetEnable
        {
            get => isResetEnable;
            set
            {
                isResetEnable = value;
                OnPropertyChanged(nameof(IsResetEnable));
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
                if (selectedAutoPart != null)
                {
                    IsResetEnable = true;
                }
            }
        }
        public RelayCommand AddComp
        {
            get
            {
                return addComp ??
                      (addComp = new RelayCommand((o) =>
                      {
                          using (var context = new AutoServiceContext())
                          {
                              StringBuilder errors = new StringBuilder();
                              if (SelectedAutoPart == null)
                                  errors.AppendLine("Выберете запчасть.");
                              if (SelectedModel == null)
                                  errors.AppendLine("Выберете модель.");
                              if (SelectedCarBrand == null)
                                  errors.AppendLine("Выберете марку.");
                              if (errors.Length > 0)
                              {
                                  MessageBox.Show(errors.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }
                              Compatibility tmp = new Compatibility()
                              {
                                  Idmodel = SelectedModel.Idmodel,
                                  IdautoPart = selectedAutoPart.IdautoPart
                              };
                              if (context.Compatibilities.FirstOrDefault(A => A == tmp) != null)
                              {
                                  MessageBox.Show("Такая сходимость уже есть.", "Error", MessageBoxButton.OK);
                                  return;
                              }

                              context.Compatibilities.Add(tmp);
                              try
                              {
                                  context.SaveChanges();
                                  MessageBox.Show("Информация сохранена!");

                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message.ToString());
                              }
                              SetProperties();
                              Compatibilities = displayCompatibilities;
                              SelectedCarBrand = null;
                              SelectedModel = null;
                              SelectedAutoPart = null;
                              IsResetEnable = false;
                          }
                      }
                       ));
            }
        }
        public RelayCommand ResetAll
        {
            get
            {
                return resetAll ??
                      (resetAll = new RelayCommand((o) =>
                      {
                          SetProperties();
                          SelectedCarBrand = null;
                          SelectedModel = null;
                          SelectedAutoPart = null;
                          IsResetEnable = false;
                          Compatibilities = displayCompatibilities;
                      }));
            }
        }
    }
}
