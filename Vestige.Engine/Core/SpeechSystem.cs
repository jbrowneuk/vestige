using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Core
{
    internal class SpeechSystem
    {
        private const int visualAreaHeight = 240;
        private const float moveAnimationSeconds = 0.25f;

        private string[] messages;
        private int currentMessageIndex;
        private string displayText;

        private bool isShown;
        private bool isAnimating;
        private float animationOffset;
        private float drawOffset;

        internal SpeechSystem()
        {
            isShown = false;
            isAnimating = false;
        }

        internal Texture2D BaseArea { get; set; }

        internal Texture2D SpeechBubble { get; set; }

        internal SpriteFont MainFont { get; set; }

        internal Rectangle Viewport { get; set; }

        internal void ShowText()
        {
            if (isShown)
            {
                return;
            }

            // Todo: read messages from file
            messages = new string[] { "Message 1", "Message 2", "Message 3" };

            // Reset control variables
            currentMessageIndex = 0;
            displayText = CalculateFormattedText(messages[currentMessageIndex]);

            // show visual area
            isShown = true;
            isAnimating = true;
            animationOffset = 0;
            drawOffset = visualAreaHeight;
        }

        internal void AdvanceText()
        {
            if (!isShown || isAnimating)
            {
                return;
            }

            if (currentMessageIndex < messages.Length - 1)
            {
                currentMessageIndex += 1;
                displayText = CalculateFormattedText(messages[currentMessageIndex]);
            }
            else
            {
                isAnimating = true;
            }
        }

        internal void Update(GameTime gameTime)
        {
            if (!isShown)
            {
                return;
            }

            if (isAnimating && animationOffset < 1)
            {
                animationOffset += (float)gameTime.ElapsedGameTime.TotalSeconds / moveAnimationSeconds;

                // Apply animation direction based on whether this is the first
                // message string
                int from = 0;
                int to = 0;
                if (currentMessageIndex == 0)
                {
                    from = visualAreaHeight;
                }
                else
                {
                    to = visualAreaHeight;
                }

                drawOffset = MathHelper.Lerp(from, to, animationOffset);
            }
            else
            {
                if (isAnimating)
                {
                    onAnimationFinished();
                }

                isAnimating = false;
                animationOffset = 0;
                drawOffset = 0;
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!isShown)
            {
                return;
            }

            // Base area
            float yPosition = Viewport.Bottom - visualAreaHeight + drawOffset;
            Rectangle drawableArea = new Rectangle(0, (int)yPosition, Viewport.Width, visualAreaHeight);
            spriteBatch.Draw(BaseArea, drawableArea, Color.Gray);

            // Text area (comes up at different speed)
            float centerY = yPosition + visualAreaHeight / 2;
            Vector2 drawCenter = new Vector2(drawableArea.Center.X, centerY + (drawOffset / 2));
            Vector2 bubbleOffset = new Vector2(SpeechBubble.Width, SpeechBubble.Height) / 2;
            spriteBatch.Draw(SpeechBubble, drawCenter - bubbleOffset, Color.White);
            Vector2 textCenter = MainFont.MeasureString(displayText) / 2;
            Vector2 drawPosition = drawCenter - textCenter;
            spriteBatch.DrawString(MainFont, displayText, drawPosition, Color.Black);
        }

        private string CalculateFormattedText(string rawText)
        {
            // todo - calc drawable area, control widths of strings based upon SpriteFont.MeasureString
            return rawText;
        }

        private void onAnimationFinished()
        {
            if (currentMessageIndex == messages.Length - 1)
            {
                isShown = false;
            }
        }
    }
}
