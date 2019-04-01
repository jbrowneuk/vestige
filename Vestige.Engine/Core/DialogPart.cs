namespace Vestige.Engine.Core
{
    /// <summary>
    /// Used to control a segment of a speech dialog conversation in the <see cref="DialogSystem"/>.
    /// </summary>
    /// <seealso cref="DialogDirection"/>
    internal class DialogPart
    {
        public string MessageText { get; private set; }
        public DialogDirection Direction { get; private set; }
        public DialogDirection LeftCharacterDirection { get; private set; }
        public DialogDirection RightCharacterDirection { get; private set; }

        internal DialogPart(
            string message,
            DialogDirection dir,
            DialogDirection leftChar = DialogDirection.Right,
            DialogDirection rightChar = DialogDirection.Left)
        {
            MessageText = message;
            Direction = dir;
            LeftCharacterDirection = leftChar;
            RightCharacterDirection = rightChar;
        }
    }
}
