using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Dialogue
{
    /// <summary>
    /// Base class for all dialogue section elements.
    /// </summary>
    internal abstract class DialoguePart
    {
        internal DialoguePart(DialogueDirection bubble,
            DialogueDirection leftChar,
            DialogueDirection rightChar)
        {
            BubbleDirection = bubble;
            LeftCharacterDirection = leftChar;
            RightCharacterDirection = rightChar;
        }

        /// <summary>Direction of the speech bubble.</summary>
        internal DialogueDirection BubbleDirection { get; set; }

        /// <summary>Direction of the left character.</summary>
        internal DialogueDirection LeftCharacterDirection { get; set; }

        /// <summary>Direction of the Right character.</summary>
        internal DialogueDirection RightCharacterDirection { get; set; }

        /// <summary>
        /// Draws this dialogue part’s text.
        /// </summary>
        /// <param name="spriteBatch">Activated SpriteBatch</param>
        /// <param name="font">Font to use</param>
        /// <param name="drawCenter">Center of drawable area</param>
        internal abstract void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 drawCenter);

        /// <summary>
        /// Used to draw a string of text using the specified position as the center. Rounds drawing position to integer.
        /// </summary>
        /// <param name="spriteBatch">Activated SpriteBatch</param>
        /// <param name="font">Font to use</param>
        /// <param name="text">Text to draw</param>
        /// <param name="position">Position of the center of text</param>
        /// <param name="textColor">Color of text</param>
        protected void DrawTextLine(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color textColor)
        {
            Vector2 textOffset = font.MeasureString(text) / 2;
            Vector2 textPosition = new Vector2((int)(position.X - textOffset.X), (int)(position.Y - textOffset.Y));
            spriteBatch.DrawString(font, text, textPosition, textColor);
        }
    }
}
