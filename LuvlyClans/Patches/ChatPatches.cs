using HarmonyLib;
using LuvlyClans.Types;
using System;
using ZNE = Jotunn.ZNetExtension;
using Log = Jotunn.Logger;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public class ChatPatches
    {
        [HarmonyPatch(typeof(Chat), "InputText")]
        [HarmonyPrefix]
        public static bool ChatInputText(Chat __instance)
        {
            string msg = __instance.m_input.text;
            string prefix = "/clans";
            int prefixIndex = prefix.Length;

            if (msg == null)
            {
                return true;
            }

            Log.LogInfo(msg.ToLower());
            Log.LogInfo(msg.ToLower().StartsWith("/clans "));

            if (msg.ToLower().StartsWith("/clans "))
            {
                Log.LogInfo("Clans command received");

                string[] args = msg.Substring(prefixIndex).Split(' ');

                if (args.Length > 0)
                {
                    string argString = "";

                    for (int i = 0; i < args.Length; i++)
                    {
                        argString += i == args.Length - 1 ? args[i] : $"{args[i]},";
                    }

                    ZPackage zpkg = new ZPackage();
                    zpkg.Write(argString);

                    if (args[0] == "leave")
                    {
                        zpkg.Write(Player.m_localPlayer.GetPlayerName());
                    }

                    ZRoutedRpc.instance.InvokeRoutedRPC("UpdateClans", new object[] { zpkg });

                    return false;
                }
            }

            return true;
        }
    }
}