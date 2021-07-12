// LuvlyClans
// a Valheim mod skeleton using Jötunn
// 
// File:    LuvlyClans.cs
// Project: LuvlyClans

using BepInEx;
using UnityEngine;
using HarmonyLib;
using LuvlyClans.Patches;

namespace LuvlyClans
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
  [BepInDependency(Jotunn.Main.ModGuid)]
  //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
  internal class LuvlyClans : BaseUnityPlugin
  {
        public const string PluginGUID = "com.aschenbuilder.luvlyclans";
        public const string PluginName = "LuvlyClans";
        public const string PluginVersion = "0.0.1";

        private readonly Harmony harmony = new Harmony(PluginGUID);

        private void Awake()
        {
            Jotunn.Logger.LogInfo("LuvlyClans Initialized");

            Jotunn.Logger.LogInfo("Patching Clan Permissions");

            harmony.PatchAll(typeof(DoorPatches));
            harmony.PatchAll(typeof(ContainerPatches));
            harmony.PatchAll(typeof(ShipControllsPatches));
            harmony.PatchAll(typeof(TeleportWorldPatches));
            harmony.PatchAll(typeof(ZNetPatches));
        }

#if DEBUG
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F6))
            {

            }
        }
#endif
  }
}