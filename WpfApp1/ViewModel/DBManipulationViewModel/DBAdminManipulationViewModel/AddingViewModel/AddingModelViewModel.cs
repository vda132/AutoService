using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel.AddingViewModel
{
    class AddingModelViewModel : BaseViewModel
    {
        CarBrand tmp;
        List<CarBrand> carBrands = AutoServiceContext.GetContext().CarBrands.ToList();
        private string modelName;
        RelayCommand addCarModel;
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
            }
        }
        public string ModelName
        {
            get => modelName;
            set
            {
                modelName = value;
                OnPropertyChanged(nameof(ModelName));
            }
        }
        public RelayCommand AddCarModel
        {
            get
            {
                return addCarModel ??
                      (addCarModel = new RelayCommand((o) =>
                      {
                          StringBuilder errors = new StringBuilder();
                          if (String.IsNullOrWhiteSpace(modelName))
                              errors.AppendLine("Укажите название автоконцерна.");
                          if (selecteCarBrand == null)
                              errors.AppendLine("Укажите страну автоконцерна.");
                          if (errors.Length > 0)
                          {
                              MessageBox.Show(errors.ToString());
                              return;
                          }

                          tmp = AutoServiceContext.GetContext().CarBrands.FirstOrDefault(A => A.NameCarBrand == selecteCarBrand.NameCarBrand);
                          int id = tmp.IdcarBrand;
                          Model tmpModel = new Model() { NameModel = modelName, IdcarBrand = id };

                          AutoServiceContext.GetContext().Models.Add(tmpModel);
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
