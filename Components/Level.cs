using System;
using System.Threading;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SalvagerEngine.Games;
using SalvagerEngine.Objects;

namespace SalvagerEngine.Components
{
    public class Level : Component
    {
        /* Class Variables */

        GameObject mRoot;
        public GameObject Root
        {
            get { return mRoot; }
        }

        List<Camera> mCameras;
        ReaderWriterLockSlim mCameraLock;

        /* Constructors */

        public Level(SalvagerGame game)
            : base(game)
        {
            mCameras = new List<Camera>();
            mCameraLock = new ReaderWriterLockSlim();
            mRoot = new GameObject(this, 0.0f);
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

        public bool ContainsCamera(Camera camera)
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

        /* Mutators */

        public void AddCamera(Camera camera)
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

        public bool RemoveCamera(Camera camera)
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
    }
}
