using System;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace SalvagerEngine.Utilities.Extensions
{
    public static class MonoGameExtensions
    {
        public static void SetPosition(this GameWindow window, Point position)
        {
            //OpenTK.GameWindow OTKWindow = GetForm(window);
            //if (OTKWindow != null)
            //{
            //    OTKWindow.X = position.X;
            //    OTKWindow.Y = position.Y;
            //}
        }

        //public static OpenTK.GameWindow GetForm(this GameWindow gameWindow)
        //{
        //    Type type = typeof(OpenTKGameWindow);
        //    FieldInfo field = type.GetField("window", BindingFlags.NonPublic | BindingFlags.Instance);
        //    if (field == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        return field.GetValue(gameWindow) as OpenTK.GameWindow;
        //    }
        //}
    }
}
