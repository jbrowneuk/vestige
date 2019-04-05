using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Dialogue
{
    /// <summary>
    /// Base interface for all dialogue section elements.
    /// </summary>
    internal interface IDialoguePart
    {
        DialogueDirection BubbleDirection { get; }
        DialogueDirection LeftCharacterDirection { get; }
        DialogueDirection RightCharacterDirection { get; }
        void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 drawCenter);
    }

    /// <summary>
    /// Used to control a segment of a speech dialog conversation in the <see cref="DialogueSystem"/>.
    /// </summary>
    internal class TextDialoguePart : IDialoguePart
    {
        public DialogueDirection BubbleDirection { get; private set; }
        public DialogueDirection LeftCharacterDirection { get; private set; }
        public DialogueDirection RightCharacterDirection { get; private set; }
        public string MessageText { get; private set; }

        internal TextDialoguePart(
            DialogueDirection bubble,
            DialogueDirection leftChar,
            DialogueDirection rightChar,
            string message)
        {
            BubbleDirection = bubble;
            LeftCharacterDirection = leftChar;
            RightCharacterDirection = rightChar;
            MessageText = message;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 drawCenter)
        {
            Vector2 textCenter = font.MeasureString(MessageText) / 2;
            Vector2 drawPosition = drawCenter - textCenter;
            spriteBatch.DrawString(font, MessageText, drawPosition, Color.Black);
        }
    }
}
