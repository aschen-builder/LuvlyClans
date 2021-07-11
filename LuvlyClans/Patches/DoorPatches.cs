using HarmonyLib;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public static class DoorPatches
    {
        [HarmonyPatch(typeof(Door), "Interact")]
        [HarmonyPrefix]
        public static bool DoorInteract(bool __result, Door __instance, Humanoid character)
        {
            long characterOwner = character.GetOwner();
            long doorOwner = __instance.m_nview.GetZDO().m_owner;

            Player characterPlayer = Utils.GetPlayerFromOwner(characterOwner);
            Player doorPlayer = Utils.GetPlayerFromOwner(doorOwner);

            TribeManager.TribeManager tm = new TribeManager.TribeManager(doorPlayer, characterPlayer);

            Jotunn.Logger.LogWarning($"is Player[{characterPlayer.GetPlayerName()}] in the same tribe as Player[{doorPlayer.GetPlayerName()}] :: {tm.isSameTribe}");

            return tm.isSameTribe;
        }
    }
}
