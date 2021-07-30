using LuvlyClans.Server.Types;
using LuvlyClans.Server;
using System;
using Log = Jotunn.Logger;

namespace LuvlyClans.Server.Utils
{
    public class ClansPermissions
    {
        public static bool IsSameClan(string aName, string bName)
        {
            if (ClansOperators.ClanMemberExists(aName) && ClansOperators.ClanMemberExists(bName))
            {
                Clan aClan = ClansOperators.GetClanByClanMember(aName);
                Clan bClan = ClansOperators.GetClanByClanMember(bName);

                if (aClan != null && bClan != null)
                {
                    return aClan.clanName == bClan.clanName;
                }
            }

            return false;
        }

        public static bool IsSameClan(long aSid, long bSid)
        {
            if (ClansOperators.ClanMemberExists(aSid) && ClansOperators.ClanMemberExists(bSid))
            {
                Clan aClan = ClansOperators.GetClanByClanMember(aSid);
                Clan bClan = ClansOperators.GetClanByClanMember(bSid);

                if (aClan != null && bClan != null)
                {
                    return aClan.clanName == bClan.clanName;
                }
            }

            return false;
        }
    }
}
