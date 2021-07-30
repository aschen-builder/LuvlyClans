using Log = Jotunn.Logger;
using LuvlyClans.Server.Commands;
using LuvlyClans.Types;
using System.Collections.Generic;

namespace LuvlyClans.Server
{
    public class RPC
    {
        public static void RPC_RequestClans(long sender, ZPackage pkg)
        {
            ZPackage zpkg = new ZPackage();
            bool SYNC_FLAG = false; /** flag to sync all clients (including redis clients) */

            if (pkg != null && pkg.Size() > 0)
            {
                Log.LogInfo("Server received request from peer");

                ZNetPeer peer = ZNet.instance.GetPeer(sender);

                if (peer != null)
                {
                    Log.LogInfo("Server received clans data request from valid peer");

                    string playerName = pkg.ReadString();
                    long playerID = pkg.ReadLong();

                    bool playerExistsByName = LuvlyClans.clansman.ClansHasClanMemberByName(playerName);
                    bool playerExistsByID = LuvlyClans.clansman.ClansHasClanMemberByID(playerID);

                    if (!playerExistsByName)
                    {
                        Log.LogWarning($"Unable to find Player [{playerName}] in clans by name");

                        ClanMember newMember = LuvlyClans.clansman.CreateClanMember(playerName, playerID);

                        /** need to extract this following block to a method in clansman */
                        Clan wildlings = LuvlyClans.clansman.GetClanByName("Wildlings");

                        if (wildlings != null)
                        {
                            List<ClanMember> membersList = new List<ClanMember>();

                            for (int i = 0; i < wildlings.clanMembers.Length; i++)
                            {
                                membersList.Add(wildlings.clanMembers[i]);
                            }

                            membersList.Add(newMember);

                            wildlings.clanMembers = membersList.ToArray();

                            SYNC_FLAG = true;
                        }

                        Log.LogWarning($"Player [{playerName}] added to wildlings");
                    }
                    else
                    {
                        if (!playerExistsByID)
                        {
                            Log.LogWarning($"Player [{playerName}] does not have PlayerID set");

                            ClanMember member = LuvlyClans.clansman.GetClanMemberByName(playerName);

                            member.playerID = playerID;

                            Log.LogWarning($"PlayerID now set for Player [{playerName}]");
                        }
                    }

                    if (SYNC_FLAG)
                    {
                        LuvlyClans.clansman.SyncRedisClans();
                    }

                    zpkg.Write(LuvlyClans.clansman.GetServerString());

                    ZRoutedRpc.instance.InvokeRoutedRPC(SYNC_FLAG ? 0L : sender, "ResponseClans", new object[] { zpkg });

                    return;
                }

                Log.LogWarning("Server was unable to find a peer from the sender");

                zpkg.Write("no peer");

                ZRoutedRpc.instance.InvokeRoutedRPC(sender, "BadRequest", new object[] { zpkg });
            }

            zpkg.Write("null package received");

            ZRoutedRpc.instance.InvokeRoutedRPC(sender, "BadRequest", new object[] { zpkg });
        }

        public static void RPC_UpdateClans(long sender, ZPackage pkg)
        {
            ZPackage zpkg = new ZPackage();

            bool SYNC_FLAG = false; /** flag to sync all clients (including redis clients) */

            if (pkg != null && pkg.Size() > 0)
            {
                Log.LogInfo("Server received request from peer");

                ZNetPeer peer = ZNet.instance.GetPeer(sender);

                if (peer != null)
                {
                    Log.LogInfo("Server received clans command from valid peer");

                    string[] args = pkg.ReadString().Split(',');

                    if (args.Length > 0)
                    {
                        switch (args[0])
                        {
                            case "join":
                                ClanCommands.Join(args[1]);
                                break;
                            case "leave":
                                ClanCommands.Leave(pkg.ReadString());
                                break;
                            case "add":
                                ClanCommands.Add(args[1], args[2]);
                                break;
                            case "remove":
                                ClanCommands.Remove(args[1], args[2]);
                                break;
                            case "create":
                                ClanCommands.Create(args[1], args[2], !(args[3] == "0"));
                                break;
                            case "destroy":
                                break;
                        }

                        SYNC_FLAG = true;
                    }
                }
            }

            if (SYNC_FLAG)
            {
                LuvlyClans.clansman.UpdateServerString();

                zpkg.Write(LuvlyClans.clansman.GetServerString());

                ZRoutedRpc.instance.InvokeRoutedRPC(0L, "ResponseClans", new object[] { zpkg });
            }
        }

        public static void RPC_ResponseClans(long sender, ZPackage pkg)
        {
            return;
        }

        public static void RPC_BadRequest(long sender, ZPackage pkg)
        {
            return;
        }
    }
}
