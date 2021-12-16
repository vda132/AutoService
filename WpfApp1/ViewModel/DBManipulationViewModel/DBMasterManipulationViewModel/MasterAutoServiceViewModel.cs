using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBMasterManipulationViewModel
{
    class MasterAutoServiceViewModel : BaseViewModel
    {
        List<AutoService> autoServices;
        List<AutoService> displayAutoServices;
        List<Client> clients = AutoServiceContext.GetContext().Clients.ToList();
        Client selectedClient;
        List<CarBrand> carBrands;
        CarBrand selectedCarBrand;
        AutoService service;
        List<Model> models;
        Model selectedModel;
        List<ServiceType> serviceTypes = AutoServiceContext.GetContext().ServiceTypes.ToList();
        ServiceType selectedServiceType;
        List<AutoPart> autoParts;
        AutoPart selectedAutoPart;
        List<Car> stateNumbers;
        Car selectedStateNumber;
        Visibility visibility = Visibility.Hidden;
        DateTime date = DateTime.Now;
        RelayCommand addService;
        RelayCommand addAutoPartToService;
        RelayCommand endAddingAutoPartToService;
        RelayCommand resetCommand;
        decimal priceForRepairs;
        string nameClientFilter;
        string nameMarkFilter;
        string nameModelFilter;
        string nameAutoPartFilter;
        bool isEnable = false;
        bool isAddingButtonEnable = true;
        bool isEnableEndButton = false;
        bool isResetButtonEnable = false;
        bool isBrandEnable = false;
        bool isModelEnable=false;
        bool isReadOnly=false;
        bool isStateNumberEnable = false;
        bool isClientEnable = true;
        bool isServiceTypeEnable = false;
        public MasterAutoServiceViewModel()
        {
            SetProperties();
        }
        private void SetProperties()
        {
            autoServices = AutoServiceContext.GetContext().AutoServices.ToList();
            var workers = AutoServiceContext.GetContext().Workers.ToList();
            var cars = AutoServiceContext.GetContext().Cars.ToList();
            var service = AutoServiceContext.GetContext().ServiceTypes.ToList();
            var models = AutoServiceContext.GetContext().Models.ToList();
            var clients = AutoServiceContext.GetContext().Clients.ToList();
            var brands = AutoServiceContext.GetContext().CarBrands.ToList();
            foreach (var element in autoServices)
            {
                element.IdserviceTypeNavigation = service.FirstOrDefault(A => A.IdserviceType == element.IdserviceType);
                element.IdworkerNavigation = workers.FirstOrDefault(A => A.Idworker == element.Idworker);
                element.StateNumberNavigation = cars.FirstOrDefault(A => A.StateNumber == element.StateNumber);
                element.StateNumberNavigation.IdmodelNavigation = models.FirstOrDefault(A => A.Idmodel == element.StateNumberNavigation.Idmodel);
                element.StateNumberNavigation.IdclientNavigation = clients.FirstOrDefault(A => A.Idclient == element.StateNumberNavigation.Idclient);
                element.StateNumberNavigation.IdmodelNavigation.IdcarBrandNavigation = brands.FirstOrDefault(A => A.IdcarBrand == element.StateNumberNavigation.IdmodelNavigation.IdcarBrand);
            }
            displayAutoServices = autoServices;
        }
        private void ResetAll()
        {
            SetProperties();
            AutoServices = displayAutoServices;
            SelectedClient = null;
            SelectedAutoPart = null;
            SelectedCarBrand = null;
            SelectedModel = null;
            SelectedStateNumber = null;
            ServiceTypes = null;
            Date = DateTime.Now;
            Visibility = Visibility.Hidden;
            NameAutoPartFilter = null;
            NameModelFilter = null;
            NameMarkFilter = null;
            NameClientFilter = null;
            IsEnable = false;
            IsAddingButtonEnable = true;
            IsEnableEndButton = false;
            IsResetButtonEnable = false;
            IsBrandEnable = false;
            IsModelEnable = false;
            IsReadOnly = false;
            IsResetButtonEnable = false;
            IsClientEnable = true;
            IsStateNumberEnable = false;
            IsServiceTypeEnable = false;
        }
        public List<AutoService> AutoServices
        {
            get => displayAutoServices;
            set
            {
                displayAutoServices = value;
                OnPropertyChanged(nameof(AutoServices));
            }
        }
        public List<Client> Clients
        {
            get => clients;
            set
            {
                clients = value;
                OnPropertyChanged(nameof(Clients));
            }
        }
        public Client SelectedClient
        {
            get => selectedClient;
            set
            {
                selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
                if (selectedClient != null)
                {
                    List<CarBrand> tmp = new List<CarBrand>();
                    var clientCars = AutoServiceContext.GetContext().Cars.Where(A => A.Idclient == selectedClient.Idclient).ToList();
                    foreach (var brand in clientCars)
                    {
                        brand.IdmodelNavigation = AutoServiceContext.GetContext().Models.FirstOrDefault(A => A.Idmodel == brand.Idmodel);
                        brand.IdmodelNavigation.IdcarBrandNavigation = AutoServiceContext.GetContext().CarBrands.FirstOrDefault(A => A.IdcarBrand == brand.IdmodelNavigation.IdcarBrand);
                        tmp.Add(AutoServiceContext.GetContext().CarBrands.FirstOrDefault(A => A == brand.IdmodelNavigation.IdcarBrandNavigation));
                    }
                    CarBrands = tmp.Distinct().ToList();
                    IsResetButtonEnable = true;
                    IsBrandEnable = true;
                }
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
        public CarBrand SelectedCarBrand
        {
            get => selectedCarBrand;
            set
            {
                selectedCarBrand = value;
                OnPropertyChanged(nameof(SelectedCarBrand));
                if (selectedCarBrand != null && selectedClient != null)
                {
                    List<Model> tmp = new List<Model>();
                    var models = AutoServiceContext.GetContext().Models.ToList();
                    var clientCars = AutoServiceContext.GetContext().Cars.Where(A => A.Idclient == selectedClient.Idclient&&A.IdmodelNavigation.IdcarBrandNavigation.IdcarBrand==selectedCarBrand.IdcarBrand).ToList();
                    foreach (var tmpt in clientCars)
                    {
                        tmp.Add(AutoServiceContext.GetContext().Models.FirstOrDefault(A => A.Idmodel == tmpt.Idmodel));
                    }
                    Models = tmp.Distinct().ToList();
                    IsResetButtonEnable = true;
                    IsModelEnable = true;
                }
            }
        }
        public string NameClientFilter
        {
            get => nameClientFilter;
            set
            {
                nameClientFilter = value;
                OnPropertyChanged(nameof(NameClientFilter));
                if (nameClientFilter != null)
                {
                    Clients = AutoServiceContext.GetContext().Clients.Where(A => A.NameClient.ToLower().Contains(nameClientFilter.ToLower())).ToList();
                    IsResetButtonEnable = true;
                }
            }
        }
        public string NameMarkFilter
        {
            get => nameMarkFilter;
            set
            {
                nameMarkFilter = value;
                OnPropertyChanged(nameof(NameMarkFilter));
                if (nameMarkFilter != null)
                {
                   CarBrands = AutoServiceContext.GetContext().CarBrands.Where(A => A.NameCarBrand.ToLower().Contains(nameMarkFilter.ToLower())).ToList();
                   IsResetButtonEnable = true;
                }
            }
        }
        public string NameModelFilter
        {
            get => nameModelFilter;
            set
            {
                nameModelFilter = value;
                OnPropertyChanged(nameof(NameModelFilter));
                if (nameModelFilter != null)
                {
                    Models = AutoServiceContext.GetContext().Models.Where(A => A.NameModel.ToLower().Contains(nameModelFilter.ToLower())).ToList();
                    IsResetButtonEnable = true;
                }
            }
        }
        public string NameAutoPartFilter
        {
            get => nameAutoPartFilter;
            set
            {
                nameAutoPartFilter = value;
                OnPropertyChanged(nameof(NameAutoPartFilter));
                if (nameAutoPartFilter != null)
                {
                    AutoParts = AutoServiceContext.GetContext().AutoParts.Where(A => A.NameAutoPart.ToLower().Contains(nameAutoPartFilter.ToLower())).ToList();
                    IsResetButtonEnable = true;
                }
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
        public Model SelectedModel
        {
            get => selectedModel;
            set
            {
                selectedModel = value;
                OnPropertyChanged(nameof(SelectedModel));
                if (selectedModel != null && selectedCarBrand != null && selectedClient != null)
                {
                    var cars = AutoServiceContext.GetContext().Cars.Where(A=>A.Idclient==selectedClient.Idclient&&A.Idmodel==selectedModel.Idmodel).ToList();
                    StateNumbers = cars;
                    IsResetButtonEnable = true;
                    IsStateNumberEnable = true;
                }
            }
        }
        public bool IsAddingButtonEnable
        {
            get => isAddingButtonEnable;
            set
            {
                isAddingButtonEnable = value;
                OnPropertyChanged(nameof(IsAddingButtonEnable));
            }
        }
        public bool IsServiceTypeEnable
        {
            get => isServiceTypeEnable;
            set
            {
                isServiceTypeEnable = value;
                OnPropertyChanged(nameof(IsServiceTypeEnable));
            }
        }
        public bool IsStateNumberEnable
        {
            get => isStateNumberEnable;
            set
            {
                isStateNumberEnable = value;
                OnPropertyChanged(nameof(IsStateNumberEnable));
            }
        }
        public bool IsModelEnable
        {
            get => isModelEnable;
            set
            {
                isModelEnable = value;
                OnPropertyChanged(nameof(IsModelEnable));
            }
        }
        public bool IsBrandEnable
        {
            get => isBrandEnable;
            set
            {
                isBrandEnable = value;
                OnPropertyChanged(nameof(IsBrandEnable));
            }
        }
        public List<Car> StateNumbers
        {
            get => stateNumbers;
            set
            {
                stateNumbers = value;
                OnPropertyChanged(nameof(StateNumbers));
            }
        }
        public bool IsEnableEndButton
        {
            get => isEnableEndButton;
            set
            {
                isEnableEndButton = value;
                OnPropertyChanged(nameof(IsEnableEndButton));
            }
        }
        public bool IsResetButtonEnable
        {
            get => isResetButtonEnable;
            set
            {
                isResetButtonEnable = value;
                OnPropertyChanged(nameof(IsResetButtonEnable));
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
        public Car SelectedStateNumber
        {
            get => selectedStateNumber;
            set
            {
                selectedStateNumber = value;
                OnPropertyChanged(nameof(selectedStateNumber));
                if (selectedStateNumber != null)
                {
                    IsResetButtonEnable = true;
                    IsServiceTypeEnable = true;
                }
            }
        }
        public List<ServiceType> ServiceTypes
        {
            get => serviceTypes;
            set
            {
                serviceTypes = value;
                OnPropertyChanged(nameof(ServiceTypes));
            }
        }
        public ServiceType SelectedServiceType
        {
            get => selectedServiceType;
            set
            {
                selectedServiceType = value;
                OnPropertyChanged(nameof(SelectedServiceType));
                if (selectedServiceType == AutoServiceContext.GetContext().ServiceTypes.FirstOrDefault(A=>A.NameServiceType=="Ремонт") && selectedModel != null)
                {
                    Visibility = Visibility.Visible;
                    List<AutoPart> tmpParts = new List<AutoPart>();
                    var autoParts = AutoServiceContext.GetContext().Compatibilities.Where(A=>A.Idmodel==selectedModel.Idmodel).ToList();
                    foreach (var autoPart in autoParts)
                    {
                        tmpParts.Add(AutoServiceContext.GetContext().AutoParts.FirstOrDefault(A => A.IdautoPart == autoPart.IdautoPart));
                    }
                    AutoParts = tmpParts;
                    
                }
                else
                {
                    Visibility = Visibility.Hidden;
                }
                IsResetButtonEnable = true;
                IsReadOnly = true;
                IsClientEnable = false;
                IsBrandEnable = false;
                IsModelEnable = false;
                IsStateNumberEnable = false;
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
        public bool IsEnable
        {
            get => isEnable;
            set
            {
                isEnable = value;
                OnPropertyChanged(nameof(IsEnable));
            }
        }
        public bool IsClientEnable
        {
            get => isClientEnable;
            set
            {
                isClientEnable = value;
                OnPropertyChanged(nameof(IsClientEnable));
            }
        }
        public AutoPart SelectedAutoPart
        {
            get => selectedAutoPart;
            set
            {
                selectedAutoPart = value;
                OnPropertyChanged(nameof(SelectedAutoPart));
                if (selectedAutoPart != null) {
                    IsResetButtonEnable = true;
                    IsEnable = true;
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
                    MessageBox.Show("Выбранная дата не может быть больше сегодняшней.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    date = DateTime.Now;
                    return;
                }
            }
        }
        public Visibility Visibility
        {
            get => visibility;
            set
            {
                visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }
        public RelayCommand AddService
        {
            get
            {
                return addService ??
                      (addService = new RelayCommand((o) =>
                      {
                          StringBuilder errors = new StringBuilder();
                          if (SelectedClient == null)
                              errors.AppendLine("Выберите клиента.");
                          if (SelectedCarBrand == null)
                              errors.AppendLine("Выберите марку.");
                          if (SelectedModel == null)
                              errors.AppendLine("Выберите модель.");
                          if (SelectedStateNumber == null)
                              errors.AppendLine("Выберите гос.номер.");
                          if (Date.ToString() == "")
                              errors.AppendLine("Выберите дату.");
                          if (SelectedServiceType == null)
                              errors.AppendLine("Выберите тип обслуживания.");
                          if (errors.Length > 0)
                          {
                              MessageBox.Show(errors.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                              return;
                          }
                          if (selectedServiceType.NameServiceType == "Диагностика" || selectedServiceType.NameServiceType == "Консультация")
                          {
                              ServiceType serviceType = AutoServiceContext.GetContext().ServiceTypes.FirstOrDefault(A => A.IdserviceType == selectedServiceType.IdserviceType);
                              service = new AutoService() { StateNumber = selectedStateNumber.StateNumber, Idworker = Navigation.MasterNavigation.UserId, DateAutoService = Date, IdserviceType = selectedServiceType.IdserviceType, Price = (serviceType.PriceServiceType * Convert.ToDecimal(1.2)) };
                              AutoServiceContext.GetContext().AutoServices.Add(service);
                              try
                              {
                                  AutoServiceContext.GetContext().SaveChanges();
                                  MessageBox.Show("Информация сохранена!");
                                  service = null;
                                  SetProperties();
                                  AutoServices = displayAutoServices;
                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message.ToString());
                              }
                          }
                          else
                          {
                              ServiceType serviceType = AutoServiceContext.GetContext().ServiceTypes.FirstOrDefault(A => A.IdserviceType == selectedServiceType.IdserviceType);
                              service = new AutoService() { StateNumber = selectedStateNumber.StateNumber, Idworker = Navigation.MasterNavigation.UserId, DateAutoService = Date, IdserviceType = selectedServiceType.IdserviceType, Price = (serviceType.PriceServiceType * Convert.ToDecimal(1.2)) };
                              AutoServiceContext.GetContext().AutoServices.Add(service);
                              try
                              {
                                  AutoServiceContext.GetContext().SaveChanges();
                                  MessageBox.Show("Информация о данном ремонте сохранена! Пожалуйста, выберите запчасти, которые требуются на ремонт и добавьте их в учет для данного ремонта.","Sucess",MessageBoxButton.OK,MessageBoxImage.Information);
                                  IsEnable = true;
                                  IsAddingButtonEnable = false;
                              }
                              catch (Exception ex)
                              {
                                MessageBox.Show(ex.Message.ToString());
                              }
                          }
                          ResetAll();


                      }
                       ));
            }
        }
        public RelayCommand AddAutoPartToService
        {
            get
            {
                return addAutoPartToService ??
                      (addAutoPartToService = new RelayCommand((o) =>
                      {
                          if (selectedAutoPart == null) 
                          {
                              MessageBox.Show("Выберите запчасть.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                              return;
                          }
                          
                          AutoServiceAutoPart autoServiceAutoPart = new AutoServiceAutoPart() { IdautoPart = selectedAutoPart.IdautoPart, IdautoService = service.IdautoService };
                          
                          if (AutoServiceContext.GetContext().AutoServiceAutoParts.FirstOrDefault(A => A == autoServiceAutoPart) != null)
                          {
                              MessageBox.Show("Данная информация уже есть!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                              return;
                          }
                          else
                          {
                              DateTime dateForPrice = AutoServiceContext.GetContext().AutoPartPrices.Where(A => A.IdautoPart == selectedAutoPart.IdautoPart).Max(A=>A.DateChange);
                              priceForRepairs += AutoServiceContext.GetContext().AutoPartPrices.FirstOrDefault(A => A.DateChange == dateForPrice && A.IdautoPart == selectedAutoPart.IdautoPart).PriceWithoutRepair;
                              AutoServiceContext.GetContext().AutoServiceAutoParts.Add(autoServiceAutoPart);
                              try
                              {
                                  AutoServiceContext.GetContext().SaveChanges();
                                  MessageBox.Show("Информация сохранена!");
                                  IsEnableEndButton=true;
                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message.ToString());
                              }
                          }
                      }));
            }
                       
        }
        public RelayCommand EndAddingAutoPartToService
        {
            get
            {
                return endAddingAutoPartToService ?? (endAddingAutoPartToService = new RelayCommand((o) => 
                {
                    AutoService tmp = AutoServiceContext.GetContext().AutoServices.FirstOrDefault(A => A.IdautoService == service.IdautoService);
                    tmp.Price = ((priceForRepairs + selectedServiceType.PriceServiceType) * Convert.ToDecimal(1.2));
                    AutoServiceContext.GetContext().AutoServices.Update(tmp);
                    try
                    {
                        AutoServiceContext.GetContext().SaveChanges();
                        MessageBox.Show("Информация сохранена!");
                        service = null;
                        priceForRepairs = 0;
                        SetProperties();
                        AutoServices = displayAutoServices;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                    ResetAll();
                }));
            }
        }
        public RelayCommand ResetCommand
        {
            get
            {
                return resetCommand ?? (resetCommand = new RelayCommand((o) =>
                {
                    ResetAll();
                }));
            }
        }
    }
}
