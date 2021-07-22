using HarmonyLib;
using Log = Jotunn.Logger;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public class ContainerPatches
    {
        [HarmonyPatch(typeof(Container), "Interact")]
        [HarmonyPrefix]
        public static bool ContainerInteract(Container __instance, Humanoid character, bool hold)
        {
            if (hold)
            {
                return false;
            }

            if (__instance.m_checkGuardStone && !PrivateArea.CheckAccess(__instance.transform.position))
            {
                return true;
            }

            if (!__instance.CheckAccess(Game.instance.GetPlayerProfile().GetPlayerID()))
            {
                character.Message(MessageHud.MessageType.Center, "$msg_cantopen");
                return true;
            }

            Piece piece = __instance.m_piece;

            if (piece != null && piece.IsPlacedByPlayer() && character != null)
            {
                PlayerProfile characterPlayer = Game.instance.GetPlayerProfile();

                if (piece.GetCreator() == 0)
                {
                    Log.LogWarning("Piece creator doesn't exist");
                    return true;
                }

                if (characterPlayer == null)
                {
                    Log.LogWarning("Character player is null");
                    return true;
                }

                bool canInteract = LuvlyClans.clansman.IsSameClanByClanMemberID(piece.GetCreator(), characterPlayer.GetPlayerID());

                if (!canInteract)
                {
                    Log.LogInfo($"Player[{piece.GetCreator()}] is not same clan as Player[{characterPlayer.GetPlayerID()}]");
                    return false;
                }
                else
                {
                    Log.LogInfo($"Player[{piece.GetCreator()}] is same clan as Player[{characterPlayer.GetPlayerID()}]");
                    return true;
                }
            }

            Log.LogWarning("Null piece or character");

            return true;
        }
    }
}
