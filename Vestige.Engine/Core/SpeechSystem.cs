using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Core
{
    internal class SpeechSystem
    {
        private const int visualAreaHeight = 240;

        private string[] messages;
        private int currentMessageIndex;
        private string displayText;
        private TimeSpan timeSinceLetter;
        private bool isShown;

        internal SpeechSystem()
        {
            messages = new string[] { "Message 1", "Message 2", "Message 3" };
            currentMessageIndex = 0;
            displayText = CalculateFormattedText(messages[currentMessageIndex]);
            timeSinceLetter = TimeSpan.Zero;
            isShown = true;
        }

        internal Texture2D BaseArea { get; set; }

        internal SpriteFont MainFont { get; set; }

        internal Rectangle Viewport { get; set; }

        internal void AdvanceText()
        {
            if (currentMessageIndex < messages.Length - 1)
            {
                currentMessageIndex += 1;
                displayText = CalculateFormattedText(messages[currentMessageIndex]);
            }
            else
            {
                isShown = false;
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!isShown)
            {
                return;
            }

            Rectangle drawableArea = new Rectangle(0, Viewport.Bottom - visualAreaHeight, Viewport.Width, visualAreaHeight);
            spriteBatch.Draw(BaseArea, drawableArea, Color.Black);
            spriteBatch.DrawString(MainFont, displayText, new Vector2(drawableArea.Left, drawableArea.Top), Color.White);
        }

        private string CalculateFormattedText(string rawText)
        {
            // todo - calc drawable area, control widths of strings based upon SpriteFont.MeasureString
            return rawText;
        }
    }
}
