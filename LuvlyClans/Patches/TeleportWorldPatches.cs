using HarmonyLib;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public class TeleportWorldPatches
    {
        public static bool TeleportWorldInteract(TeleportWorld __instance, Humanoid character, bool hold)
        {
            if (!hold)
            {
                Player player = character as Player;

                if (player)
                {
                    Piece piece = __instance.GetComponent<Piece>();

                    if (piece)
                    {
                        bool canInteract = LuvlyClans.clansman.IsSameClanByClanMemberID(piece.GetCreator(), player.GetPlayerID());

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
