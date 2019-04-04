namespace Vestige.Engine.Dialogue
{
    /// <summary>
    /// Used to control a segment of a speech dialog conversation in the <see cref="DialogueSystem"/>.
    /// </summary>
    /// <seealso cref="DialogueDirection"/>
    internal class DialoguePart
    {
        public string MessageText { get; private set; }
        public DialogueDirection Direction { get; private set; }
        public DialogueDirection LeftCharacterDirection { get; private set; }
        public DialogueDirection RightCharacterDirection { get; private set; }

        internal DialoguePart(
            string message,
            DialogueDirection dir,
            DialogueDirection leftChar,
            DialogueDirection rightChar)
        {
            MessageText = message;
            Direction = dir;
            LeftCharacterDirection = leftChar;
            RightCharacterDirection = rightChar;
        }
    }
}
