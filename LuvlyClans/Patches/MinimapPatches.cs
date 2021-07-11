using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using static Minimap;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    class MinimapPatches
    {
        [HarmonyPatch(typeof(Minimap), "UpdatePlayerPins")]
        [HarmonyPostfix]
        public static void MinimapUpdatePlayerPins(Minimap __instance, float dt)
        {
			__instance.m_tempPlayerInfo.Clear();

			ZNet.instance.GetOtherPublicPlayers(__instance.m_tempPlayerInfo);

			if (__instance.m_playerPins.Count != __instance.m_tempPlayerInfo.Count)
			{
				foreach (PinData playerPin in __instance.m_playerPins)
				{
					__instance.RemovePin(playerPin);
				}

				__instance.m_playerPins.Clear();

				foreach (ZNet.PlayerInfo item2 in __instance.m_tempPlayerInfo)
				{
					_ = item2;
					PinData item = __instance.AddPin(Vector3.zero, PinType.Player, "", save: false, isChecked: false);
					__instance.m_playerPins.Add(item);
				}
			}

			for (int i = 0; i < __instance.m_tempPlayerInfo.Count; i++)
			{
				PinData pinData = __instance.m_playerPins[i];
				ZNet.PlayerInfo playerInfo = __instance.m_tempPlayerInfo[i];
                Jotunn.Logger.LogInfo(pinData.m_name);
				Jotunn.Logger.LogInfo(playerInfo.m_name);
				if (pinData.m_name == playerInfo.m_name)
				{
					pinData.m_pos = Vector3.MoveTowards(pinData.m_pos, playerInfo.m_position, 200f * dt);
					continue;
				}
				pinData.m_name = playerInfo.m_name;
				pinData.m_pos = playerInfo.m_position;
			}
		}
    }
}
