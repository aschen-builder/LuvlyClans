using LuvlyClans.Server.Types;
using System.IO;
using Log = Jotunn.Logger;

namespace LuvlyClans.Server
{
    public class ServerManager
    {
        public static void LoadClans()
        {
            LuvlyClans.m_db_server = new DB(
                Path.Combine(LuvlyClans.m_path_config, LuvlyClans.m_path_db), 
                Path.Combine(LuvlyClans.m_path_config, LuvlyClans.m_path_db_backup)
            );

            LuvlyClans.m_clans_db = LuvlyClans.m_db_server != null ? LuvlyClans.m_db_server.Read() : new Clans();
            LuvlyClans.m_clans_server = LuvlyClans.m_clans_db;

            Log.LogInfo($"Loaded {LuvlyClans.m_clans_db.m_clans.Length} clans");
        }
    }
}
