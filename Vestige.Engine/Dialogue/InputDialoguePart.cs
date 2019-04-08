using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Dialogue
{
    /// <summary>
    /// Used to control a multiple-choice segment of a speech dialog conversation.
    /// </summary>
    internal class InputDialoguePart : DialoguePart
    {
        private readonly List<string> options;
        private int selectedOption;

        internal InputDialoguePart(
            DialogueDirection bubble,
            DialogueDirection leftChar,
            DialogueDirection rightChar,
            List<string> choices) : base(bubble, leftChar, rightChar)
        {
            options = choices;
            selectedOption = 0;
        }

        internal override void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 drawCenter)
        {
            int fontHeight = (int)font.MeasureString("dp").Y;
            Vector2 topLine = drawCenter - new Vector2(0, (fontHeight * options.Count) / 2 - fontHeight / 2); // Move central line point down

            for (int index = 0; index < options.Count; index++)
            {
                string option = options[index];

                // Calculate draw position
                Vector2 lineOffset = Vector2.UnitY * fontHeight * index;
                Vector2 drawPosition = topLine + lineOffset;

                // Calculate text effects
                Color textColor = index == selectedOption ? Color.Red : Color.Black;

                DrawTextLine(spriteBatch, font, option, drawPosition, textColor);
            }
        }

        internal void SelectPreviousOption()
        {
            selectedOption = selectedOption > 0 ? selectedOption - 1 : options.Count - 1;
        }

        internal void SelectNextOption()
        {
            selectedOption = (selectedOption + 1) % options.Count;
        }
    }
}
