namespace LuvlyClans.Types
{
    public class ClanMember
    {
        public string playerName { get; set; }
        public long[] playerIDs { get; set; }
        public long playerSID { get; set; }
        public int playerRank { get; set; }
    }

    public class Clan
    {
        public string clanName { get; set; }

        public string clanCode { get; set; }

        public long clanID { get; set; }

        public bool isPublic { get; set; }

        public ClanMember[] clanMembers { get; set; }
    }

    public class Clans
    {
        public Clan[] clans { get; set; }
    }
}
