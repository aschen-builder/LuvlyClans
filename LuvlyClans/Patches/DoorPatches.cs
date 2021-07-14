using HarmonyLib;
using LuvlyClans.Utils;
using UnityEngine;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public static class DoorPatches
    {
        [HarmonyPatch(typeof(Door), "Interact")]
        [HarmonyPrefix]
        public static bool DoorInteract(bool __result, Door __instance, Humanoid character, bool hold)
        {
			Piece piece = __instance.GetComponent<Piece>();
			if (piece && piece.IsPlacedByPlayer())
            {
				Player localPlayer = Player.m_localPlayer;
				Player piecePlayer = Player.GetPlayer(piece.GetCreator());

				if (!localPlayer.IsPlayer() || !piecePlayer.IsPlayer())
                {
					Jotunn.Logger.LogMessage("no players");
					return true;
                }

                if (piecePlayer.GetPlayerID() == localPlayer.GetPlayerID())
                {
                    Jotunn.Logger.LogMessage("same player");
                    return true;
                }

                TribeManager.TribeManager tm = new TribeManager.TribeManager(piecePlayer, localPlayer);

                if (!tm.isSameTribe)
                {
                    Jotunn.Logger.LogMessage("wrong tribe");
                    return false;
                }
            }

            Jotunn.Logger.LogMessage("no player placed piece");
            return true;
		}
    }
}
