using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvlyClans.Utils
{
    class PlayerUtils
    {
        public static Player GetGlobalPlayerByZDOID(ZDOID pz)
        {
            return Player.GetAllPlayers().FirstOrDefault(p => p.GetZDOID() == pz);
        }
    }

    class CharacterUtils
    {
        public static Character GetGlobalCharacterByZDOID(ZDOID cz)
        {
            return Character.GetAllCharacters().FirstOrDefault(c => c.GetZDOID() == cz);
        }
    }
}
