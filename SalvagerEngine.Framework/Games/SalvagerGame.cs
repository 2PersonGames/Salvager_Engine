using System;

using Microsoft.Xna.Framework;

using SalvagerEngine.Storage.Content;
using SalvagerEngine.Framework.Input;
using SalvagerEngine.Framework.Objects.Physics.Collisions;

namespace SalvagerEngine.Framework.Games
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

        SalvagerContentManager mContent;
        public new SalvagerContentManager Content
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
            mContent = new SalvagerContentManager(this, "Content");
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
                Log(e);
            }
        }

        /* Accessors */

        public static SalvagerGame GetGame(string game_name)
        {
#if WINDOWS
            return new WindowsGame(game_name);
#endif
            throw new NotImplementedException();
        }

        /* Mutators */

        public void Log(Exception exception)
        {
            Log(string.Format("{0}{3}\tError: {1}{3}\tStacktrace:{3}{2}", 
                exception, exception.Message, exception.StackTrace, Environment.NewLine));
        }

        /* Abstracts */

        public abstract void Log(string message);
    }
}
