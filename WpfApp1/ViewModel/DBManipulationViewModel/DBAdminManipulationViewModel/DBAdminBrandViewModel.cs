using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel
{
    class DBAdminBrandViewModel:BaseViewModel
    {
        List<CarBrand> carBrands;
        List<CarBrand> displaycarBrands;
        private bool isEnable;
        public DBAdminBrandViewModel()
        {
            SetProperties();
        }
        private void SetProperties()
        {
            carBrands = AutoServiceContext.GetContext().CarBrands.ToList();
            var autocincerns = AutoServiceContext.GetContext().AutoConcerns.ToList();
            foreach (var concern in carBrands)
            {
                concern.IdautoConcernNavigation = autocincerns.FirstOrDefault(A => A.IdautoConcern == concern.IdautoConcern);
            }
            displaycarBrands = carBrands;
        }
        public List<CarBrand> CarBrands
        {
            get => displaycarBrands;
            set
            {
                displaycarBrands = value;
                OnPropertyChanged(nameof(CarBrands));
            }
        }
        
        AutoConcern tmp;
        List<AutoConcern> autoConcerns = AutoServiceContext.GetContext().AutoConcerns.ToList();
        private string brandName;
        RelayCommand addCarBrand;
        public List<AutoConcern> AutoConcerns
        {
            get => autoConcerns;
            set
            {
                autoConcerns = value;
                OnPropertyChanged(nameof(AutoConcerns));
            }
        }
        private AutoConcern selectedAutoConcern;
        public AutoConcern SelectedAutoConcern
        {
            get => selectedAutoConcern;
            set
            {
                selectedAutoConcern = value;
                OnPropertyChanged(nameof(SelectedAutoConcern));
            }
        }
        public string BrandName
        {
            get => brandName;
            set
            {
                brandName = value;
                OnPropertyChanged(nameof(BrandName));
            }
        }
        public bool IsEnable
        {
            get => isEnable;
            set
            {
                isEnable = value;
                OnPropertyChanged(nameof(IsEnable));
            }
        }
        private CarBrand selectedCarBrand;
        public CarBrand SelectedCarBrand
        {
            get => selectedCarBrand;
            set 
            {
                selectedCarBrand = value;
                OnPropertyChanged(nameof(SelectedCarBrand));
                IsEnable = true;
                BrandName = selectedCarBrand.NameCarBrand;
                SelectedAutoConcern = selectedCarBrand.IdautoConcernNavigation;
            }
        }
        public RelayCommand AddCarBrand
        {
            get
            {
                return addCarBrand ??
                      (addCarBrand = new RelayCommand((o) =>
                      {
                          StringBuilder errors = new StringBuilder();
                          if (String.IsNullOrWhiteSpace(brandName))
                              errors.AppendLine("Укажите название автоконцерна.");
                          if (selectedAutoConcern == null)
                              errors.AppendLine("Укажите страну автоконцерна.");
                          if (errors.Length > 0)
                          {
                              MessageBox.Show(errors.ToString());
                              return;
                          }

                          tmp = AutoServiceContext.GetContext().AutoConcerns.FirstOrDefault(A => A.NameAutoConcern == selectedAutoConcern.NameAutoConcern);
                          int id = tmp.IdautoConcern;
                          CarBrand tmpBrand = new CarBrand() { NameCarBrand = brandName, IdautoConcern = id };

                          AutoServiceContext.GetContext().CarBrands.Add(tmpBrand);
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
                          CarBrands = displaycarBrands;
                          SelectedAutoConcern = null;
                          BrandName = null;
                      }
                       ));
            }
        }
        RelayCommand editBrand;
        public RelayCommand EditBrand
        {
            get
            {
                return editBrand ??
                      (editBrand = new RelayCommand((o) =>
                      {
                          if (MessageBox.Show($"Вы точно хотите редактировать выбранную марку под названием " +
                              $"{SelectedCarBrand.NameCarBrand}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                          {
                              try
                              {
                                  CarBrand tmp = AutoServiceContext.GetContext().CarBrands.FirstOrDefault(A => A.IdcarBrand == SelectedCarBrand.IdcarBrand);
                                  tmp.NameCarBrand = BrandName;
                                  tmp.IdautoConcernNavigation = selectedAutoConcern;
                                  AutoServiceContext.GetContext().CarBrands.Update(tmp);
                                  MessageBox.Show("Данные обновлены.");
                                  AutoServiceContext.GetContext().SaveChanges();
                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message);
                              }
                              SetProperties();
                              CarBrands = displaycarBrands;
                              IsEnable = false;
                              SelectedAutoConcern = null;
                              BrandName = null;
                          }
                      }));
            }
        }

    }
}
