using System;
using System.Collections.Generic;

using SalvagerEngine.Interfaces.Objects;
using SalvagerEngine.Interfaces.Components;
using SalvagerEngine.Interfaces.Objects.Graphics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SalvagerEngine.Objects
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

        public PhysicalObject(Vector2 position, ILevel component_owner)
            : base(component_owner, 0.0f)
        {
            mPosition = position;
        }

        /* Overrides */

        protected override void Tick(float delta)
        {
        }

        protected override void Render(ICamera camera)
        {
        }
    }
}
