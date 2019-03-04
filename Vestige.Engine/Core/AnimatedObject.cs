using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Core
{
    public class AnimatedObject
    {
        const int animationMaxFrames = 4;
        const int horizFrames = 4;
        const int vertFrames = 4;
        const int totalFrames = horizFrames * vertFrames;
        const double animationMsec = 1000 / 4d;

        int currentFrame;
        int frameWidth;
        int frameHeight;

        TimeSpan lastFrameTime;

        public AnimatedObject()
        {
            currentFrame = 0;
            lastFrameTime = TimeSpan.Zero;
        }

        public Texture2D SpriteSheet { get; set; } = null;

        public int FrameOffset { get; set; } = 0;

        public Vector2 Position { get; set; } = Vector2.Zero;

        public void Update(GameTime time)
        {
            if (SpriteSheet != null && frameWidth == 0 && frameHeight == 0)
            {
                frameWidth = SpriteSheet.Width / horizFrames;
                frameHeight = SpriteSheet.Height / vertFrames;
            }

            lastFrameTime += time.ElapsedGameTime;

            if (lastFrameTime.TotalMilliseconds > animationMsec)
            {
                currentFrame = (currentFrame + 1) % animationMaxFrames;
                lastFrameTime = TimeSpan.Zero;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            int actualFrame = currentFrame + FrameOffset;
            int frameX = (actualFrame % vertFrames) * frameHeight;
            int frameY = (int)Math.Floor(actualFrame / (double)vertFrames) * frameWidth;
            Rectangle frameSource = new Rectangle(frameX, frameY, frameWidth, frameHeight);
            sb.Draw(SpriteSheet, Position, frameSource, Color.White);
        }
    }
}
