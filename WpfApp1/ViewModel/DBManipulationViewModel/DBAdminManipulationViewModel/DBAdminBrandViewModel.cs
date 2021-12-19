using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel
{
    class DBAdminBrandViewModel : BaseViewModel
    {
        List<CarBrand> carBrands;
        List<CarBrand> displaycarBrands;
        private bool isEnable;
        private bool isResetEnable;
        AutoConcern tmp;
        List<AutoConcern> autoConcerns = new List<AutoConcern>();
        private string brandName;
        RelayCommand addCarBrand;
        private AutoConcern selectedAutoConcern;
        private CarBrand selectedCarBrand;
        RelayCommand editBrand;
        RelayCommand resetAll;
        public DBAdminBrandViewModel()
        {
            SetProperties();
        }
        private void SetProperties()
        {
            using (var context = new AutoServiceContext())
            {
                autoConcerns = context.AutoConcerns.ToList();
                carBrands = context.CarBrands.ToList();
                var autocincerns = context.AutoConcerns.ToList();
                foreach (var concern in carBrands)
                {
                    concern.IdautoConcernNavigation = autoConcerns.FirstOrDefault(A => A.IdautoConcern == concern.IdautoConcern);
                }
                displaycarBrands = carBrands;
            }
        }
        private void ReseteAll()
        {
            IsResetEnable = false;
            IsEnable = false;
            BrandName = null;
            SelectedAutoConcern = null;
            SelectedCarBrand = null;
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


        public List<AutoConcern> AutoConcerns
        {
            get => autoConcerns;
            set
            {
                autoConcerns = value;
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
                    IsResetEnable = true;
                }
            }
        }
        public string BrandName
        {
            get => brandName;
            set
            {
                brandName = value;
                OnPropertyChanged(nameof(BrandName));
                if (brandName != null)
                {
                    IsResetEnable = true;
                }
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
        public bool IsResetEnable
        {
            get => isResetEnable;
            set
            {
                isResetEnable = value;
                OnPropertyChanged(nameof(IsResetEnable));
            }
        }

        public CarBrand SelectedCarBrand
        {
            get => selectedCarBrand;
            set
            {
                selectedCarBrand = value;
                OnPropertyChanged(nameof(SelectedCarBrand));
                if (selectedCarBrand != null)
                {
                    IsEnable = true;
                    BrandName = selectedCarBrand.NameCarBrand;
                    SelectedAutoConcern = selectedCarBrand.IdautoConcernNavigation;
                    IsResetEnable = true;
                }
            }
        }
        public RelayCommand AddCarBrand
        {
            get
            {
                return addCarBrand ??
                      (addCarBrand = new RelayCommand((o) =>
                      {
                          using (var context = new AutoServiceContext())
                          {
                              StringBuilder errors = new StringBuilder();
                              if (String.IsNullOrWhiteSpace(brandName))
                                  errors.AppendLine("Укажите название автоконцерна.");
                              if (selectedAutoConcern == null)
                                  errors.AppendLine("Укажите страну автоконцерна.");
                              if (context.CarBrands.FirstOrDefault(A => A.NameCarBrand == brandName) != null)
                                  errors.AppendLine("Такая марка уже есть.");
                              if (errors.Length > 0)
                              {
                                  MessageBox.Show(errors.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }

                              tmp = context.AutoConcerns.FirstOrDefault(A => A.NameAutoConcern == selectedAutoConcern.NameAutoConcern);
                              int id = tmp.IdautoConcern;
                              CarBrand tmpBrand = new CarBrand() { NameCarBrand = brandName, IdautoConcern = id };

                              context.CarBrands.Add(tmpBrand);
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
                              CarBrands = displaycarBrands;
                              SelectedAutoConcern = null;
                              BrandName = null;
                          }
                      }
                       ));
            }
        }

        public RelayCommand EditBrand
        {
            get
            {
                return editBrand ??
                      (editBrand = new RelayCommand((o) =>
                      {
                          using (var context = new AutoServiceContext())
                          {
                              if (MessageBox.Show($"Вы точно хотите редактировать выбранную марку под названием " +
                                  $"{SelectedCarBrand.NameCarBrand}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                              {
                                  try
                                  {
                                      CarBrand tmp = context.CarBrands.FirstOrDefault(A => A.IdcarBrand == SelectedCarBrand.IdcarBrand);
                                      tmp.NameCarBrand = BrandName;
                                      tmp.IdautoConcernNavigation = selectedAutoConcern;
                                      context.CarBrands.Update(tmp);
                                      MessageBox.Show("Данные обновлены.");
                                      context.SaveChanges();
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
                          ReseteAll();
                      }));
            }
        }
    }
}
