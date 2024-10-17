﻿using System;
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

using Slime_Busters;

namespace Slime_Busters_Menu
{
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void SpelenButtonClick(object sender, RoutedEventArgs e)
        {
            Menu.Content = new NamesPage();
        }

        private void WinkelButtonClick(object sender, RoutedEventArgs e)
        {
            Menu.Content = new WinkelPage();
        }

        private void StoppenButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}