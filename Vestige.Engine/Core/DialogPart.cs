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
        public bool IsLeftCharacterVisible { get; private set; }
        public bool IsRightCharacterVisible { get; private set; }

        internal DialogPart(
            string message,
            DialogDirection dir,
            bool leftChar,
            bool rightChar)
        {
            MessageText = message;
            Direction = dir;
            IsLeftCharacterVisible = leftChar;
            IsRightCharacterVisible = rightChar;
        }
    }
}
