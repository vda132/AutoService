﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WpfApp1.ViewModel;

namespace WpfApp1.UserControllers
{
    /// <summary>
    /// Interaction logic for LoginController.xaml
    /// </summary>
    public partial class LoginController : UserControl
    { 
        
        public LoginController()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
        }

       
       
    }
}
