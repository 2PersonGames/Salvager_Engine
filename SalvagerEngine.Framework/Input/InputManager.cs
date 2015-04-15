using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

using SalvagerEngine.Framework.Games;
using SalvagerEngine.Framework.Components;

namespace SalvagerEngine.Framework.Input
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

        TouchCollection mCurrentTouches;
        TouchCollection mPreviousTouches;

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

            /* Update the touch panel */
            mPreviousTouches = mCurrentTouches;
            mCurrentTouches = Microsoft.Xna.Framework.Input.Touch.TouchPanel.GetState();
        }

        /* Accessors */

        public bool IsKeyDown(params Keys[] key)
        {
            foreach (Keys k in key)
            {
                if (mCurrentKeyboard.IsKeyDown(k))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsKeyDownExclusive(params Keys[] key)
        {
            foreach (Keys k in key)
            {
                if (mCurrentKeyboard.IsKeyDown(k) && mPreviousKeyboard.IsKeyUp(k))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsKeyUp(params Keys[] key)
        {
            foreach (Keys k in key)
            {
                if (mCurrentKeyboard.IsKeyUp(k))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsKeyUpExclusive(params Keys[] key)
        {
            foreach (Keys k in key)
            {
                if (mCurrentKeyboard.IsKeyUp(k) && mPreviousKeyboard.IsKeyDown(k))
                {
                    return true;
                }
            }

            return false;
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

        public bool IsMouseButtonDownExclusive(MouseButton button)
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

        public int GetMouseScrollDelta()
        {
            return mCurrentMouse.ScrollWheelValue - mPreviousMouse.ScrollWheelValue;
        }

        public bool IsTouchDown(int index)
        {
            return index < mCurrentTouches.Count;
        }
    }
}
