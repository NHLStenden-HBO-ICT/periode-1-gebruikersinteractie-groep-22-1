using Slime_Busters_Menu;
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
        private Dictionary<string, string> playerNames = new Dictionary<string, string>();
       


        public NamesPage()
        {
            InitializeComponent();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            Names.Content = new MenuPage();
        }

        private void StartButtonClick(object sender, RoutedEventArgs e)
        {
            Values.playerOneName = namePlayer1.Text;
            Values.playerTwoName = namePlayer2.Text;
            Values.enteredNames = true;

            Values.playerOneCurrentHealth = Values.playersMaxHealth;
            Values.playerTwoCurrentHealth = Values.playersMaxHealth;
            Values.playerOneDied = false;
            Values.playerTwoDied = false;


            NavigationService.Navigate(new PlayPage(Values.playerOneName, Values.playerTwoName));

        }

        private void Names_Navigated(object sender, NavigationEventArgs e)
        {
            // Handle navigation events if needed
        }
    }
}