using System;
using Microsoft.Xna.Framework;

namespace SalvagerEngine.Utilities.Extensions
{
    public static class Vector2Extensions
    {
        public static float CrossProduct(this Vector2 me, Vector2 other)
        {
            return (me.X * other.Y) - (me.Y * other.X);
        }

        public static float AngleBetween(this Vector2 me, Vector2 other)
        {
            Vector2 direction = Vector2.Normalize(other - me);
            return (float)Math.Atan2(direction.Y, direction.X);
        }

        public static float ToRadians(this Vector2 me)
        {
            me.Normalize();
            return (float)Math.Atan2(me.Y, me.X);
        }

        public static Point ToPoint(this Vector2 me)
        {
            return new Point((int)Math.Round(me.X), (int)Math.Round(me.Y));
        }

        public static Vector2 Modulus(this Vector2 me, Vector2 other)
        {
            return new Vector2(me.X % other.X, me.Y % other.Y);
        }
    }
}
