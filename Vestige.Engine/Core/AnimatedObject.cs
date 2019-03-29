using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Core
{
    /// <summary>
    /// Encapsulates an animated graphic from a sprite sheet.
    /// </summary>
    internal class AnimatedObject
    {
        const double animationMsec = 1000 / 4d; // Todo: this should be a variable

        int currentFrame;
        int frameWidth;
        int frameHeight;

        TimeSpan timeSinceLastFrame;

        internal AnimatedObject()
        {
            currentFrame = 0;
            timeSinceLastFrame = TimeSpan.Zero;
        }

        /// <summary>
        /// The spritesheet to use.
        /// </summary>
        internal Texture2D SpriteSheet { get; set; } = null;

        /// <summary>
        /// The offset of the first frame in the animation.
        /// </summary>
        internal int FrameOffset { get; set; } = 0;

        /// <summary>
        /// The number of frames to show before looping back to the first frame.
        /// </summary>
        internal int FramesPerAnimation { get; set; } = 4;

        /// <summary>
        /// Number of frames horizontally in the sprite sheet.
        /// </summary>
        internal int HorizontalFrames { get; set; } = 4;

        /// <summary>
        /// Number of frames vertically in the sprite sheet.
        /// </summary>
        internal int VerticalFrames { get; set; } = 4;

        /// <summary>
        /// The screen position of this object.
        /// </summary>
        internal Vector2 Position { get; set; } = Vector2.Zero;

        /// <summary>
        /// Used to update the current internal state of the object.
        /// </summary>
        /// <param name="gameTime">Current GameTime value from game runner</param>
        internal void Update(GameTime time)
        {
            if (SpriteSheet != null && frameWidth == 0 && frameHeight == 0)
            {
                frameWidth = SpriteSheet.Width / HorizontalFrames;
                frameHeight = SpriteSheet.Height / VerticalFrames;
            }

            timeSinceLastFrame += time.ElapsedGameTime;

            if (timeSinceLastFrame.TotalMilliseconds > animationMsec)
            {
                currentFrame = (currentFrame + 1) % FramesPerAnimation;
                timeSinceLastFrame = TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Draws the sprite to screen.
        /// </summary>
        /// <param name="sb">Activated <see cref="SpriteBatch"/></param>
        internal void Draw(SpriteBatch sb)
        {
            int actualFrame = currentFrame + FrameOffset;
            int frameX = (actualFrame % VerticalFrames) * frameHeight;
            int frameY = (int)Math.Floor(actualFrame / (float)VerticalFrames) * frameWidth;
            Rectangle frameSource = new Rectangle(frameX, frameY, frameWidth, frameHeight);
            sb.Draw(SpriteSheet, Position, frameSource, Color.White);
        }
    }
}
