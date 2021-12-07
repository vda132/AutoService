using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AutoServiceContext serviceDB = new AutoServiceContext();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Account acc = new Account();
            try
            {
                acc = serviceDB.Accounts.FirstOrDefault(A => A.LoginAccount == loginTextBox.Text);
                if (acc == null) throw new ArgumentNullException();
                if (acc.PasswordAccount == passwordTextBox.Password)
                {
                    Manipulation manipulationWindow = new Manipulation(ref acc);
                    this.Close();
                    manipulationWindow.Show();
                }
                else throw new Exception();

            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Поля должны быть заполнены.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Пароль неверный.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
