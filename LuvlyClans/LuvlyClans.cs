// LuvlyClans
// a Valheim mod skeleton using Jötunn
// 
// File:    LuvlyClans.cs
// Project: LuvlyClans

using BepInEx;
using HarmonyLib;
using Jotunn;
using LuvlyClans.Patches;
using LuvlyClans.Server.Types;
using LuvlyClans.Server;
using Log = Jotunn.Logger;
using JSON = SimpleJson.SimpleJson;
using Jotunn.Utils;
using BepInEx.Configuration;
using Jotunn.Managers;

namespace LuvlyClans
{
  [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
  [BepInDependency(Main.ModGuid)]
  [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
  internal class LuvlyClans : BaseUnityPlugin
  {
        public const string PluginGUID = "com.aschenbuilder.luvlyclans";
        public const string PluginName = "LuvlyClans";
        public const string PluginVersion = "0.0.1";

        public static string m_path_config = BepInEx.Paths.ConfigPath;
        public const string m_path_db = "luvly.clans.json";
        public const string m_path_db_backup = "luvly.clans.old.json";
        private ConfigEntry<string> m_path_db_config;

        public static bool m_is_server;
        public static bool m_is_client;

        public static DB m_db_server;
        public static Clans m_clans_server;
        public static Clans m_clans_client;
        public static Clans m_clans_db;

        private readonly Harmony harmony = new Harmony(PluginGUID);

        private void Awake()
        {
            CreateConfigValues();

            Log.LogInfo("Loading patches");

            harmony.PatchAll(typeof(GamePatches));
            harmony.PatchAll(typeof(ZNetPatches));
            harmony.PatchAll(typeof(DoorPatches));
            harmony.PatchAll(typeof(ContainerPatches));
            harmony.PatchAll(typeof(MinimapPatches));
            harmony.PatchAll(typeof(ShipControllsPatches));
            harmony.PatchAll(typeof(TeleportWorldPatches));
            harmony.PatchAll(typeof(CharacterPatches));
            harmony.PatchAll(typeof(EnemyHudPatches));
        }

        private void Update()
        {
            return;
        }

        public void CreateConfigValues()
        {
            Config.SaveOnConfigSet = true;

            m_path_db_config = Config.Bind(
                "Server Config",
                "DB_PATH",
                m_path_config + m_path_db,
                new ConfigDescription("absolute path to clans db", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

            SynchronizationManager.OnConfigurationSynchronized += (obj, attr) =>
            {
                if (attr.InitialSynchronization)
                {
                    Log.LogMessage("Initial Config sync event received");
                }
                else
                {
                    Log.LogMessage("Config sync event received");
                }
            };
        }

        public static string GetServerClansJSON()
        {
            return JSON.SerializeObject(m_clans_server);
        }
    }
}