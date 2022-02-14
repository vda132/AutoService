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
using WpfApp1.ViewModel.DBManipulationViewModel.DBAdminManipulationViewModel;

namespace WpfApp1.UserControllers.DBManipulationControllers.DBAdminManipulationControllers
{
    /// <summary>
    /// Interaction logic for DBAdminModelUserControl.xaml
    /// </summary>
    public partial class DBAdminModelUserControl : UserControl
    {
        public DBAdminModelUserControl()
        {
            InitializeComponent();
            this.DataContext = new DBAdminModelViewModel();
        }
    }
}
