using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SalvagerEngine.Games;
using SalvagerEngine.Components;

namespace SalvagerEngine.Input
{
    public class InputManager : Component
    {
        /* Constants and Typedefs */

        const int MaxGamePads = 4;

        public enum MouseButton
        {
            Left,
            Right,
            Centre,
            X1,
            X2
        };

        /* Class Variables */

        GamePadState[] mCurrentGamePads;
        GamePadState[] mPreviousGamePads;

        KeyboardState mCurrentKeyboard;
        KeyboardState mPreviousKeyboard;

        MouseState mCurrentMouse;
        MouseState mPreviousMouse;

        /* Constructors */

        public InputManager(SalvagerGame game)
            : base(game)
        {
            mCurrentGamePads = new GamePadState[MaxGamePads];
            mPreviousGamePads = new GamePadState[MaxGamePads];
        }

        /* Overrides */

        public override void Update(GameTime gameTime)
        {
            /* Call the base update */
            base.Update(gameTime);

            /* Update the gamepads */
            for (int i = 0; i < mCurrentGamePads.Length; i++)
            {
                /* Archive the state */
                mPreviousGamePads[i] = mCurrentGamePads[i];

                /* Get the current gamepad */
                mCurrentGamePads[i] = GamePad.GetState((PlayerIndex)i);
            }

            /* Update the keyboard and mouse */
            mPreviousKeyboard = mCurrentKeyboard;
            mCurrentKeyboard = Keyboard.GetState();

            mPreviousMouse = mCurrentMouse;
            mCurrentMouse = Mouse.GetState();
        }

        /* Accessors */

        public bool IsKeyDown(Keys key)
        {
            return mCurrentKeyboard.IsKeyDown(key);
        }

        public bool IsKeyDownExclusive(Keys key)
        {
            return mCurrentKeyboard.IsKeyDown(key) && mCurrentKeyboard.IsKeyUp(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return mCurrentKeyboard.IsKeyUp(key);
        }

        public bool IsKeyUpExclusive(Keys key)
        {
            return mCurrentKeyboard.IsKeyUp(key) && mCurrentKeyboard.IsKeyDown(key);
        }

        public bool IsButtonDown(Buttons button, PlayerIndex player = PlayerIndex.One)
        {
            return mCurrentGamePads[(int)player].IsButtonDown(button);
        }

        public bool IsButtonDownExclusive(Buttons button, PlayerIndex player = PlayerIndex.One)
        {
            return mCurrentGamePads[(int)player].IsButtonDown(button) && 
                mCurrentGamePads[(int)player].IsButtonUp(button);
        }

        public bool IsButtonUp(Buttons button, PlayerIndex player = PlayerIndex.One)
        {
            return mCurrentGamePads[(int)player].IsButtonUp(button);
        }

        public bool IsButtonUpExclusive(Buttons button, PlayerIndex player = PlayerIndex.One)
        {
            return mCurrentGamePads[(int)player].IsButtonUp(button) && 
                mCurrentGamePads[(int)player].IsButtonDown(button);
        }

        public bool HasMouseMoved()
        {
            return mCurrentMouse.X != mPreviousMouse.X || mCurrentMouse.Y != mPreviousMouse.Y;
        }

        public bool IsMouseButtonDown(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return mCurrentMouse.LeftButton == ButtonState.Pressed;

                case MouseButton.Centre:
                    return mCurrentMouse.MiddleButton == ButtonState.Pressed;

                case MouseButton.Right:
                    return mCurrentMouse.RightButton == ButtonState.Pressed;

                case MouseButton.X1:
                    return mCurrentMouse.XButton1 == ButtonState.Pressed;

                case MouseButton.X2:
                    return mCurrentMouse.XButton2 == ButtonState.Pressed;

                default:
                    return false;
            }
        }

        public bool IMouseButtonDownExclusive(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return mCurrentMouse.LeftButton == ButtonState.Pressed && mPreviousMouse.LeftButton == ButtonState.Released;

                case MouseButton.Centre:
                    return mCurrentMouse.MiddleButton == ButtonState.Pressed && mPreviousMouse.MiddleButton == ButtonState.Released;

                case MouseButton.Right:
                    return mCurrentMouse.RightButton == ButtonState.Pressed && mPreviousMouse.RightButton == ButtonState.Released;

                case MouseButton.X1:
                    return mCurrentMouse.XButton1 == ButtonState.Pressed && mPreviousMouse.XButton1 == ButtonState.Released;

                case MouseButton.X2:
                    return mCurrentMouse.XButton2 == ButtonState.Pressed && mPreviousMouse.XButton2 == ButtonState.Released;

                default:
                    return false;
            }
        }

        public bool IsMouseButtonUp(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return mCurrentMouse.LeftButton == ButtonState.Released;

                case MouseButton.Centre:
                    return mCurrentMouse.MiddleButton == ButtonState.Released;

                case MouseButton.Right:
                    return mCurrentMouse.RightButton == ButtonState.Released;

                case MouseButton.X1:
                    return mCurrentMouse.XButton1 == ButtonState.Released;

                case MouseButton.X2:
                    return mCurrentMouse.XButton2 == ButtonState.Released;

                default:
                    return false;
            }
        }

        public bool IsMouseButtonUpExclusive(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return mCurrentMouse.LeftButton == ButtonState.Released && mPreviousMouse.LeftButton == ButtonState.Pressed;

                case MouseButton.Centre:
                    return mCurrentMouse.MiddleButton == ButtonState.Released && mPreviousMouse.MiddleButton == ButtonState.Pressed;

                case MouseButton.Right:
                    return mCurrentMouse.RightButton == ButtonState.Released && mPreviousMouse.RightButton == ButtonState.Pressed;

                case MouseButton.X1:
                    return mCurrentMouse.XButton1 == ButtonState.Released && mPreviousMouse.XButton1 == ButtonState.Pressed;

                case MouseButton.X2:
                    return mCurrentMouse.XButton2 == ButtonState.Released && mPreviousMouse.XButton2 == ButtonState.Pressed;

                default:
                    return false;
            }
        }
    }
}
