using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;
namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel
{
    class DBAdminAutoPartPriceViewModel : BaseViewModel
    {
        bool isEnableFilter = false;
        List<AutoPartPrice> autoPartPrices;
        List<AutoPartPrice> displayAutoPartPrices;
        List<AutoPart> autoParts = new List<AutoPart>();
        AutoPart selectedAutoPart;
        private DateTime selectedDate = DateTime.Now;
        RelayCommand resetFilter;
        private string price;
        private RelayCommand addInformation;
        decimal pricePart;
        AutoPart selectedFilter;
        public List<AutoPart> AutoParts
        {
            get => autoParts;
            set
            {
                autoParts = value;
                OnPropertyChanged(nameof(AutoParts));
            }
        }
        public DBAdminAutoPartPriceViewModel()
        {
            SetProperties();
        }
        private void SetProperties()
        {
            using (var context = new AutoServiceContext())
            {
                autoParts = context.AutoParts.ToList();
                autoPartPrices = context.AutoPartPrices.ToList();
                var autoPartNames = context.AutoParts.ToList();
                foreach (var names in autoPartPrices)
                {
                    names.IdautoPartNavigation = autoPartNames.FirstOrDefault(A => A.IdautoPart == names.IdautoPart);
                }
                displayAutoPartPrices = autoPartPrices;
            }
        }
        public AutoPart SelectedFilter
        {
            get => selectedFilter;
            set
            {

                selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                if (selectedFilter != null)
                {
                    using (var context = new AutoServiceContext())
                    {
                        IsEnableFilter = true;
                        var tmp = context.AutoPartPrices.Where(A => A.IdautoPart == SelectedFilter.IdautoPart).ToList();
                        foreach (var names in tmp)
                        {
                            names.IdautoPartNavigation = context.AutoParts.First(A => A.IdautoPart == names.IdautoPart);
                        }
                        AutoPartsPrices = tmp;
                    }
                }
            }
        }
        public List<AutoPartPrice> AutoPartsPrices
        {
            get => displayAutoPartPrices;
            set
            {
                displayAutoPartPrices = value;
                OnPropertyChanged(nameof(AutoPartsPrices));
            }
        }

        public bool IsEnableFilter
        {
            get => isEnableFilter;
            set
            {
                isEnableFilter = true;
                OnPropertyChanged(nameof(IsEnableFilter));
            }
        }
        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
        public string Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged(nameof(Price));
            }
        }
        public AutoPart SelectedAutoPart
        {
            get => selectedAutoPart;
            set
            {
                selectedAutoPart = value;
                OnPropertyChanged(nameof(SelectedAutoPart));
            }
        }
        public RelayCommand ResetFilter
        {
            get
            {
                return resetFilter ??
                      (resetFilter = new RelayCommand((o) =>
                      {
                          using (var context = new AutoServiceContext())
                          {
                              SelectedFilter = null;
                              IsEnableFilter = true;
                              var tmp = context.AutoPartPrices.ToList();
                              foreach (var names in tmp)
                              {
                                  names.IdautoPartNavigation = context.AutoParts.First(A => A.IdautoPart == names.IdautoPart);
                              }
                              AutoPartsPrices = tmp;
                          }
                      }));
            }
        }
        public RelayCommand AddInformation
        {
            get
            {
                return addInformation ??
                      (addInformation = new RelayCommand((o) =>
                      {
                          using (var context = new AutoServiceContext())
                          {
                              StringBuilder errors = new StringBuilder();
                              if (selectedAutoPart == null)
                                  errors.AppendLine("Выберите запчасть.");
                              if (price == null)
                                  errors.AppendLine("Введите цену.");
                              if (!Decimal.TryParse(price, out pricePart))
                                  errors.AppendLine("Введите корректную цену.");

                              if (context.AutoPartPrices.Where(A => A.IdautoPart == selectedAutoPart.IdautoPart).Count() > 0)
                              {
                                  DateTime date = context.AutoPartPrices.Where(A => A.IdautoPart == selectedAutoPart.IdautoPart).Max(A => A.DateChange).Date;
                                  if (date > selectedDate)
                                      errors.AppendLine("Выбранная дата не может быть меньше даты последнего изменения.");
                              }

                              if (errors.Length > 0)
                              {
                                  MessageBox.Show(errors.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }
                              AutoPartPrice tmp = new AutoPartPrice() { IdautoPart = SelectedAutoPart.IdautoPart, PriceWithoutRepair = pricePart, DateChange = SelectedDate };
                              if (context.AutoPartPrices.FirstOrDefault(A => A == tmp) != null)
                              {
                                  MessageBox.Show("Такая информация уже существует.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }
                              try
                              {
                                  context.AutoPartPrices.Add(tmp);
                                  context.SaveChanges();
                                  MessageBox.Show("Информация успешно добавлена.", "Успешно", MessageBoxButton.OK);
                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message);
                              }
                              SetProperties();
                              AutoPartsPrices = displayAutoPartPrices;
                              SelectedAutoPart = null;
                              Price = null;
                              SelectedDate = DateTime.Now;
                          }
                      }));
            }
        }
    }
}
