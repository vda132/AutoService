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
    class LoginViewModel:BaseViewModel
    {
       
        AutoServiceContext DB = new AutoServiceContext();
        private string userName;
        private string password;
        RelayCommand loginCommand;
        bool isLogin;
        Position position;
        Account acc;
        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ??
                      (loginCommand = new RelayCommand((o) =>
                      {
                          Navigation.Navigation.ToDirector();
                          //try
                          //{
                          //    if (userName == null || password == null) throw new Exception();
                          //    acc = DB.Accounts.FirstOrDefault(A => A.LoginAccount == userName);
                          //    if (acc == null) throw new LoginException("Такого пользователя не существует.");
                          //    if (acc.PasswordAccount == password)
                          //    {
                          //        IsLogin = true;
                          //        Worker worker = DB.Workers.FirstOrDefault(A => A.Idworker == acc.Idworker);
                          //        position = DB.Positions.Find(worker.Idposition);
                          //        ShowLogic();
                          //    }
                          //    else throw new PasswordException("Пароль неверный.");
                          //}
                          //catch (LoginException ex) 
                          //{
                          //    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                          //}
                          //catch (PasswordException ex)
                          //{
                          //    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                          //}
                          //catch (Exception)
                          //{
                          //    MessageBox.Show("Поля должны быть заполнены.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                          //}
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
            Models.Enums.Position enumPosition = (Models.Enums.Position)position.Idposition;
            switch (enumPosition)
            {
                case Models.Enums.Position.Director:
                    {
                        Navigation.Navigation.ToDirector();
                        break;
                    }
                case Models.Enums.Position.Worker:
                    {
                        Navigation.Navigation.ToMaster();
                        break;
                    }
                case Models.Enums.Position.DBAdmin:
                    {
                        Navigation.Navigation.ToDBAdmin();
                        break;
                    }
            }

        }

    }
}
