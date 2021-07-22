using HarmonyLib;

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
                    bool canDamage = !LuvlyClans.clansman.IsSameClanByClanMemberName(victim.GetPlayerName(), attacker.GetPlayerName());

                    if (!canDamage)
                    {
                        hit.ApplyModifier(0);
                    }
                }
            }
        }
    }
}