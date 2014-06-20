using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SalvagerEngine.Tools
{
    public static class DrawManager
    {
        public static Color PremultiplyAlpha(Color colour, float alpha)
        {
            /* Check the alpha */
            if (alpha >= 1.0f)
            {
                colour.A = 255;
                return colour;
            }

            /* Calculate the RGB */
            colour.R = (byte)Math.Round(colour.R * alpha);
            colour.G = (byte)Math.Round(colour.G * alpha);
            colour.B = (byte)Math.Round(colour.B * alpha);

            /* Calculate the alpha */
            int total = 0;
            float local = 0.0f;
            if (colour.R > 0)
            {
                local += colour.R;
                total++;
            }
            if (colour.G > 0)
            {
                local += colour.G;
                total++;
            }
            if (colour.B > 0)
            {
                local += colour.B;
                total++;
            }

            /* Compute the alpha */
            if (total > 0)
            {
                colour.A = (byte)Math.Round(local / total);
            }
            else
            {
                colour.A = (byte)Math.Round(255.0f * alpha);
            }

            /* Return the colour */
            return colour;
        }
    }
}
