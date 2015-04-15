using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Utilities;
using SalvagerEngine.Utilities.Extensions;

using SalvagerEngine.Framework.Games;
using SalvagerEngine.Framework.Objects;
using SalvagerEngine.Framework.Components;
using SalvagerEngine.Framework.Objects.Graphics;

namespace SalvagerEngine.Framework.Objects.Physics.Collisions
{
    public class Circle : CollisionObject
    {
        /* Class Variables */

        float mRadius;
        public float Radius
        {
            get { return mRadius; }
            set { mRadius = value; }
        }
        
        /* Constructors */

        public Circle(GraphicsObject parent, float radius)
            : base(parent)
        {
            Radius = radius;
        }

        public Circle(GraphicsObject parent)
            : base(parent)
        {
            Radius = new Vector2(parent.GetBounds().X, parent.GetBounds().Y).Length() * 0.5f;
        }

        /* Overrides */

        protected override void Render(Camera camera)
        {
#if DEBUG
            /* Draw a collision circle */
            camera.Renderer.DrawCircle(DebugTexture, Color.Yellow, 1, Parent.GetActualPosition(), mRadius, MathHelper.TwoPi, 0.0f, true);
#endif
        }

        public override float CalculateAngularMass(float mass)
        {
            return mass;
        }

        protected override bool CheckForCollision(CollisionObject obj)
        {
            /* Get the circular collision */
            Circle other = obj as Circle;

            /* Check the collision */
            if (other != null)
            {
                float radii = mRadius + other.mRadius;
                return Vector2.DistanceSquared(other.Parent.GetActualPosition(), Parent.GetActualPosition()) - (radii * radii) < 0.0f;
            }
            else
            {
                return false;
            }
        }
    }
}
