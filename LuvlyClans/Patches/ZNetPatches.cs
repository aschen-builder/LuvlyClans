using System.Collections.Generic;
using HarmonyLib;
using LuvlyClans.Utils;
using static ZNet;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    class ZNetPatches
    {
        [HarmonyPatch(typeof(ZNet), "GetOtherPublicPlayers")]
        [HarmonyPostfix]
        public static void ZNetGetOtherPublicPlayers(ZNet __instance, List<PlayerInfo> playerList)
        {
            foreach (PlayerInfo player in __instance.m_players)
            {
                if (player.m_publicPosition)
                {
                    ZDOID characterID = player.m_characterID;
                    ZDOID instanceID = __instance.m_characterID;

                    if (!characterID.IsNone() && !instanceID.IsNone())
                    {
                        Player characterPlayer = PlayerUtils.GetGlobalPlayerByZDOID(characterID);
                        Player instancePlayer = PlayerUtils.GetGlobalPlayerByZDOID(instanceID);
                        
                        bool isPlayerNull = characterPlayer is null || instancePlayer is null;
                        bool isSameCharacter = player.m_characterID == __instance.m_characterID;

                        if (!isPlayerNull && !isSameCharacter)
                        {
                            TribeManager.TribeManager tm = new TribeManager.TribeManager(characterPlayer, instancePlayer);

                            if (tm.isSameTribe)
                            {
                                playerList.Remove(player);
                            }
                        }
                    }
                }
            }
        }
    }
}
