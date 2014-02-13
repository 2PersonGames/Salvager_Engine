using System;
using Microsoft.Xna.Framework;

namespace SalvagerEngine.Games
{
    public abstract class SalvagerGame : Game
    {
        /* Class Variables */
        
        GraphicsDeviceManager mGraphics;
        public GraphicsDeviceManager Graphics
        {
            get { return mGraphics; }
        }

        Color mClearColour;
        public Color ClearColour
        {
            get { return mClearColour; }
            set { mClearColour = value; }
        }

        /* Constructors */

        public SalvagerGame()
        {
            mClearColour = Color.CornflowerBlue;
            mGraphics = new GraphicsDeviceManager(this);
        }

        /* Overrides */

        protected override void Update(GameTime gameTime)
        {
            try
            {
                base.Update(gameTime);
            }
            catch (Exception e)
            {
                Log(e.ToString());
            }
        }

        protected override bool BeginDraw()
        {
            return !GraphicsDevice.IsDisposed && base.BeginDraw();
        }

        protected override void Draw(GameTime gameTime)
        {
            try
            {
                /* Clear the display */
                GraphicsDevice.Clear(mClearColour);

                /* Call the base draw */
                base.Draw(gameTime);
            }
            catch (Exception e)
            {
                Log(e.ToString());
            }
        }

        /* Mutators */

        public void Log(Exception e)
        {
            Log(e.ToString());
        }

        /* Abstracts */

        public abstract void Log(string message);
    }
}
