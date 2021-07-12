using HarmonyLib;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public static class ShipControllsPatches
    {
        [HarmonyPatch(typeof(Door), "Interact")]
        [HarmonyPrefix]
        public static bool ShipControllsInteract(bool __result, ShipControlls __instance, Humanoid character)
        {
            long characterOwner = character.GetOwner();
            long shipOwner = __instance.GetShip().m_nview.GetZDO().m_owner;

            Player characterPlayer = Utils.GetPlayerFromOwner(characterOwner);
            Player shipPlayer = Utils.GetPlayerFromOwner(shipOwner);

            TribeManager.TribeManager tm = new TribeManager.TribeManager(shipPlayer, characterPlayer);

            Jotunn.Logger.LogWarning($"is Player[{characterPlayer.GetPlayerName()}] in the same tribe as Player[{shipPlayer.GetPlayerName()}] :: {tm.isSameTribe}");

            return tm.isSameTribe;
        }
    }
}
