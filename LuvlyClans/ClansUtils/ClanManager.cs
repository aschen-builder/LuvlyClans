using LC = LuvlyClans.LuvlyClans;
using LuvlyClans.Types.Clans;
using System;

namespace LuvlyClans.ClansUtils
{
    public class ClanManager
    {
        public static bool IsSameClanByPlayerID(long playerA, long playerB)
        {
            Clan clanA = GetClanByPlayerID(playerA);
            Clan clanB = GetClanByPlayerID(playerB);

            return clanA.m_clanName == clanB.m_clanName;
        }

        public static bool ClanHasPlayerByPlayerID(Clan clan, long playerID)
        {
            return Array.Exists(clan.m_members, member => member.m_playerID == playerID);
        }

        public static bool ClanHasPlayerByPlayerName(Clan clan, string playerName)
        {
            return Array.Exists(clan.m_members, member => member.m_playerName == playerName);
        }

        public static Clan GetClanByPlayerID(long playerID)
        {
            Clan[] clans = LC.AllClans.m_clans;

            Clan playerClan = Array.Find(clans, clan => ClanHasPlayerByPlayerID(clan, playerID));

            if (playerClan.m_clanName != null)
            {
                return playerClan;
            }

            return null;
        }

        public static Clan GetClanByPlayerName(string playerName)
        {
            Clan[] clans = LC.AllClans.m_clans;

            Clan playerClan = Array.Find(clans, clan => ClanHasPlayerByPlayerName(clan, playerName));

            if (playerClan.m_clanName != null)
            {
                return playerClan;
            }

            return null;
        }

        public static Member GetClanMemberByPlayerName(string playerName)
        {
            Clan playerClan = GetClanByPlayerName(playerName);

            if (playerClan != null)
            {
                Member clanMember = Array.Find(playerClan.m_members, member => member.m_playerName == playerName);

                return clanMember;
            }

            return null;
        }

        public static bool ClanMemberHasPlayerIDByPlayerName(string playerName)
        {
            Member clanMember = GetClanMemberByPlayerName(playerName);

            if (clanMember != null)
            {
                /** a null/undefined value will be assigned
                 * 0 int during deserialization
                 */
                return clanMember.m_playerID != 0;
            }

            return false;
        }
    }
}
