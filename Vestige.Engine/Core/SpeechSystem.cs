using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Core
{
    internal enum DialogDirection
    {
        Left,
        Right
    }

    internal class DialogPart
    {
        public string messageText;
        public DialogDirection direction;

        internal DialogPart(string message, DialogDirection dir)
        {
            messageText = message;
            direction = dir;
        }
    }

    internal class SpeechSystem
    {
        private const int visualAreaHeight = 240;
        private const float moveAnimationSeconds = 0.25f;

        private readonly Color shadeColor;

        private DialogPart[] messages;
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
            shadeColor = new Color(Color.Black, 0.5f);
        }

        /// <summary>
        /// Used for drawing the blank areas
        /// </summary>
        internal Texture2D BlankTexture { get; set; }

        /// <summary>
        /// Used for drawing the speech bubble
        /// </summary>
        internal Texture2D SpeechBubble { get; set; }

        internal Texture2D DebugCharacter { get; set; }

        /// <summary>
        /// The font used to draw the dialog
        /// </summary>
        internal SpriteFont Font { get; set; }

        /// <summary>
        /// The game window viewport
        /// </summary>
        internal Rectangle Viewport { get; set; }

        internal void ShowText()
        {
            if (isShown)
            {
                return;
            }

            // Todo: read messages from file
            messages = new DialogPart[] {
                new DialogPart("Message 1", DialogDirection.Right),
                new DialogPart("Message 2", DialogDirection.Right),
                new DialogPart("Message 3 facing left", DialogDirection.Left)
            };

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

            // Darken play area
            spriteBatch.Draw(BlankTexture, Viewport, shadeColor);

            // Base image area
            float yPosition = Viewport.Bottom - visualAreaHeight + drawOffset;
            Rectangle drawableArea = new Rectangle(0, (int)yPosition, Viewport.Width, visualAreaHeight);
            spriteBatch.Draw(BlankTexture, drawableArea, Color.SkyBlue);

            // Characters
            var rightCharacterPos = new Vector2(Viewport.Right - DebugCharacter.Width, yPosition + drawOffset / 2);
            spriteBatch.Draw(DebugCharacter, rightCharacterPos, Color.White);

            // Text area (comes up at different speed)
            float centerY = yPosition + visualAreaHeight / 2;
            Vector2 drawCenter = new Vector2(drawableArea.Center.X, centerY + (drawOffset / 2));
            Vector2 bubbleOffset = new Vector2(SpeechBubble.Width, SpeechBubble.Height) / 2;
            SpriteEffects effects = messages[currentMessageIndex].direction == DialogDirection.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(SpeechBubble, drawCenter - bubbleOffset, null, Color.White, 0.0f, Vector2.Zero, 1f, effects, 0.0f);

            Vector2 textCenter = Font.MeasureString(displayText) / 2;
            Vector2 drawPosition = drawCenter - textCenter;
            spriteBatch.DrawString(Font, displayText, drawPosition, Color.Black);
        }

        private string CalculateFormattedText(DialogPart part)
        {
            // todo - calc drawable area, control widths of strings based upon SpriteFont.MeasureString
            return part.messageText;
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
