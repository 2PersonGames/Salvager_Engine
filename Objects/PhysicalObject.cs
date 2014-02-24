using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SalvagerEngine.Games;
using SalvagerEngine.Objects;
using SalvagerEngine.Components;

namespace SalvagerEngine.Objects
{
    public class PhysicalObject : GameObject
    {
        /* Class Variables */

        Vector2 mPosition;
        public Vector2 Position
        {
            get { return mPosition; }
        }
        float mRotation;
        public float Rotation
        {
            get { return mRotation; }
            set { mRotation = value; }
        }

        /* Constructors */

        public PhysicalObject(Vector2 position, Level component_owner)
            : base(component_owner)
        {
            mPosition = position;
        }

        /* Overrides */

        protected override void Initialise(out float tick_max)
        {
            tick_max = 0.0f;
        }

        protected override void Tick(float delta)
        {
        }

        protected override void Render(SpriteBatch renderer)
        {
        }
    }
}
