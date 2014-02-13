using System;
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

        SpriteBatch mRenderer;
        List<Camera> mCameras;

        /* Constructors */

        public Level(SalvagerGame game)
            : base(game)
        {
            mRenderer = new SpriteBatch(game.GraphicsDevice);
            mCameras = new List<Camera>();
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

                /* Draw the root node */
                foreach (Camera camera in mCameras)
                {
                    /* Begin rendering */
                    mRenderer.Begin(camera.SpriteSortMode, camera.BlendState, camera.SamplerState, camera.DepthStencilState, camera.RasterizerState, camera.Effect, camera.View);
                    try
                    {
                        /* Render the node */
                        mRoot.Draw(mRenderer);
                    }
                    catch (Exception e)
                    {
                        /* Log the exception */
                        Game.Log(e);
                    }
                    finally
                    {
                        /* Finish rendering */
                        mRenderer.End();
                    }
                }
            }
            catch (Exception e)
            {
                Game.Log(e);
            }
        }
    }
}
