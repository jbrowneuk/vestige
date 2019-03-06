using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vestige.Engine.Core;
using Vestige.Engine.Input;

namespace Vestige.Engine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameRunner : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private KeyboardHandler keyboardHandler;
        private readonly OverworldObject player;
        private readonly AnimatedObject playerSprite;

        // Todo: there should probably be 2 below and 2 above the player to
        // allow for complex structures to appear above and below
        private readonly TileSystem lowerTileSystem;
        private readonly TileSystem upperTileSystem;

        public GameRunner()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            keyboardHandler = new KeyboardHandler();
            player = new OverworldObject();
            playerSprite = new AnimatedObject();
            lowerTileSystem = new TileSystem();
            upperTileSystem = new TileSystem();

            player.Sprite = playerSprite;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            lowerTileSystem.Initialize(24, 24, 4, 5);

            lowerTileSystem.AddTile(0, 0, 9);
            lowerTileSystem.AddTile(1, 0, 10);
            lowerTileSystem.AddTile(2, 0, 10);
            lowerTileSystem.AddTile(3, 0, 11);

            lowerTileSystem.AddTile(0, 1, 6);
            lowerTileSystem.AddTile(1, 1, 7);
            lowerTileSystem.AddTile(2, 1, 7);
            lowerTileSystem.AddTile(3, 1, 8);

            lowerTileSystem.AddTile(0, 2, 6);
            lowerTileSystem.AddTile(1, 2, 7);
            lowerTileSystem.AddTile(2, 2, 7);
            lowerTileSystem.AddTile(3, 2, 8);

            lowerTileSystem.AddTile(0, 3, 3);
            lowerTileSystem.AddTile(1, 3, 4);
            lowerTileSystem.AddTile(2, 3, 4);
            lowerTileSystem.AddTile(3, 3, 5);

            lowerTileSystem.AddTile(0, 4, 0);
            lowerTileSystem.AddTile(1, 4, 1);
            lowerTileSystem.AddTile(2, 4, 1);
            lowerTileSystem.AddTile(3, 4, 2);

            upperTileSystem.Initialize(24, 24, 4, 5);

            upperTileSystem.AddTile(1, 3, 13);
            upperTileSystem.AddTile(2, 3, 13);

            upperTileSystem.AddTile(1, 4, 12);
            upperTileSystem.AddTile(2, 4, 13);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerSprite.SpriteSheet = Content.Load<Texture2D>(@"Images/char-f");
            lowerTileSystem.SpriteSheet = Content.Load<Texture2D>(@"Images/outdoor");
            upperTileSystem.SpriteSheet = lowerTileSystem.SpriteSheet;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            keyboardHandler.Update();

            if (keyboardHandler.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            Vector2 keyboardMovement = Vector2.Zero;
            if (keyboardHandler.IsKeyDown(Keys.Right) || keyboardHandler.IsKeyDown(Keys.Left))
            {
                keyboardMovement.X = keyboardHandler.IsKeyDown(Keys.Right) ? 1 : -1;
            }
            else if (keyboardHandler.IsKeyDown(Keys.Up) || keyboardHandler.IsKeyDown(Keys.Down))
            {
                keyboardMovement.Y = keyboardHandler.IsKeyDown(Keys.Up) ? -1 : 1;
            }

            if (keyboardMovement != Vector2.Zero)
            {
                player.Move(keyboardMovement);
            }

            player.Update(gameTime);

            // TODO make this sensible and the class use more vars
            if (keyboardMovement.X < 0)
            {
                playerSprite.FrameOffset = 12;
            }
            else if (keyboardMovement.X > 0)
            {
                playerSprite.FrameOffset = 4;
            }
            else if (keyboardMovement.Y > 0)
            {
                playerSprite.FrameOffset = 8;
            }
            else if (keyboardMovement.Y < 0)
            {
                playerSprite.FrameOffset = 0;
            }
            playerSprite.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            lowerTileSystem.Draw(spriteBatch);
            upperTileSystem.Draw(spriteBatch);
            playerSprite.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
