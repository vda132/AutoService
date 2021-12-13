using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel.AddingViewModel
{
    class AddingAutoConcernViewModel:BaseViewModel
    {
        Country tmp;
        List<Country> countries = AutoServiceContext.GetContext().Countries.ToList();
        private string autoConcernName;
        RelayCommand addAutoConcern;
        public List<Country> CountryNames
        {
            get => countries;
            set
            {
                countries = value;
                OnPropertyChanged("CountryNames");
            }
        }
        private Country selectedCountry;
        public Country SelectedCountry
        {
            get => selectedCountry;
            set
            {
                selectedCountry = value;
                OnPropertyChanged("SelectedCountry");
            }
        }
        public string AutoConcernName
        {
            get => autoConcernName;
            set
            {
                autoConcernName = value;
                OnPropertyChanged("AutoConcernName");
            }
        }
        public RelayCommand AddAutoConcern
        {
            get
            {
                return addAutoConcern ??
                      (addAutoConcern = new RelayCommand((o) =>
                      {
                          StringBuilder errors = new StringBuilder();
                          if (String.IsNullOrWhiteSpace(autoConcernName))
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
                          AutoConcern tmpCon = new AutoConcern { NameAutoConcern = autoConcernName, Idcountry = id };
                          
                          AutoServiceContext.GetContext().AutoConcerns.Add(tmpCon);
                          try
                          {
                              AutoServiceContext.GetContext().SaveChanges();
                              MessageBox.Show("Информация сохранена!");
                             
                          }
                          catch(Exception ex)
                          {
                              MessageBox.Show(ex.Message.ToString());
                          }
                      }
                       ));
            }
        }
    }
}
