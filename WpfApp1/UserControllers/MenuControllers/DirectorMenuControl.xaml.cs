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
    /// Interaction logic for DirectorMenuControl.xaml
    /// </summary>
    public partial class DirectorMenuControl : UserControl
    {
        public DirectorMenuControl()
        {
            InitializeComponent();
            this.DataContext = new DirectorMenuViewModel();
        }
    }
}
