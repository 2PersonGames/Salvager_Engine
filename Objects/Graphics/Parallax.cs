using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Objects;
using SalvagerEngine.Components;

namespace SalvagerEngine.Objects.Graphics
{
    public class Parallax : GameObject
    {
        // Typedefs and Constants

        public enum ScrollType
        {
            Horizontal  = 0x1 << 0,
            Vertical    = 0x1 << 1
        };

        // Class Variables

        Texture2D mTexture;
        Rectangle mSource;

        float mDepth;
        float mSpeed;

        ScrollType mScrollType;

        // Constructors

        public Parallax(Level component_owner, string texture_name, float depth, float speed, ScrollType scroll_type)
            : base(component_owner, float.MaxValue)
        {
            // Copy some parameters
            mSpeed = speed;
            mDepth = depth;
            mScrollType = scroll_type;

            // Load the texture
            mTexture = component_owner.Game.Content.GetTexture(texture_name, out mSource);
        }

        // Overrides

        protected override void Render(Camera renderer)
        {
            // Retrieve the viewport
            Vector2 viewport = new Vector2((float)renderer.Renderer.GraphicsDevice.Viewport.Width,
                (float)renderer.Renderer.GraphicsDevice.Viewport.Height);

            // Calculate the scale
            Vector2 scale = new Vector2(viewport.X / mSource.Width, viewport.Y / mSource.Height);
            
            // Calculate the positions
            Vector2[] positions = null;
            switch (mScrollType)
            {
                case ScrollType.Horizontal:
                    positions = new Vector2[]
                    {
                        new Vector2(viewport.X, viewport.Y),
                        new Vector2(viewport.X, 0.0f)
                    };
                    break;

                case ScrollType.Vertical:
                    positions = new Vector2[]
                    {
                        new Vector2(0.0f, 0.0f),
                        new Vector2(0.0f, viewport.Y)
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

            // Modify the viewport
            viewport = viewport / mSpeed;

            // Iterate through each position
            for (int i = 0; i < positions.Length; i++)
            {
                // Offset the position according to the camera position
                positions[i].X -= renderer.View.Translation.X % viewport.X;
                positions[i].Y -= renderer.View.Translation.Y % viewport.Y;
                
                // Render the parallax
                renderer.Renderer.Draw(mTexture, positions[i], mSource, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, mDepth);
            }
        }

        public override float GetDepth()
        {
            return mDepth;
        }
    }
}
