﻿using Slime_Busters;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Navigation;

namespace Slime_Busters_Menu
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new MenuPage());
            
        }
    }

}