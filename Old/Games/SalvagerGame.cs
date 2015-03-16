using System;

using Microsoft.Xna.Framework;

using SalvagerEngine.Input;
using SalvagerEngine.Objects.Physics.Collisions;

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

        SalvagerEngine.Content.ContentManager mContent;
        public new SalvagerEngine.Content.ContentManager Content
        {
            get { return mContent; }
        }

        Random mRandom;
        public Random Random
        {
            get { return mRandom; }
        }

        InputManager mInputManager;
        public InputManager InputManager
        {
            get { return mInputManager; }
        }

        /* Constructors */

        public SalvagerGame(string game_name)
        {
            /* Set some defaults */
            mRandom = new System.Random();
            mClearColour = Color.CornflowerBlue;
            mGraphics = new GraphicsDeviceManager(this);
            mContent = new SalvagerEngine.Content.ContentManager(this, "Content");
            Window.Title = game_name;
        }

        /* Overrides */

        protected override void Initialize()
        {
            /* Call the base method */
            base.Initialize();
             
            /* Add the input manager */
            Components.Add((mInputManager = new InputManager(this)));
        }

        protected override void LoadContent()
        {
            /* Call the base method */
            base.LoadContent();
        }

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

        /* Accessors */

        public static SalvagerGame GetGame(string game_name)
        {
#if WINDOWS
            return new WindowsGame(game_name);
#endif
        }

        /* Mutators */

        public void Log(Exception exception)
        {
            Log(string.Format("{0}\n\tError: {1}\n\tStacktrace:\n{2}", 
                exception, exception.Message, exception.StackTrace));
        }

        /* Abstracts */

        public abstract void Log(string message);
    }
}
