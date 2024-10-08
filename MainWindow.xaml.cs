using Slime_Busters;
using System.Windows;
using System.Windows.Controls;

namespace Slime_Busters_Main_Menu
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SpelenButtonClick(object sender, RoutedEventArgs e)
        {
            Main.Content = new NamesPage();
        }

        private void WinkelButtonClick(object sender, RoutedEventArgs e)
        {
            Main.Content = new WinkelPage();
        }

        private void StoppenButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}