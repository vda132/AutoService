using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel
{
    class DBAdminAutoPartViewModel : BaseViewModel
    {
        Country tmp;
        List<Country> countries = new List<Country>();
        private string autoPartName;
        RelayCommand addAutoPart;
        RelayCommand resetAll;
        RelayCommand editAutoPart;
        List<AutoPart> autoParts;
        List<AutoPart> displayAutoParts;
        AutoPart selectedAutoPart;
        bool isEnable = false;
        bool isAddButtonEnable = true;
        bool isResetButtonEnable = false;
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
            using (var context = new AutoServiceContext())
            {
                countries = context.Countries.ToList();
                autoParts = context.AutoParts.ToList();
                var _countries = context.Countries.ToList();
                foreach (var country in autoParts)
                {
                    country.IdcountryNavigation = _countries.FirstOrDefault(A => A.Idcountry == country.Idcountry);
                }
                AutoParts = autoParts;
            }
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
                if (selectedAutoPart != null)
                {
                    IsEnable = true;
                    AutoPartName = SelectedAutoPart.NameAutoPart;
                    SelectedCountry = SelectedAutoPart.IdcountryNavigation;
                    IsAddButtonEnable = false;
                    IsResetButtonEnable = true;
                }
            }
        }
        public bool IsAddButtonEnable
        {
            get => isAddButtonEnable;
            set
            {
                isAddButtonEnable = value;
                OnPropertyChanged(nameof(IsAddButtonEnable));
            }
        }
        public bool IsResetButtonEnable
        {
            get => isResetButtonEnable;
            set
            {
                isResetButtonEnable = value;
                OnPropertyChanged(nameof(IsResetButtonEnable));
            }
        }

        public RelayCommand EditAutoPart
        {
            get
            {
                return editAutoPart ??
                      (editAutoPart = new RelayCommand((o) =>
                      {
                          using (var context = new AutoServiceContext())
                          {
                              if (MessageBox.Show($"Вы точно хотите редактировать выбранную деталь под названием " +
                                  $"{SelectedAutoPart.NameAutoPart}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                              {
                                  try
                                  {
                                      AutoPart tmp = context.AutoParts.FirstOrDefault(A => A.IdautoPart == SelectedAutoPart.IdautoPart);
                                      tmp.NameAutoPart = AutoPartName;
                                      tmp.IdcountryNavigation = SelectedCountry;
                                      context.AutoParts.Update(tmp);
                                      MessageBox.Show("Данные обновлены.");
                                      context.SaveChanges();


                                  }
                                  catch (Exception ex)
                                  {
                                      MessageBox.Show(ex.Message);
                                  }
                                  SetProperties();
                                  AutoParts = displayAutoParts;
                                  IsEnable = false;
                                  AutoPartName = null;
                                  SelectedCountry = null;
                                  IsAddButtonEnable = true;
                                  IsResetButtonEnable = false;
                              }
                          }
                      }));
            }
        }

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
                if (selectedCountry != null)
                {
                    IsResetButtonEnable = true;
                }
            }
        }
        public string AutoPartName
        {
            get => autoPartName;
            set
            {
                autoPartName = value;
                OnPropertyChanged(nameof(AutoPartName));
                if (autoPartName != null)
                {
                    IsResetButtonEnable = true;
                }
            }
        }
        public RelayCommand AddAutoPart
        {
            get
            {
                return addAutoPart ??
                      (addAutoPart = new RelayCommand((o) =>
                      {
                          using (var context = new AutoServiceContext())
                          {
                              StringBuilder errors = new StringBuilder();
                              if (String.IsNullOrWhiteSpace(autoPartName))
                                  errors.AppendLine("Укажите название запчасти.");
                              if (selectedCountry == null)
                                  errors.AppendLine("Укажите страну производитель.");
                              if ((context.AutoParts.FirstOrDefault(A => A.NameAutoPart == AutoPartName)) != null)
                                  errors.AppendLine("Такая запчасть уже существует.");
                              if (errors.Length > 0)
                              {
                                  MessageBox.Show(errors.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }

                              tmp = countries.FirstOrDefault(A => A.NameCountry == selectedCountry.NameCountry);
                              int id = tmp.Idcountry;
                              AutoPart tmpPart = new AutoPart() { NameAutoPart = autoPartName, Idcountry = id };

                              context.AutoParts.Add(tmpPart);
                              try
                              {
                                  context.SaveChanges();
                                  MessageBox.Show("Информация сохранена!");
                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message.ToString());
                              }
                              SetProperties();
                              AutoParts = displayAutoParts;
                              AutoPartName = null;
                              SelectedCountry = null;
                              IsResetButtonEnable = false;
                          }
                      }
                       ));
            }
        }
        public RelayCommand ResetAll
        {
            get
            {
                return resetAll ??
                      (resetAll = new RelayCommand((o) =>
                      {
                          SetProperties();
                          AutoParts = displayAutoParts;
                          AutoPartName = null;
                          SelectedCountry = null;
                          IsResetButtonEnable = false;
                          IsAddButtonEnable = true;
                          IsEnable = false;
                      }
                       ));
            }
        }

    }
}
