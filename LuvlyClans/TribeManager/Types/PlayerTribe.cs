using Valheim.EnhancedProgressTracker.Tribe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvlyClans.TribeManager.Types
{
    class PlayerTribe
    {
        public string TribeByPlayerName(string name)
        {
            string tribe;
            TribeHelper.TryGetPlayerTribe(name, out tribe);

            return tribe;
        }
        public PlayerTribe(Player player)
        {
            p_name = player.GetPlayerName();
            tribe = TribeByPlayerName(p_name);
        }

        public string p_name { get; }
        public string tribe { get; }

        public override string ToString()
        {
            return $"player: ${p_name}; tribe: ${tribe}";
        }
    }
}
