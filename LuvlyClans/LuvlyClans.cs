// LuvlyClans
// a Valheim mod skeleton using Jötunn
//
// File:    LuvlyClans.cs
// Project: LuvlyClans

using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Jotunn;
using Jotunn.Configs;
using Jotunn.Managers;
using Jotunn.Utils;
using LuvlyClans.Patches;
using LuvlyClans.Server;
using LuvlyClans.Server.Redis;
using LuvlyClans.Server.Types;
using LuvlyClans.Client.GUI;
using UnityEngine;
using JSON = SimpleJson.SimpleJson;
using Log = Jotunn.Logger;

namespace LuvlyClans
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class LuvlyClans : BaseUnityPlugin
    {
        /** plugin config */
        public const string PluginGUID = "com.aschenbuilder.luvlyclans";
        public const string PluginName = "LuvlyClans";
        public const string PluginVersion = "0.0.1";

        /** deprecated db file paths */
        public static string m_path_config = BepInEx.Paths.ConfigPath;
        public const string m_path_db = "luvly.clans.json";
        public const string m_path_db_backup = "luvly.clans.old.json";

        /** redis config */
        public static ConfigEntry<bool> m_redis_enabled;
        public static ConfigEntry<string> m_redis_host;
        public static ConfigEntry<int> m_redis_port;
        public static ConfigEntry<string> m_redis_pass;
        public static ConfigEntry<int> m_redis_db;

        /** redis global singleton */
        public static RedisManager redisman;

        /** game instance gates */
        public static bool m_is_server;
        public static bool m_is_client;

        /** db/item instances */
        public static DB m_db_server;
        public static Clans m_clans_server;
        public static Clans m_clans_client;
        public static Clans m_clans_db;

        private ClansMenu clansMenu;

        private readonly Harmony harmony = new Harmony(PluginGUID);

        private void Awake()
        {
            CreateConfigValues();

            clansMenu = new ClansMenu();

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

            //redisman = RedisManager.GetInstance();
        }

        private void Update()
        {
            if (ZInput.instance != null)
            {
                clansMenu.ToggleMenu();
            }
        }

        private void RedisConfigValues()
        {
            m_redis_enabled = Config.Bind("Redis Config", "ENABLED", true, new ConfigDescription("enable Redis server for clan hosting", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            m_redis_host = Config.Bind("Redis Config", "HOST", "127.0.0.1", new ConfigDescription("hostname of Redis server", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            m_redis_port = Config.Bind("Redis Config", "PORT", 6379, new ConfigDescription("port of Redis server", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            m_redis_pass = Config.Bind("Redis Config", "PASSWORD", "password", new ConfigDescription("password of Redis server", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            m_redis_db = Config.Bind("Redis Config", "DB", -1, new ConfigDescription("db in Redis server", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }

        public void CreateConfigValues()
        {
            Config.SaveOnConfigSet = true;

            RedisConfigValues();

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