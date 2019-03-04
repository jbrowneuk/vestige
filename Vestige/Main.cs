#region Using Statements
using Vestige.Engine;
using AppKit;
#endregion

namespace Vestige.MacOS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            NSApplication.Init();

            using (var game = new GameRunner())
            {
                game.Run();
            }
        }
    }
}
