using Microsoft.Xna.Framework.Input;

namespace Vestige.Engine.Input
{
    /// <summary>
    /// Keyboard abstraction layer
    /// </summary>
    internal class KeyboardHandler
    {
        private KeyboardState currentState;
        private KeyboardState lastState;

        /// <summary>
        /// Used to update the current internal state of the keyboard handler.
        /// </summary>
        internal void Update()
        {
            lastState = currentState;
            currentState = Keyboard.GetState();
        }

        /// <summary>
        /// Returns true if a specified key is pressed.
        /// </summary>
        /// <param name="key">The keyboard key to check</param>
        internal bool IsKeyDown(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        /// <summary>
        /// Returns true if a specified key is not pressed.
        /// </summary>
        /// <param name="key">The keyboard key to check</param>
        internal bool IsKeyUp(Keys key)
        {
            return currentState.IsKeyUp(key);
        }

        /// <summary>
        /// Returns true when a keyboard key has changed state from not pressed to pressed.
        /// </summary>
        /// <param name="key">The keyboard key to check</param>
        internal bool WasKeyJustPressed(Keys key)
        {
            return currentState.IsKeyDown(key) && lastState.IsKeyUp(key);
        }

        /// <summary>
        /// Returns true when a keyboard key has changed state from pressed to not pressed.
        /// </summary>
        /// <param name="key">The keyboard key to check</param>
        internal bool WasKeyJustReleased(Keys key)
        {
            return currentState.IsKeyUp(key) && lastState.IsKeyDown(key);
        }
    }
}
