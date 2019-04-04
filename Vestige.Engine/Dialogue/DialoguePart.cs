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
    }
}
