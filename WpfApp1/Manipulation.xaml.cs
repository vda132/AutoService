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
using System.Windows.Shapes;
using WpfApp1.UserControllers;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Manipulation.xaml
    /// </summary>
    public partial class Manipulation : Window
    {
        Position position;
        AutoServiceContext autoServiceDB = new AutoServiceContext();
        private void ShowLogic(Account account)
        {
            Worker worker = autoServiceDB.Workers.FirstOrDefault(A=>A.Idworker==account.Idworker);
            position = autoServiceDB.Positions.Find(worker.Idposition);
            
            switch (position.NamePosition) 
            {
                case "Директор":
                {
                    DirectorController.Visibility = Visibility.Visible;
                    break;
                }
                case "Мастер":
                {
                    MasterController.Visibility = Visibility.Visible;
                    break;
                }
                case "Администратор БД":
                {
                    DBAdministratorController.Visibility = Visibility.Visible;
                    break;
                }
            }

        }
        public Manipulation(ref Account account)
        {
            InitializeComponent();
            ShowLogic(account);
        }

    }
}
