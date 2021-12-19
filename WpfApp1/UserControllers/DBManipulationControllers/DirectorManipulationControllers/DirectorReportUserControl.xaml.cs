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
using WpfApp1.ViewModel.DBManipulationViewModel.DBDirectorManipulationViewModel;

namespace WpfApp1.UserControllers.DBManipulationControllers.DirectorManipulationControllers
{
    /// <summary>
    /// Interaction logic for DirectorReportUserControl.xaml
    /// </summary>
    public partial class DirectorReportUserControl : UserControl
    {
        public DirectorReportUserControl()
        {
            InitializeComponent();
            this.DataContext = new DirectorReportViewModel();
        }

        private void Draw()
        {
            
        }
    }
}
