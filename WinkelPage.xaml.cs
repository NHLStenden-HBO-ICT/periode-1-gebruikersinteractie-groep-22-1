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
using System.Windows.Threading;

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
            
                // Controleren of er genoeg munten zijn om te upgraden
                int currentCoins = int.Parse(CoinsAmount.Text);
            if (currentCoins < 10)
            {
                // Toon melding dat er te weinig munten zijn
                ShowErrorMessage("Je hebt te weinig munten!");
            }
            else
            {
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
                            AttackButton.Content = "Aanval maximaal";
                        }
                    }
                }

                }
             
            
        }

        private void DefenseButton_Click(object sender, RoutedEventArgs e)
        {
            
                // Controleren of er genoeg munten zijn om te upgraden
                int currentCoins = int.Parse(CoinsAmount.Text);
            if (currentCoins < 10)
            {
                // Toon melding dat er te weinig munten zijn
                ShowErrorMessage("Je hebt te weinig munten!");
            }
            else
            {
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
                            DefenseButton.Content = "Verdediging maximaal";
                        }
               
                       
                    }
                
                }
            }
                
            
        }
        private void ShowErrorMessage(string message)
        {
            // Toon de melding en achtergrond
          
            NotEnoughCoinsText.Text = message;
            NotEnoughCoinsText.Visibility = Visibility.Visible;
            NotEnoughCoinsRectangle.Visibility = Visibility.Visible;

            // Start een timer om de melding na 2 secondes te verbergen
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(2);
            timer.Tick += (s, e) =>
            {
                // Verberg de melding en achtergrond
                NotEnoughCoinsText.Visibility = Visibility.Hidden;
                NotEnoughCoinsRectangle.Visibility = Visibility.Hidden;
                timer.Stop(); // Stop de timer
            };
            timer.Start();
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Winkel.Content = new MenuPage();
            WinkelPageName.Visibility = Visibility.Hidden;
        }

        private void Winkel_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Winkel.Content = new MenuPage();
            WinkelPageName.Visibility = Visibility.Hidden;
        }

        // Event handler voor de InfoAttack-knop
        private void InfoAttack_Click(object sender, RoutedEventArgs e)
        {
            // Maak de uitleg zichtbaar
            CloseInfoAttack.Visibility = Visibility.Visible;
            InfoAttackRectangle.Visibility = Visibility.Visible;
            InfoAttackText.Visibility = Visibility.Visible;
            

        }

        private void CloseInfoAttack_Click(object sender, RoutedEventArgs e)
        {

            CloseInfoAttack.Visibility = Visibility.Hidden;
            InfoAttackRectangle.Visibility = Visibility.Hidden;
            InfoAttackText.Visibility = Visibility.Hidden;

        }
        // Event handler voor de InfoDefense-knop
        private void InfoDefense_Click(object sender, RoutedEventArgs e)
        {
            // Maak de uitleg zichtbaar
            CloseInfoDefense.Visibility = Visibility.Visible;
            InfoDefenseRectangle.Visibility = Visibility.Visible;
            InfoDefenseText.Visibility = Visibility.Visible;


        }

        private void CloseInfoDefense_Click(object sender, RoutedEventArgs e)
        {

            CloseInfoDefense.Visibility = Visibility.Hidden;
            InfoDefenseRectangle.Visibility = Visibility.Hidden;
            InfoDefenseText.Visibility = Visibility.Hidden;

        }


    }
}