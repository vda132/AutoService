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
using WpfApp1.ViewModel.MenuViewModel;

namespace WpfApp1.UserControllers.MenuControllers
{
    /// <summary>
    /// Interaction logic for MasterMenuControl.xaml
    /// </summary>
    public partial class MasterMenuControl : UserControl
    {
        public MasterMenuControl()
        {
            InitializeComponent();
            this.DataContext = new MasterMenuViewModel();
        }
    }
}
