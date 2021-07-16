using HarmonyLib;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    class ContainerPatches
    {
        [HarmonyPatch(typeof(Container), "Interact")]
        [HarmonyPrefix]
        public static bool ContainerInteract(bool __result, Container __instance, Humanoid character)
        {
            Piece piece = __instance.GetComponent<Piece>();

            if (piece && piece.IsPlacedByPlayer() && character)
            {
                Player piecePlayer = Player.GetPlayer(piece.GetCreator());
                Player characterPlayer = character.GetComponent<Player>();

                if (!piecePlayer || !characterPlayer)
                {
                    return true;
                }

                if (true)
                {
                    Jotunn.Logger.LogMessage($"Player[{piecePlayer.GetPlayerName()}] is not same clan as Player[{characterPlayer.GetPlayerName()}]");
                    return false;
                } else
                {
                    Jotunn.Logger.LogMessage($"Player[{piecePlayer.GetPlayerName()}] is same clan as Player[{characterPlayer.GetPlayerName()}]");
                    return true;
                }
            }


            return true;
        }
    }
}
