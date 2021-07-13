using HarmonyLib;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public static class ShipControllsPatches
    {
        [HarmonyPatch(typeof(ShipControlls), "Interact")]
        [HarmonyPrefix]
        public static bool ShipControllsInteract(bool __result, ShipControlls __instance, Humanoid character)
        {
            long characterOwner = character.GetOwner();

            Ship ship = __instance.GetShip();

            if (ship is null)
            {
                return true;
            }

            long shipOwner = ship.m_nview.GetZDO().m_owner;

            Player characterPlayer = Utils.GetPlayerFromOwner(characterOwner);
            Player shipPlayer = Utils.GetPlayerFromOwner(shipOwner);

            bool isPlayerNull = characterPlayer is null || shipPlayer is null;

            if (isPlayerNull)
            {
                return true;
            }

            TribeManager.TribeManager tm = new TribeManager.TribeManager(shipPlayer, characterPlayer);

            Jotunn.Logger.LogWarning($"is Player[{characterPlayer.GetPlayerName()}] in the same tribe as Player[{shipPlayer.GetPlayerName()}] :: {tm.isSameTribe}");

            return tm.isSameTribe;
        }
    }
}
