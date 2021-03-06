﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using SalvagerEngine.Objects;
using SalvagerEngine.Objects.Graphics;
using SalvagerEngine.Interfaces.Components;
using SalvagerEngine.Objects.Physics.Collisions;
using SalvagerEngine.Interfaces.Objects.Physics.Collisions;

namespace SalvagerEngine.Objects.Physics
{
    public class Gravity : GameObject
    {
        // Class Variables

        Vector2 mPosition;
        public Vector2 Position
        {
            get { return mPosition; }
            protected set { mPosition = value; }
        }

        float mPower;
        public float Power
        {
            get { return mPower; }
            protected set { mPower = value; }
        }

        Circle mEffectiveRange;

        // Constructors

        public Gravity(ILevel component_owner, Vector2 position, float power, float effective_range)
            : base(component_owner, 0.0f)
        {
            // Copy the defaults
            mPosition = position;
            mPower = power;

            // Generate the collision circle
            if (effective_range > 0.0f)
            {
                mEffectiveRange = new Circle(null, effective_range)
                {
                    OnCollision = delegate(ICollisionObject me, ICollisionObject other)
                    {
                        ApplyGravity(other.Parent.Parent as PhysicalObject);
                    }
                };
                mEffectiveRange.AddCollideableType(typeof(PhysicalObject));
                AddChild(mEffectiveRange);
            }
        }

        // Overrides

        protected override void Tick(float delta)
        {
            // Call the base method
            base.Tick(delta);

            // Check the effective range
            if (mEffectiveRange == null)
            {
                // Apply gravity to all physics objects
                foreach (PhysicalObject obj in ComponentOwner.Root.ForEachAll<PhysicalObject>())
                {
                    ApplyGravity(obj);
                }
            }
        }

        // Utilities

        void ApplyGravity(PhysicalObject obj)
        {
            throw new NotImplementedException();
        }
    }
}
