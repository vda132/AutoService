using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel.DBManipulationViewModel.DBDirectorManipulationViewModel.AddingViewModel
{
    class AddingWorkerViewModel:BaseViewModel
    {
        string workerName;
        string workerLogin;
        string workerPassword;
        List<Position> positions = AutoServiceContext.GetContext().Positions.ToList();
        Position selectedPosition;
        RelayCommand addWorker;
        public string WorkerName
        {
            get => workerName;
            set
            {
                workerName = value;
                OnPropertyChanged(nameof(WorkerName));
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
                      }
                       ));
            }
        }
    }
}
