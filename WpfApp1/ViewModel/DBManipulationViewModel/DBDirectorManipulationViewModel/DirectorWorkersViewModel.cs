using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBDirectorManipulationViewModel
{
    class DirectorWorkersViewModel : BaseViewModel
    {
        string workerName;
        string workerLogin;
        string workerPassword;
        List<Position> positions = new List<Position>();
        Position selectedPosition;
        RelayCommand addWorker;
        bool isAddingButtonEnable = true;
        List<Worker> workers;
        List<Worker> displayWorkers;
        Worker selectedWorker;
        RelayCommand deleteCommand;
        RelayCommand editWorker;
        RelayCommand resetAll;
        bool isEnable;
        bool isResetEnable;
        public DirectorWorkersViewModel()
        {
            SetProperties();
        }
        private void SetProperties()
        {
            isEnable = false;
            using (var context = new AutoServiceContext())
            {
                workers = context.Workers.ToList();
                positions = context.Positions.ToList();
                var accounts = context.Accounts.ToList();
                var _positions = context.Positions.ToList();
                foreach (var account in workers)
                {
                    account.Account = accounts.FirstOrDefault(A => A.Idworker == account.Idworker);
                    account.IdpositionNavigation = _positions.FirstOrDefault(A => A.Idposition == account.Idposition);
                }
                Workers = workers;
            }
        }
        public string WorkerName
        {
            get => workerName;
            set
            {
                workerName = value;
                OnPropertyChanged(nameof(WorkerName));
                if (workerName != null)
                    IsResetEnable = true;
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
                if (workerLogin != null)
                    IsResetEnable = true;
            }
        }
        public string WorkerPassword
        {
            get => workerPassword;
            set
            {
                workerPassword = value;
                OnPropertyChanged(nameof(WorkerPassword));
                if (workerPassword != null)
                    IsResetEnable = true;
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
                if (selectedPosition != null)
                    IsResetEnable = true;
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
            IsResetEnable = false;
        }

        public RelayCommand AddWorker
        {
            get
            {
                return addWorker ??
                      (addWorker = new RelayCommand((o) =>
                      {
                          using (var context = new AutoServiceContext())
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
                              if (context.Accounts.FirstOrDefault(A => A.LoginAccount == workerLogin) != null)
                                  errors.AppendLine("Такой логин уже существует");
                              if (errors.Length > 0)
                              {
                                  MessageBox.Show(errors.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                  return;
                              }

                              Position tmp = positions.FirstOrDefault(A => A.NamePosition == selectedPosition.NamePosition);
                              int id = tmp.Idposition;
                              Worker tmpWork = new Worker() { NameWorker = workerName, Idposition = tmp.Idposition };
                              context.Workers.Add(tmpWork);
                              try
                              {
                                  context.SaveChanges();
                              }
                              catch (Exception ex)
                              {
                                  MessageBox.Show(ex.Message.ToString());
                              }
                              Worker tmpWorkerForAccount = context.Workers.FirstOrDefault(A => A.NameWorker == workerName);
                              int idWorkerForAccount = tmpWorkerForAccount.Idworker;
                              Account account = new Account() { Idworker = idWorkerForAccount, LoginAccount = workerLogin, PasswordAccount = workerPassword };
                              context.Accounts.Add(account);
                              try
                              {
                                  context.SaveChanges();
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
                          using (var context = new AutoServiceContext())
                          {
                              if (MessageBox.Show($"Вы точно хотите удалить сотрудника " +
                                  $"{SelectedWorker.NameWorker}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                              {
                                  if (selectedWorker.Idworker == Navigation.DirectorNavigation.UserId)
                                  {
                                      MessageBox.Show("Вы не можете удалить сами себя!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                      return;
                                  }
                                  try
                                  {
                                      context.Workers.Remove(SelectedWorker);
                                      MessageBox.Show("Данные удалены.");
                                      context.SaveChanges();
                                      workers = context.Workers.ToList();
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
                          }
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
                          using (var context = new AutoServiceContext())
                          {
                              if (MessageBox.Show($"Вы точно хотите редактировать выбранного сотрудника " +
                                  $"{SelectedWorker.NameWorker}?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                              {
                                  try
                                  {
                                      Worker tmp = context.Workers.FirstOrDefault(A => A.Idworker == SelectedWorker.Idworker);
                                      tmp.NameWorker = WorkerName;
                                      tmp.IdpositionNavigation = selectedPosition;
                                      tmp.Account.LoginAccount = WorkerLogin;
                                      tmp.Account.PasswordAccount = WorkerPassword;
                                      context.Workers.Update(tmp);
                                      MessageBox.Show("Данные обновлены.");
                                      context.SaveChanges();
                                  }
                                  catch (Exception ex)
                                  {
                                      MessageBox.Show(ex.Message);
                                  }
                                  SetProperties();
                                  ResetAll();
                                  Workers = displayWorkers;
                              }
                          }
                      }));
            }
        }
        public RelayCommand _ResetAll
        {
            get
            {
                return resetAll ??
                    (resetAll = new RelayCommand((o) =>
                {
                    ResetAll();
                }));
            }
        }
    }
}
