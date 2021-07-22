using HarmonyLib;
using LuvlyClans.Types;
using Log = Jotunn.Logger;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public class PlayerPatches
    {
        [HarmonyPatch(typeof(Player), "UpdateBiome")]
        [HarmonyPostfix]
        public static void PlayerUpdateBiome(Player __instance, float dt)
        {
            Heightmap.Biome biome = Heightmap.FindBiome(__instance.transform.position);

            if (biome == Heightmap.Biome.BlackForest)
            {
                Log.LogInfo("Player entering Black Forest");

                Clan clan = LuvlyClans.clansman.GetClanByName(__instance.GetPlayerName());
                
                if (clan.clanName == "Wildlings")
                {
                    Character character = __instance;

                    if (character != null)
                    {
                        SEMan seman = character.GetSEMan();

                        if (seman != null)
                        {

                        }

                        Log.LogInfo("Adding the Curse of the Wildlings");
                    }
                }
            }

            return;
        }
    }
}