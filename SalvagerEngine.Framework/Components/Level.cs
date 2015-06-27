using System;
using System.Threading;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Interfaces.Games;
using SalvagerEngine.Interfaces.Objects;
using SalvagerEngine.Interfaces.Components;
using SalvagerEngine.Interfaces.Objects.Managers;
using SalvagerEngine.Interfaces.Objects.Graphics;

namespace SalvagerEngine.Framework.Components
{
    public class Level : Component, ILevel
    {
        /* Class Variables */

        IGameObject mRoot;
        public IGameObject Root
        {
            get { return mRoot; }
        }

        List<ICamera> mCameras;
        ReaderWriterLockSlim mCameraLock;

        /* Constructors */

        public Level(ISalvagerGame game)
            : base(game)
        {
            mCameras = new List<ICamera>();
            mCameraLock = new ReaderWriterLockSlim();
            mRoot = new SalvagerEngine.Objects.GameObject(this, 0.0f);
        }

        /* Overrides */

        public override void Update(GameTime gameTime)
        {
            try
            {
                /* Call the base update */
                base.Update(gameTime);

                /* Update the root node */
                mRoot.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            catch (Exception e)
            {
                /* Log the exception */
                Game.Log(e);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            try
            {
                /* Call the base draw */
                base.Draw(gameTime);

                /* Lock the cameras */
                mCameraLock.EnterReadLock();

                /* Draw the root node */
                foreach (Camera camera in mCameras)
                {
                    try
                    {
                        /* Begin rendering */
                        camera.Begin();

                        /* Render the node */
                        mRoot.Draw(camera);
                    }
                    catch (Exception e)
                    {
                        /* Log the exception */
                        Game.Log(e);
                    }
                    finally
                    {
                        /* Finish rendering */
                        camera.End();
                    }
                }
            }
            catch (Exception e)
            {
                /* Log the exception */
                Game.Log(e);
            }
            finally
            {
                /* Release the camera lock */
                mCameraLock.ExitReadLock();
            }
        }

        /* Accessors */

        public bool ContainsCamera(ICamera camera)
        {
            try
            {
                mCameraLock.EnterReadLock();
                return mCameras.Contains(camera);
            }
            finally
            {
                mCameraLock.ExitReadLock();
            }
        }

        public ICollisionManager GetCollisionManager()
        {
            /* Attempt to find an existing collision manager */
            foreach (var obj in mRoot.ForEachChild<ICollisionManager>())
            {
                return obj;
            }

            /* Return null */
            return null;
        }

        public void Log(string message)
        {
            Game.Log(message);
        }

        public void Log(Exception exception)
        {
            Game.Log(exception);
        }

        /* Mutators */

        public void AddCamera(ICamera camera)
        {
            try
            {
                mCameraLock.EnterWriteLock();
                mCameras.Add(camera);
            }
            finally
            {
                mCameraLock.ExitWriteLock();
            }
        }

        public bool RemoveCamera(ICamera camera)
        {
            try
            {
                mCameraLock.EnterWriteLock();
                return mCameras.Remove(camera);
            }
            finally
            {
                mCameraLock.ExitWriteLock();
            }
        }

        /* Events */

        public void ToggleCollisionManager(bool enable)
        {
            /* Add a collision manager */
            if (enable)
            {
                if (GetCollisionManager() == null)
                {
                    mRoot.AddChild(new SalvagerEngine.Objects.Managers.CollisionManager(this));
                    mRoot.Update(0.0f);
                }
            }
            /* Remove the collision manager */
            else if (GetCollisionManager() != null)
            {
                GetCollisionManager().Destroy();
            }
        }
    }
}
