using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Framework.Games;
using SalvagerEngine.Framework.Objects;
using SalvagerEngine.Framework.Components;

namespace SalvagerEngine.Framework.Objects.Effects
{
    public class Rotator : EffectObject
    {
        /* Class Variables */

        public new PhysicalObject Parent
        {
            get { return base.Parent as PhysicalObject; }
        }

        float mRotation;
        public float Rotation
        {
            get { return mRotation; }
            set { mRotation = value; }
        }

        /* Constructors */

        public Rotator(PhysicalObject parent, float rotation)
            : base(parent, 0.0f)
        {
            mRotation = rotation;
        }

        /* Overrides */

        protected override void Tick(float delta)
        {
            Parent.Rotation += mRotation * delta;
        }

        public override float GetDepth()
        {
            return -1.0f;
        }
    }
}
