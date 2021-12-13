using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBDirectorManipulationViewModel
{
    class DirectorWorkersViewModel:BaseViewModel
    {
        RelayCommand toAddingWorker;
        List<Worker> workers;
        List<Worker> displayWorkers;
        Worker selectedWorker;
        RelayCommand deleteCommand;
        bool isEnable;
        public DirectorWorkersViewModel()
        {
            SetProperties();
        }
        private void SetProperties()
        {
            isEnable = false;
            workers = AutoServiceContext.GetContext().Workers.ToList();
            var accounts = AutoServiceContext.GetContext().Accounts.ToList();
            var positions = AutoServiceContext.GetContext().Positions.ToList();
            foreach (var account in workers)
            {
                account.Account = accounts.FirstOrDefault(A => A.Idworker == account.Idworker);
                account.IdpositionNavigation = positions.FirstOrDefault(A => A.Idposition == account.Idposition);
            }
            displayWorkers = workers;
        }
        public RelayCommand ToAddingWorker
        {
            get
            {
                return toAddingWorker ??
                      (toAddingWorker = new RelayCommand((o) =>
                      {
                          Navigation.DirectorNavigation.ToAddingWorker();
                      }
                       ));
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
        public List<Worker> Workers
        {
            get => displayWorkers;
            set
            {
                displayWorkers = value;
                OnPropertyChanged(nameof(Workers));
            }
        }

        public Worker SelectedWorker
        {
            get => selectedWorker;
            set
            {
                selectedWorker = value;
                OnPropertyChanged(nameof(SelectedWorker));
                IsEnable = true;
            }
        }
        public RelayCommand relayCommand
        {
            get
            {
                return deleteCommand ??
                      (deleteCommand = new RelayCommand((o) =>
                      {
                          if (MessageBox.Show($"Вы точно хотите удалить сотрудника " +
                              $"{SelectedWorker.NameWorker}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                          {
                              try
                              {
                                  AutoServiceContext.GetContext().Workers.Remove(SelectedWorker);
                                  MessageBox.Show("Данные удалены.");
                                  AutoServiceContext.GetContext().SaveChanges();
                                  workers = AutoServiceContext.GetContext().Workers.ToList();
                                  Workers = workers;

                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message);
                              }
                          }
                      }));
            }
        }
    }
}
