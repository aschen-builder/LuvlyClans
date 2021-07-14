using HarmonyLib;
using LuvlyClans.Utils;
using UnityEngine;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    class TeleportWorldPatches
    {
        [HarmonyPatch(typeof(TeleportWorldTrigger), "OnTriggerEnter")]
        [HarmonyPrefix]
        public static bool TeleportWorldOnTriggerEnter(TeleportWorldTrigger __instance, Collider collider)
        {
            Player character = collider.GetComponent<Player>();
            Piece piece = __instance.GetComponentInParent<Piece>();

            if (piece && character)
            {
                Player characterPlayer = PlayerUtils.GetGlobalPlayerByZDOID(character.GetZDOID());
                long characterPID = characterPlayer.GetPlayerID();
                long doorPID = piece.GetCreator();
                Player doorPlayer = Player.GetPlayer(doorPID);

                if (!characterPlayer || !doorPlayer)
                {
                    return true;
                }

                if (characterPID == doorPID)
                {
                    return true;
                }

                TribeManager.TribeManager tm = new TribeManager.TribeManager(doorPlayer, characterPlayer);

                if (!tm.isSameTribe)
                {
                    Jotunn.Logger.LogMessage($"Player[{characterPlayer.GetPlayerName()}] is not same clan as Player[{doorPlayer.GetPlayerName()}]");
                    return false;
                }
            }

            return true;
        }
    }
}
