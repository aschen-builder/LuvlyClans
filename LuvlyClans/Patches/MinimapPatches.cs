using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using static Minimap;
using Log = Jotunn.Logger;

namespace LuvlyClans.Patches
{
	[HarmonyPatch]
    public class MinimapPatches
    {
		[HarmonyPatch(typeof(Minimap), "UpdatePlayerPins")]
		[HarmonyPrefix]
        public static bool MinimapUpdatePlayerPins(Minimap __instance, float dt)
        {
			PlayerProfile player = Game.instance.GetPlayerProfile();

			__instance.m_tempPlayerInfo.Clear();

			ZNet.instance.GetOtherPublicPlayers(__instance.m_tempPlayerInfo);

			List<ZNet.PlayerInfo> filteredTempPlayers = new List<ZNet.PlayerInfo>();

			foreach (ZNet.PlayerInfo tpi in __instance.m_tempPlayerInfo)
			{
				bool canSee = ClansHelper.ClansHelper.IsSameClanByPlayerName(tpi.m_name, player != null ? player.m_playerName : "", true);

				if (canSee)
				{
					filteredTempPlayers.Add(tpi);
				}
			}

			if (__instance.m_playerPins.Count != filteredTempPlayers.Count)
			{
				foreach (PinData playerPin in __instance.m_playerPins)
				{
					__instance.RemovePin(playerPin);
				}

				__instance.m_playerPins.Clear();

				foreach (ZNet.PlayerInfo item2 in filteredTempPlayers)
				{
					_ = item2;
					PinData item = __instance.AddPin(Vector3.zero, PinType.Player, "", save: false, isChecked: false);
					__instance.m_playerPins.Add(item);
				}
			}

			for (int i = 0; i < filteredTempPlayers.Count; i++)
			{
				PinData pinData = __instance.m_playerPins[i];

				ZNet.PlayerInfo playerInfo = filteredTempPlayers[i];

				if (pinData.m_name == playerInfo.m_name)
				{
					pinData.m_pos = Vector3.MoveTowards(pinData.m_pos, playerInfo.m_position, 200f * dt);
					continue;
				}

				pinData.m_name = playerInfo.m_name;
				pinData.m_pos = playerInfo.m_position;
			}

			return false;
		}
    }
}
