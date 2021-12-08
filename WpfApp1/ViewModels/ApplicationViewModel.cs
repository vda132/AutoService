using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WpfApp1
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        AutoServiceContext DB;
        private string userName;
        private string password;
        RelayCommand loginCommand;
        MainWindow mainWindow;
        bool isLogin;
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

        public RelayCommand LoginCommand
        {
            get
            {
                return loginCommand ??
                      (loginCommand = new RelayCommand((o) =>
                      {
                          try
                          {
                              if (userName == null || password == null) throw new Exception();
                              Account acc = DB.Accounts.FirstOrDefault(A => A.LoginAccount == userName);
                              if (acc.PasswordAccount == password)
                              {
                                  Manipulation manipulation = new Manipulation(ref acc);
                                  IsLogin = true;
                                  mainWindow.Close();
                                  manipulation.Show();
                              }
                              else throw new ArgumentNullException();
                          }
                          catch (ArgumentNullException)
                          {
                              MessageBox.Show("Пароль неверный.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                          }
                          catch (Exception)
                          {
                              MessageBox.Show("Поля должны быть заполнены.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                          }
                      }
                       ));
            }
        }
        public ApplicationViewModel(MainWindow obj)
        {
            DB = new AutoServiceContext();
            isLogin = false;
            mainWindow = obj;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
