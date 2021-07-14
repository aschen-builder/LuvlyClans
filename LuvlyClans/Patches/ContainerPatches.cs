using HarmonyLib;
using LuvlyClans.Utils;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    class ContainerPatches
    {
        [HarmonyPatch(typeof(Container), "Interact")]
        [HarmonyPrefix]
        public static bool ContainerInteract(bool __result, Container __instance, Humanoid character)
        {
            Piece piece = __instance.GetComponentInParent<Piece>();

            if (piece && character)
            {
                Player characterPlayer = PlayerUtils.GetGlobalPlayerByZDOID(character.GetZDOID());
                long characterPID = characterPlayer.GetPlayerID();
                long containerPID = piece.GetCreator();
                Player containerPlayer = Player.GetPlayer(containerPID);

                if (!characterPlayer || !containerPlayer)
                {
                    Jotunn.Logger.LogMessage("no player");
                    return true;
                }

                if (characterPID == containerPID)
                {
                    Jotunn.Logger.LogMessage("no id");
                    return true;
                }

                TribeManager.TribeManager tm = new TribeManager.TribeManager(containerPlayer, characterPlayer);

                if (!tm.isSameTribe)
                {
                    Jotunn.Logger.LogMessage($"Player[{characterPlayer.GetPlayerName()}] is not same clan as Player[{containerPlayer.GetPlayerName()}]");
                    return false;
                }
            }

            return true;
        }
    }
}
