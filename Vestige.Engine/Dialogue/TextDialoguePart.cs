using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Dialogue
{
    /// <summary>
    /// Used to control a segment of a speech dialog conversation.
    /// </summary>
    internal class TextDialoguePart : DialoguePart
    {
        private readonly string rawText;

        internal TextDialoguePart(
            DialogueDirection bubble,
            DialogueDirection leftChar,
            DialogueDirection rightChar,
            string message) : base(bubble, leftChar, rightChar)
        {
            rawText = message;
        }

        internal override void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 drawCenter)
        {
            DrawTextLine(spriteBatch, font, rawText, drawCenter, Color.Black);
        }
    }
}
