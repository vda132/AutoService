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
        List<CarBrand> carBrands = new List<CarBrand>();
        CarBrand selectedCarBrand;
        List<Model> models = new List<Model>();
        Model selectedCarModel;
        DateTime date = DateTime.Now;
        RelayCommand addCommand;
        List<Car> cars;
        List<Car> displayCars;
        bool isAddButtonEnable;
        Car selectedCar;
        bool isAddNewCarButtonEnable;
        RelayCommand addNewCarToClient;
        bool isReadOnly = false;

        string stateNumberFilter;
        RelayCommand resetFilters;
        RelayCommand searchCommand;
        bool isFilterButtonEnable = false;
        public MasterClientViewModel()
        {
            SetProperties();
        }

        private void SetProperties()
        {
            using (var context = new AutoServiceContext())
            {
                carBrands = context.CarBrands.ToList();
                cars = context.Cars.ToList();
                
                var clients = context.Clients.ToList();
                var _models = context.Models.ToList();
                var brands = context.CarBrands.ToList();
                foreach (var car in cars)
                {
                    car.IdclientNavigation = clients.FirstOrDefault(A => A.Idclient == car.Idclient);
                    car.IdmodelNavigation = _models.FirstOrDefault(A => A.Idmodel == car.Idmodel);
                    car.IdmodelNavigation.IdcarBrandNavigation = brands.FirstOrDefault(A => A.IdcarBrand == car.IdmodelNavigation.IdcarBrand);
                }
                displayCars = cars;
                Cars = displayCars;
            }
        }

        private void RecetAll()
        {
            SetProperties();
            Cars = displayCars;
            Models = new List<Model>();
            NameClient = null;
            SelectedCarBrand = null;
            SelectedCarModel = null;
            Date = DateTime.Now;
            StateNumber = null;
            IsAddButtonEnable = false;
            IsReadOnly = false;
            IsAddNewCarButtonEnable = false;
            StateNumberFilter = null;
            IsFilterButtonEnable = false;
        }

        public Car SelectedCar
        {
            get => selectedCar;
            set
            {
                selectedCar = value;
                OnPropertyChanged(nameof(SelectedCar));
                if (selectedCar != null)
                {
                    IsFilterButtonEnable = true;
                    IsAddButtonEnable = false;
                    IsAddNewCarButtonEnable = true;
                    NameClient = selectedCar.IdclientNavigation.NameClient;
                    IsReadOnly = true;
                }

            }
        }
        public bool IsAddButtonEnable
        {
            get => isAddButtonEnable;
            set
            {
                isAddButtonEnable = value;
                OnPropertyChanged(nameof(IsAddButtonEnable));
            }
        }

        public bool IsReadOnly
        {
            get => isReadOnly;
            set
            {
                isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }
        public List<Car> Cars
        {
            get => displayCars;
            set
            {
                displayCars = value;
                OnPropertyChanged(nameof(Cars));
            }
        }
        public string StateNumberFilter
        {
            get => stateNumberFilter;
            set
            {
                stateNumberFilter = value;
                OnPropertyChanged(nameof(StateNumberFilter));
                if (stateNumberFilter != null)
                {
                    IsFilterButtonEnable = true;
                }

            }
        }

        public string NameClient
        {
            get => nameClient;
            set
            {
                nameClient = value;
                OnPropertyChanged(nameof(NameClient));
                if (nameClient != null)
                {
                    IsFilterButtonEnable = true;
                }
            }
        }

        public bool IsAddNewCarButtonEnable
        {
            get => isAddNewCarButtonEnable;
            set
            {
                isAddNewCarButtonEnable = value;
                OnPropertyChanged(nameof(IsAddNewCarButtonEnable));
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
                if (selectedCarBrand != null)
                {
                    Models = new List<Model>();
                    IsFilterButtonEnable = true;
                    using (var context = new AutoServiceContext())
                    {
                        Models = context.Models.Where(A => A.IdcarBrand == selectedCarBrand.IdcarBrand).ToList();
                    }
                }
            }
        }
        public Model SelectedCarModel
        {
            get => selectedCarModel;
            set
            {
                selectedCarModel = value;
                OnPropertyChanged(nameof(SelectedCarModel));
                if (SelectedCarModel != null && !IsReadOnly)
                {
                    IsFilterButtonEnable = true;
                    IsAddButtonEnable = true;
                }
            }
        }
        public DateTime Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged(nameof(Date));
                if (date > DateTime.Now)
                {
                    MessageBox.Show("Выбранная дата не может быть больше сегодняшней даты.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    date = DateTime.Now;
                    return;
                }
            }
        }
        public bool IsFilterButtonEnable
        {
            get => isFilterButtonEnable;
            set
            {
                isFilterButtonEnable = value;
                OnPropertyChanged(nameof(IsFilterButtonEnable));
            }
        }
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                      (addCommand = new RelayCommand((o) =>
                      {
                          using (var context = new AutoServiceContext())
                          {
                              StringBuilder errors = new StringBuilder();
                              if (String.IsNullOrWhiteSpace(nameClient))
                                  errors.AppendLine("Укажите ФИО клиента.");
                              if (selectedCarBrand == null)
                                  errors.AppendLine("Укажите марку автомобиля.");
                              if (selectedCarModel == null)
                                  errors.AppendLine("Укажите модель автомобиля.");
                              if (String.IsNullOrWhiteSpace(stateNumber))
                                  errors.AppendLine("Укажите гос.номер автомобиля.");
                              if (context.Cars.FirstOrDefault(A => A.StateNumber == stateNumber) != null)
                                  errors.AppendLine("Такой гос.номер уже существует.");
                              if (date.ToString() == "")
                                  errors.AppendLine("Введите дату.");
                              if (errors.Length > 0)
                              {
                                  MessageBox.Show(errors.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }
                              Client client = new Client() { NameClient = NameClient };
                              context.Clients.Add(client);
                              context.SaveChanges();
                              Car car = new Car() { StateNumber = StateNumber, Idmodel = SelectedCarModel.Idmodel, DataOfRelease = Date, Idclient = client.Idclient };
                              context.Cars.Add(car);
                              context.SaveChanges();
                              MessageBox.Show("Информация успешно сохранена.", "Success", MessageBoxButton.OK);
                              RecetAll();
                          }
                      }
                       ));
            }
        }
        public RelayCommand AddNewCarToClient
        {
            get
            {
                return addNewCarToClient ??
                      (addNewCarToClient = new RelayCommand((o) =>
                      {
                          using (var context = new AutoServiceContext())
                          {
                              StringBuilder errors = new StringBuilder();
                              if (selectedCarBrand == null)
                                  errors.AppendLine("Укажите марку автомобиля.");
                              if (selectedCarModel == null)
                                  errors.AppendLine("Укажите модель автомобиля.");
                              if (String.IsNullOrWhiteSpace(stateNumber))
                                  errors.AppendLine("Укажите гос.номер автомобиля.");
                              if (context.Cars.FirstOrDefault(A => A.StateNumber == stateNumber) != null)
                                  errors.AppendLine("Такая машина уже существует.");
                              if (date.ToString() == "")
                                  errors.AppendLine("Введите дату.");
                              if (errors.Length > 0)
                              {
                                  MessageBox.Show(errors.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }
                              context.SaveChanges();
                              Car car = new Car() { StateNumber = StateNumber, Idmodel = SelectedCarModel.Idmodel, DataOfRelease = Date, Idclient = SelectedCar.IdclientNavigation.Idclient };
                              context.Cars.Add(car);
                              context.SaveChanges();
                              MessageBox.Show("Информация успешно сохранена.", "Success", MessageBoxButton.OK);
                              RecetAll();
                          }
                      }
                       ));
            }
        }
        public RelayCommand SearchCommand
        {
            get
            {
                return searchCommand ??
                    (searchCommand = new RelayCommand((o) =>
                    {
                        using (var context = new AutoServiceContext())
                        {
                            if (stateNumberFilter != null)
                            {
                                var tmp = context.Cars.Where(A => A.StateNumber.ToLower().Contains(stateNumberFilter.ToLower())).ToList();
                                foreach (var car in tmp)
                                {
                                    car.IdclientNavigation = context.Clients.FirstOrDefault(A => A.Idclient == car.Idclient);
                                    car.IdmodelNavigation = context.Models.FirstOrDefault(A => A.Idmodel == car.Idmodel);
                                    car.IdmodelNavigation.IdcarBrandNavigation = context.CarBrands.FirstOrDefault(A => A.IdcarBrand == car.IdmodelNavigation.IdcarBrand);
                                }
                                Cars = tmp;
                            }
                        }
                    }));
            }
        }
        public RelayCommand ResetFilters
        {
            get
            {
                return resetFilters ??
                     (resetFilters = new RelayCommand((o) =>
                     {
                         RecetAll();
                     }
                      ));
            }
        }
    }
}
