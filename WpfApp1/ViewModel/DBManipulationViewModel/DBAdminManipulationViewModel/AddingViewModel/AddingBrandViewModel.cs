using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel.AddingViewModel
{
    class AddingBrandViewModel : BaseViewModel
    {
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
                      }
                       ));
            }
        }
    }
}
