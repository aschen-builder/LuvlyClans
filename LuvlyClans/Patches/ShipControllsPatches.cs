using HarmonyLib;
using LuvlyClans.Utils;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public static class ShipControllsPatches
    {
        [HarmonyPatch(typeof(ShipControlls), "Interact")]
        [HarmonyPrefix]
        public static bool ShipControllsInteract(bool __result, ShipControlls __instance, Humanoid character)
        {
            Ship ship = __instance.GetShip();

            if (ship is null)
            {
                return true;
            }

            Piece piece = ship.GetComponentInParent<Piece>();

            if (piece && character)
            {
                Player characterPlayer = PlayerUtils.GetGlobalPlayerByZDOID(character.GetZDOID());
                long characterPID = characterPlayer.GetPlayerID();
                long shipPID = piece.GetCreator();
                Player shipPlayer = Player.GetPlayer(shipPID);

                if (!characterPlayer || !shipPlayer)
                {
                    return true;
                }

                if (characterPID == shipPID)
                {
                    return true;
                }

                TribeManager.TribeManager tm = new TribeManager.TribeManager(shipPlayer, characterPlayer);

                if (!tm.isSameTribe)
                {
                    Jotunn.Logger.LogMessage($"Player[{characterPlayer.GetPlayerName()}] is not same clan as Player[{shipPlayer.GetPlayerName()}]");
                    return false;
                }
            }

            return true;
        }
    }
}
