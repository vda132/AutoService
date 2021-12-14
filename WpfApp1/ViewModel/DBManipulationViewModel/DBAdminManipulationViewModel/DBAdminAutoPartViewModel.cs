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
                AutoPartName = SelectedAutoPart.NameAutoPart;
                SelectedCountry = SelectedAutoPart.IdcountryNavigation;
            }
        }

        RelayCommand editAutoPart;
        public RelayCommand EditAutoPart
        {
            get
            {
                return editAutoPart ??
                      (editAutoPart = new RelayCommand((o) =>
                      {
                          if (MessageBox.Show($"Вы точно хотите редактировать выбранную деталь под названием " +
                              $"{SelectedAutoPart.NameAutoPart}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                          {
                              try
                              {
                                  AutoPart tmp = AutoServiceContext.GetContext().AutoParts.FirstOrDefault(A => A.IdautoPart == SelectedAutoPart.IdautoPart);
                                  tmp.NameAutoPart = AutoPartName;
                                  tmp.IdcountryNavigation = SelectedCountry;
                                  AutoServiceContext.GetContext().AutoParts.Update(tmp);
                                  MessageBox.Show("Данные обновлены.");
                                  AutoServiceContext.GetContext().SaveChanges();


                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message);
                              }
                              SetProperties();
                              AutoParts = displayAutoParts;
                              IsEnable = false;
                              AutoPartName = null;
                              selectedCountry = null;
                          }
                      }));
            }
        }
        Country tmp;
        List<Country> countries = AutoServiceContext.GetContext().Countries.ToList();
        private string autoPartName;
        RelayCommand addAutoPart;
        public List<Country> CountryNames
        {
            get => countries;
            set
            {
                countries = value;
                OnPropertyChanged(nameof(CountryNames));
            }
        }
        private Country selectedCountry;
        public Country SelectedCountry
        {
            get => selectedCountry;
            set
            {
                selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
            }
        }
        public string AutoPartName
        {
            get => autoPartName;
            set
            {
                autoPartName = value;
                OnPropertyChanged(nameof(AutoPartName));
            }
        }
        public RelayCommand AddAutoPart
        {
            get
            {
                return addAutoPart ??
                      (addAutoPart = new RelayCommand((o) =>
                      {
                          StringBuilder errors = new StringBuilder();
                          if (String.IsNullOrWhiteSpace(autoPartName))
                              errors.AppendLine("Укажите название запчасти.");
                          if (selectedCountry == null)
                              errors.AppendLine("Укажите страну производитель.");
                          if (errors.Length > 0)
                          {
                              MessageBox.Show(errors.ToString());
                              return;
                          }

                          tmp = countries.FirstOrDefault(A => A.NameCountry == selectedCountry.NameCountry);
                          int id = tmp.Idcountry;
                          AutoPart tmpPart = new AutoPart() { NameAutoPart = autoPartName, Idcountry = id };

                          AutoServiceContext.GetContext().AutoParts.Add(tmpPart);
                          try
                          {
                              AutoServiceContext.GetContext().SaveChanges();
                              MessageBox.Show("Информация сохранена!");
                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message.ToString());
                          }
                          SetProperties();
                          AutoParts = displayAutoParts;
                          AutoPartName = null;
                          selectedCountry = null;
                      }
                       ));
            }
        }
    }
}
