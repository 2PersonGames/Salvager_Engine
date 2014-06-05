using System;
using Microsoft.Xna.Framework;

namespace SalvagerEngine.Tools.Extensions
{
    public static class FloatExtensions
    {
        public static float ClampRotation(this float rotation)
        {
            /* Test if the value is greater or equal than two pi */
            if (rotation >= MathHelper.TwoPi)
            {
                rotation %= MathHelper.TwoPi;
            }

            /* Test if the rotation has gone below zero */
            if (rotation < 0.0f)
            {
                rotation += MathHelper.TwoPi;
            }

            /* Return the value */
            return rotation;
        }

        public static float LerpAngle(this float current, float target, float amount)
        {
            /* Declare the rotation */
            float rotation = current;

            /* Check if the difference */
            if (target < current)
            {
                rotation = target + MathHelper.TwoPi;
                rotation = rotation - current > current - target
                    ? MathHelper.Lerp(current, target, amount)
                    : MathHelper.Lerp(current, rotation, amount);

            }
            else if (target > current)
            {
                rotation = target - MathHelper.TwoPi;
                rotation = target - current > current - rotation
                    ? MathHelper.Lerp(current, rotation, amount)
                    : MathHelper.Lerp(current, target, amount);

            }

            /* Return the angle */
            return MathHelper.WrapAngle(rotation);
        }

        public static float LerpFloat(this float current, float target, float amount)
        {
            return LerpAngle(current, target, amount);
            //return LerpAngle(current * MathHelper.TwoPi, target * MathHelper.TwoPi, amount * MathHelper.TwoPi) / MathHelper.TwoPi;
        }

        public static Vector2 ToVector2(this float radians)
        {
            return new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));
        }

        public static float Randomise(float max, float min, Random random)
        {
            return min + (float)random.NextDouble() * (max - min);
        }
    }
}
