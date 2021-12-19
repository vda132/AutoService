using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel
{
    class DBAdminAutoConcernViewModel : BaseViewModel
    {
        RelayCommand deletingAutoconcern;
        List<AutoConcern> autoConcerns;
        private string autoConcernName;
        RelayCommand addAutoConcern;
        RelayCommand editConcern;
        private Country selectedCountry;
        List<AutoConcern> displayAutoConcern;
        AutoConcern selectedAutoConcern;
        bool isEnable = false;
        bool isAddButtonEnable = true;
        bool isResetButtonEnable = false;
        Country tmp;
        List<Country> countries = new List<Country>();
        RelayCommand resetAll;
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
            using (var context = new AutoServiceContext())
            {

                autoConcerns = context.AutoConcerns.ToList();
                var _countries = context.Countries.ToList();
                foreach (var concern in autoConcerns)
                {
                    concern.IdcountryNavigation = _countries.FirstOrDefault(A => A.Idcountry == concern.Idcountry);
                }
                displayAutoConcern = autoConcerns;
                countries = context.Countries.ToList();
            }
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
                    IsAddButtonEnable = false;
                    AutoConcernName = SelectedAutoConcern.NameAutoConcern;
                    SelectedCountry = SelectedAutoConcern.IdcountryNavigation;
                    IsResetButtonEnable = true;
                }
            }
        }



        public RelayCommand DeletingAutoconcern
        {
            get
            {
                return deletingAutoconcern ??
                      (deletingAutoconcern = new RelayCommand((o) =>
                      {
                          using (var context = new AutoServiceContext())
                          {
                              if (MessageBox.Show($"Вы точно хотите удалить выбранный автоконцерн под названием " +
                                  $"{SelectedAutoConcern.NameAutoConcern}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                              {
                                  try
                                  {
                                      context.AutoConcerns.Remove(SelectedAutoConcern);
                                      MessageBox.Show("Данные удалены.");
                                      context.SaveChanges();
                                      autoConcerns = context.AutoConcerns.ToList();
                                      AutoConcerns = autoConcerns;
                                      IsEnable = false;
                                  }
                                  catch (Exception ex)
                                  {
                                      MessageBox.Show(ex.Message);
                                  }
                                  SetProperties();
                                  AutoConcerns = displayAutoConcern;
                                  AutoConcernName = null;
                                  SelectedCountry = null;
                                  IsResetButtonEnable = false;
                                  IsAddButtonEnable = true;
                                  SelectedAutoConcern = null;
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
                OnPropertyChanged("CountryNames");
            }
        }

        public Country SelectedCountry
        {
            get => selectedCountry;
            set
            {
                selectedCountry = value;
                OnPropertyChanged("SelectedCountry");
                if (selectedCountry != null)
                {
                    IsResetButtonEnable = true;
                }
            }
        }
        public string AutoConcernName
        {
            get => autoConcernName;
            set
            {
                autoConcernName = value;
                OnPropertyChanged("AutoConcernName");
                if (autoConcernName != null)
                {
                    IsResetButtonEnable = true;
                }
            }
        }
        public RelayCommand AddAutoConcern
        {
            get
            {
                return addAutoConcern ??
                      (addAutoConcern = new RelayCommand((o) =>
                      {
                          using (var context = new AutoServiceContext())
                          {
                              StringBuilder errors = new StringBuilder();
                              if (String.IsNullOrWhiteSpace(autoConcernName))
                                  errors.AppendLine("Укажите название автоконцерна.");
                              if (selectedCountry == null)
                                  errors.AppendLine("Укажите страну автоконцерна.");
                              if (context.AutoConcerns.FirstOrDefault(A => A.NameAutoConcern == autoConcernName) != null)
                                  errors.AppendLine("Такой автоконцерн уже есть.");
                              if (errors.Length > 0)
                              {
                                  MessageBox.Show(errors.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }

                              tmp = countries.FirstOrDefault(A => A.NameCountry == selectedCountry.NameCountry);
                              int id = tmp.Idcountry;
                              AutoConcern tmpCon = new AutoConcern { NameAutoConcern = autoConcernName, Idcountry = id };

                              context.AutoConcerns.Add(tmpCon);

                              try
                              {
                                  context.SaveChanges();
                                  MessageBox.Show("Информация сохранена!");

                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message.ToString());
                              }
                              autoConcerns = context.AutoConcerns.ToList();
                              SetProperties();
                              AutoConcerns = displayAutoConcern;
                              AutoConcernName = null;
                              SelectedCountry = null;
                              IsResetButtonEnable = false;
                          }
                      }
                       ));
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
        public bool IsAddButtonEnable
        {
            get => isAddButtonEnable;
            set
            {
                isAddButtonEnable = value;
                OnPropertyChanged(nameof(IsAddButtonEnable));
            }
        }

        public RelayCommand EditConcern
        {
            get
            {
                return editConcern ??
                      (editConcern = new RelayCommand((o) =>
                      {
                          using (var context = new AutoServiceContext())
                          {
                              if (MessageBox.Show($"Вы точно хотите редактировать выбранный автоконцерн под названием " +
                                  $"{SelectedAutoConcern.NameAutoConcern}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                              {
                                  try
                                  {
                                      AutoConcern tmp = context.AutoConcerns.FirstOrDefault(A => A.IdautoConcern == SelectedAutoConcern.IdautoConcern);
                                      tmp.NameAutoConcern = AutoConcernName;
                                      tmp.IdcountryNavigation = SelectedCountry;
                                      context.AutoConcerns.Update(tmp);
                                      MessageBox.Show("Данные обновлены.");
                                      context.SaveChanges();


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
                                  SelectedAutoConcern = null;
                                  IsResetButtonEnable = false;
                                  IsAddButtonEnable = true;
                              }
                          }
                      }));
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
                          AutoConcerns = displayAutoConcern;
                          IsEnable = false;
                          AutoConcernName = null;
                          SelectedCountry = null;
                          SelectedAutoConcern = null;
                          IsResetButtonEnable = false;
                          IsAddButtonEnable = true;


                      }));
            }
        }
    }
}
