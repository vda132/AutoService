using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBDirectorManipulationViewModel
{
    class MasterAutoServiceServiceViewModel : BaseViewModel
    {
        List<AutoService> autoServices;
        List<AutoService> displayAutoServices;
        List<Worker> workers = new List<Worker>();
        Worker selectedWorker;
        string nameMasterFilter;
        bool isResetButtonEnable = false;
        RelayCommand resetAllCommand;
        public MasterAutoServiceServiceViewModel()
        {
            SetProperties();
        }
        private void SetProperties()
        {
            using (var context = new AutoServiceContext())
            {
                workers = context.Workers.Where(A => A.Idposition == context.Positions.FirstOrDefault(A => A.NamePosition == "Мастер").Idposition).ToList();
                autoServices = context.AutoServices.ToList();
                var workers_ = context.Workers.ToList();
                var cars = context.Cars.ToList();
                var service = context.ServiceTypes.ToList();
                var models = context.Models.ToList();
                var clients = context.Clients.ToList();
                var brands = context.CarBrands.ToList();
                foreach (var element in autoServices)
                {

                    element.IdworkerNavigation = workers_.FirstOrDefault(A => A.Idworker == element.Idworker);
                    element.IdserviceTypeNavigation = service.FirstOrDefault(A => A.IdserviceType == element.IdserviceType);
                    element.StateNumberNavigation = cars.FirstOrDefault(A => A.StateNumber == element.StateNumber);
                    element.StateNumberNavigation.IdmodelNavigation = models.FirstOrDefault(A => A.Idmodel == element.StateNumberNavigation.Idmodel);
                    element.StateNumberNavigation.IdclientNavigation = clients.FirstOrDefault(A => A.Idclient == element.StateNumberNavigation.Idclient);
                    element.StateNumberNavigation.IdmodelNavigation.IdcarBrandNavigation = brands.FirstOrDefault(A => A.IdcarBrand == element.StateNumberNavigation.IdmodelNavigation.IdcarBrand);
                }
                displayAutoServices = autoServices;
                AutoServices = displayAutoServices;
                Workers = workers;
            }
        }
        private void ResetAll()
        {
            SetProperties();
            AutoServices = displayAutoServices;
            SelectedWorker = null;
            NameMasterFilter = null;
            IsResetButtonEnable = false;

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
        public bool IsResetButtonEnable
        {
            get => isResetButtonEnable;
            set
            {
                isResetButtonEnable = value;
                OnPropertyChanged(nameof(IsResetButtonEnable));
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
                {
                    using (var context = new AutoServiceContext())
                    {
                        Workers = context.Workers.Where(A => A.NameWorker.ToLower().Contains(nameMasterFilter.ToLower()) && A.Idposition == context.Positions.FirstOrDefault(A => A.NamePosition == "Мастер").Idposition).ToList();
                        IsResetButtonEnable = true;
                    }
                }
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
        public Worker SelectedWorker
        {
            get => selectedWorker;
            set
            {
                selectedWorker = value;
                OnPropertyChanged(nameof(SelectedWorker));
                if (selectedWorker != null)
                {
                    using (var context = new AutoServiceContext())
                    {
                        var tmp = context.AutoServices.Where(A => A.Idworker == selectedWorker.Idworker).ToList();
                        foreach (var info in tmp)
                        {
                            info.IdserviceTypeNavigation = context.ServiceTypes.FirstOrDefault(A => A.IdserviceType == info.IdserviceType);
                            info.IdworkerNavigation = context.Workers.FirstOrDefault(A => A.Idworker == info.Idworker);
                            info.StateNumberNavigation = context.Cars.FirstOrDefault(A => A.StateNumber == info.StateNumber);
                            info.StateNumberNavigation.IdmodelNavigation = context.Models.FirstOrDefault(A => A.Idmodel == info.StateNumberNavigation.Idmodel);
                            info.StateNumberNavigation.IdclientNavigation = context.Clients.FirstOrDefault(A => A.Idclient == info.StateNumberNavigation.Idclient);
                            info.StateNumberNavigation.IdmodelNavigation.IdcarBrandNavigation = context.CarBrands.FirstOrDefault(A => A.IdcarBrand == info.StateNumberNavigation.IdmodelNavigation.IdcarBrand);
                        }
                        AutoServices = tmp;
                        IsResetButtonEnable = true;
                    }
                }
            }
        }
        public RelayCommand ResetAllCommand
        {
            get
            {
                return resetAllCommand ?? (resetAllCommand = new RelayCommand((o) =>
                {
                    ResetAll();
                }));
            }
        }
    }
}
