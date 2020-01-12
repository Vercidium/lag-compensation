using System.Collections.Generic;

namespace lagcompensation
{
    public static class Helper
    {
        public static double Interpolate(double first, double second, double by)
        {
            return first + by * (second - first);
        }

        public static Vector3 Interpolate(in Vector3 first, in Vector3 second, double by)
        {
            double x = Interpolate(first.X, second.X, by);
            double y = Interpolate(first.Y, second.Y, by);
            double z = Interpolate(first.Z, second.Z, by);
            return new Vector3(x, y, z);
        }

        public static bool DoCollision(Projectile proj, List<Player> players)
        {
            return true;
        }
    }
}