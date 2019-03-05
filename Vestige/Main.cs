using Vestige.Engine;

#if MACOS
using AppKit;
#endif

namespace Vestige
{
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

            using (var game = new GameRunner())
            {
                game.Run();
            }
        }
    }
}
