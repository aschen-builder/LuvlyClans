using HarmonyLib;
using Jotunn;
using Log = Jotunn.Logger;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public class ZNetPatches
    {
        [HarmonyPatch(typeof(ZNet), "RPC_PeerInfo")]
        [HarmonyPostfix]
        public static void ZNetOnNewConnection(ZNet __instance, ZRpc rpc, ZPackage pkg)
        {
            if (!ZNetExtension.IsServerInstance(__instance))
            {
                ZPackage zpkg = new ZPackage();

                PlayerProfile p = Game.instance.GetPlayerProfile();

                if (p != null)
                {
                    zpkg.Write(p.GetName());
                    zpkg.Write(p.GetPlayerID());

                    Log.LogInfo($"Client sending message to {__instance.m_routedRpc.GetServerPeerID()}");
                    ZRoutedRpc.instance.InvokeRoutedRPC(ZRoutedRpc.instance.GetServerPeerID(), "RequestClans", new object[] { zpkg });
                    return;
                }

                Log.LogWarning("Null player on attempt to request clans from server");
            }
        }
    }
}