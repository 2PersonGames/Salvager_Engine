using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Utilities.Extensions;

using SalvagerEngine.Objects;
using SalvagerEngine.Objects.Effects;
using SalvagerEngine.Objects.Physics.Collisions;
using SalvagerEngine.Interfaces.Objects.Graphics;
using SalvagerEngine.Interfaces.Objects.Physics.Collisions;

namespace SalvagerEngine.Objects.Physics
{
    public class PhysicsEffect : EffectObject
    {
        /* Class Variables */

        public new PhysicalObject Parent
        {
            get { return base.Parent as PhysicalObject; }
        }

        float mRestitution;
        public float Restitution
        {
            get { return mRestitution; }
            set { mRestitution = value; }
        }

        float mPower;
        public float Power
        {
            get { return mPower; }
            set { mPower = value; }
        }
        float mFriction;
        public float Friction
        {
            get { return mFriction; }
            set { mFriction = value; }
        }

        Vector2 mForce;
        float mMass;
        public float Mass
        {
            get { return mMass; }
        }
        Vector2 mVelocity;
        public Vector2 Velocity
        {
            get { return mVelocity; }
        }

        float mAngularForce;
        float mAngularVelocity;
        float mAngularMass;

        ICollisionObject mCollisionMesh;
        public ICollisionObject CollisionMesh
        {
            get { return mCollisionMesh; }
            set 
            {
                /* Remove the previous collision mesh */
                if (mCollisionMesh != null)
                {
                    RemoveChild(mCollisionMesh);
                }

                /* Add the new collision mesh */
                mCollisionMesh = value;
                if (mCollisionMesh != null)
                {
                    AddChild(mCollisionMesh);
                }
            }
        }

        /* Constructors */

        public PhysicsEffect(PhysicalObject parent)
            : base(parent, 0.0f)
        {
            mRestitution = 1.0f;
            mPower = 0.0f;
            mFriction = 0.0f;
            mForce = Vector2.Zero;
            mMass = 1.0f;
            mAngularForce = 0.0f;
            mAngularVelocity = 0.0f;
            mAngularMass = 1.0f;
        }

        /* Overrides */

        protected override void Tick(float delta)
        {
            /* Calculate the linear velocity */
            mVelocity += (mForce / mMass) * delta;
            mForce = Vector2.Zero;

            /* Increment the position */
            Parent.Position += mVelocity * delta;

            /* Update the angular mass */
            if (mCollisionMesh != null && mCollisionMesh.Enabled)
            {
                mAngularMass = mCollisionMesh.CalculateAngularMass(mMass);
            }
            else
            {
                mAngularMass = Mass;
            }

            /* Calculate the angular velocity */
            mAngularVelocity += (mAngularForce / mAngularMass) * delta;
            mAngularForce = 0.0f;

            /* Increment the rotation */
            if (!(mAngularVelocity == 0.0f || float.IsNaN(mAngularVelocity)))
            {
                Parent.Rotation += mAngularVelocity * delta;
            }

            /* Decrement the velocity */
            if (mFriction > 0.0f)
            {
                mVelocity -= mVelocity * mFriction * delta;
                mAngularVelocity -= mAngularVelocity * mFriction * delta;
            }
        }

        protected override void Render(ICamera camera)
        {
        }

        /* Events */

        public void MoveToward(PhysicalObject target)
        {
            MoveToward(target.Position);
        }

        public void MoveToward(PhysicalObject target, float multiplier)
        {
            MoveToward(target.Position, multiplier);
        }

        public void MoveToward(Vector2 position)
        {
            MoveToward(position, 1.0f);
        }

        public void MoveToward(Vector2 position, float multiplier)
        {
            /* Call move */
            Move(Vector2.Subtract(position, Parent.Position), multiplier);
        }

        public void Move(Vector2 direction)
        {
            Move(direction, 1.0f);
        }

        public virtual void Move(Vector2 direction, float multiplier)
        {
            /* Clamp the values */
            multiplier = MathHelper.Clamp(multiplier, 0.0f, 1.0f);
            direction.Normalize();

            /* Calculate the new velocity */
            ApplyForce(mPower * multiplier * direction);
        }

        public virtual void ApplyForce(Vector2 force)
        {
            mForce += force * 100.0f;
        }

        public void ApplyForce(Vector2 force, Vector2 point)
        {
            /* Apply the force */
            ApplyForce(force);

            /* Calculate the torque */
            mAngularForce += (Parent.Position - point).CrossProduct(force);
        }

        public void ForceImmediateHalt()
        {
            mForce = Vector2.Zero;
            mVelocity = Vector2.Zero;

            mAngularForce = 0.0f;
            mAngularVelocity = 0.0f;
        }

        public void ResolveCollision(PhysicsEffect other, Vector2 location, Vector2 normal)
        {
            /* Get the normal of the impact point */
            Vector2 rAP = location - Parent.Position;
            rAP = new Vector2(rAP.Y, -rAP.X);

            Vector2 rBP = location - other.Parent.Position;
            rBP = new Vector2(rBP.Y, -rBP.X);

            /* Calculte the angular velocities */
            float angular_mass_a = mAngularMass;
            float angular_mass_b = other.mAngularMass;

            /* Calculate the impulse */
            float impulse = 0.0f;
            {
                /* Calculate the resultant velocity */
                Vector2 resultant = (mVelocity + mAngularVelocity * rAP) -
                    (other.mVelocity + other.mAngularVelocity * rBP);

                /* Compute the normal */
                impulse = (-(1 + mRestitution) * Vector2.Dot(resultant, normal)) /
                    (Vector2.Dot(normal, normal) * ((1.0f / mMass) + (1.0f / other.mMass)) +
                    ((float)Math.Pow(Vector2.Dot(rAP, normal), 2.0d) / angular_mass_a) +
                    ((float)Math.Pow(Vector2.Dot(rBP, normal), 2.0d) / angular_mass_b));
            }

            /* Calculate the new linear velocities */
            Vector2 velocity_a = mVelocity + ((normal * impulse) / mMass);
            Vector2 velocity_b = other.mVelocity - ((normal * impulse) / other.mMass);

            /* Calculate the new angular velocities */
            float angular_velocity_a = mAngularVelocity + Vector2.Dot(rAP, normal * impulse) / angular_mass_a;
            float angular_velocity_b = other.mAngularVelocity + Vector2.Dot(rBP, normal * impulse) / angular_mass_b;

            /* Check the resultant velocities */
            if (!float.IsNaN(velocity_a.X) && !float.IsNaN(velocity_a.Y) &&
                !float.IsNaN(velocity_b.X) && !float.IsNaN(velocity_b.Y) &&
                !float.IsNaN(angular_velocity_a) && !float.IsNaN(angular_velocity_b))
            {
                mVelocity = velocity_a;
                other.mVelocity = velocity_b;

                mAngularVelocity = angular_velocity_a;
                other.mAngularVelocity = angular_velocity_b;
            }
            else
            {
                /* Log the issues with the collision */
                List<string> messages = new List<string>();

                messages.Add("Collision resultant velocities NaN");

                messages.Add("Object A velocity: " + velocity_a.ToString());
                messages.Add("Object A resultant velocity: " + Velocity.ToString());
                messages.Add("Object A angular velocity: " + mAngularVelocity.ToString());
                messages.Add("Object A resultant angular velocity: " + angular_velocity_a.ToString());
                messages.Add("Object A mass: " + Mass.ToString());

                messages.Add("Object B velocity: " + velocity_b.ToString());
                messages.Add("Object B resultant velocity: " + other.Velocity.ToString());
                messages.Add("Object B angular velocity: " + other.mAngularVelocity.ToString());
                messages.Add("Object B resultant angular velocity: " + angular_velocity_b.ToString());
                messages.Add("Object B mass: " + other.Mass.ToString());

                foreach (string msg in messages)
                {
                    ComponentOwner.Game.Log(msg);
                }
            }
        }
    }
}
