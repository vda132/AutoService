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
                if (selectedAutoConcern != null)
                {
                    IsEnable = true;
                    AutoConcernName = SelectedAutoConcern.NameAutoConcern;
                    SelectedCountry = SelectedAutoConcern.IdcountryNavigation;
                }
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
                                  IsEnable = false;
                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message);
                              }
                              AutoConcerns = displayAutoConcern;
                              AutoConcernName = null;
                              SelectedCountry = null;
                          }
                      }));
            }
        }
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
                              MessageBox.Show(errors.ToString(),"Error",MessageBoxButton.OK,MessageBoxImage.Error);
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
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message.ToString());
                          }
                          autoConcerns = AutoServiceContext.GetContext().AutoConcerns.ToList();
                          SetProperties();
                          AutoConcerns = displayAutoConcern;
                          AutoConcernName = null;
                          SelectedCountry = null;
                      }
                       ));
            }
        }
        RelayCommand editConcern;
        public RelayCommand EditConcern
        {
            get
            {
                return editConcern ??
                      (editConcern = new RelayCommand((o) =>
                      {
                          if (MessageBox.Show($"Вы точно хотите редактировать выбранный автоконцерн под названием " +
                              $"{SelectedAutoConcern.NameAutoConcern}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                          {
                              try
                              {
                                  AutoConcern tmp = AutoServiceContext.GetContext().AutoConcerns.FirstOrDefault(A => A.IdautoConcern == SelectedAutoConcern.IdautoConcern);
                                  tmp.NameAutoConcern = AutoConcernName;
                                  tmp.IdcountryNavigation = SelectedCountry;
                                  AutoServiceContext.GetContext().AutoConcerns.Update(tmp);
                                  MessageBox.Show("Данные обновлены.");
                                  AutoServiceContext.GetContext().SaveChanges();


                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message);
                              }
                              SetProperties();
                              AutoConcerns = displayAutoConcern;
                              IsEnable = false;
                              AutoConcernName = null;
                              SelectedCountry = null;
                          }
                      }));
            }
        }
    }
}
