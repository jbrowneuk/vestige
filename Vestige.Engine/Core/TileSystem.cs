using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Core
{
    public class TileSystem
    {
        const int gridSize = 24;

        private int gridHeight;
        private int gridWidth;
        private Point topLeft;

        public int[] TileArray { get; private set; }
        public Texture2D SpriteSheet { get; set; }
        public int ImageWidth { get { return SpriteSheet != null ? SpriteSheet.Width : 0; } }
        public int ImageHeight { get { return SpriteSheet != null ? SpriteSheet.Height : 0; } }

        public void Initialize(int x, int y, int w, int h)
        {
            topLeft.X = x;
            topLeft.Y = y;
            gridWidth = w;
            gridHeight = h;
            TileArray = new int[w * h];
        }

        public void AddTile(int position, int tileId)
        {
            if (TileArray == null)
            {
                return;
            }

            if (tileId < 0 || tileId > gridWidth * gridHeight)
            {
                return;
            }

            TileArray[position] = tileId;
        }

        public void Draw(SpriteBatch sb)
        {
            if (SpriteSheet == null || TileArray == null || TileArray.Length == 0)
            {
                return;
            }

            var tileSource = new Rectangle(0, 0, gridSize, gridSize);
            for (var xpos = 0; xpos < gridWidth; xpos++)
            {
                for (var ypos = 0; ypos < gridHeight; ypos++)
                {
                    var tileId = TileArray[xpos + (ypos * gridWidth)];
                    if (tileId == 0)
                    {
                        // Magic blank tile!
                        continue;
                    }

                    tileSource.Location = GetTilePosition(tileId);
                    if (tileSource.Location.X == -1 || tileSource.Location.Y == -1)
                    {
                        // No tile here
                        continue;
                    }

                    var outputArea = new Rectangle(xpos * gridSize + (int)topLeft.X, ypos * gridSize + (int)topLeft.Y, gridSize, gridSize);
                    sb.Draw(SpriteSheet, outputArea, tileSource, Color.White);
                }
            }
        }

        private Point GetTilePosition(int tileId)
        {
            int normalisedId = tileId - 1;
            if (normalisedId < 0)
            {
                return new Point(-1);
            }

            const int tileSpacing = 1;

            // Calculate usable area
            int paddingX = (ImageWidth / gridSize) * tileSpacing;
            int usableImageWidth = ImageWidth - paddingX;

            // Calculate actual location
            Point location;
            location.Y = normalisedId / (usableImageWidth / gridSize);
            location.X = normalisedId - (location.Y * (usableImageWidth / gridSize));

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
                location.X = -1;
                location.Y = -1;
            }

            return location;
        }
    }
}
