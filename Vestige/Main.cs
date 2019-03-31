using System.Linq;
using Microsoft.Xna.Framework;
using Vestige.Engine;

#if MACOS
using AppKit;
#endif

namespace Vestige
{
    /// <summary>
    /// Game bootstrapper
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
#if MACOS
            NSApplication.Init();
#endif

            bool runEditor = args.Contains("--editor");
            using (var game = runEditor ? new EditorRunner() as Game : new GameRunner())
            {
                game.Run();
            }
        }
    }
}
