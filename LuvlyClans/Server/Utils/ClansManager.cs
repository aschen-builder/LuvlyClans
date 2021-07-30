using LuvlyClans.Server.Types;

namespace LuvlyClans.Server.Utils
{
    public class ClansManager
    {
        private static ClansManager instance;

        public Clans clansObject;

        private ClansManager()
        {
            if (LuvlyClans.redisMan != null)
            {
                SetClansObject();
            }
        }

        public static ClansManager GetInstance()
        {
            if (instance == null)
            {
                instance = new ClansManager();
            }

            return instance;
        }

        public Clans GetClansObject()
        {
            if (clansObject == null)
            {
                SetClansObject();
            }

            return clansObject;
        }

        public void SetClansObject(Clans obj=null)
        {
            if (obj == null)
            {
                clansObject = SimpleJson.SimpleJson.DeserializeObject<Clans>(LuvlyClans.redisMan.GetClansString());
                return;
            }

            clansObject = obj;
        }
    }
}
