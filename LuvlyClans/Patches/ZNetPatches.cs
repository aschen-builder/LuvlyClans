using HarmonyLib;
using LuvlyClans.ClansUtils;
using LuvlyClans.Types.Clans;
using JZnet = Jotunn.ZNetExtension;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public class ZNetPatches
    {
        [HarmonyPatch(typeof(ZNet), "OnNewConnection")]
        [HarmonyPostfix]
        public static void ZNetOnNewConnection(ZNet __instance, ZNetPeer peer)
        {
            if (JZnet.IsServerInstance(__instance))
            {
                /** 
                 * probably need to load the clans db to a dict here, wait already done
                 */
                return;
            }

            if (JZnet.IsClientInstance(__instance))
            {
                /**
                 * need to check the clans db to see if playerID has been set for this peer
                 */

                string playerName = Player.m_localPlayer.GetPlayerName();
                long playerID = Player.m_localPlayer.GetPlayerID();

                Member playerClanMember = ClanManager.GetClanMemberByPlayerName(playerName);

                bool hasPlayerID = playerClanMember.m_playerID != 0;

                if (hasPlayerID)
                {
                    if (playerID != playerClanMember.m_playerID)
                    {
                        playerClanMember.m_playerID = playerID;
                    }

                    return;
                }

                playerClanMember.m_playerID = playerID;

                return;
            }
        }
    }
}
