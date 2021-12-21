using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Navigation;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel
{
    class MasterViewModel : BaseViewModel
    {
        RelayCommand clientButtonCommnad;
        RelayCommand autoServiceButtonCommnad;
        RelayCommand reportButton;
        RelayCommand backButtonCommand;
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
        public RelayCommand BackButtonCommand
        {
            get
            {
                return backButtonCommand ??
                      (backButtonCommand = new RelayCommand((o) =>
                      {
                          if (Navigation.MasterNavigation.CurrentViewModel.GetType() == new MasterViewModel().GetType() && MessageBox.Show($"Вы точно хотите вернуться на страницу авторизации? ", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                          {
                              Navigation.Navigation.ToLogin();
                          }
                          else if (Navigation.MasterNavigation.CurrentViewModel.GetType() != new MasterViewModel().GetType())
                          {
                              Navigation.MasterNavigation.ToPreviuosViewModel();
                          }
                          else
                          {
                              return;
                          }
                      }));
            }
        }
    }
}
