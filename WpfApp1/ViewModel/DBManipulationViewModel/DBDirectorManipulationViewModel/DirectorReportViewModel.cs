using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ADO.Net_DB;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBDirectorManipulationViewModel
{
    class DirectorReportViewModel:BaseViewModel
    {
        DBContext obj;
        private DateTime dateBegin = DateTime.Now;
        private DateTime dateEnd = DateTime.Now;
        bool isResetEnable = false;
        bool isEnable = false;
        bool isPieEnable = true;
        List<KeyValuePair<string, int>> statistic = new List<KeyValuePair<string, int>>();
        List<KeyValuePair<string, decimal>> brandStatistic = new List<KeyValuePair<string, decimal>>();
        Visibility barChartVisibilty = Visibility.Visible;
        Visibility pieChartVisibility = Visibility.Hidden;
        RelayCommand dateStatistic;
        RelayCommand resetAll;
        RelayCommand toServiceStatistic;
        RelayCommand toBrandStatistic;

        public DirectorReportViewModel()
        {
            obj = new DBContext();
            statistic = obj.GetWorkerStatistic();
        }

        public List<KeyValuePair<string, int>> Statistic
        {
            get => statistic;
            set
            {
                statistic = value;
                OnPropertyChanged(nameof(Statistic));
            }
        }
        public List<KeyValuePair<string, decimal>> BrandStatistic
        {
            get => brandStatistic;
            set
            {
                brandStatistic = value;
                OnPropertyChanged(nameof(BrandStatistic));
            }
        }

        public DateTime DateBegin
        {
            get => dateBegin;
            set
            {
                dateBegin = value;
                OnPropertyChanged(nameof(DateBegin));
            }
        }
        public DateTime DateEnd
        {
            get => dateEnd;
            set
            {
                dateEnd = value;
                OnPropertyChanged(nameof(DateEnd));
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
        public RelayCommand DateStatistic
        {
            get
            {
                return dateStatistic ??
                    (dateStatistic = new RelayCommand((o) =>
                    {
                        if (DateBegin > DateEnd)
                        {
                            MessageBox.Show("Дата от не может быть больше Даты до.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            DateBegin = DateTime.Now;
                            DateEnd = DateTime.Now;
                            return;
                        }
                        Statistic = obj.GetWorkerStatistic(DateBegin, DateEnd);
                        IsResetEnable = true;
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
                        DateBegin = DateTime.Now;
                        DateEnd = DateTime.Now;
                        Statistic = obj.GetWorkerStatistic();
                        IsResetEnable = false;
                    }));
            }
        }
        public Visibility BarChartVisibilty
        {
            get => barChartVisibilty;
            set
            {
                barChartVisibilty = value;
                OnPropertyChanged(nameof(BarChartVisibilty));
            }
        }
        public Visibility PieChartVisibility
        {
            get => pieChartVisibility;
            set
            {
                pieChartVisibility = value;
                OnPropertyChanged(nameof(PieChartVisibility));
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
        public bool IsPieEnable
        {
            get => isPieEnable;
            set
            {
                isPieEnable = value;
                OnPropertyChanged(nameof(IsPieEnable));
            }
        }
        public RelayCommand ToServiceStatistic
        {
            get
            {
                return toServiceStatistic ??
                    (toServiceStatistic = new RelayCommand((o) =>
                    {
                        PieChartVisibility = Visibility.Hidden;
                        BarChartVisibilty = Visibility.Visible;
                        IsPieEnable = true;
                        IsEnable = false;
                        DateBegin = DateTime.Now;
                        DateEnd = DateTime.Now;
                        Statistic = obj.GetWorkerStatistic();
                        IsResetEnable = false;
                    }));
            }
        }
        public RelayCommand ToBrandStatistic
        {
            get
            {
                return toBrandStatistic ??
                    (toBrandStatistic = new RelayCommand((o) =>
                    {
                        BarChartVisibilty = Visibility.Hidden;
                        PieChartVisibility = Visibility.Visible;
                        IsPieEnable = false;
                        IsEnable = true;
                        BrandStatistic = obj.GetBrandStatistic();
                        IsResetEnable = false;
                    }));
            }
        }
    }
}
