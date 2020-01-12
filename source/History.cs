using System.Collections.Generic;
using System.Linq;

namespace lagcompensation
{
    class History
    {
        public History(GameServer parent)
        {
            this.parent = parent;
        }

        GameServer parent;
        List<Record> records;

        Record GetClosestRecord(int tick)
        {
            for (int i = 0; i < records.Count; i++)
                if (records[i].tick == tick)
                    return records[i];

            return records.Last();
        }

        public void ProjectileLoop(double tick, Projectile proj)
        {
            Record lastRecord = records.Last();
            // If we have checked each Record and the projectile is still alive, add it to the present
            if (tick > lastRecord.tick)
            {
                parent.Projectiles.Add(proj);
                return;
            }

            bool repeat = false;

            // If tick is 4.75, we want to interpolate 25% between the oldest and most recent entry
            double interpolation = 1 - (tick - (int)tick);

            // Interpolate between the most recent Record and the present
            if ((int)tick == lastRecord.tick)
            {
                repeat = ProcessCollision(lastRecord.players, parent.Players, interpolation, proj);
            }

            // Interpolate between two Records
            else
            {
                var eOld = GetClosestRecord((int)tick);         // Less recent
                var eRecent = GetClosestRecord((int)tick + 1);     // More recent

                repeat = ProcessCollision(eOld.players, eRecent.players, interpolation, proj);
            }

            // Some projectiles are hitcast, which means they have infinite velocity and therefore we only need to check one frame
            if (proj.IsHitCast)
                return;

            // If the projectile didn't hit anything
            if (repeat)
            {
                // Move the projectile
                proj.Position += proj.Velocity * Constants.TICK_TIME;

                // Check the next most recent record
                ProjectileLoop(tick + 1, proj);
            }
        }

        bool ProcessCollision(List<Player> playersOld, List<Player> playersRecent, double interpolation, Projectile proj)
        {
            // We don't want to modify the player positions stored in the Record, so we use a new list
            List<Player> playerCopies = new List<Player>();

            foreach (var pOld in playersOld)
            {
                // Create a copy of the player containing only the information
                // we need for collision (position, velocity, crouching, aiming, etc)
                var pCopy = pOld.Copy();

                // Find a player with a matching ID in the more recent Record
                var pRecent = GetPlayer(playersRecent, pOld.ID);

                // If the player hasn't disconnected, interpolate their position between the two ticks for more accurate collision
                if (pRecent != null)
                    pCopy.position = Helper.Interpolate(pOld.position, pRecent.position, interpolation);

                playerCopies.Add(pCopy);
            }

            // Calculate collision, deal damage to the players and map, etc
            // Returns true if the projectile didn't hit anything
            return Helper.DoCollision(proj, playerCopies);
        }

        Player GetPlayer(List<Player> players, byte ID)
        {
            for (int i = players.Count - 1; i >= 0; i--)
                if (players[i].ID == ID)
                    return players[i];

            return null;
        }
    }
}