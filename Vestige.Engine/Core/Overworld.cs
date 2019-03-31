using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Core
{
    /// <summary>
    /// Main class used to handle loading levels.
    /// </summary>
    internal class Overworld
    {
        private readonly TileSystem belowPlayer;
        private readonly TileSystem abovePlayer;

        internal Overworld()
        {
            belowPlayer = new TileSystem();
            abovePlayer = new TileSystem();
        }

        /// <summary>Width of the loaded world, in tiles</summary>
        internal int WorldWidth { get; private set; }

        /// <summary>Height of the loaded world, in tiles</summary>
        internal int WorldHeight { get; private set; }

        /// <summary>Debug – intent to control the tileset when level is loaded.</summary>
        internal void UpdateTileSet(Texture2D tileset)
        {
            belowPlayer.SpriteSheet = tileset;
            abovePlayer.SpriteSheet = tileset;
        }

        /// <summary>
        /// Loads a level from an XML level file.
        /// </summary>
        /// <param name="filename">Path to the level file</param>
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
            var systemX = GetTileSystemAttribute(document.Root, "left", 0);
            var systemY = GetTileSystemAttribute(document.Root, "top", 0);
            WorldWidth = GetTileSystemAttribute(document.Root, "width", 2);
            WorldHeight = GetTileSystemAttribute(document.Root, "height", 2);

            belowPlayer.Initialize(systemX, systemY, WorldWidth, WorldHeight);
            abovePlayer.Initialize(systemX, systemY, WorldWidth, WorldHeight);

            // Load tile layers
            var layersContainerEl = document.Root.Element("TileLayers");
            if (layersContainerEl == null)
            {
                // Todo: debugging log messages should be removed
                Console.WriteLine("Cannot find layers container in XML");
                return;
            }

            LoadLayer(layersContainerEl, belowPlayer, "Below");
            LoadLayer(layersContainerEl, abovePlayer, "Above");
        }

        /// <summary>
        /// Attempts to load a tile layer with specific attributes.
        /// </summary>
        /// <param name="layersContainerEl">The <see cref="XElement"/> containing layer information</param>
        /// <param name="tileSystem">The tile system to load tiles into</param>
        /// <param name="layerName">The layer name to reference</param>
        private void LoadLayer(XElement layersContainerEl, TileSystem tileSystem, string layerName)
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
                int x = GetTileSystemAttribute(tile, "x", -1);
                int y = GetTileSystemAttribute(tile, "y", -1);
                int tileId;
                tileId = int.TryParse(tile.Value, out tileId) ? tileId : -1;

                if (x >= 0 && y >= 0 && tileId >= 0)
                {
                    tileSystem.AddTile(x, y, tileId);
                }
            }
        }

        /// <summary>
        /// Draws the level to screen.
        /// </summary>
        /// <param name="sb">Activated <see cref="SpriteBatch"/></param>
        internal void Draw(SpriteBatch spriteBatch)
        {
            belowPlayer.Draw(spriteBatch);
            abovePlayer.Draw(spriteBatch);
        }

        /// <summary>
        /// Convenience method to grab data from an attribute. Returns the default value if not possible.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private int GetTileSystemAttribute(XElement element, string attributeName, int defaultValue)
        {
            var rawValue = element.Attribute(attributeName)?.Value;
            if (rawValue == null)
            {
                return defaultValue;
            }

            // Fixme: if it weren't for the TryParse this could be generic
            return int.TryParse(rawValue, out int parsedValue) ? parsedValue : defaultValue;
        }
    }
}
