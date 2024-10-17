using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slime_Busters
{
    class Value
    {
        // DEZE CODE NIET GEBRUIKEN
        // Is oude code van Daniel, laat er voor nu in staan,
        // hebben we miss later nog wat aan maar voor nu niet
        // gebruiken want werkt NIET
        

        // Methodes voor de values
        public static dynamic NewPlayerStats()
        {
            var playerStats = new Dictionary<string, dynamic>();
            playerStats.Add("namePlayerOne", "blank");
            playerStats.Add("namePlayerTwo", "blank");
            playerStats.Add("currentHealthPlayerOne", 100);
            playerStats.Add("currentHealthPlayerTwo", 100);

            return playerStats;
        }

        public static dynamic GameStats()
        {
            var gameStats = new Dictionary<string, dynamic>();
            gameStats.Add("level", 0);
            gameStats.Add("coins", 0);
            gameStats.Add("maxHP", 100);
            gameStats.Add("movementSpeed", 10);
            gameStats.Add("damage", 5);

            return gameStats;
        }
    }
}
