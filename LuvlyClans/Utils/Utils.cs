using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvlyClans
{
    class Utils
    {
        public static Player GetPlayerFromOwner(long owner)
        {
            List<Player> allPlayers = Player.GetAllPlayers();

            foreach (Player player in allPlayers)
            {
                if (player.GetOwner() == owner)
                {
                    return player;
                }
            }

            return null;
        }
    }
}
