using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Core
{
    public class Overworld
    {
        private readonly TileSystem belowPlayer;
        private readonly TileSystem abovePlayer;

        internal Overworld()
        {
            belowPlayer = new TileSystem();
            abovePlayer = new TileSystem();
        }

        /// <summary>Debug – intent to control the tileset when level is loaded</summary>
        internal void UpdateTileSet(Texture2D tileset)
        {
            belowPlayer.SpriteSheet = tileset;
            abovePlayer.SpriteSheet = tileset;
        }

        internal void LoadLevel(string filename)
        {
            XDocument document;
            try
            {
                document = XDocument.Load(filename);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Could not load world: {0} thrown.\n{1}", exception.GetType(), exception.Message);
                return;
            }

            // Initialise base tile systems
            var systemX = getTileSystemAttribute(document.Root, "left", 0);
            var systemY = getTileSystemAttribute(document.Root, "top", 0);
            var systemW = getTileSystemAttribute(document.Root, "width", 2);
            var systemH = getTileSystemAttribute(document.Root, "height", 2);

            belowPlayer.Initialize(systemX, systemY, systemW, systemH);
            abovePlayer.Initialize(systemX, systemY, systemW, systemH);

            // Load tile layers
            var layersContainerEl = document.Root.Element("TileLayers");
            if (layersContainerEl == null)
            {
                Console.WriteLine("Cannot find layers container in XML");
                return;
            }

            loadLayer(layersContainerEl, belowPlayer, "Below");
            loadLayer(layersContainerEl, abovePlayer, "Above");
        }

        private void loadLayer(XElement layersContainerEl, TileSystem tileSystem, string layerName)
        {
            var relatedLayerEl = (from layerEl in layersContainerEl.Elements("Layer")
                                  where layerEl.Attribute("type").Value == layerName
                                  select layerEl).FirstOrDefault();
            if (relatedLayerEl == null)
            {
                return;
            }

            var tiles = relatedLayerEl.Elements("Tile");
            foreach (var tile in tiles)
            {
                int x = getTileSystemAttribute(tile, "x", -1);
                int y = getTileSystemAttribute(tile, "y", -1);
                int tileId;
                tileId = int.TryParse(tile.Value, out tileId) ? tileId : -1;

                if (x >= 0 && y >= 0 && tileId >= 0)
                {
                    tileSystem.AddTile(x, y, tileId);
                }
            }
        }

        private int getTileSystemAttribute(XElement element, string attributeName, int defaultValue)
        {
            var rawValue = element.Attribute(attributeName)?.Value;
            if (rawValue == null)
            {
                return defaultValue;
            }

            int parsedValue;
            return int.TryParse(rawValue, out parsedValue) ? parsedValue : defaultValue;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            belowPlayer.Draw(spriteBatch);
            abovePlayer.Draw(spriteBatch);
        }
    }
}
