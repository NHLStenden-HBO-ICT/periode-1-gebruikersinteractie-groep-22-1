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
    /// Interaction logic for WinkelPage.xaml
    /// </summary>
    public partial class WinkelPage : Page
    {




        public WinkelPage()
        {
            InitializeComponent();
        }


        private void AttackButton(object sender, RoutedEventArgs e)
        {
            // Voeg hier de logica toe die je wilt uitvoeren wanneer op de verdediging-knop wordt geklikt
            Console.WriteLine("aanvalsknop aangeklikt");
        }
        
        
        private void DefenseButton_Click(object sender, RoutedEventArgs e)
        {
            // Voeg hier de logica toe die je wilt uitvoeren wanneer op de verdediging-knop wordt geklikt
            Console.WriteLine("verdedigingsknop aangeklikt");
        }
        
        
        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
  

        }

        private void Winkel_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
