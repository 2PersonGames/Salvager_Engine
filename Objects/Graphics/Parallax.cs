using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Objects;
using SalvagerEngine.Components;
using SalvagerEngine.Tools.Extensions;

namespace SalvagerEngine.Objects.Graphics
{
    public class Parallax : GameObject
    {
        // Typedefs and Constants

        public enum ScrollType
        {
            Horizontal = 0x1 << 0,
            Vertical = 0x1 << 1
        };

        // Class Variables

        Texture2D mTexture;
        Rectangle mSource;
        Color mColour;

        float mDepth;
        float mSpeed;

        ScrollType mScrollType;

        // Constructors

        public Parallax(Level component_owner, string texture_name, float depth, float speed, ScrollType scroll_type, Color colour)
            : base(component_owner, float.MaxValue)
        {
            // Copy some parameters
            mSpeed = speed;
            mDepth = depth;
            mScrollType = scroll_type;
            mColour = colour;

            // Load the texture
            mTexture = component_owner.Game.Content.GetTexture(texture_name, out mSource);
        }

        // Overrides

        protected override void Render(Camera camera)
        {
            // Retrieve the viewport
            Vector2 viewport = new Vector2((float)camera.Renderer.GraphicsDevice.Viewport.Width,
                (float)camera.Renderer.GraphicsDevice.Viewport.Height);

            // Calculate the scale
            Vector2 scale = new Vector2(viewport.X / mSource.Width, viewport.Y / mSource.Height);

            // Retrieve the camera position
            Vector2 position = camera.GetPosition();

            // Calculate the positions
            Vector2[] positions = null;
            Vector2 speed = new Vector2(mSpeed);
            switch (mScrollType)
            {
                case ScrollType.Horizontal:
                    speed = new Vector2(speed.X, 1.0f);
                    positions = new Vector2[]
                    {
                        new Vector2(0.0f, viewport.Y - position.Y),
                        new Vector2(-viewport.X, viewport.Y - position.Y)
                    };
                    break;

                case ScrollType.Vertical:
                    speed = new Vector2(1.0f, speed.Y);
                    positions = new Vector2[]
                    {
                        new Vector2(viewport.X - position.X, 0.0f),
                        new Vector2(viewport.X - position.X, -viewport.Y)
                    };
                    break;
                    
                case ScrollType.Horizontal | ScrollType.Vertical:
                    positions = new Vector2[]
                    {
                        new Vector2(0.0f, 0.0f),
                        new Vector2(0.0f, viewport.Y),
                        new Vector2(viewport.X, viewport.Y),
                        new Vector2(viewport.X, 0.0f)
                    };
                    break;
                    
                default:
                    positions = new Vector2[0];
                    break;
            }

            // Iterate through each position
            for (int i = 0; i < positions.Length; i++)
            {
                // Render the parallax
                camera.Renderer.Draw(mTexture, position - (((position * speed) - positions[i]).Modulus(viewport * 2.0f) - (viewport * 0.5f)), 
                    mSource, mColour, 0.0f, Vector2.Zero, scale, (SpriteEffects)i, mDepth);
            }
        }

        public override float GetDepth()
        {
            return mDepth;
        }
    }
}
