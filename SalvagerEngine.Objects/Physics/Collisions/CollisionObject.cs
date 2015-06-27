using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Objects;
using SalvagerEngine.Objects.Effects;
using SalvagerEngine.Objects.Graphics;
using SalvagerEngine.Objects.Managers;
using SalvagerEngine.Utilities.Extensions;
using SalvagerEngine.Interfaces.Objects.Graphics;
using SalvagerEngine.Interfaces.Objects.Physics.Collisions;

namespace SalvagerEngine.Objects.Physics.Collisions
{
    public abstract class CollisionObject : EffectObject, ICollisionObject
    {
        /* Class Variables*/

#if DEBUG
        static Texture2D mDebugTexture = null;
        protected static Texture2D DebugTexture
        {
            get { return mDebugTexture; }
        }
#endif

        CollisionHandler mOnCollision;
        public CollisionHandler OnCollision
        {
            get { return mOnCollision; }
            set { mOnCollision = value; }
        }

        Type[] mCollideableTypes;

        public new IGraphicsObject Parent
        {
            get { return base.Parent as IGraphicsObject; }
        }

        /* Constructors */

        public CollisionObject(IGraphicsObject parent)
            : base(parent, 0.0f)
        {
#if DEBUG
            /* Create the 1x1 white texture */
            if (mDebugTexture == null)
            {
                mDebugTexture = new Texture2D(parent.ComponentOwner.GraphicsDevice, 1, 1);
                mDebugTexture.SetData(new Color[] { Color.White });
            }
#endif

            mCollideableTypes = new Type[0];

            /* Add the collision object to the collision manager */
            var manager = parent.ComponentOwner.GetCollisionManager();
            if (manager == null)
            {
                parent.ComponentOwner.Game.Log("No collision manager present!");
            }
            else
            {
                manager.AddChild(this);
            }
        }

        /* Abstracts */

        public abstract float CalculateAngularMass(float mass);

        public abstract bool CheckForCollision(ICollisionObject obj);

        /* Mutators */

        public void AddCollideableType(params Type[] type)
        {
            List<Type> types = new List<Type>();
            types.AddRange(mCollideableTypes);
            types.AddRange(type);
            mCollideableTypes = types.ToArray();
        }

        public void RemoveCollideabeType(params Type[] type)
        {
            List<Type> types = new List<Type>();
            types.AddRange(mCollideableTypes);
            foreach (Type t in type) { types.RemoveAll(delegate(Type compare) { return t == compare; }); }
            mCollideableTypes = types.ToArray();
        }

        /* Events */

        public void CheckCollision(ICollisionObject obj)
        {
            /* Check for collision */
            bool this_collides = IsCollideableType(obj.Parent.Parent.GetType());
            bool other_collideds = obj.IsCollideableType(Parent.Parent.GetType());
            if ((this_collides || other_collideds) && CheckForCollision(obj))
            {
                /* Call this objects 'On Collision' event */
                if (this_collides && mOnCollision != null)
                {
                    mOnCollision(this, obj);
                }

                /* Call the other objects 'On Collision' event */
                if (other_collideds && obj.OnCollision != null)
                {
                    obj.OnCollision(obj, this);
                }
            }
        }

        /* Utilities */

        public bool IsCollideableType(Type type)
        {
            /* Check each collideable type */
            foreach (Type t in mCollideableTypes)
            {
                if (type.IsOfTypeOrSubClassOf(t))
                {
                    return true;
                }
            }

            /* Not of collidable type */
            return false;
        }
    }
}
