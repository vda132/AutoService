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
            addingButton.Visibility = Visibility.Visible;
            Worker worker = autoServiceDB.Workers.FirstOrDefault(A=>A.Idworker==account.Idworker);
            position = autoServiceDB.Positions.Find(worker.Idposition);
            switch (position.NamePosition) 
            {
                case "Директор":
                {
                    addingButton.Content = "Добавить сотрудника";
                    break;
                }
                case "Мастер":
                {
                    addingButton.Content = "Добавить клиента";
                    break;
                }
                case "Администратор БД":
                {
                    addingButton.Content = "Добавить автоконцерн";
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
