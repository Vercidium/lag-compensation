namespace lagcompensation
{
    public struct Vector3
    {
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double X;
        public double Y;
        public double Z;

        public static Vector3 operator +(in Vector3 v, in Vector3 w)
        {
            return new Vector3(v.X + w.X, v.Y + w.Y, v.Z + w.Z);
        }

        public static Vector3 operator *(in Vector3 v, int i)
        {
            return new Vector3(v.X * i, v.Y * i, v.Z * i);
        }
    }
}