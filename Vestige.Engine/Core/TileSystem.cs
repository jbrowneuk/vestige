using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Core
{
    public class TileSystem
    {
        const int gridSize = 24;
        const int blankTileId = -1;
        const int invalidTileCoord = -1;

        private int gridHeight;
        private int gridWidth;
        private Point topLeft;

        public int[,] TileArray { get; private set; }
        public Texture2D SpriteSheet { get; set; }
        public int ImageWidth { get { return SpriteSheet != null ? SpriteSheet.Width : 0; } }
        public int ImageHeight { get { return SpriteSheet != null ? SpriteSheet.Height : 0; } }

        public void Initialize(int x, int y, int w, int h)
        {
            topLeft.X = x;
            topLeft.Y = y;
            gridWidth = w;
            gridHeight = h;
            TileArray = new int[w, h];

            for (var xpos = 0; xpos < gridWidth; xpos++)
            {
                for (var ypos = 0; ypos < gridHeight; ypos++)
                {
                    TileArray[xpos, ypos] = blankTileId;
                }
            }
        }

        public void AddTile(int x, int y, int tileId)
        {
            if (TileArray == null)
            {
                return;
            }

            if (x < 0 || x > gridWidth || y < 0 || y > gridHeight)
            {
                return;
            }

            TileArray[x, y] = tileId;
        }

        public void Draw(SpriteBatch sb)
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

        private Point GetTileSourcePosition(int tileId)
        {
            const int tileSpacing = 1;

            // Calculate usable area
            int paddingX = (ImageWidth / gridSize) * tileSpacing;
            int usableImageWidth = ImageWidth - paddingX;

            // Calculate actual location
            Point location;
            location.Y = tileId / (usableImageWidth / gridSize);
            location.X = tileId - (location.Y * (usableImageWidth / gridSize));

            // Calculate spacing
            Point spacing;
            spacing.X = location.X * tileSpacing;
            spacing.Y = location.Y * tileSpacing;

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
