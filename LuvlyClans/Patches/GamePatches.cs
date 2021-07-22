using HarmonyLib;
using Jotunn;
using LuvlyClans.Server.Redis;
using System;
using Log = Jotunn.Logger;

namespace LuvlyClans.Patches
{
    [HarmonyPatch]
    public class GamePatches
    {
        [HarmonyPatch(typeof(Game), "Start")]
        [HarmonyPrefix]
        public static void GameStart()
        {
            LuvlyClans.isServer = ZNet.instance.IsServer() || ZNet.instance.IsDedicated() || ZNet.instance.IsLocalInstance();
            LuvlyClans.isClient = ZNet.instance.IsClientInstance();

            if (LuvlyClans.isServer)
            {
                Log.LogInfo("Server instance found");

                LuvlyClans.redisman = RedisManager.GetInstance();

                RegisterServerRPCs();
            }

            if (LuvlyClans.isClient)
            {
                Log.LogInfo("Client instance found");

                RegisterClientRPCs();
            }

            LuvlyClans.clansman = ClansManager.GetInstance();
        }

        public static void RegisterServerRPCs()
        {
            Log.LogInfo("Registering Clans RPCs");

            ZRoutedRpc.instance.Register("RequestClans", new Action<long, ZPackage>(Server.RPC.RPC_RequestClans));
            ZRoutedRpc.instance.Register("ResponseClans", new Action<long, ZPackage>(Server.RPC.RPC_ResponseClans));
            ZRoutedRpc.instance.Register("BadMessage", new Action<long,ZPackage>(Server.RPC.RPC_BadRequest));
        }

        public static void RegisterClientRPCs()
        {
            Log.LogInfo("Registering Clans RPCs");

            ZRoutedRpc.instance.Register("RequestClans", new Action<long, ZPackage>(Client.RPC.RPC_RequestClans));
            ZRoutedRpc.instance.Register("ResponseClans", new Action<long, ZPackage>(Client.RPC.RPC_ResponseClans));
            ZRoutedRpc.instance.Register("BadMessage", new Action<long, ZPackage>(Client.RPC.RPC_BadRequest));
        }
    }
}
