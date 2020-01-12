using System;
using System.Collections.Generic;
using System.Text;

namespace lagcompensation
{
    public class Record
    {
        public int tick;
        public List<Player> players = new List<Player>();

        // As there are 16 players max, we use a linear lookup
        public Player GetPlayer(byte ID)
        {
            for (int i = 0; i < players.Count; i++)
                if (players[i].ID == ID)
                    return players[i];

            return null;
        }
    }
}
