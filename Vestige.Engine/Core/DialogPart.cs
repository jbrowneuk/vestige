namespace Vestige.Engine.Core
{
    /// <summary>
    /// Used to control a segment of a speech dialog conversation in the <see cref="DialogSystem"/>.
    /// </summary>
    /// <seealso cref="DialogDirection"/>
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
}
