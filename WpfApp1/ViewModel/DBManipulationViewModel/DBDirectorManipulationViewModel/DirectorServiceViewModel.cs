using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBDirectorManipulationViewModel
{
    class DirectorServiceViewModel:BaseViewModel
    {
        List<AutoService> autoServices;
        List<AutoService> displayAutoServices;
        List<Client> clients = AutoServiceContext.GetContext().Clients.ToList();
        List<Worker> workers = AutoServiceContext.GetContext().Workers.Where(A => A.Idposition == AutoServiceContext.GetContext().Positions.FirstOrDefault(A => A.NamePosition == "Мастер").Idposition).ToList();
        Client selectedClient;
        string nameClientFilter;
        string nameMasterFilter;
        bool isResetButtonEnable = false;
        public DirectorServiceViewModel()
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
                    List<AutoService> tmpAutoServices = new List<AutoService>();
                    var cars = AutoServiceContext.GetContext().Cars.Where(A => A.Idclient == selectedClient.Idclient).ToList();
                    foreach (var tmp in cars)
                    {
                        tmpAutoServices.Add(AutoServiceContext.GetContext().AutoServices.FirstOrDefault(A => A.StateNumber == tmp.StateNumber));
                    }
                    AutoServices = tmpAutoServices;
                    IsResetButtonEnable = true;
                }
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
        public string NameMasterFilter
        {
            get => nameMasterFilter;
            set
            {
                nameMasterFilter = value;
                OnPropertyChanged(nameof(NameMasterFilter));
                if (nameMasterFilter != null)
                    Workers = AutoServiceContext.GetContext().Workers.Where(A => A.NameWorker.ToLower().Contains(nameMasterFilter.ToLower())&& A.Idposition == AutoServiceContext.GetContext().Positions.FirstOrDefault(A => A.NamePosition == "Мастер").Idposition).ToList();
            }
        }
        public List<Worker> Workers
        {
            get => workers;
            set
            {
                workers = value;
                OnPropertyChanged(nameof(Workers));
            }
        }
    }
}
