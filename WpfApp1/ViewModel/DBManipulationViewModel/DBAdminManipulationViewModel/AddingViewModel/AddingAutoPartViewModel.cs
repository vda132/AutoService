using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel.AddingViewModel
{
    class AddingAutoPartViewModel:BaseViewModel
    {
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
                              errors.AppendLine("Укажите название автоконцерна.");
                          if (selectedCountry == null)
                              errors.AppendLine("Укажите страну автоконцерна.");
                          if (errors.Length > 0)
                          {
                              MessageBox.Show(errors.ToString());
                              return;
                          }

                          tmp = countries.FirstOrDefault(A => A.NameCountry == selectedCountry.NameCountry);
                          int id = tmp.Idcountry;
                          AutoPart tmpPart = new AutoPart() {  NameAutoPart= autoPartName, Idcountry = id };

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
                      }
                       ));
            }
        }
    }
}

