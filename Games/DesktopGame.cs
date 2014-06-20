using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using SalvagerEngine.Tools.Extensions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SalvagerEngine.Games
{
    public abstract class DesktopGame : SalvagerGame
    {
        // Class Variables

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndIntertAfter, int X, int Y, int cx, int cy, int uFlags);

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int Which);

        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;
        private static IntPtr HWND_TOP = IntPtr.Zero;
        private const int SWP_SHOWWINDOW = 64;

        public static int ScreenX
        {
            get { return GetSystemMetrics(SM_CXSCREEN); }
        }
        public static int ScreenY
        {
            get { return GetSystemMetrics(SM_CYSCREEN); }
        }

        // Constructors

        public DesktopGame(string game_name)
            : base(game_name)
        {
        }

        // Overrides

        protected override void Initialize()
        {
            // Call the base method
            base.Initialize();

            // Resize the window
#if DEBUG
            Windowed();
#else
            Fullscreen();
#endif
        }

        // Events

        private void Fullscreen()
        {
            /* Set the resolution to the native resolution */
            Screen screen = Screen.FromHandle(Window.Handle);
            screen = screen == null ? Screen.PrimaryScreen : screen;

            Graphics.PreferredBackBufferWidth = screen.Bounds.Width;
            Graphics.PreferredBackBufferHeight = screen.Bounds.Height;
            Graphics.ApplyChanges();

            /* Set fullscreen */
            //Window.IsBorderless = true;
            Window.SetPosition(Microsoft.Xna.Framework.Point.Zero);
            SetWindowPos(Window.Handle, HWND_TOP, 0, 0, ScreenX, ScreenY, SWP_SHOWWINDOW);
        }

        private void Windowed()
        {
            // Allow the player to resize the game window
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += delegate
            {
                Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            };

            // Set the window to the centre of the screen
            Screen screen = Screen.FromHandle(Window.Handle);
            screen = screen == null ? Screen.PrimaryScreen : screen;

            Point position = new Point
            (
                (int)Math.Round((screen.Bounds.Width - GraphicsDevice.Viewport.Width) * 0.5f),
                (int)Math.Round((screen.Bounds.Height - GraphicsDevice.Viewport.Height) * 0.5f)
            );

            Window.SetPosition(position);
            SetWindowPos(Window.Handle, HWND_TOP, position.Y, position.X, ScreenX, ScreenY, SWP_SHOWWINDOW);
        }
    }
}
