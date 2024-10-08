using Slime_Busters_Main_Menu;
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

namespace Slime_Busters
{
    /// <summary>
    /// Interaction logic for NamesPage.xaml
    /// </summary>
    public partial class NamesPage : Page
    {
        public NamesPage()
        {
            InitializeComponent();
        }
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            Names.Content = new MainWindow();
        }
    }
}
