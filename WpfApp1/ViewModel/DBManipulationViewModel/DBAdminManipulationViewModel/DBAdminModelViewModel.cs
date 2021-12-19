using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel
{
    class DBAdminModelViewModel : BaseViewModel
    {
        List<Model> carModels;
        List<Model> displaycarModels;
        CarBrand tmp;
        List<CarBrand> carBrands = new List<CarBrand>();
        private string modelName;
        bool isEnable;
        RelayCommand addCarModel;
        RelayCommand resetAll;
        public DBAdminModelViewModel()
        {
            SetProperties();
        }
        private void SetProperties()
        {
            using (var context = new AutoServiceContext())
            {
                carBrands = context.CarBrands.ToList();
                carModels = context.Models.ToList();
                var autoBrands = context.CarBrands.ToList();
                foreach (var brand in carModels)
                {
                    brand.IdcarBrandNavigation = autoBrands.FirstOrDefault(A => A.IdcarBrand == brand.IdcarBrand);
                }
                CarModels = carModels;
            }
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

        public List<CarBrand> CarBrands
        {
            get => carBrands;
            set
            {
                carBrands = value;
                OnPropertyChanged(nameof(CarBrands));
            }
        }
        private CarBrand selecteCarBrand;
        public CarBrand SelecteCarBrand
        {
            get => selecteCarBrand;
            set
            {
                selecteCarBrand = value;
                OnPropertyChanged(nameof(SelecteCarBrand));
                if (selecteCarBrand != null)
                {
                    IsEnable = true;
                }
            }
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
        public string ModelName
        {
            get => modelName;
            set
            {
                modelName = value;
                OnPropertyChanged(nameof(ModelName));
                if (modelName != null)
                {
                    IsEnable = true;
                }
            }
        }
        public RelayCommand AddCarModel
        {
            get
            {
                return addCarModel ??
                      (addCarModel = new RelayCommand((o) =>
                      {
                          using (var context = new AutoServiceContext())
                          {
                              StringBuilder errors = new StringBuilder();
                              if (String.IsNullOrWhiteSpace(modelName))
                                  errors.AppendLine("Укажите название модели.");
                              if (selecteCarBrand == null)
                                  errors.AppendLine("Укажите марку.");
                              if (errors.Length > 0)
                              {
                                  MessageBox.Show(errors.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }

                              tmp = context.CarBrands.FirstOrDefault(A => A.NameCarBrand == selecteCarBrand.NameCarBrand);
                              int id = tmp.IdcarBrand;
                              Model tmpModel = new Model() { NameModel = modelName, IdcarBrand = id };

                              context.Models.Add(tmpModel);
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
                              CarModels = displaycarModels;
                              ModelName = null;
                              SelecteCarBrand = null;
                              IsEnable = false;
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
                         CarModels = displaycarModels;
                         ModelName = null;
                         SelecteCarBrand = null;
                         IsEnable = false;

                     }
                     ));
            }
        }
    }
}
