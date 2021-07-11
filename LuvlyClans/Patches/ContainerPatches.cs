﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    class ContainerPatches
    {
        [HarmonyPatch(typeof(Container), "Interact")]
        [HarmonyPrefix]
        public bool ContainerInteract(bool __result, Container __instance, Humanoid character)
        {
            long characterOwner = character.GetOwner();
            long containerOwner = __instance.m_nview.GetZDO().m_owner;

            Player characterPlayer = Utils.GetPlayerFromOwner(characterOwner);
            Player containerPlayer = Utils.GetPlayerFromOwner(containerOwner);

            TribeManager.TribeManager tm = new TribeManager.TribeManager(containerPlayer, characterPlayer);

            Jotunn.Logger.LogWarning($"is Player[{characterPlayer.GetPlayerName()}] in the same tribe as Player[{containerPlayer.GetPlayerName()}] :: {tm.isSameTribe}");

            return tm.isSameTribe;
        }
    }
}