using LuvlyClans.Types;
using System;

namespace LuvlyClans.Server.Commands
{
    public class ClanCommands
    {
        private static bool IsAdmin(long uid)
        {
            return Jotunn.ZNetExtension.IsAdmin(ZNet.instance, uid);
        }

        public static void Join(string clanName)
        {
            if (clanName != null && Player.m_localPlayer != null)
            {
                if (LuvlyClans.clansman.ClansHasClanByName(clanName))
                {
                    Clan clan = LuvlyClans.clansman.GetClanByName(clanName);

                    if (clan.isPublic)
                    {
                        ClanMember member = new ClanMember
                        {
                            playerName = Player.m_localPlayer.GetPlayerName(),
                            playerID = Player.m_localPlayer.GetPlayerID(),
                            playerRank = 4
                        };

                        LuvlyClans.clansman.KickClanMember(Player.m_localPlayer.GetPlayerName());

                        LuvlyClans.clansman.AddNewClanMemberToClan(clan, member);
                    }
                }
            }
        }

        public static void Leave(string name)
        {
            LuvlyClans.clansman.KickClanMember(name);
        }

        public static void Add(string clanName, string playerName)
        {
            Clan requesterClan = LuvlyClans.clansman.GetClanByClanMemberName(Player.m_localPlayer.GetPlayerName());

            Clan addClan = LuvlyClans.clansman.GetClanByName(clanName);

            Player memberPlayer = null;

            if (addClan.clanName == requesterClan.clanName)
            {
                foreach (Player player in Player.GetAllPlayers())
                {
                    if (player.GetPlayerName() == playerName)
                    {
                        memberPlayer = player;
                    }
                }

                if (memberPlayer == null)
                {
                    ClanMember newAddMember = LuvlyClans.clansman.CreateClanMember(playerName);

                    LuvlyClans.clansman.AddNewClanMemberToClan(addClan, newAddMember);

                    return;
                }

                ClanMember addMember = LuvlyClans.clansman.GetClanMemberByName(playerName);

                LuvlyClans.clansman.KickClanMember(playerName);

                LuvlyClans.clansman.AddNewClanMemberToClan(addClan, addMember);
            }
        }

        public static void Remove(string clanName, string playerName)
        {

        }

        public static void Create(string name, string abbr, bool isPublic)
        {
            Clan clan = new Clan
            {
                clanCode = abbr,
                clanName = name,
                isPublic = isPublic,
                clanID = LuvlyClans.LongRandom(-100000000000000000, 100000000000000000, new Random())
            };

            LuvlyClans.clansman.AddNewClanToClans(clan);
        }
    }
}
