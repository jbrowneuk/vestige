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
        private int drawOffset;

        internal SpeechSystem()
        {
            isShown = false;
            isAnimating = false;
        }

        internal Texture2D BaseArea { get; set; }

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

                drawOffset = (int)MathHelper.Lerp(from, to, animationOffset);
            }
            else
            {
                // Todo: refactor this - pretty hacky
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

            var yPosition = Viewport.Bottom - visualAreaHeight + drawOffset;
            Rectangle drawableArea = new Rectangle(0, yPosition, Viewport.Width, visualAreaHeight);
            spriteBatch.Draw(BaseArea, drawableArea, Color.Black);

            Vector2 textCenter = MainFont.MeasureString(displayText) / 2;
            Vector2 drawPosition = new Vector2(drawableArea.Center.X, drawableArea.Center.Y) - textCenter;
            spriteBatch.DrawString(MainFont, displayText, drawPosition, Color.White);

            string debugVars = String.Format("a:{0} aO:{1} dO:{2}", isAnimating, animationOffset, drawOffset);
            spriteBatch.DrawString(MainFont, debugVars, Vector2.One, Color.Red);
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
