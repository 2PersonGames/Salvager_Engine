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
        Vector2 mCentre;
        Color mColour;
        Vector2 mScale;

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
            mCentre = new Vector2(mSource.Width, mSource.Height) * 0.5f;

            // Calculate the scale
            mScale = new Vector2(component_owner.GraphicsDevice.Viewport.Width / (float)mSource.Width, 
                component_owner.GraphicsDevice.Viewport.Height / (float)mSource.Height);
        }

        // Overrides

        protected override void Render(Camera camera)
        {
            // Retrieve the viewport
            Vector2 viewport = new Vector2((float)camera.Viewport.Width, 
                (float)camera.Viewport.Height);

            // Retrieve the camera position
            Vector2 position = camera.Position + (mCentre * mScale);

            // Calculate the positions
            Vector2[] positions = null;
            Vector2 speed = new Vector2(mSpeed);
            switch (mScrollType)
            {
                case ScrollType.Horizontal:
                    speed = new Vector2(speed.X, 1.0f);
                    positions = new Vector2[]
                    {
                        new Vector2(0.0f, position.Y - viewport.Y),
                        new Vector2(-viewport.X, position.Y - viewport.Y)
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
                    mSource, mColour, 0.0f, mCentre, mScale, (SpriteEffects)i, mDepth);
            }
        }

        public override float GetDepth()
        {
            return mDepth;
        }
    }
}
