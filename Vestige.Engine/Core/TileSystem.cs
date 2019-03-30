using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Core
{
    /// <summary>
    /// Class used to handle level drawing using the tilemap concept.
    /// 
    /// The game world is built up out of small squares called tiles.
    /// Each tile can have a graphic assigned to it that is taken from
    /// a subset of a larger spritesheet of component images.
    /// </summary>
    /// <example>
    /// A detailed explanation of the concept can be found at the MDN
    /// by following the following link:
    /// https://developer.mozilla.org/en-US/docs/Games/Techniques/Tilemaps
    /// </example>
    internal class TileSystem
    {
        const int gridSize = 24;
        const int blankTileId = -1;
        const int invalidTileCoord = -1;

        private int gridHeight;
        private int gridWidth;
        private Point topLeft;

        /// <summary>
        /// Used to set the tile atlas spritesheet.
        /// </summary>
        internal Texture2D SpriteSheet { get; set; }

        private int[,] TileArray { get; set; }
        private int ImageWidth { get { return SpriteSheet != null ? SpriteSheet.Width : 0; } }
        private int ImageHeight { get { return SpriteSheet != null ? SpriteSheet.Height : 0; } }

        /// <summary>
        /// Initializes the tile system to the specified size and position.
        /// </summary>
        /// <param name="x">The x position, in pixels, of the top left of the system</param>
        /// <param name="y">The y position, in pixels, of the top left of the system</param>
        /// <param name="w">The width, in tiles, of the system</param>
        /// <param name="h">The height, in tiles, of the system</param>
        internal void Initialize(int x, int y, int w, int h)
        {
            if (w < 1 || h < 1)
            {
                throw new ArgumentOutOfRangeException("Width or height need to be a positive integer", (Exception)null);
            }

            topLeft.X = x;
            topLeft.Y = y;
            gridWidth = w;
            gridHeight = h;
            TileArray = new int[w, h];

            // Initialise the array with blank tiles
            for (var xpos = 0; xpos < gridWidth; xpos++)
            {
                for (var ypos = 0; ypos < gridHeight; ypos++)
                {
                    TileArray[xpos, ypos] = blankTileId;
                }
            }
        }

        /// <summary>
        /// Adds a specific tile, identified by <paramref name="tileId"/>, to the specified coordinates in the system.
        /// If a tile exists at the position, it will be overwritten.
        /// </summary>
        /// <param name="x">X position, in tiles, of the new tile</param>
        /// <param name="y">Y position, in tiles, of the new tile</param>
        /// <param name="tileId">The identifier of the tile on the tile atlas spritesheet</param>
        internal void AddTile(int x, int y, int tileId)
        {
            if (TileArray == null)
            {
                return;
            }

            if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
            {
                return;
            }

            TileArray[x, y] = tileId;
        }

        /// <summary>
        /// Draws the current system to screen.
        /// </summary>
        /// <param name="sb">Activated <see cref="SpriteBatch"/></param>
        internal void Draw(SpriteBatch sb)
        {
            if (SpriteSheet == null || TileArray == null || TileArray.Length == 0)
            {
                return;
            }

            var tileSource = new Rectangle(0, 0, gridSize, gridSize);
            var outputArea = new Rectangle(0, 0, gridSize, gridSize);
            for (var xpos = 0; xpos < gridWidth; xpos++)
            {
                for (var ypos = 0; ypos < gridHeight; ypos++)
                {
                    var tileId = TileArray[xpos, ypos];
                    if (tileId == blankTileId)
                    {
                        continue;
                    }

                    tileSource.Location = GetTileSourcePosition(tileId);
                    if (tileSource.Location.X == invalidTileCoord || tileSource.Location.Y == invalidTileCoord)
                    {
                        continue;
                    }

                    var destination = new Point(xpos * gridSize + (int)topLeft.X, ypos * gridSize + (int)topLeft.Y);
                    outputArea.Location = destination;

                    sb.Draw(SpriteSheet, outputArea, tileSource, Color.White);
                }
            }
        }

        /// <summary>
        /// Used to calculate the position of a tile with <paramref name="tileId"/> on the tile atlas spritesheet.
        /// </summary>
        /// <param name="tileId">The identifier of the tile on the tile atlas spritesheet</param>
        /// <returns>The top left point of the tile, or an invalid point of <see cref="invalidTileCoord"/> if not found</returns>
        private Point GetTileSourcePosition(int tileId)
        {
            Point location;
            location.Y = tileId / (ImageWidth / gridSize);
            location.X = tileId - (location.Y * (ImageWidth / gridSize));

            // Multiply up for tile size
            location.X *= gridSize;
            location.Y *= gridSize;

            // Sanity check
            if (location.X > ImageWidth || location.Y > ImageHeight)
            {
                return new Point(invalidTileCoord);
            }

            return location;
        }
    }
}
