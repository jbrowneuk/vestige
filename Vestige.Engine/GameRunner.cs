using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vestige.Engine.Core;
using Vestige.Engine.Input;

namespace Vestige.Engine
{
    /// <summary>
    /// This is the main type for the game.
    /// </summary>
    public class GameRunner : Game
    {
        private readonly KeyboardHandler keyboardHandler;
        private readonly OverworldObject player;
        private readonly AnimatedObject playerSprite;
        private readonly Overworld overworld;
        private readonly DialogSystem speechSystem;
        private readonly GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;

        public GameRunner()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            keyboardHandler = new KeyboardHandler();
            speechSystem = new DialogSystem();

            overworld = new Overworld();
            playerSprite = new AnimatedObject();
            player = new OverworldObject
            {
                DrawOffset = new Vector2(0, -8),
                Sprite = playerSprite
            };
        }

        /// <summary>
        /// Used by the MonoGame system to initialise the game.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            overworld.LoadLevel(Content.RootDirectory + "/Maps/town1.xml");
        }

        /// <summary>
        /// Used to load any content whose lifetime is the same as the game.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            speechSystem.Viewport = graphics.GraphicsDevice.Viewport.Bounds;

            playerSprite.SpriteSheet = Content.Load<Texture2D>(@"Images/char-f");
            overworld.UpdateTileSet(Content.Load<Texture2D>(@"Images/outdoor"));

            speechSystem.Font = Content.Load<SpriteFont>(@"Fonts/default");
            speechSystem.BlankTexture = Content.Load<Texture2D>(@"Images/square");
            speechSystem.SpeechBubble = Content.Load<Texture2D>(@"Images/speechBubble");
            speechSystem.DebugCharacter = Content.Load<Texture2D>(@"Images/test-001");
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

            // Speech system
            speechSystem.Update(gameTime);
            if (keyboardHandler.WasKeyJustPressed(Keys.Space))
            {
                speechSystem.AdvanceText();
            }

            if (keyboardHandler.WasKeyJustPressed(Keys.Enter))
            {
                speechSystem.ShowText();
            }

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
            overworld.Draw(spriteBatch);
            playerSprite.Draw(spriteBatch);
            speechSystem.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
