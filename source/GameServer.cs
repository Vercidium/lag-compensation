using System.Collections.Generic;
using System.Diagnostics;

namespace lagcompensation
{
    class GameServer
    {
        public List<Player> Players;
        public List<Projectile> Projectiles;
        History history;

        Stopwatch tickWatch = new Stopwatch();

        public GameServer()
        {
            history = new History(this);
            tickWatch.Start();
        }

        public int PresentTick
        {
            get
            {
                return (int)(tickWatch.ElapsedMilliseconds / Constants.TICK_TIME);
            }
        }

        void HandleMouseDown(Player player)
        {
            // Calculate the player's current tick
            double tick = PresentTick - player.AverageTickLag;

            // Create a projectile
            Projectile proj = player.CreateProjectile();

            // Process the projectile
            history.ProjectileLoop(tick, proj);
        }
    }
}
