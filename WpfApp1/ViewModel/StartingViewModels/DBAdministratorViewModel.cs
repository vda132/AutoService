using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel
{
    class DBAdministratorViewModel : BaseViewModel
    {
        RelayCommand autoConcernButtonCommnad;
        RelayCommand autoPartModelButtonCommnad;
        RelayCommand autoPartPriceButtonCommnad;
        RelayCommand autoPartButtonCommnad;
        RelayCommand brandButtonCommnad;
        RelayCommand modelButtonCommnad;
        public RelayCommand AutoConcernButtonCommnad
        {
            get
            {
                return autoConcernButtonCommnad ??
                      (autoConcernButtonCommnad = new RelayCommand((o) =>
                      {
                          Navigation.DBAdminNavigation.ToAutoConcern();
                      }));
            }
        }

        public RelayCommand AutoPartModelButtonCommnad
        {
            get
            {
                return autoPartModelButtonCommnad ??
                      (autoPartModelButtonCommnad = new RelayCommand((o) =>
                      {
                          Navigation.DBAdminNavigation.ToAutoPartModel();
                      }));
            }
        }
        public RelayCommand AutoPartPriceButtonCommnad
        {
            get
            {
                return autoPartPriceButtonCommnad ??
                      (autoPartPriceButtonCommnad = new RelayCommand((o) =>
                      {
                          Navigation.DBAdminNavigation.ToAutoPartPrice();
                      }));
            }
        }
        public RelayCommand AutoPartButtonCommnad
        {
            get
            {
                return autoPartButtonCommnad ??
                      (autoPartButtonCommnad = new RelayCommand((o) =>
                      {
                          Navigation.DBAdminNavigation.ToAutoPart();
                      }));
            }
        }
        public RelayCommand BrandButtonCommnad
        {
            get
            {
                return brandButtonCommnad ??
                      (brandButtonCommnad = new RelayCommand((o) =>
                      {
                          Navigation.DBAdminNavigation.ToBrand();
                      }));
            }
        }
        public RelayCommand ModelButtonCommnad
        {
            get
            {
                return modelButtonCommnad ??
                      (modelButtonCommnad = new RelayCommand((o) =>
                      {
                          Navigation.DBAdminNavigation.ToModel();
                      }));
            }
        }

    }
}
