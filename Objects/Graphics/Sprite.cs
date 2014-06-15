using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Games;
using SalvagerEngine.Objects;
using SalvagerEngine.Components;

namespace SalvagerEngine.Objects.Graphics
{
    public class Sprite : GraphicsObject
    {
        /* Class Variables */

        Rectangle mSource;
        public Rectangle Source
        {
            get { return mSource; }
            protected set 
            { 
                mSource = value;
                Centre = new Vector2(mSource.Width, mSource.Height) * 0.5f;
            }
        }

        Texture2D mTexture;
        public Texture2D Texture
        {
            get { return mTexture; }
        }

        /* Constructors */

        public Sprite(Level component_owner, PhysicalObject parent, string texture_name)
            : base(component_owner, parent)
        {
            mTexture = component_owner.Game.Content.GetTexture(texture_name, out mSource);
            Centre = new Vector2(mSource.Width, mSource.Height) * 0.5f;
        }

        /* Overrides */

        protected override void Render(Camera camera)
        {
            /* Render the sprite */
            camera.Renderer.Draw(mTexture, GetActualPosition(), mSource, RenderColour, 
                Rotation + Parent.Rotation, Centre, Scale, SpriteEffects, Depth);
        }

        public override Point GetBounds()
        {
            return new Point((int)Math.Round(mSource.Width * Scale.X), (int)Math.Round(mSource.Height * Scale.Y));
        }
    }
}
