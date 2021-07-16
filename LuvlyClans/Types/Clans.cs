using System;

namespace LuvlyClans.Types.Clans
{
    public class Member
    {
        public string m_playerName { get; set; }
        public long m_playerID { get; set; }
        public long m_playerSID { get; set; }
        public int m_playerRank { get; set; }
    }

    public class Clan
    {
        public string m_clanName { get; set; }
        public Member[] m_members { get; set; }
    }

    public class Clans
    {
        public Clan[] m_clans { get; set; }
    }
}
