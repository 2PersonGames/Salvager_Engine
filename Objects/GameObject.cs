using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SalvagerEngine.Components;

namespace SalvagerEngine.Objects
{
    public abstract class GameObject
    {
        /* Typdefs and Constants */

        private long MasterIdentifier = long.MinValue;

        /* Class Variables */

        long mIdentifier;
        public long Identifier
        {
            get { return mIdentifier; }
        }

        Level mComponentOwner;
        public Level ComponentOwner
        {
            get { return mComponentOwner; }
        }
        
        ReaderWriterLockSlim mLock;

        float mTickMax;
        float mTickCurrent;

        bool mEnabled;
        public bool Enabled
        {
            get { return mEnabled; }
            set { mEnabled = value; }
        }

        bool mVisible;
        public bool Visible
        {
            get { return mVisible; }
            set { mVisible = value; }
        }

        List<GameObject> mChildren;
        ReaderWriterLockSlim mChildrenLock;

        /* Constructors */

        public GameObject(Level component_owner)
        {
            /* Get a unique ID for this object */
            mIdentifier = Interlocked.Increment(ref MasterIdentifier) - 1;

            /* Set some defaults */
            mLock = new ReaderWriterLockSlim();
            mComponentOwner = component_owner;
            mVisible = true;
            mEnabled = true;
            mTickCurrent = 0.0f;
            mTickMax = 0.0f;

            mChildren = new List<GameObject>();
            mChildrenLock = new ReaderWriterLockSlim();

            /* Initialise the object */
            Initialise(out mTickMax);
        }

        /* Functions */

        protected abstract void Initialise(out float tick_max);

        public void Update(float delta)
        {
            try
            {
                /* Lock this object */
                mLock.EnterUpgradeableReadLock();

                /* Check the tick counter */
                if ((mTickCurrent += delta) >= mTickMax && mEnabled)
                {
                    /* Store the tick amount */
                    delta = mTickCurrent;

                    try
                    {
                        /* Lock this object as a write lock */
                        mLock.EnterWriteLock();

                        /* Update this */
                        Tick(mTickCurrent);

                        /* Reset the tick counter */
                        mTickCurrent = 0.0f;
                    }
                    finally
                    {
                        mLock.ExitWriteLock();
                    }

                    /* Update each child */
                    foreach (GameObject obj in GetChildrenAsArray())
                    {
                        obj.Update(delta);
                    }
                }
            }
            catch (Exception e)
            {
                /* Log the error */
                mComponentOwner.Game.Log(e.ToString());
            }
            finally
            {
                /* Unlock this object */
                if (mLock.IsUpgradeableReadLockHeld) mLock.ExitUpgradeableReadLock();
            }
        }

        protected abstract void Tick(float delta);

        public void Draw(SpriteBatch renderer)
        {
            try
            {
                /* Lock this object */
                mLock.EnterReadLock();

                /* Check the visible flag */
                if (mVisible)
                {
                    /* Render this */
                    Render(renderer);

                    /* Render each child */
                    foreach (GameObject obj in GetChildrenAsArray())
                    {
                        obj.Draw(renderer);
                    }
                }
            }
            catch (Exception e)
            {
                /* Log the error */
                mComponentOwner.Game.Log(e.ToString());
            }
            finally
            {
                /* Unlock this object */
                mLock.ExitReadLock();
            }
        }

        protected abstract void Render(SpriteBatch renderer);

        /* Accessors */

        private GameObject[] GetChildrenAsArray()
        {
            try
            {
                mChildrenLock.EnterReadLock();
                return mChildren.ToArray();
            }
            catch (Exception e)
            {
                mComponentOwner.Game.Log(e);
                return new GameObject[0];
            }
            finally
            {
                mChildrenLock.ExitReadLock();
            }
        }

        private GameObject[] GetChildrenAsArray(Type type)
        {
            try
            {
                /* Lock the list */
                mChildrenLock.EnterReadLock();

                /* Build the list according to the object type */
                List<GameObject> objects = new List<GameObject>();
                foreach (GameObject obj in mChildren)
                {
                    if (obj.GetType() == type || obj.GetType().IsSubclassOf(type)) objects.Add(obj);
                }

                /* Return the list as an array */
                return objects.ToArray();
            }
            catch (Exception e)
            {
                mComponentOwner.Game.Log(e);
                return new GameObject[0];
            }
            finally
            {
                mChildrenLock.ExitReadLock();
            }
        }

        public bool IsChildOf(GameObject obj)
        {
            try
            {
                obj.mChildrenLock.EnterReadLock();
                return obj.mChildren.Contains(this);
            }
            catch (Exception e)
            {
                mComponentOwner.Game.Log(e);
                return false;
            }
            finally
            {
                obj.mChildrenLock.ExitReadLock();
            }
        }

        public bool IsParentOf(GameObject obj)
        {
            try
            {
                mChildrenLock.EnterReadLock();
                return mChildren.Contains(obj);
            }
            catch (Exception e)
            {
                mComponentOwner.Game.Log(e);
                return false;
            }
            finally
            {
                mChildrenLock.ExitReadLock();
            }
        }

        public GameObject IsParentOf(long identifier)
        {
            try
            {
                mChildrenLock.EnterReadLock();
                return mChildren.Find(delegate(GameObject obj) { return obj.Identifier == identifier; });
            }
            catch (Exception e)
            {
                mComponentOwner.Game.Log(e);
                return null;
            }
            finally
            {
                mChildrenLock.ExitReadLock();
            }
        }

        /* Mutators */

        public bool AddChild(GameObject obj)
        {
            try
            {
                mChildrenLock.EnterWriteLock();
                mChildren.Add(obj);
                return true;
            }
            catch (Exception e)
            {
                mComponentOwner.Game.Log(e);
                return false;
            }
            finally
            {
                mChildrenLock.ExitWriteLock();
            }
        }

        public bool RemoveChild(GameObject obj)
        {
            try
            {
                mChildrenLock.EnterWriteLock();
                return mChildren.Remove(obj);
            }
            catch (Exception e)
            {
                mComponentOwner.Game.Log(e);
                return false;
            }
            finally
            {
                mChildrenLock.ExitWriteLock();
            }
        }

        public bool RemoveChild(long identifier)
        {
            try
            {
                mChildrenLock.EnterWriteLock();
                return mChildren.RemoveAll(delegate(GameObject obj) { return obj.Identifier == identifier; }) > 0;
            }
            catch (Exception e)
            {
                mComponentOwner.Game.Log(e);
                return false;
            }
            finally
            {
                mChildrenLock.ExitWriteLock();
            }
        }
    }
}
