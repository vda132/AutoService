using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Navigation;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel
{
    class MasterViewModel : BaseViewModel
    {
        RelayCommand clientButtonCommnad;
        RelayCommand autoServiceButtonCommnad;
        RelayCommand reportButton;
        public RelayCommand ClientButtonCommnad
        {
            get
            {
                return clientButtonCommnad ??
                      (clientButtonCommnad = new RelayCommand((o) =>
                      {
                          MasterNavigation.ToClientMaster();
                      }));
            }
        }
        public RelayCommand AutoServiceButtonCommnad
        {
            get
            {
                return autoServiceButtonCommnad ??
                      (autoServiceButtonCommnad = new RelayCommand((o) =>
                      {
                          MasterNavigation.ToMasterAutoService();
                      }));
            }
        }
        public RelayCommand ReportButton
        {
            get
            {
                return reportButton ??
                      (reportButton = new RelayCommand((o) =>
                      {
                          MasterNavigation.ToReport();
                      }));
            }
        }
    }
}
