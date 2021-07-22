using HarmonyLib;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public class ShipControllsPatches
    {
        [HarmonyPatch(typeof(ShipControlls), "Interact")]
        [HarmonyPrefix]
        public static bool ShipControllsInteract(ShipControlls __instance, Humanoid character, bool repeat)
        {
            Player player = character as Player;
            Ship ship = __instance.GetShip();

            if (player && ship)
            {
                Piece shipPiece = ship.GetComponentInParent<Piece>();

                if (shipPiece)
                {
                    if (player.GetPlayerID() == shipPiece.GetCreator())
                    {
                        bool canInteract = LuvlyClans.clansman.IsSameClanByClanMemberID(shipPiece.GetCreator(), player.GetPlayerID());

                        if (!canInteract)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
