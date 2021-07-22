using JSON = SimpleJson.SimpleJson;
using Log = Jotunn.Logger;
using LuvlyClans.Types;
using System;

namespace LuvlyClans
{
    public class ClansManager
    {
        private static ClansManager clansman;

        public string redisString;
        public string serverString;
        public string clientString;

        public Clans redisClans;
        public Clans serverClans;
        public Clans clientClans;

        private ClansManager()
        {
            if (LuvlyClans.isServer)
            {
                if (LuvlyClans.redisman != null)
                {
                    redisString = LuvlyClans.redisman.GetDatabase().StringGet("clans");

                    if (redisString != null)
                    {
                        Log.LogInfo($"retrieved clan data {redisString}");
                        serverString = redisString;
                    }
                }
            }
        }

        public static ClansManager GetInstance()
        {
            if (clansman == null)
            {
                clansman = new ClansManager();
            }

            return clansman;
        }

        public static Clans DeserializeClans(string data)
        {
            return JSON.DeserializeObject<Clans>(data);
        }

        public static string SerializeClans(Clans data)
        {
            return JSON.SerializeObject(data);
        }

        public class ClansComparison
        {
            public static bool CompareByString(string dataA, string dataB)
            {
                return dataA == dataB;
            }

            public static bool CompareByClans(Clans clansA, Clans clansB)
            {
                return clansA.Equals(clansB);
            }
        }

        public Clans GetRedisClans()
        {
            if (redisClans == null)
            {
                redisClans = redisString == null ? new Clans() : DeserializeClans(redisString);
            }

            return redisClans;
        }

        public Clans GetServerClans()
        {
            if (serverClans == null)
            {
                serverClans = serverString == null ? new Clans() : DeserializeClans(serverString);
            }

            return serverClans;
        }

        public Clans GetClientClans()
        {
            if (clientClans == null)
            {
                clientClans = clientString == null ? new Clans() : DeserializeClans(clientString);
            }

            return clientClans;
        }

        public string GetRedisString()
        {
            return redisString;
        }

        public string GetServerString()
        {
            return serverString;
        }

        public string GetClientString()
        {
            return clientString;
        }

        public void UpdateServerString()
        {
            if (serverClans == null)
            {
                serverClans = GetServerClans();
            }

            serverString = SerializeClans(serverClans);
        }

        public void UpdateClientString()
        {
            if (clientClans == null)
            {
                clientClans = GetClientClans();
            }

            clientString = SerializeClans(clientClans);
        }

        /** All further navigator methods will reference serverClans as god */

        public bool ClansHasClanByName(string name)
        {
            if (serverClans.clans != null && serverClans.clans.Length > 0)
            {
                return Array.Exists(serverClans.clans, (clan) => clan.clanName == name);
            }

            return false;
        }

        public bool ClansHasClanMemberByName(string name)
        {
            if (serverClans.clans != null && serverClans.clans.Length > 0)
            {
                Array.Exists(serverClans.clans, (clan) => ClanHasClanMemberByName(clan, name));
            }

            return false;
        }

        public bool ClanHasClanMemberByName(Clan clan, string clanMemberName)
        {
            if (clan.clanMembers != null && clan.clanMembers.Length > 0)
            {
                return Array.Exists(clan.clanMembers, (clanMember) => clanMember.playerName == clanMemberName);
            }

            return false;
        }

        public bool ClansHasClanMemberByID(long id)
        {
            if (serverClans.clans != null && serverClans.clans.Length > 0)
            {
                Array.Exists(serverClans.clans, (clan) => ClanHasClanMemberByID(clan, id));
            }

            return false;
        }

        public bool ClanHasClanMemberByID(Clan clan, long clanMemberID)
        {
            if (clan.clanMembers != null && clan.clanMembers.Length > 0)
            {
                return Array.Exists(clan.clanMembers, (clanMember) => clanMember.playerID == clanMemberID);
            }

            return false;
        }

        public Clan GetClanByName(string name)
        {
            if (serverClans.clans != null && serverClans.clans.Length > 0)
            {
                return Array.Find(serverClans.clans, (clan) => clan.clanName == name);
            }

            return null;
        }

        public Clan GetClanByClanMemberName(string name)
        {
            bool hasClanMember = ClansHasClanMemberByName(name);

            if (hasClanMember)
            {
                return Array.Find(serverClans.clans, (clan) => ClanHasClanMemberByName(clan, name));
            }

            return null;
        }

        public ClanMember GetClanMemberByName(string name)
        {
            Clan clan = GetClanByClanMemberName(name);

            if (clan != null)
            {
                return Array.Find(clan.clanMembers, (clanMember) => clanMember.playerName == name);
            }

            return null;
        }

        public Clan GetClanByClanMemberID(long id)
        {
            bool hasClanMember = ClansHasClanMemberByID(id);

            if (hasClanMember)
            {
                return Array.Find(serverClans.clans, (clan) => ClanHasClanMemberByID(clan, id));
            }

            return null;
        }

        public ClanMember GetClanMemberByID(long id)
        {
            Clan clan = GetClanByClanMemberID(id);

            if (clan != null)
            {
                return Array.Find(clan.clanMembers, (clanMember) => clanMember.playerID == id);
            }

            return null;
        }

        public Clan CreateClan(string name, ClanMember[] members, string code="", long id=0)
        {
            Clan clan = new Clan();

            clan.clanName = name;
            clan.clanCode = code;
            clan.clanMembers = members.Length > 0 ? members : new ClanMember[0];
            clan.clanID = id != 0 ? id : LuvlyClans.LongRandom(-100000000000000000, 100000000000000000, new Random());

            return clan;
        }

        public ClanMember CreateClanMember(string name, long pid=0, long sid=0, int rank = 4)
        {
            ClanMember member = new ClanMember();

            member.playerName = name;
            member.playerID = pid;
            member.playerSID = sid;
            member.playerRank = rank;

            return member;
        }

        public bool IsSameClanByClanMemberName(string nameA, string nameB)
        {
            Clan clanA = GetClanByClanMemberName(nameA);
            Clan clanB = GetClanByClanMemberName(nameB);

            if (clanA != null && clanB != null)
            {
                return clanA.clanName == clanB.clanName;
            }

            return false;
        }

        public bool IsSameClanByClanMemberID(long idA, long idB)
        {
            Clan clanA = GetClanByClanMemberID(idA);
            Clan clanB = GetClanByClanMemberID(idB);

            if (clanA != null && clanB != null)
            {
                return clanA.clanName == clanB.clanName;
            }

            return false;
        }
    }
}
