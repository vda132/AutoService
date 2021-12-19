using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBMasterManipulationViewModel
{
    class MasterReportViewModel : BaseViewModel
    {
        List<AutoServiceAutoPart> autoServiceAutoParts = new List<AutoServiceAutoPart>();
        List<AutoServiceAutoPart> displayAutoServiceAutoParts;
        List<Client> clients;
        List<Car> stateNumbers;
        Car selectedStateNumber;
        Client selectedClient;
        string numberService;
        RelayCommand resetCommand;
        int number;
        bool isEnable;

        public MasterReportViewModel()
        {
            using (var context = new AutoServiceContext())
            {
                clients = context.Clients.ToList();
            }
        }
        public string NumberService
        {
            get => numberService;
            set
            {
                numberService = value;
                OnPropertyChanged(nameof(NumberService));
                if (numberService != null)
                    IsEnable = true;
            }
        }
        public List<AutoServiceAutoPart> DisplayAutoServiceAutoParts
        {
            get => displayAutoServiceAutoParts;
            set
            {
                displayAutoServiceAutoParts = value;
                OnPropertyChanged(nameof(DisplayAutoServiceAutoParts));
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
        public List<Car> StateNumbers
        {
            get => stateNumbers;
            set
            {
                stateNumbers = value;
                OnPropertyChanged(nameof(StateNumbers));
            }
        }
        public Car SelectedStateNumber
        {
            get => selectedStateNumber;
            set
            {
                selectedStateNumber = value;
                OnPropertyChanged(nameof(SelectedStateNumber));
                if (selectedStateNumber != null)
                {
                    using (var context = new AutoServiceContext())
                    {
                        autoServiceAutoParts = new List<AutoServiceAutoPart>();
                        var autoService = context.AutoServices.Where(A => A.StateNumber == selectedStateNumber.StateNumber&&A.IdserviceType==context.ServiceTypes.FirstOrDefault(A=>A.NameServiceType=="Ремонт").IdserviceType).ToList();
                        if (autoService == null)
                        {
                            MessageBox.Show($"Клиент с ФИО {selectedClient.NameClient} не проходил ремонтов.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                            ResetAll();
                            return;
                        }
                        foreach (var service in autoService)
                        {
                            autoServiceAutoParts.Add(context.AutoServiceAutoParts.FirstOrDefault(A => A.IdautoService == service.IdautoService));
                        }
                        foreach (var tmp in autoServiceAutoParts)
                        {
                            tmp.IdautoPartNavigation = context.AutoParts.FirstOrDefault(A => A.IdautoPart == tmp.IdautoPart);
                            tmp.IdautoServiceNavigation = context.AutoServices.FirstOrDefault(A => A.IdautoService == tmp.IdautoService);
                            tmp.IdautoServiceNavigation.StateNumberNavigation = context.Cars.FirstOrDefault(A => A.StateNumber == tmp.IdautoServiceNavigation.StateNumber);
                            tmp.IdautoServiceNavigation.StateNumberNavigation.IdclientNavigation = context.Clients.FirstOrDefault(A => A.Idclient == tmp.IdautoServiceNavigation.StateNumberNavigation.Idclient);
                            tmp.IdautoServiceNavigation.StateNumberNavigation.IdmodelNavigation = context.Models.FirstOrDefault(A => A.Idmodel == tmp.IdautoServiceNavigation.StateNumberNavigation.Idmodel);
                            tmp.IdautoServiceNavigation.StateNumberNavigation.IdmodelNavigation.IdcarBrandNavigation = context.CarBrands.FirstOrDefault(A => A.IdcarBrand == tmp.IdautoServiceNavigation.StateNumberNavigation.IdmodelNavigation.IdcarBrand);
                        }
                        DisplayAutoServiceAutoParts = autoServiceAutoParts;
                    }
                }
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
                    using (var context = new AutoServiceContext())
                    {
                        IsEnable = true;
                        StateNumbers = context.Cars.Where(A => A.Idclient == selectedClient.Idclient).ToList();
                    }
                }
            }
        }
        
        private void ResetAll()
        {
            NumberService = null;
            autoServiceAutoParts = new List<AutoServiceAutoPart>();
            DisplayAutoServiceAutoParts = null;
            IsEnable = false;
            SelectedClient = null;
            SelectedStateNumber = null;
            StateNumbers = null;
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
        public RelayCommand ResetCommand
        {
            get
            {
                return resetCommand ??
                     (resetCommand = new RelayCommand((o) =>
                     {
                         ResetAll();
                     }));
            }
        }
    }
}
