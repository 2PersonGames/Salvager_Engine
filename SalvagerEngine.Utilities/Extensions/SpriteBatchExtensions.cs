﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SalvagerEngine.Utilities.Extensions
{
    public static class SpriteBatchExtensions
    {
        public static void DrawLineVectored(this SpriteBatch renderer, Texture2D sprite, Color colour, int width, Vector2 a, Vector2 b, Rectangle source)
        {
            Vector2 origin = new Vector2(0.5f, 0.0f);
            Vector2 diff = b - a;
            Vector2 scale = new Vector2(width, diff.Length() / source.Height);
            float angle = (float)(Math.Atan2(diff.Y, diff.X)) - MathHelper.PiOver2;

            renderer.Draw(sprite, a, source, colour, angle, origin, scale, SpriteEffects.None, 1.0f);
        }

        public static void DrawLine(this SpriteBatch renderer, Texture2D sprite, Color colour, int width, Vector2 a, Vector2 b, float depth = 0.0f)
        {
            DrawLine(renderer, sprite, colour, width, a, b, sprite.Bounds, depth);
        }

        public static void DrawLine(this SpriteBatch renderer, Texture2D sprite, Color colour, int width, Vector2 a, Vector2 b, Rectangle source, float depth = 0.0f)
        {
            /* Calculate the angle and distance */
            float angle = (float)Math.Atan2(a.Y - b.Y, a.X - b.X) - MathHelper.PiOver2;
            float distance = Vector2.Distance(a, b);
            Vector2 offset = Vector2.Transform(new Vector2(width * 0.5f, 0.0f), Matrix.CreateRotationZ(angle));

            /* Calculate the rectangle */
            Rectangle rectangle = new Rectangle((int)Math.Round(b.X - offset.X),
                (int)Math.Round(b.Y - offset.Y), width, (int)Math.Round(distance));

            /* Render the line */
            renderer.Draw(sprite, rectangle, source, colour, angle, Vector2.Zero, SpriteEffects.None, depth);
        }

        public static void DrawLine(this SpriteBatch renderer, Texture2D sprite, int width, Color[] colours, Vector2[] vertices, Rectangle source)
        {
            /* Store the previous and next */
            Vector2 previous;
            Vector2 next;

            /* Iterate through the vertices */
            for (int i = 1; i < vertices.Length - 1; i++)
            {
                /* Swap the previous */
                previous = vertices[i - 1];
                next = vertices[i + 1];

                /* Render the vertex */
                DrawLine(renderer, sprite, colours[i], width, previous, next, source);
            }
        }

        public static void DrawCircle(this SpriteBatch renderer, Texture2D sprite, Color colour, int width, Vector2 centre, float radius, float arc, float start, bool clockwise)
        {
            DrawCircle(renderer, sprite, colour, width, centre, radius, arc, start, clockwise, sprite.Bounds);
        }

        public static void DrawCircle(this SpriteBatch renderer, Texture2D sprite, Color colour, int width, Vector2 centre, float radius, float arc, float start, bool clockwise, Rectangle source)
        {
            /* Calculate the number of vertices */
            int count = (int)Math.Ceiling((arc / MathHelper.TwoPi) * radius);
            float increment = MathHelper.TwoPi / radius;
            increment = clockwise ? increment : -increment;

            /* Create the vertex array */
            Vector2[] vertices = new Vector2[count + 2];
            Color[] colours = new Color[count + 2];
            for (int i = 0; i < vertices.Length; i++)
            {
                /* Calculate the next position */
                Vector2 vertex = Vector2.TransformNormal(new Vector2(0.0f, 1.0f),               /*< Points down */
                    Matrix.CreateRotationZ(start + (increment * i) - MathHelper.Pi)) * radius;  /*< Compensates for pointing down */

                /* Add the vertex */
                vertices[i] = vertex + centre;
                colours[i] = colour;
            }

            /* Render the circle */
            DrawLine(renderer, sprite, width, colours, vertices, source);
        }
    
    }
}
