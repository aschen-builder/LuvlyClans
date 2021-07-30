namespace LuvlyClans.Server.Types
{
    public class Clans
    {
        public Clan[] clans { get; set; }
    }

    public class Clan
    {
        public string clanName { get; set; }
        public string clanAbbr { get; set; }
        public ClanMember[] clanMembers { get; set; }
    }

    public class ClanMember
    {
        public string playerName { get; set; }
        public string discordName { get; set; }
        public long[] playerIDs { get; set; }
        public long steamID { get; set; }
        public int rank { get; set; }

    }
}
