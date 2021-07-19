using LuvlyClans.Server.Types;
using System.IO;
using JSON = SimpleJson.SimpleJson;

namespace LuvlyClans.Server
{
    public class DB
    {
        public static string m_path_db;
        public static string m_path_db_backup;

        public static bool m_has_db;
        public static bool m_has_db_backup;

        public DB(string path, string path_backup = null)
        {
            m_path_db = path;
            m_path_db_backup = path_backup == null ? "old." + path : path_backup;

            m_has_db = Exists();
            m_has_db_backup = Exists(m_path_db_backup);
        }

        public static bool Exists(string path = null)
        {
            return File.Exists(path == null ? m_path_db : path);
        }

        public Clans Read()
        {
            if (m_has_db)
            {
                string data = File.ReadAllText(m_path_db);

                if (data != null)
                {
                    return JSON.DeserializeObject<Clans>(data);
                }

                return new Clans();
            }

            return new Clans();
        }

        public static void Write(string json)
        {
            if (File.Exists(m_path_db_backup))
            {
                File.Delete(m_path_db_backup);
            }

            File.Copy(m_path_db, m_path_db_backup);
            File.WriteAllText(m_path_db, json);
        }
    }
}
