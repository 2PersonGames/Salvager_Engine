using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Interfaces.Objects;
using SalvagerEngine.Interfaces.Components;
using SalvagerEngine.Interfaces.Objects.Graphics;

namespace SalvagerEngine.Objects
{
    public class GameObject : IGameObject
    {
        /* Typdefs and Constants */

        private long MasterIdentifier = long.MinValue;

        /* Class Variables */

        long mIdentifier;
        public long Identifier
        {
            get { return mIdentifier; }
        }

        ILevel mComponentOwner;
        public ILevel ComponentOwner
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

        bool mChildrenArrayDirty;
        IGameObject[] mChildrenArray;
        ReaderWriterLockSlim mChildrenArrayLock;
        List<IGameObject> mChildrenList;
        ReaderWriterLockSlim mChildrenListLock;

        /* Constructors */

        public GameObject(ILevel component_owner, float tick_max)
        {
            /* Get a unique ID for this object */
            mIdentifier = Interlocked.Increment(ref MasterIdentifier) - 1;

            /* Set some defaults */
            mLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            mComponentOwner = component_owner;
            mVisible = true;
            mEnabled = true;
            mTickCurrent = 0.0f;
            mTickMax = tick_max;

            mChildrenArrayDirty = false;
            mChildrenArray = new IGameObject[0];
            mChildrenArrayLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            mChildrenList = new List<IGameObject>();
            mChildrenListLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        }

        /* Functions */

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
                }
            }
            catch (Exception e)
            {
                /* Log the error */
                mComponentOwner.Log(e);
            }
            finally
            {
                /* Unlock this object */
                if (mLock.IsUpgradeableReadLockHeld)
                {
                    mLock.ExitUpgradeableReadLock();
                }
            }

            try
            {
                /* Lock the children array */
                mChildrenArrayLock.EnterUpgradeableReadLock();

                /* Check if the child array is dirty */
                if (mChildrenArrayDirty)
                {
                    try
                    {
                        /* Enter a write lock */
                        mChildrenArrayLock.EnterWriteLock();

                        /* Copy the array into the list */
                        mChildrenList = mChildrenList.OrderBy(obj => obj.GetDepth()).ToList();
                        mChildrenArray = mChildrenList.ToArray();

                        /* Set the flag to clean */
                        mChildrenArrayDirty = false;
                    }
                    finally
                    {
                        /* Release the write lock */
                        mChildrenArrayLock.ExitWriteLock();
                    }
                }

                /* Update the children in parallel */
                Parallel.ForEach(mChildrenArray, obj =>
                {
                    obj.Update(delta);
                });
            }
            catch (Exception e)
            {
                /* Log the exception */
                mComponentOwner.Log(e);
            }
            finally
            {
                /* Release the lock */
                mChildrenArrayLock.ExitUpgradeableReadLock();
            }
        }

        protected virtual void Tick(float delta)
        {
        }

        public void Draw(ICamera camera)
        {
            try
            {
                /* Lock this object */
                mLock.EnterReadLock();

                /* Check the visible flag */
                if (mVisible)
                {
                    /* Render this */
                    Render(camera);

                    /* Render each child */
                    foreach (GameObject obj in ForEachChild())
                    {
                        obj.Draw(camera);
                    }
                }
            }
            catch (Exception e)
            {
                /* Log the error */
                mComponentOwner.Log(e);
            }
            finally
            {
                /* Unlock this object */
                mLock.ExitReadLock();
            }
        }

        protected virtual void Render(ICamera camera)
        {
        }

        /* Events */

        public void Destroy()
        {
            var parent = FindParent();
            if (parent != null)
            {
                parent.RemoveChild(this);
            }
        }

        protected virtual void OnDestroy()
        {
        }

        /* Accessors */

        public bool IsChildOf(IGameObject obj)
        {
            return obj.IsParentOf(this);
        }

        public bool IsParentOf(IGameObject obj)
        {
            try
            {
                mChildrenArrayLock.EnterReadLock();
                return mChildrenArray.Contains(obj);
            }
            catch (Exception e)
            {
                mComponentOwner.Game.Log(e);
                return false;
            }
            finally
            {
                mChildrenArrayLock.ExitReadLock();
            }
        }

        public IGameObject IsParentOf(long identifier)
        {
            try
            {
                mChildrenArrayLock.EnterReadLock();
                return mChildrenArray.Where(obj => obj.Identifier == identifier).FirstOrDefault();
            }
            catch (Exception e)
            {
                mComponentOwner.Game.Log(e);
                return null;
            }
            finally
            {
                mChildrenArrayLock.ExitReadLock();
            }
        }

        public IEnumerable<IGameObject> ForEachChild()
        {
            try
            {
                mChildrenArrayLock.EnterReadLock();
                foreach (GameObject obj in mChildrenArray)
                {
                    yield return obj;
                }
            }
            finally
            {
                mChildrenArrayLock.ExitReadLock();
            }
        }

        public IEnumerable<T> ForEachChild<T>() 
            where T : IGameObject
        {
            try
            {
                mChildrenArrayLock.EnterReadLock();
                foreach (T obj in mChildrenArray.Where(obj => obj is T))
                {
                    yield return obj;
                }
            }
            finally
            {
                mChildrenArrayLock.ExitReadLock();
            }
        }

        public IEnumerable<IGameObject> ForEachAll()
        {
            /* Iterate through each child */
            foreach (GameObject a in ForEachChild())
            {
                /* Yield the child */
                yield return a;

                /* Recursively yield all it's children and their children */
                foreach (GameObject b in a.ForEachAll())
                {
                    yield return b;
                }
            }
        }

        public IEnumerable<T> ForEachAll<T>()
            where T : IGameObject
        {
            /* Iterate through each child */
            foreach (var a in ForEachChild())
            {
                /* Yield the child */
                if (a is T)
                {
                    yield return (T)a;
                }

                /* Recursively yield all it's children and their children */
                foreach (T b in a.ForEachAll<T>())
                {
                    yield return b;
                }
            }
        }

        public IGameObject FindParent()
        {
            return FindParent(mComponentOwner.Root);
        }

        private IGameObject FindParent(IGameObject obj)
        {
            if (obj.IsParentOf(this))
            {
                return obj;
            }
            else
            {
                foreach (IGameObject child in obj.ForEachChild())
                {
                    var parent = FindParent(child);
                    if (parent != null)
                    {
                        return parent;
                    }
                }
            }

            return null;
        }

        public virtual float GetDepth()
        {
            return 0.0f;
        }

        /* Mutators */

        public bool AddChild(IGameObject obj)
        {
            try
            {
                mChildrenListLock.EnterWriteLock();
                mChildrenList.Add(obj);
                return true;
            }
            catch (Exception e)
            {
                mComponentOwner.Game.Log(e);
                return false;
            }
            finally
            {
                mChildrenArrayDirty = true;
                mChildrenListLock.ExitWriteLock();
            }
        }

        public bool RemoveChild(IGameObject obj)
        {
            try
            {
                mChildrenListLock.EnterWriteLock();
                if (mChildrenList.Remove(obj))
                {
                    mChildrenArrayDirty = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                mComponentOwner.Game.Log(e);
                return false;
            }
            finally
            {
                mChildrenListLock.ExitWriteLock();
            }
        }

        public bool RemoveChild(long identifier)
        {
            try
            {
                /* Get a write lock*/
                mChildrenListLock.EnterWriteLock();

                /* Remove all the children with this ID */
                if (mChildrenList.RemoveAll(obj => obj.Identifier == Identifier) > 0)
                {
                    mChildrenArrayDirty = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                mComponentOwner.Game.Log(e);
                return false;
            }
            finally
            {
                mChildrenListLock.ExitWriteLock();
            }
        }
    }
}
