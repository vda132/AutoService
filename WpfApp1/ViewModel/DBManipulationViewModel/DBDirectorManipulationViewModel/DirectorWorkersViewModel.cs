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
        string workerName;
        string workerLogin;
        string workerPassword;
        List<Position> positions = AutoServiceContext.GetContext().Positions.ToList();
        Position selectedPosition;
        RelayCommand addWorker;
        bool isAddingButtonEnable = true;
        List<Worker> workers;
        List<Worker> displayWorkers;
        Worker selectedWorker;
        RelayCommand deleteCommand;
        RelayCommand editWorker;
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
        public string WorkerName
        {
            get => workerName;
            set
            {
                workerName = value;
                OnPropertyChanged(nameof(WorkerName));
            }
        }

        public bool IsAddingButtonEnable
        {
            get => isAddingButtonEnable;
            set
            {
                isAddingButtonEnable = value;
                OnPropertyChanged(nameof(IsAddingButtonEnable));
            }
        }
        public string WorkerLogin
        {
            get => workerLogin;
            set
            {
                workerLogin = value;
                OnPropertyChanged(nameof(WorkerLogin));
            }
        }
        public string WorkerPassword
        {
            get => workerPassword;
            set
            {
                workerPassword = value;
                OnPropertyChanged(nameof(WorkerPassword));
            }
        }
        public List<Position> Positions
        {
            get => positions;
            set
            {
                positions = value;
                OnPropertyChanged(nameof(Positions));
            }
        }
        public Position SelectedPosition
        {
            get => selectedPosition;
            set
            {
                selectedPosition = value;
                OnPropertyChanged(nameof(SelectedPosition));
            }
        }

       private void ResetAll()
       {
            IsAddingButtonEnable = true;
            IsEnable = false;
            WorkerLogin = null;
            WorkerPassword = null;
            WorkerName = null;
            SelectedPosition = null;
       }
        
        public RelayCommand AddWorker
        {
            get
            {
                return addWorker ??
                      (addWorker = new RelayCommand((o) =>
                      {
                          StringBuilder errors = new StringBuilder();
                          if (String.IsNullOrWhiteSpace(workerName))
                              errors.AppendLine("Укажите ФИО сотрудника.");
                          if (selectedPosition == null)
                              errors.AppendLine("Укажите должность сотрудника.");
                          if (String.IsNullOrWhiteSpace(workerLogin))
                              errors.AppendLine("Укажите логин сотрудника.");
                          if (String.IsNullOrWhiteSpace(workerPassword))
                              errors.AppendLine("Укажите пароль сотрудника.");
                          if (AutoServiceContext.GetContext().Accounts.FirstOrDefault(A => A.LoginAccount == workerLogin) != null)
                              errors.AppendLine("Такой логин уже существует");
                          if (errors.Length > 0)
                          {
                              MessageBox.Show(errors.ToString());
                              return;
                          }

                          Position tmp = positions.FirstOrDefault(A => A.NamePosition == selectedPosition.NamePosition);
                          int id = tmp.Idposition;
                          Worker tmpWork = new Worker() { NameWorker = workerName, Idposition = tmp.Idposition };
                          AutoServiceContext.GetContext().Workers.Add(tmpWork);
                          try
                          {
                              AutoServiceContext.GetContext().SaveChanges();
                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message.ToString());
                          }
                          Worker tmpWorkerForAccount = AutoServiceContext.GetContext().Workers.FirstOrDefault(A => A.NameWorker == workerName);
                          int idWorkerForAccount = tmpWorkerForAccount.Idworker;
                          Account account = new Account() { Idworker = idWorkerForAccount, LoginAccount = workerLogin, PasswordAccount = workerPassword };
                          AutoServiceContext.GetContext().Accounts.Add(account);
                          try
                          {
                              AutoServiceContext.GetContext().SaveChanges();
                              MessageBox.Show("Информация сохранена!");
                          }
                          catch (Exception ex)
                          {
                              MessageBox.Show(ex.Message.ToString());
                          }
                          ResetAll();
                          SetProperties();
                          Workers = displayWorkers;
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
                if (selectedWorker != null) 
                {
                    IsAddingButtonEnable = false;
                    IsEnable = true;
                    WorkerLogin = SelectedWorker.Account.LoginAccount;
                    WorkerPassword = SelectedWorker.Account.PasswordAccount;
                    WorkerName = SelectedWorker.NameWorker;
                    SelectedPosition = SelectedWorker.IdpositionNavigation;
                }
                
            }
        }
        public RelayCommand DeleteCommand
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
                          SetProperties();
                          ResetAll();
                          Workers = displayWorkers;
                      }));
            }
        }
        public RelayCommand EditWorker
        {
            get
            {
                return editWorker ??
                      (editWorker = new RelayCommand((o) =>
                      {
                          if (MessageBox.Show($"Вы точно хотите редактировать выбранного сотрудника " +
                              $"{SelectedWorker.NameWorker}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                          {
                              try
                              {
                                  Worker tmp = AutoServiceContext.GetContext().Workers.FirstOrDefault(A => A.Idworker == SelectedWorker.Idworker);
                                  tmp.NameWorker = WorkerName;
                                  tmp.IdpositionNavigation = selectedPosition;
                                  tmp.Account.LoginAccount = WorkerLogin;
                                  tmp.Account.PasswordAccount = WorkerPassword;
                                  AutoServiceContext.GetContext().Workers.Update(tmp);
                                  MessageBox.Show("Данные обновлены.");
                                  AutoServiceContext.GetContext().SaveChanges();
                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message);
                              }
                              SetProperties();
                              ResetAll();
                              Workers = displayWorkers;
                          }
                      }));
            }
        }
    }
}
