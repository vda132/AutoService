using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Exceptions;
using WpfApp1.UserControllers;
using WpfApp1.ViewModel.Abstract;

namespace WpfApp1.ViewModel
{
    class LoginViewModel : BaseViewModel
    {

        AutoServiceContext DB = new AutoServiceContext();
        private string userName;
        private string password;
        RelayCommand loginCommand;
        bool isLogin;
        Position position;
        Account acc;
        Worker worker;
        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ??
                      (loginCommand = new RelayCommand((o) =>
                      {
                          try
                          {
                              if (userName == null || password == null) throw new FieldException("Все поля должны быть заполнены.");
                              acc = DB.Accounts.FirstOrDefault(A => A.LoginAccount == userName);
                              if (acc == null) throw new LoginException("Такого пользователя не существует.");
                              if (acc.PasswordAccount == password)
                              {
                                  IsLogin = true;
                                  worker = DB.Workers.FirstOrDefault(A => A.Idworker == acc.Idworker);
                                  position = DB.Positions.Find(worker.Idposition);

                                  ShowLogic();
                              }
                              else throw new PasswordException("Пароль неверный.");
                          }
                          catch (LoginException ex)
                          {
                              MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                          }
                          catch (PasswordException ex)
                          {
                              MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                          }
                          catch (FieldException ex)
                          {
                              MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                          }
                      }
                       ));
            }
        }
        public bool IsLogin
        {
            set
            {
                isLogin = value;
            }
            get => isLogin;
        }
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged("UserName");
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }


        private void ShowLogic()
        {
            switch (position.NamePosition)
            {
                case "Директор":
                    {
                        Navigation.DirectorNavigation.UserId = worker.Idposition;
                        Navigation.Navigation.ToDirector();
                        break;
                    }
                case "Мастер":
                    {
                        Navigation.MasterNavigation.UserId = worker.Idworker;
                        Navigation.Navigation.ToMaster();
                        break;
                    }
                case "Администратор БД":
                    {
                        Navigation.Navigation.ToDBAdmin();
                        break;
                    }
            }

        }

    }
}
