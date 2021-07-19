using LuvlyClans.Server.Types;
using System;
using Log = Jotunn.Logger;

namespace ClansHelper
{
    internal class ClansHelper
    {
        internal static bool m_has_clans_server = LuvlyClans.LuvlyClans.m_clans_server != null && LuvlyClans.LuvlyClans.m_clans_server.m_clans.Length > 0;
        internal static bool m_has_clans_client = LuvlyClans.LuvlyClans.m_clans_client != null && LuvlyClans.LuvlyClans.m_clans_client.m_clans.Length > 0;

        public static Member GetClanMemberFromClanByPlayerID(Clan clan, long playerID)
        {
            if (clan.m_members != null && clan.m_members.Length > 0)
            {
                return Array.Find(clan.m_members, (Member m) => m.m_playerID == playerID);
            }

            return null;
        }

        public static Member GetClanMemberFromClanByPlayerName(Clan clan, string playerName)
        {
            if (clan.m_members != null && clan.m_members.Length > 0)
            {
                return Array.Find(clan.m_members, (Member m) => m.m_playerName == playerName);
            }

            return null;
        }

        public static bool ClanHasMemberByPlayerID(Clan clan, long playerID)
        {
            return Array.Exists(clan.m_members, (Member m) => m.m_playerID == playerID);
        }

        public static bool ClanHasMemberByPlayerName(Clan clan, string playerName)
        {
            return Array.Exists(clan.m_members, (Member m) => m.m_playerName == playerName);
        }

        public static Clan GetClanByPlayerID(long playerID, bool isClient)
        {
            Clans clans = isClient ? LuvlyClans.LuvlyClans.m_clans_client : LuvlyClans.LuvlyClans.m_clans_server;
            bool hasClans = isClient ? m_has_clans_client : m_has_clans_server;

            if (hasClans)
            {
                return Array.Find(clans.m_clans, (Clan c) => ClanHasMemberByPlayerID(c, playerID));
            }

            return null;
        }

        public static Clan GetClanByPlayerName(string playerName, bool isClient)
        {
            Clans clans = isClient ? LuvlyClans.LuvlyClans.m_clans_client : LuvlyClans.LuvlyClans.m_clans_server;
            bool hasClans = isClient ? m_has_clans_client : m_has_clans_server;

            if (hasClans)
            {
                return Array.Find(clans.m_clans, (Clan c) => ClanHasMemberByPlayerName(c, playerName));
            }

            return null;
        }

        public static string GetClanNameByPlayerID(long playerID, bool isClient)
        {
            bool hasClans = isClient ? m_has_clans_client : m_has_clans_server;

            if (hasClans)
            {
                Clan c = GetClanByPlayerID(playerID, isClient);

                if (c != null && c.m_clanName != null)
                {
                    return c.m_clanName;
                }
            }

            return null;
        }

        public static bool IsSameClanByPlayerID(long a_playerID, long b_playerID, bool isClient)
        {
            bool hasClans = isClient ? m_has_clans_client : m_has_clans_server;

            if (hasClans)
            {
                Clan a_clan = GetClanByPlayerID(a_playerID, isClient);
                Clan b_clan = GetClanByPlayerID(b_playerID, isClient);

                return a_clan.m_clanName == b_clan.m_clanName;
            }

            /** no clans == ffa */
            return true;
        }

        public static bool IsSameClanByPlayerName(string a_playerName, string b_playerName, bool isClient)
        {
            bool hasClans = isClient ? m_has_clans_client : m_has_clans_server;

            if (hasClans)
            {
                Clan a_clan = GetClanByPlayerName(a_playerName, isClient);
                Clan b_clan = GetClanByPlayerName(b_playerName, isClient);

                return a_clan.m_clanName == b_clan.m_clanName;
            }

            return true;
        }
    }
}
