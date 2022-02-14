using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel
{
    class DirectorViewModel : BaseViewModel
    {
        RelayCommand directorWorkertButtonCommnad;
        RelayCommand autoServiceButtonCommnad;
        RelayCommand reportButtonCommnad;
        public RelayCommand DirectorWorkertButtonCommnad
        {
            get
            {
                return directorWorkertButtonCommnad ??
                      (directorWorkertButtonCommnad = new RelayCommand((o) =>
                      {
                          Navigation.DirectorNavigation.ToDirectorWorkers();
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
                          Navigation.DirectorNavigation.ToDirectorService();
                      }));
            }
        }
        public RelayCommand ReportButtonCommnad
        {
            get
            {
                return reportButtonCommnad ??
                      (reportButtonCommnad = new RelayCommand((o) =>
                      {
                          Navigation.DirectorNavigation.ToDirectorReport();
                      }));
            }
        }
    }
}
