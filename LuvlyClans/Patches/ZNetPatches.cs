using System.Collections.Generic;
using HarmonyLib;
using static ZNet;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    class ZNetPatches
    {
        public static Player GetGlobalPlayerByZDOID(ZDOID pz)
        {
            List<Player> globalPlayers = Player.m_players;

            foreach (Player player in globalPlayers)
            {
                if (player.GetZDOID() == pz)
                {
                    return player;
                }
            }

            return null;
        }

        [HarmonyPatch(typeof(ZNet), "GetOtherPublicPlayers")]
        [HarmonyPostfix]
        public static void ZNetGetOtherPublicPlayers(ZNet __instance, List<PlayerInfo> playerList)
        {
            Player localPlayer = GetGlobalPlayerByZDOID(__instance.m_characterID);
            Player instancePlayer;

            foreach (PlayerInfo player in __instance.m_players)
            {
                if (localPlayer is null)
                {
                    //Jotunn.Logger.LogDebug("localPlayer is null");
                    return;
                }

                ZDOID playerZDOID = player.m_characterID;
                ZDOID localPlayerZDOID = localPlayer.GetZDOID();

                instancePlayer = GetGlobalPlayerByZDOID(playerZDOID);

                bool isSamePlayerByZDOID = playerZDOID == localPlayerZDOID;
                bool isNullZDOID = playerZDOID.IsNone() || localPlayerZDOID.IsNone();

                /**
                 * Need to do a lot of cleanup but broke out these gates
                 * in order to better understand what was triggering
                 */

                if (isSamePlayerByZDOID) {
                    //Jotunn.Logger.LogDebug("truthy same player by zdoid");
                    return;
                }

                if (isNullZDOID)
                {
                    //Jotunn.Logger.LogDebug("one of the two zdoids is null");
                    return;
                }
                    
                if (instancePlayer is null)
                {
                    //Jotunn.Logger.LogDebug("instancePlayer is null");
                    return;
                }

                TribeManager.TribeManager tm = new TribeManager.TribeManager(localPlayer, instancePlayer);
                bool isSameTribe = tm.IsSameTribeByPlayer();

                if (!isSameTribe)
                {
                    //Jotunn.Logger.LogDebug("Players are not same tribe, hiding pin");
                    playerList.Remove(player);
                }

                if (isSameTribe)
                {
                    //Jotunn.Logger.LogDebug("All players are in the same tribe, no pins hidden");
                }
            }
        }
    }
}
