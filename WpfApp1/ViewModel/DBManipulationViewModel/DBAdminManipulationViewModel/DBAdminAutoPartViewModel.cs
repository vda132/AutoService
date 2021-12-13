using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel
{
    class DBAdminAutoPartViewModel:BaseViewModel
    {
        List<AutoPart> autoParts;
        List<AutoPart> displayAutoParts;
        AutoPart selectedAutoPart;
        bool isEnable = false;
        public bool IsEnable
        {
            get => isEnable;
            set
            {
                isEnable = value;
                OnPropertyChanged(nameof(IsEnable));
            }
        }
        public DBAdminAutoPartViewModel()
        {
            SetProperties();
        }
        private void SetProperties()
        {
            autoParts = AutoServiceContext.GetContext().AutoParts.ToList();
            var countries = AutoServiceContext.GetContext().Countries.ToList();
            foreach (var country in autoParts)
            {
                country.IdcountryNavigation = countries.FirstOrDefault(A => A.Idcountry == country.Idcountry);
            }
            displayAutoParts = autoParts;
        }
        public List<AutoPart> AutoParts
        {
            get => displayAutoParts;
            set
            {
                displayAutoParts = value;
                OnPropertyChanged(nameof(AutoParts));
            }
        }

        public AutoPart SelectedAutoPart
        {
            get => selectedAutoPart;
            set
            {
                selectedAutoPart = value;
                OnPropertyChanged(nameof(SelectedAutoPart));
                IsEnable = true;
            }
        }

        RelayCommand toAddingAutoPart;
        public RelayCommand ToAddingAutoPart
        {
            get
            {
                return toAddingAutoPart ??
                      (toAddingAutoPart = new RelayCommand((o) =>
                      {
                          Navigation.DBAdminNavigation.ToAddingAutoPart();
                      }
                       ));
            }
        }
        RelayCommand deletingAutoPart;
        public RelayCommand DeletingAutoPart
        {
            get
            {
                return deletingAutoPart ??
                      (deletingAutoPart = new RelayCommand((o) =>
                      {
                          if (MessageBox.Show($"Вы точно хотите удалить выбранную деталь под названием " +
                              $"{SelectedAutoPart.NameAutoPart}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                          {
                              try
                              {
                                  AutoServiceContext.GetContext().AutoParts.Remove(SelectedAutoPart);
                                  MessageBox.Show("Данные удалены.");
                                  AutoServiceContext.GetContext().SaveChanges();
                                  autoParts = AutoServiceContext.GetContext().AutoParts.ToList();
                                  AutoParts = autoParts;

                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message);
                              }
                          }
                      }));
            }
        }
    }
}
