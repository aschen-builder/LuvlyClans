// LuvlyClans
// a Valheim mod skeleton using Jötunn
//
// File:    LuvlyClans.cs
// Project: LuvlyClans

using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Jotunn;
using Jotunn.Managers;
using Jotunn.Utils;

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

        private readonly Harmony harmony = new Harmony(PluginGUID);

        /** redis config */
        public const string redisSection = "Redis Config";
        public static ConfigEntry<bool> redisEnabled;
        public static ConfigEntry<string> redisHost;
        public static ConfigEntry<int> redisPort;
        public static ConfigEntry<string> redisPass;
        public static ConfigEntry<int> redisDB;

        private void Awake()
        {
            InitConfig();
        }

        private void Update() { }

        private void RedisConfig()
        {
            redisEnabled = Config.Bind(
                redisSection,
                "ENABLED",
                true,
                new ConfigDescription("enable Redis server for clan hosting", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            
            redisHost = Config.Bind(
                redisSection,
                "HOST",
                "127.0.0.1",
                new ConfigDescription("hostname of Redis server", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));

            redisPort = Config.Bind(
                redisSection,
                "PORT",
                6379,
                new ConfigDescription("port of Redis server", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            
            redisPass = Config.Bind(
                redisSection,
                "PASSWORD",
                "password",
                new ConfigDescription("password of Redis server", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            
            redisDB = Config.Bind(
                "Redis Config",
                "DB",
                0,
                new ConfigDescription("db in Redis server", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }

        private void InitConfig()
        {
            Config.SaveOnConfigSet = true;

            RedisConfig();

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
    }
}