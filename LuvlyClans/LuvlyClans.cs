// LuvlyClans
// a Valheim mod skeleton using Jötunn
// 
// File:    LuvlyClans.cs
// Project: LuvlyClans

using BepInEx;
using HarmonyLib;
using Log = Jotunn.Logger;
using JSON = SimpleJson.SimpleJson;
using LuvlyClans.Patches;
using LuvlyClans.Types.Clans;
using System.IO;

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

        public static Clans AllClans;

        private readonly Harmony harmony = new Harmony(PluginGUID);

        private void Awake()
        {
           Log.LogInfo("LuvlyClans Initialized");
            LoadClansDB();
            LoadPatches();
        }

        private void Update()
        {
            //WriteToClansDB();

            return;
        }

        private void LoadClansDB()
        {
            string path = Path.Combine(ConfigPath, ClansPath);

            if (File.Exists(path))
            {
                Log.LogMessage("Found Clans DB");
                string file = File.ReadAllText(path);

                AllClans = JSON.DeserializeObject<Clans>(file);
                Log.LogInfo($"Loaded {AllClans.m_clans.Length} clans");
                Log.LogInfo(AllClans.m_clans[0].m_members[0].m_playerID);

                return;
            }

            Log.LogMessage("No Clans DB Found");
        }

        private void LoadPatches()
        {
            Log.LogInfo("Patching Clan Permissions");

            harmony.PatchAll(typeof(ContainerPatches));
            harmony.PatchAll(typeof(CharacterPatches));
        }

        public void BackupClansDB()
        {
            if (File.Exists(Path.Combine(ConfigPath, ClansPath)))
            {
                if (File.Exists(Path.Combine(ConfigPath, BackupClansPath)))
                {
                    File.Delete(Path.Combine(ConfigPath, BackupClansPath));
                }

                File.Copy(Path.Combine(ConfigPath, ClansPath), Path.Combine(ConfigPath, BackupClansPath));

                Log.LogMessage("Clans DB Backed Up Successfully");

                return;
            }
        }

        private void WriteToClansDB()
        {
            /** 
             * create global clans update array to then merge with AllClans
             */
            Log.LogMessage("Updateing Clans DB");

            BackupClansDB();

            string file = JSON.SerializeObject(AllClans);

            if (file != null)
            {
                File.WriteAllText(Path.Combine(ConfigPath, ClansPath), file);
            }

            Log.LogMessage("Successfully Updated Clans DB");

            return;
        }
    }
}