using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slime_Busters
{
    class Values
    {
        // Game
        public static int currentLevel = 0; // Wordt 1 wanneer eerste spel begint, kan later gebruikt worden om meerdere levels te maken
        public static int coins = 0;

        // Namen 
        public static string playerOneName = "blank"; // Aanpassen op naam scherm
        public static string playerTwoName = "blank"; // Aanpassen op naam scherm

        // Player set
        public static int playersMovementSpeed = 10; // Snelheid dat speler loopt
        public static int playersMaxHealth = 100; // Maximale hp (aanpasbaar in winkel)
        public static int playersDamage = 5; // Damage per tick (aanpasbaar in winkel)

        // Player upgrade
        public static int playerHealthUpgrade = 0;
        public static int playerDamageUpgrade = 0;

        // Player game
        public static int playerOneCurrentHealth = playersMaxHealth; // Aanpassen tijdens spel
        public static int playerTwoCurrentHealth = playersMaxHealth; // Aanpassen tijdens spel

        // Hoe gebruik ik dit?
        // Deze tab is om alle waarden te constateren in 1 script. Hierdoor staan alle waarden bij elkaar en kun je makkelijk
        // bepaalde waarden aanmaken zonder dat alles door elkaar staat. Om een bepaalde waarde aan te roepen doe je dit:
        // Values.waardeNaam
        // Als je bijv. de currentLevel wilt hebben gebruik je in een ander tab: Values.currentLevel
        // <3 Danyl

    }   
}
