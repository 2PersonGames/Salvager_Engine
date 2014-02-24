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
        Texture2D mTexture;

        /* Constructors */

        public Sprite(Level component_owner, PhysicalObject parent, string texture_name)
            : base(component_owner, parent)
        {
            throw new NotImplementedException("Texture content manager not finished yet!");

            Centre = new Vector2(mSource.Width, mSource.Height) * 0.5f;
        }

        /* Overrides */

        protected override void Tick(float delta)
        {
            throw new NotImplementedException();
        }

        protected override void Render(SpriteBatch renderer)
        {
            /* Calculate the offset */
            Vector2 offset = Vector2.Transform(Offset, Matrix.CreateRotationZ(Parent.Rotation));

            /* Render the sprite */ 
            renderer.Draw(mTexture, Parent.Position + offset, mSource, RenderColour, Rotation, Centre, Scale, SpriteEffects, Depth);
        }
    }
}
