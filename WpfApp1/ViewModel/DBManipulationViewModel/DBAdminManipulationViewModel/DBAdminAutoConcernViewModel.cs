using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel
{
    class DBAdminAutoConcernViewModel:BaseViewModel
    {
        List<AutoConcern> autoConcerns;
        List<AutoConcern> displayAutoConcern;
        AutoConcern selectedAutoConcern;
        bool isEnable=false;
        public bool IsEnable
        {
            get => isEnable;
            set
            {
                isEnable = value;
                OnPropertyChanged(nameof(IsEnable));
            }
        }
        public DBAdminAutoConcernViewModel()
        {
            SetProperties();
        }
        private void SetProperties()
        {
            autoConcerns = AutoServiceContext.GetContext().AutoConcerns.ToList();
            var countries = AutoServiceContext.GetContext().Countries.ToList();
            foreach (var concern in autoConcerns)
            {
                concern.IdcountryNavigation = countries.FirstOrDefault(A => A.Idcountry == concern.Idcountry);
            }
            displayAutoConcern = autoConcerns;
        }
        public List<AutoConcern> AutoConcerns
        {
            get => displayAutoConcern;
            set
            {
                displayAutoConcern = value;
                OnPropertyChanged(nameof(AutoConcerns));
            }
        }

        public AutoConcern SelectedAutoConcern
        {
            get => selectedAutoConcern;
            set
            {
                selectedAutoConcern = value;
                OnPropertyChanged(nameof(SelectedAutoConcern));
                IsEnable = true;
            }
        }

        RelayCommand toAddingAutoconcern;
        public RelayCommand ToAddingAutoconcern
        {
            get
            {
                return toAddingAutoconcern ??
                      (toAddingAutoconcern = new RelayCommand((o) =>
                      {
                          Navigation.DBAdminNavigation.ToAddingAutoConcern();
                      }
                       ));
            }
        }
        RelayCommand deletingAutoconcern;
        public RelayCommand DeletingAutoconcern
        {
            get
            {
                return deletingAutoconcern ??
                      (deletingAutoconcern = new RelayCommand((o) =>
                      {
                          if (MessageBox.Show($"Вы точно хотите удалить выбранный автоконцерн под названием " +
                              $"{SelectedAutoConcern.NameAutoConcern}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                          {
                              try
                              {
                                  AutoServiceContext.GetContext().AutoConcerns.Remove(SelectedAutoConcern);
                                  MessageBox.Show("Данные удалены.");
                                  AutoServiceContext.GetContext().SaveChanges();
                                  autoConcerns = AutoServiceContext.GetContext().AutoConcerns.ToList();
                                  AutoConcerns = autoConcerns;

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
