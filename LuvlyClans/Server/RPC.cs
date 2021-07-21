using Log = Jotunn.Logger;
using ClansHelper;
using LuvlyClans.Server.Types;

namespace LuvlyClans.Server
{
    public class RPC
    {
        public static void RPC_RequestClans(long sender, ZPackage pkg)
        {
            ZPackage zpkg = new ZPackage();

            if (pkg != null && pkg.Size() > 0)
            {
                Log.LogInfo("Receiving message from peer");

                ZNetPeer peer = ZNet.instance.GetPeer(sender);

                if (peer != null)
                {
                    Log.LogInfo("Received Clans data request from valid peer");

                    string peerPlayerName = pkg.ReadString();
                    long peerPlayerID = pkg.ReadLong();

                    Clan peerClan = ClansHelper.ClansHelper.GetClanByPlayerName(peerPlayerName, false);
                    Member peerMember = ClansHelper.ClansHelper.GetClanMemberFromClanByPlayerName(peerClan, peerPlayerName);

                    bool syncFlag = false;

                    if (peerMember != null)
                    {
                        if (peerMember.m_playerID != peerPlayerID)
                        {
                            Log.LogWarning($"Updating Player [{peerPlayerName}] with current playerID [{peerPlayerID}]");
                            peerMember.m_playerID = peerPlayerID;
                            syncFlag = true;
                        }

                        Log.LogInfo("Writing updated clans to db");
                        DB.Write(LuvlyClans.GetServerClansJSON());
                    } else
                    {
                        Log.LogWarning($"Player [{peerPlayerName}::{peerPlayerID}] does not exist in clans DB, adding to Wildlings");

                        Member nw = new Member();
                        nw.m_playerName = peerPlayerName;
                        nw.m_playerID = peerPlayerID;

                        ClansHelper.ClansHelper.CreateWildling(peerPlayerName, peerPlayerID);

                        syncFlag = true;

                        Log.LogInfo("Writing updated clans to db");
                        DB.Write(LuvlyClans.GetServerClansJSON());
                    }

                    zpkg.Write(LuvlyClans.GetServerClansJSON());

                    ZRoutedRpc.instance.InvokeRoutedRPC(syncFlag ? 0L : sender, "ResponseClans", new object[] { zpkg });

                    return;
                }

                Log.LogWarning("Result is no peer finding sender");

                zpkg.Write("no peer");

                ZRoutedRpc.instance.InvokeRoutedRPC(sender, "BadRequest", new object[] { zpkg });
            }

            zpkg.Write("null package received");

            ZRoutedRpc.instance.InvokeRoutedRPC(sender, "BadRequest", new object[] { zpkg });
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
