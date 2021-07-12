using LuvlyClans.TribeManager.Types;

namespace LuvlyClans.TribeManager
{
    class TribeManager
    {
        internal Player player1;
        internal Player player2;
        public bool isSameTribe;

        public TribeManager(Player player_1, Player player_2)
        {
            player1 = player_1;
            player2 = player_2;
            isSameTribe = IsSameTribeByPlayer();
        }
        public bool IsSameTribeByPlayer()
        {
            if(player1 is null || player2 is null )
            {
                return false;
            }

            PlayerTribe pt_player1 = new PlayerTribe(player1);
            PlayerTribe pt_player2 = new PlayerTribe(player2);

            return pt_player1.tribe == pt_player2.tribe;
        }
    }
}
