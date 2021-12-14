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
        bool isEnableFilter=false;
        List<AutoPartPrice> autoPartPrices;
        List<AutoPartPrice> displayAutoPartPrices;
        List<AutoPart> autoParts = AutoServiceContext.GetContext().AutoParts.ToList();
        AutoPart selectedAutoPart;
        private DateTime selectedDate=DateTime.Now;
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
            autoPartPrices = AutoServiceContext.GetContext().AutoPartPrices.ToList();
            var autoPartNames = AutoServiceContext.GetContext().AutoParts.ToList();
            foreach (var names in autoPartPrices)
            {
                names.IdautoPartNavigation = autoPartNames.First(A => A.IdautoPart == names.IdautoPart);
                names.DateChange.ToShortDateString();
            }
            displayAutoPartPrices = autoPartPrices;
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
                    IsEnableFilter = true;
                    AutoPartsPrices = AutoServiceContext.GetContext().AutoPartPrices.Where(A => A.IdautoPart == SelectedFilter.IdautoPart).ToList();
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
                          SelectedFilter = null;
                          IsEnableFilter = true;
                          AutoPartsPrices = AutoServiceContext.GetContext().AutoPartPrices.ToList();
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
                          StringBuilder errors = new StringBuilder();
                          if (selectedAutoPart == null)
                              errors.AppendLine("Выберите запчасть.");
                          if (price == null)
                              errors.AppendLine("Введите цену.");
                          if (!Decimal.TryParse(price, out pricePart))
                              errors.AppendLine("Введите корректную цену.");
                          if (errors.Length > 0)
                          {
                              MessageBox.Show(errors.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                              return;
                          }
                          AutoPartPrice tmp = new AutoPartPrice() {IdautoPart=SelectedAutoPart.IdautoPart, PriceWithoutRepair=pricePart, DateChange=SelectedDate };
                          if (AutoServiceContext.GetContext().AutoPartPrices.FirstOrDefault(A => A == tmp) != null)
                          {
                              MessageBox.Show("Такая информация уже существует.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                              return;
                          }
                          try
                          {
                              AutoServiceContext.GetContext().AutoPartPrices.Add(tmp);
                              AutoServiceContext.GetContext().SaveChanges();
                              MessageBox.Show("Информация успешно добавлена.","Успешно",MessageBoxButton.OK);
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
                      }));
            }
        }
    }
}
