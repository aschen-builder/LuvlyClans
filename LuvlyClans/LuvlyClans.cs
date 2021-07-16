// LuvlyClans
// a Valheim mod skeleton using Jötunn
// 
// File:    LuvlyClans.cs
// Project: LuvlyClans

using BepInEx;
using HarmonyLib;
using Log = Jotunn.Logger;

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

        public static string ConfigPath = Paths.ConfigPath;
        public const string ClansPath = "luvly.clans.json";
        public const string BackupClansPath = "luvly.clans.old.json";

        private readonly Harmony harmony = new Harmony(PluginGUID);

        private void Awake()
        {
            return;
        }

        private void Update()
        {
            return;
        }
    }
}