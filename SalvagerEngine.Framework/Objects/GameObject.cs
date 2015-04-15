using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Framework.Components;

namespace SalvagerEngine.Framework.Objects
{
    public class GameObject
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

        bool mChildrenArrayDirty;
        GameObject[] mChildrenArray;
        ReaderWriterLockSlim mChildrenArrayLock;
        List<GameObject> mChildrenList;
        ReaderWriterLockSlim mChildrenListLock;

        /* Constructors */

        public GameObject(Level component_owner, float tick_max)
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
            mChildrenArray = new GameObject[0];
            mChildrenArrayLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            mChildrenList = new List<GameObject>();
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
                mComponentOwner.Game.Log(e.ToString());
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
                mComponentOwner.Game.Log(e);
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

        public void Draw(Camera camera)
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
                mComponentOwner.Game.Log(e.ToString());
            }
            finally
            {
                /* Unlock this object */
                mLock.ExitReadLock();
            }
        }

        protected virtual void Render(Camera camera)
        {
        }

        /* Events */

        public void Destroy()
        {
            GameObject parent = FindParent();
            if (parent != null)
            {
                parent.RemoveChild(this);
            }
        }

        protected virtual void OnDestroy()
        {
        }

        /* Accessors */

        public bool IsChildOf(GameObject obj)
        {
            return obj.IsParentOf(this);
        }

        public bool IsParentOf(GameObject obj)
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

        public GameObject IsParentOf(long identifier)
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

        public IEnumerable<GameObject> ForEachChild()
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
            where T : GameObject
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

        public IEnumerable<GameObject> ForEachAll()
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
            where T : GameObject
        {
            /* Iterate through each child */
            foreach (GameObject a in ForEachChild())
            {
                /* Yield the child */
                if (a is T)
                {
                    yield return a as T;
                }

                /* Recursively yield all it's children and their children */
                foreach (T b in a.ForEachAll<T>())
                {
                    yield return b;
                }
            }
        }

        public GameObject FindParent()
        {
            return FindParent(mComponentOwner.Root);
        }

        private GameObject FindParent(GameObject obj)
        {
            if (obj.IsParentOf(this))
            {
                return obj;
            }
            else
            {
                foreach (GameObject child in obj.ForEachChild())
                {
                    GameObject parent = FindParent(child);
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

        public bool AddChild(GameObject obj)
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

        public bool RemoveChild(GameObject obj)
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
