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
using LuvlyClans.Server.Redis;
using System;
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
        public static string pathBepinex = BepInEx.Paths.ConfigPath;
        public const string pathJSON = "luvly.clans.json";
        public const string pathJSONBackup = "luvly.clans.old.json";

        /** redis clans key */
        public const string redisMasterKey = "clans";

        /** redis config */
        public static ConfigEntry<bool> redisEnabled;
        public static ConfigEntry<string> redisHost;
        public static ConfigEntry<int> redisPort;
        public static ConfigEntry<string> redisPass;
        public static ConfigEntry<int> redisDB;

        /** redis global singleton */
        public static RedisManager redisman;

        /** clans manager global singleton */
        public static ClansManager clansman;

        /** game instance gates */
        public static bool isServer;
        public static bool isClient;

        private readonly Harmony harmony = new Harmony(PluginGUID);

        private void Awake()
        {
            CreateConfigValues();
            LoadPatches();
        }

        private void Update() { }

        private void LoadPatches()
        {
            Log.LogInfo("Loading patches");

            harmony.PatchAll();
        }

        private void RedisConfigValues()
        {
            redisEnabled = Config.Bind("Redis Config", "ENABLED", true, new ConfigDescription("enable Redis server for clan hosting", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            redisHost = Config.Bind("Redis Config", "HOST", "localhost", new ConfigDescription("hostname of Redis server", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            redisPort = Config.Bind("Redis Config", "PORT", 6379, new ConfigDescription("port of Redis server", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            redisPass = Config.Bind("Redis Config", "PASSWORD", "password", new ConfigDescription("password of Redis server", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
            redisDB = Config.Bind("Redis Config", "DB", -1, new ConfigDescription("db in Redis server", null, new ConfigurationManagerAttributes { IsAdminOnly = true }));
        }

        private void CreateConfigValues()
        {
            Log.LogInfo("Initializing config");

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
        public static long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }
    }
}