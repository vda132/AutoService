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
using WpfApp1.ViewModel.DBManipulationViewModel.DBDirectorManipulationViewModel.AddingViewModel;

namespace WpfApp1.UserControllers.DBManipulationControllers.DirectorManipulationControllers.AddingControllers
{
    /// <summary>
    /// Interaction logic for AddingWorkerUserControl.xaml
    /// </summary>
    public partial class AddingWorkerUserControl : UserControl
    {
        public AddingWorkerUserControl()
        {
            InitializeComponent();
            this.DataContext = new AddingWorkerViewModel();
        }
    }
}
