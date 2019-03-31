using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vestige.Engine.Core;
using Vestige.Engine.Input;

namespace Vestige.Engine
{
    public class EditorRunner : Game
    {
        private readonly KeyboardHandler keyboardHandler;
        private readonly MouseHandler mouseHandler;
        private readonly Overworld overworld;
        private readonly GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        private Texture2D blankSquare;
        private SpriteFont mainFont;
        private Point selection;
        private string selectedTileInfo;

        public EditorRunner()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            keyboardHandler = new KeyboardHandler();
            mouseHandler = new MouseHandler();
            overworld = new Overworld();
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            Window.AllowUserResizing = true;

            overworld.LoadLevel(Content.RootDirectory + "/Maps/town1.xml");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            overworld.UpdateTileSet(Content.Load<Texture2D>(@"Images/outdoor"));
            blankSquare = Content.Load<Texture2D>(@"Images/square");
            mainFont = Content.Load<SpriteFont>(@"Fonts/default");
        }

        protected override void Update(GameTime gameTime)
        {
            UpdateEditor(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            overworld.Draw(spriteBatch);

            RenderGrid(spriteBatch);
            RenderSelection(spriteBatch);
            RenderSidebar(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void UpdateEditor(GameTime gameTime)
        {
            // Only respond if game window is active
            if (!IsActive)
            {
                return;
            }

            keyboardHandler.Update();
            if (keyboardHandler.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            mouseHandler.Update();
            if (mouseHandler.WasButtonJustPressed(MouseButton.Left))
            {
                Point clickLocation = mouseHandler.CurrentPosition;
                Point gridLocation = new Point(clickLocation.X / Constants.TileSize, clickLocation.Y / Constants.TileSize);
                if (gridLocation.X >= 0 && gridLocation.X < overworld.WorldWidth && gridLocation.Y >= 0 || gridLocation.Y < overworld.WorldHeight)
                {
                    selection = gridLocation;
                    selectedTileInfo =string.Format("Selected tile ID is {0}", overworld.DemoBelowPlayer.GetTileId(gridLocation)); ;
                }
            }
        }

        private void RenderGrid(SpriteBatch spriteBatch)
        {
            var lineColor = Color.Blue * 0.5f;
            for (var x = 0; x < overworld.WorldWidth; x++)
            {
                var lineTemplate = new Rectangle(x * Constants.TileSize, 0, 1, overworld.WorldHeight * Constants.TileSize);
                spriteBatch.Draw(blankSquare, lineTemplate, lineColor);
            }

            for (var y = 0; y < overworld.WorldHeight; y++)
            {
                var lineTemplate = new Rectangle(0, y * Constants.TileSize, overworld.WorldWidth * Constants.TileSize, 1);
                spriteBatch.Draw(blankSquare, lineTemplate, lineColor);
            }
        }

        private void RenderSelection(SpriteBatch spriteBatch)
        {
            // fixme: demo purposes only
            if (selectedTileInfo != null)
            {
                spriteBatch.Draw(blankSquare, selection.ToVector2() * Constants.TileSize, Color.White * 0.5f);
            }
        }

        private void RenderSidebar(SpriteBatch spriteBatch)
        {
            const int sidebarWidth = 320;
            Rectangle screenBounds = graphics.GraphicsDevice.Viewport.Bounds;
            Rectangle sideBarArea = new Rectangle(screenBounds.Width - sidebarWidth, 0, 1, screenBounds.Height); // Dividing line only currently
            spriteBatch.Draw(blankSquare, sideBarArea, Color.Gray);

            if (selectedTileInfo != null)
            {
                spriteBatch.DrawString(mainFont, selectedTileInfo, new Vector2(sideBarArea.X + 8, sideBarArea.Y + 4), Color.Black);
            }

            // Fixme: this is demo only
            spriteBatch.DrawString(mainFont, "TileSet:", new Vector2(sideBarArea.X + 8, sideBarArea.Y + 24), Color.Black);
            spriteBatch.Draw(overworld.DemoBelowPlayer.SpriteSheet, new Vector2(sideBarArea.X + 8, sideBarArea.Y + 48), Color.White);
        }
    }
}
