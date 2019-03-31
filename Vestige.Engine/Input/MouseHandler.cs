using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Vestige.Engine.Input
{
    /// <summary>
    /// Used to reference a mouse button in the <see cref="MouseHandler"/>
    /// </summary>
    internal enum MouseButton
    {
        Left,
        Right,
        Middle
    }

    /// <summary>
    /// Mouse abstraction layer
    /// </summary>
    internal class MouseHandler
    {
        private MouseState currentState;
        private MouseState lastState;

        /// <summary>
        /// Gets the current position of the mouse cursor.
        /// </summary>
        internal Point CurrentPosition { get { return currentState.Position; } }

        /// <summary>
        /// Gets the scroll distance change over the last frame.
        /// </summary>
        internal Vector2 ScrollAmount
        {
            get
            {
                int xDiff = currentState.HorizontalScrollWheelValue - lastState.HorizontalScrollWheelValue;
                int yDiff = currentState.ScrollWheelValue - lastState.ScrollWheelValue;
                return new Vector2(xDiff, yDiff);
            }
        }

        /// <summary>
        /// Used to update the current internal state of the mouse handler.
        /// </summary>
        internal void Update()
        {
            lastState = currentState;
            currentState = Mouse.GetState();
        }

        /// <summary>
        /// Returns true if a specified button is pressed.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> to check</param>
        internal bool IsButtonDown(MouseButton button)
        {
            return IsButtonInState(currentState, button, ButtonState.Pressed);
        }

        /// <summary>
        /// Returns true if a specified button is not pressed.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> to check</param>
        internal bool IsButtonUp(MouseButton button)
        {
            return IsButtonInState(currentState, button, ButtonState.Released);
        }

        /// <summary>
        /// Returns true when a button has changed state from not pressed to pressed.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> to check</param>
        internal bool WasButtonJustPressed(MouseButton button)
        {
            bool prev = IsButtonInState(lastState, button, ButtonState.Released);
            bool curr = IsButtonInState(currentState, button, ButtonState.Pressed);
            return prev && curr;
        }

        /// <summary>
        /// Returns true when a button has changed state from pressed to not pressed.
        /// </summary>
        /// <param name="button">The <see cref="MouseButton"/> to check</param>
        internal bool WasButtonJustReleased(MouseButton button)
        {
            bool prev = IsButtonInState(lastState, button, ButtonState.Pressed);
            bool curr = IsButtonInState(currentState, button, ButtonState.Released);
            return prev && curr;
        }

        /// <summary>
        /// Convenience method to query the state of a button.
        /// </summary>
        /// <param name="mouseState">The <see cref="MouseState"/> to query</param>
        /// <param name="button">Which <see cref="MouseButton"/> to query for</param>
        /// <param name="buttonState">What <see cref="ButtonState"/> should be expected</param>
        /// <returns></returns>
        internal bool IsButtonInState(MouseState mouseState, MouseButton button, ButtonState buttonState)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return mouseState.LeftButton == buttonState;

                case MouseButton.Middle:
                    return mouseState.MiddleButton == buttonState;

                case MouseButton.Right:
                    return mouseState.RightButton == buttonState;
            }

            return false;
        }
    }
}
