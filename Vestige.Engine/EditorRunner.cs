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
        private readonly Overworld overworld;
        private readonly GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        private Texture2D blankSquare;

        public EditorRunner()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            keyboardHandler = new KeyboardHandler();
            overworld = new Overworld();
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            overworld.LoadLevel(Content.RootDirectory + "/Maps/town1.xml");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            overworld.UpdateTileSet(Content.Load<Texture2D>(@"Images/outdoor"));
            blankSquare = Content.Load<Texture2D>(@"Images/square");
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardHandler.Update();

            if (keyboardHandler.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            overworld.Draw(spriteBatch);

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

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
