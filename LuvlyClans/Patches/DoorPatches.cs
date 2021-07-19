using HarmonyLib;
using Log = Jotunn.Logger;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public class DoorPatches
    {
        [HarmonyPatch(typeof(Door), "Interact")]
        [HarmonyPrefix]
        public static bool DoorInteract(Door __instance, Humanoid character, bool hold)
        {
            if (hold)
            {
                return false;
            }

            if (!__instance.CanInteract())
            {
                return false;
            }

            if (!PrivateArea.CheckAccess(__instance.transform.position))
            {
                return true;
            }

            Piece piece = __instance.GetComponent<Piece>();

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

                bool isClient = LuvlyClans.m_is_client;
                bool canInteract = ClansHelper.ClansHelper.IsSameClanByPlayerID(piece.GetCreator(), characterPlayer.GetPlayerID(), isClient);

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
