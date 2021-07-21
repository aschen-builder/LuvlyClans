using HarmonyLib;
using Log = Jotunn.Logger;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public class CharacterPatches
    {
        [HarmonyPatch(typeof(Character), "Damage")]
        [HarmonyPrefix]
        public static void CharacterDamager(Character __instance, HitData hit)
        {
            if (hit != null)
            {
                Player victim = __instance as Player;
                Player attacker = hit.GetAttacker() as Player;

                if (victim && attacker)
                {
                    bool canDamage = !ClansHelper.ClansHelper.IsSameClanByPlayerName(victim.GetPlayerName(), attacker.GetPlayerName(), true);

                    if (!canDamage)
                    {
                        hit.ApplyModifier(0);
                    }
                }
            }
        }
    }
}