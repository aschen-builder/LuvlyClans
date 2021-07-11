using LuvlyClans.TribeManager.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvlyClans.TribeManager
{
    class TribeManager
    {
        public bool IsSameTribeByPlayer(Player player1, Player player2)
        {
            if(player1 is null || player2 is null )
            {
                return false;
            }

            PlayerTribe pt_player1 = new PlayerTribe(player1);
            PlayerTribe pt_player2 = new PlayerTribe(player2);

            return pt_player1.tribe == pt_player2.tribe;
        }
    }
}
