using LuvlyClans.Types;
using Log = Jotunn.Logger;
using JSON = SimpleJson.SimpleJson;

namespace LuvlyClans.Client
{
    public class RPC
    {
        public static void RPC_RequestClans(long sender, ZPackage pkg)
        {
            return;
        }

        public static void RPC_UpdateClans(long sender, ZPackage pkg)
        {
            return;
        }

        public static void RPC_ResponseClans(long sender, ZPackage pkg)
        {
            Log.LogInfo("Receiving message from server");

            if (sender == ZRoutedRpc.instance.GetServerPeerID() && pkg != null && pkg.Size() > 0)
            {
                string msg = pkg.ReadString();

                if (msg != null && msg != "" && msg != "no peer")
                {
                    Log.LogInfo("Client received clans string from Server");

                    LuvlyClans.clansman.clientString = msg;
                    LuvlyClans.clansman.GetClientClans();

                    return;
                }

                if (msg == "no peer")
                {
                    Log.LogWarning("Received: no peer from server on Clans request");
                    return;
                }

                Log.LogWarning("Unable to receive clans from server relog to try again");
            }
        }

        public static void RPC_BadRequest(long sender, ZPackage pkg)
        {
            if (sender == ZRoutedRpc.instance.GetServerPeerID() && pkg != null && pkg.Size() > 0)
            {
                string msg = pkg.ReadString();

                if (msg != null && msg != "")
                {
                    Log.LogWarning($"Received {msg} from server");
                }
            }
        }
    }
}
