using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBMasterManipulationViewModel
{
    class MasterClientViewModel : BaseViewModel
    {
        string nameClient;
        string stateNumber;
        List<CarBrand> carBrands = AutoServiceContext.GetContext().CarBrands.ToList();
        CarBrand selectedCarBrand;
        List<Model> models;
        Model selectedCarModel;
        DateTime date;
        RelayCommand addCommand;
        public string NameClient
        {
            get => nameClient;
            set
            {
                nameClient = value;
                OnPropertyChanged(nameof(NameClient));
            }
        }
        public string StateNumber
        {
            get => stateNumber;
            set
            {
                stateNumber = value;
                OnPropertyChanged(nameof(StateNumber));
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
        public List<Model> Models
        {
            get => models;
            set
            {
                models = value;
                OnPropertyChanged(nameof(Models));
            }
        }
        public CarBrand SelectedCarBrand
        {
            get => selectedCarBrand;
            set
            {
                selectedCarBrand = value;
                OnPropertyChanged(nameof(SelectedCarBrand));
                Models = AutoServiceContext.GetContext().Models.Where(A => A.IdcarBrand == selectedCarBrand.IdcarBrand).ToList();
            }
        }
        public Model SelectedCarModel
        {
            get => selectedCarModel;
            set
            {
                selectedCarModel = value;
                OnPropertyChanged(nameof(SelectedCarModel));
            }
        }
        public DateTime Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
            }
        }
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                      (addCommand = new RelayCommand((o) =>
                      {
                          StringBuilder errors = new StringBuilder();
                          if (String.IsNullOrWhiteSpace(nameClient))
                              errors.AppendLine("Укажите ФИО сотрудника.");
                          if (selectedCarBrand == null)
                              errors.AppendLine("Укажите должность сотрудника.");
                          if (selectedCarModel == null)
                              errors.AppendLine("Укажите логин сотрудника.");
                          if (String.IsNullOrWhiteSpace(stateNumber))
                              errors.AppendLine("Укажите пароль сотрудника.");
                          if (AutoServiceContext.GetContext().Cars.FirstOrDefault(A => A.StateNumber == stateNumber) != null)
                              errors.AppendLine("Такая машина уже существует.");
                          if (date.ToString() == "")
                              errors.AppendLine("Введите дату.");
                          if (errors.Length > 0)
                          {
                              MessageBox.Show(errors.ToString());
                              return;
                          }

                      }
                       ));
            }
        }
    }
}
