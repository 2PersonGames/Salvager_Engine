using System;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.Xna.Framework;

namespace SalvagerEngine.Games
{
    public sealed class WindowsGame : DesktopGame
    {
        /* Log Variables */
#if DEBUG
        private StreamWriter mLogFile;

        [DllImport("kernel32.dll")]
        private static extern Boolean AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern Boolean AttachConsole(int id);

        [DllImport("kernel32.dll")]
        private static extern Boolean FreeConsole();
#endif

        /* Constructors */

        public WindowsGame(string game_name)
            : base(game_name)
        {
#if DEBUG
            /* Open the log file */
            try
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.Create), "Salvager Engine", game_name);
                Directory.CreateDirectory(path);
                mLogFile = new StreamWriter(Path.Combine(path, string.Format("{0}.log", DateTime.Now.ToString("dd-MM-yyyy_HH-mm"))), false, System.Text.Encoding.Unicode);
            }
            catch (Exception e)
            {
                Log(e);
            }
#endif

            /* Log the game starting */
            Log(string.Format("Game starting..."));
        }

        /* Overrides */

        protected override void OnExiting(object sender, EventArgs args)
        {
            /* Call the base method */
            base.OnExiting(sender, args);

            /* Log the game closing */
            Log("Game closing...");

#if DEBUG
            /* Close the log console */
            FreeConsole();

            /* Close the log file */
            if (mLogFile != null)
            {
                try
                {
                    mLogFile.Flush();
                }
                finally
                {
                    mLogFile.Close();
                    mLogFile.Dispose();
                    mLogFile = null;
                }
            }
#endif
        }

        public override void Log(string message)
        {
#if DEBUG
            /* Append the message with the time */
            message = string.Format("{0}> {1}", DateTime.Now.ToString("HH:mm:ss:ff"), message);

            /* Log the message in the console */
            Console.WriteLine(message);
            
            /* Close the log file */
            if (mLogFile != null)
            {
                try
                {
                    mLogFile.WriteLine(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
#endif
        }
    }
}
