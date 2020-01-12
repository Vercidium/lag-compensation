using System.Collections.Generic;

namespace lagcompensation
{
    public class Player
    {
        public byte ID;
        public Vector3 position;
        double ping;

        List<double> TickLagHistory = new List<double>();
        double AccumulatedTickLag = 0;

        public Player Copy()
        {
            return new Player()
            {
                ID = ID,
                position = position,
            };
        }

        public void AddTickLag(double d)
        {
            TickLagHistory.Add(d);

            AccumulatedTickLag += d;

            if (TickLagHistory.Count > Constants.LAG_HISTORY_MAX)
            {
                AccumulatedTickLag -= TickLagHistory[0];
                TickLagHistory.RemoveAt(0);
            }
        }

        public double AverageTickLag
        {
            get
            {
                // Use ping as an approximation until TickLagHistory is populated
                if (TickLagHistory.Count < Constants.LAG_HISTORY_MAX)
                    return ping / Constants.TICK_TIME;

                return AccumulatedTickLag / Constants.LAG_HISTORY_MAX;
            }
        }

        public Projectile CreateProjectile()
        {
            return new Projectile();
        }
    }
}