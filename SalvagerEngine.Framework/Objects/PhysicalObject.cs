using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Framework.Games;
using SalvagerEngine.Framework.Objects;
using SalvagerEngine.Framework.Components;

namespace SalvagerEngine.Framework.Objects
{
    public class PhysicalObject : GameObject
    {
        /* Class Variables */

        Vector2 mPosition;
        public Vector2 Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }
        float mRotation;
        public float Rotation
        {
            get { return mRotation; }
            set { mRotation = value; }
        }

        /* Constructors */

        public PhysicalObject(Vector2 position, Level component_owner)
            : base(component_owner, 0.0f)
        {
            mPosition = position;
        }

        /* Overrides */

        protected override void Tick(float delta)
        {
        }

        protected override void Render(Camera camera)
        {
        }
    }
}
