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
        private readonly AnimatedObject testAnim;
        private readonly TileSystem tileSystem;

        public GameRunner()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            keyboardHandler = new KeyboardHandler();
            player = new OverworldObject();
            testAnim = new AnimatedObject();
            tileSystem = new TileSystem();

            player.Sprite = testAnim;
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
            tileSystem.Initialize(0, 0, 4, 4);
            tileSystem.AddTile(0, 10);
            tileSystem.AddTile(1, 11);
            tileSystem.AddTile(2, 11);
            tileSystem.AddTile(3, 12);

            tileSystem.AddTile(4, 7);
            tileSystem.AddTile(5, 8);
            tileSystem.AddTile(6, 8);
            tileSystem.AddTile(7, 9);

            tileSystem.AddTile(8, 4);
            tileSystem.AddTile(9, 5);
            tileSystem.AddTile(10, 5);
            tileSystem.AddTile(11, 6);

            tileSystem.AddTile(12, 1);
            tileSystem.AddTile(13, 2);
            tileSystem.AddTile(14, 2);
            tileSystem.AddTile(15, 3);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            testAnim.SpriteSheet = Content.Load<Texture2D>(@"Images/char-f");
            tileSystem.SpriteSheet = Content.Load<Texture2D>(@"Images/outdoor");
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
                testAnim.FrameOffset = 12;
            }
            else if (keyboardMovement.X > 0)
            {
                testAnim.FrameOffset = 4;
            }
            else if (keyboardMovement.Y > 0)
            {
                testAnim.FrameOffset = 8;
            }
            else if (keyboardMovement.Y < 0)
            {
                testAnim.FrameOffset = 0;
            }
            testAnim.Update(gameTime);

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
            tileSystem.Draw(spriteBatch);
            testAnim.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
