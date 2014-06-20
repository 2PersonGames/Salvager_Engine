using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Games;
using SalvagerEngine.Objects;
using SalvagerEngine.Components;
using SalvagerEngine.Objects.Effects;
using SalvagerEngine.Tools.Extensions;
using SalvagerEngine.Objects.Graphics;
using SalvagerEngine.Objects.Managers;

namespace SalvagerEngine.Objects.Physics.Collisions
{
    public abstract class CollisionObject : EffectObject
    {
        /* Typedefs and Constants */

        public delegate void CollisionHandler(CollisionObject me, CollisionObject other);

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

        public new GraphicsObject Parent
        {
            get { return base.Parent as GraphicsObject; }
        }

        /* Constructors */

        public CollisionObject(GraphicsObject parent)
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
            CollisionManager manager = parent.ComponentOwner.GetCollisionManager();
            if (manager == null)
            {
                parent.ComponentOwner.Game.Log("No collision manager present!");
            }
            else
            {
                manager.AddChild(this);
            }
        }

        /* Accessors */

        public abstract float CalculateAngularMass(float mass);

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

        public void CheckCollision(CollisionObject obj)
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
                if (other_collideds && obj.mOnCollision != null)
                {
                    obj.mOnCollision(obj, this);
                }
            }
        }

        protected abstract bool CheckForCollision(CollisionObject obj);

        /* Utilities */

        bool IsCollideableType(Type type)
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
