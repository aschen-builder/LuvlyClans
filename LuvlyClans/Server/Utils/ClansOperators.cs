using LuvlyClans.Server.Types;
using System;
using Log = Jotunn.Logger;

namespace LuvlyClans.Server.Utils
{
    public class ClansOperators
    {
        public static bool ClansExist()
        {
            Clans clansObject = LuvlyClans.clansMan.GetClansObject();

            return clansObject != null && clansObject.clans.Length > 0;
        }

        public static bool ClanExists(string name)
        {
            if (ClansExist())
            {
                return Array.Exists(GetClans().clans, (clan) => clan.clanName == name);
            }

            return false;
        }

        public static bool ClanMemberExists(string name)
        {
            if (ClansExist())
            {
                return Array.Exists(GetClans().clans, (clan) => Array.Exists(clan.clanMembers, (member) => member.playerName == name));
            }

            return false;
        }

        public static bool ClanMemberExists(long id, string prop="pid")
        {
            if (ClansExist())
            {
                return Array.Exists(GetClans().clans, (clan) => Array.Exists(clan.clanMembers, (member) => prop != "pid" ? (member.steamID == id) : (Array.Exists(member.playerIDs, (i) => i == id))));
            }

            return false;
        }

        public static Clans GetClans()
        {
            return ClansExist() ? LuvlyClans.clansMan.GetClansObject() : null;
        }

        public static Clan GetClan(string name)
        {
            if (ClansExist() && ClanExists(name))
            {
                return Array.Find(GetClans().clans, (clan) => clan.clanName == name);
            }

            return null;
        }

        public static Clan GetClanByClanMember(string name)
        {
            if (ClansExist() && ClanMemberExists(name))
            {
                return Array.Find(GetClans().clans, (clan) => Array.Exists(clan.clanMembers, (member) => member.playerName == name));
            }

            return null;
        }

        public static Clan GetClanByClanMember(long sid)
        {
            if (ClansExist() && ClanMemberExists(sid))
            {
                return Array.Find(GetClans().clans, (clan) => Array.Exists(clan.clanMembers, (member) => member.steamID == sid));
            }

            return null;
        }

        public static ClanMember GetClanMember(string name)
        {
            if (ClansExist() && ClanMemberExists(name))
            {
                ClanMember member = null;

                foreach (Clan clan in GetClans().clans)
                {
                    if (Array.Exists(clan.clanMembers, (m) => m.playerName == name))
                    {
                        member = Array.Find(clan.clanMembers, (m) => m.playerName == name);
                    }
                }

                return member;
            }

            return null;
        }

        public static ClanMember GetClanMember(long sid)
        {
            if (ClansExist() && ClanMemberExists(sid))
            {
                ClanMember member = null;

                foreach (Clan clan in GetClans().clans)
                {
                    if (Array.Exists(clan.clanMembers, (m) => m.steamID == sid))
                    {
                        member = Array.Find(clan.clanMembers, (m) => m.steamID == sid);
                    }
                }

                return member;
            }

            return null;
        }
    }
}
