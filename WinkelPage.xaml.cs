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
    public partial class WinkelPage : Page
    {
        // Ensure there is no duplicate definition here
        // Remove or rename any duplicate definitions
        // Example of a duplicate definition:
        // public Button AttackButton; // This should be removed if it exists

        public WinkelPage()
        {
            InitializeComponent();
        }

        private void AttackButton_Click(object sender, RoutedEventArgs e)
        {
            {
                // Controleren of er genoeg munten zijn om te upgraden
                int currentCoins = int.Parse(CoinsAmount.Text);
                if (currentCoins >= 10)
                {
                    // Controleren of de ProgressBar nog niet maximaal is
                    if (AttackProgressBar.Value < AttackProgressBar.Maximum)
                    {
                        // Verhoog de waarde van de ProgressBar
                        AttackProgressBar.Value += 1;

                        // Verminder munten met 10
                        currentCoins -= 10;
                        CoinsAmount.Text = currentCoins.ToString();

                        // Als de ProgressBar maximaal is, knop uitschakelen
                        if (AttackProgressBar.Value == AttackProgressBar.Maximum)
                        {
                            AttackButton.IsEnabled = false;
                            
                        }
                    }
                }
                else
                {
                    
                }
            }
        }

        private void DefenseButton_Click(object sender, RoutedEventArgs e)
        {
            {
                // Controleren of er genoeg munten zijn om te upgraden
                int currentCoins = int.Parse(CoinsAmount.Text);
                if (currentCoins >= 10)
                {
                    // Controleren of de ProgressBar nog niet maximaal is
                    if (DefenseProgressBar.Value < DefenseProgressBar.Maximum)
                    {
                        // Verhoog de waarde van de ProgressBar
                        DefenseProgressBar.Value += 1;

                        // Verminder munten met 10
                        currentCoins -= 10;
                        CoinsAmount.Text = currentCoins.ToString();

                        // Als de ProgressBar maximaal is, knop uitschakelen
                        if (DefenseProgressBar.Value == DefenseProgressBar.Maximum)
                        {
                            DefenseButton.IsEnabled = false;

                        }
                    }
                }
                else
                {

                }
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            Winkel.Content = new MenuPage();

        }

        private void Winkel_Navigated(object sender, NavigationEventArgs e)
        {
            // Your navigation event logic here
        }
    }
}