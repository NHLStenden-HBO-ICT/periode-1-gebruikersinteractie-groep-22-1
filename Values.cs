using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace Slime_Busters
{
    class Values
    {
        // Game
        public static int currentWave = 0; // Wordt 1 wanneer eerste spel begint, kan later gebruikt worden om meerdere levels te maken
        public static int waveRequirement = 15; // Hoeveel nodig is voor wave
        public static int coins = 100; // Aantal munten dat de spelers hebben

        // Namen 
        public static string playerOneName = "blank"; // Aanpassen op naam scherm
        public static string playerTwoName = "blank"; // Aanpassen op naam scherm

        // Player set
        public static double playersMovementSpeed = 10; // Snelheid dat spelers lopen
        public static int playersMaxHealth = 100; // Maximale hp (aanpasbaar in winkel)
        public static int playersDamage = 5; // Damage per tick (aanpasbaar in winkel)
        public static int maxBullets = 10; // Maximale aantal kogels
        public static int currentBulletsOne = maxBullets; // Aanpassen tijdens spel
        public static int currentBulletsTwo = maxBullets; // Aanpassen tijdens spel

        // Player upgrade
        public static int playerHealthUpgrade = 0; // Aantal health upgrades die geweest zijn
        public static int playerDamageUpgrade = 0; // Aantal damage upgrades die geweest zijn

        // Player game
        public static int playerOneCurrentHealth = playersMaxHealth; // Zet current health op max health, aanpassen tijdens spel
        public static int playerTwoCurrentHealth = playersMaxHealth; // Zet current health op max health, aanpassen tijdens spel


        // Slimes
        public static int slimeCounter = 0; // Bijhouden hoeveel slimes er gespawned zijn
        public static int slimesKilled = 0; // Aantal dode slimes
        public static int slimeSpeed = 5; // Snelheid waarmee slimes lopen

        public static int slime1Health = 10; // Health van slime
        public static int slime1Damage = 15; // Damage van slime
        public static int slime1Reward = 10; // Geld dat speler krijgt wanneer slime dood gaat
        public static int slime2Health = 20; // Health van slime
        public static int slime2Damage = 25; // Damage van slime
        public static int slime2Reward = 20; // Geld dat speler krijgt wanneer slime dood gaat
        // Hoe gebruik ik dit?
        // Deze tab is om alle waarden te constateren in 1 script. Hierdoor staan alle waarden bij elkaar en kun je makkelijk
        // bepaalde waarden aanmaken zonder dat alles door elkaar staat. Om een bepaalde waarde aan te roepen doe je dit:
        // Values.waardeNaam
        // Als je bijv. de currentLevel wilt hebben gebruik je in een ander tab: Values.currentLevel
        // <3 Danyl

    }   
}
