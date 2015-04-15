using System;

using Microsoft.Xna.Framework;

namespace SalvagerEngine.Utilities.Extensions
{
    public static class ColorExtensions
    {
        public static Color PremultiplyAlpha(this Color colour)
        {
            /* Check the alpha */
            float alpha = colour.A / 255.0f;
            if (alpha < 1.0f)
            {
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
            }

            /* Return the colour */
            return colour;
        }
    }
}
