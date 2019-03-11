using System;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Core
{
    public class Overworld
    {
        private readonly TileSystem belowPlayer;
        private readonly TileSystem abovePlayer;

        public Overworld()
        {
            belowPlayer = new TileSystem();
            abovePlayer = new TileSystem();
        }

        /// <summary>Debug – intent to control the tileset when level is loaded</summary>
        public void UpdateTileSet(Texture2D tileset)
        {
            belowPlayer.SpriteSheet = tileset;
            abovePlayer.SpriteSheet = tileset;
        }

        public void LoadLevel(string filename)
        {
            // intent to load from file
            Console.WriteLine(filename);

            const int tsX = 36;
            const int tsY = 36;
            const int tsW = 8;
            const int tsH = 8;
            belowPlayer.Initialize(tsX, tsY, tsW, tsH);
            abovePlayer.Initialize(tsX, tsY, tsW, tsH);

            belowPlayer.AddTile(0, 0, 0);
            belowPlayer.AddTile(1, 0, 1);
            belowPlayer.AddTile(2, 0, 1);
            belowPlayer.AddTile(3, 0, 1);
            belowPlayer.AddTile(4, 0, 2);
            belowPlayer.AddTile(0, 1, 10);
            belowPlayer.AddTile(1, 1, 11);
            belowPlayer.AddTile(2, 1, 11);
            belowPlayer.AddTile(3, 1, 11);
            belowPlayer.AddTile(4, 1, 12);
            belowPlayer.AddTile(0, 2, 10);
            belowPlayer.AddTile(1, 2, 11);
            belowPlayer.AddTile(2, 2, 11);
            belowPlayer.AddTile(3, 2, 11);
            belowPlayer.AddTile(4, 2, 12);
            belowPlayer.AddTile(0, 3, 20);
            belowPlayer.AddTile(1, 3, 21);
            belowPlayer.AddTile(2, 3, 21);
            belowPlayer.AddTile(3, 3, 21);
            belowPlayer.AddTile(4, 3, 22);
            belowPlayer.AddTile(0, 4, 30);
            belowPlayer.AddTile(1, 4, 31);
            belowPlayer.AddTile(2, 4, 31);
            belowPlayer.AddTile(3, 4, 31);
            belowPlayer.AddTile(4, 4, 32);
            belowPlayer.AddTile(2, 5, 3);

            abovePlayer.AddTile(1, 3, 23);
            abovePlayer.AddTile(3, 3, 23);
            abovePlayer.AddTile(1, 4, 23);
            abovePlayer.AddTile(2, 4, 33);
            abovePlayer.AddTile(3, 4, 23);
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            belowPlayer.Draw(spriteBatch);
            abovePlayer.Draw(spriteBatch);
        }
    }
}
