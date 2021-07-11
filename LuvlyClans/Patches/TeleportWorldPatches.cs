using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public static class TeleportWorldPatches
    {
        [HarmonyPatch(typeof(Door), "Interact")]
        [HarmonyPrefix]
        public static bool DoorInteract(bool __result, TeleportWorld __instance, Humanoid character)
        {
            long characterOwner = character.GetOwner();
            long portalOwner = __instance.m_nview.GetZDO().m_owner;

            Player characterPlayer = Utils.GetPlayerFromOwner(characterOwner);
            Player portalPlayer = Utils.GetPlayerFromOwner(portalOwner);

            TribeManager.TribeManager tm = new TribeManager.TribeManager(portalPlayer, characterPlayer);

            Jotunn.Logger.LogWarning($"is Player[{characterPlayer.GetPlayerName()}] in the same tribe as Player[{portalPlayer.GetPlayerName()}] :: {tm.isSameTribe}");

            return tm.isSameTribe;
        }
    }
}
