using HarmonyLib;
using LuvlyClans.Utils;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    class CharacterPatches
    {
        [HarmonyPatch(typeof(Character), "Damage")]
        [HarmonyPrefix]
        public static void CharacterDamage(Character __instance, HitData hit)
        {
            ZDOID attacker = hit.m_attacker;

            if (attacker.IsNone() || !__instance.IsPlayer())
            {
                return;
            }

            Player attackerPlayer = PlayerUtils.GetGlobalPlayerByZDOID(attacker);
            Player characterPlayer = PlayerUtils.GetGlobalPlayerByZDOID(__instance.GetZDOID());

            bool attackerGate = attackerPlayer is null || !attackerPlayer.IsPlayer();
            bool characterGate = characterPlayer is null || !characterPlayer.IsPlayer();

            if (attackerGate || characterGate)
            {
                return;
            }

            TribeManager.TribeManager tm = new TribeManager.TribeManager(characterPlayer, attackerPlayer);

            if (tm.isSameTribe)
            {
                Jotunn.Logger.LogWarning($"Clan FF Protection Activating for Player[{attackerPlayer.GetPlayerName()}] attack on Player[{characterPlayer.GetPlayerName()}]");
                hit.ApplyModifier(0);
                return;
            }
        }
    }
}
