using Microsoft.Xna.Framework.Input;

namespace Vestige.Engine.Input
{
    public class KeyboardHandler
    {
        private KeyboardState currentState;
        private KeyboardState lastState;

        public void Update()
        {
            lastState = currentState;
            currentState = Keyboard.GetState();
        }

        public bool IsKeyDown(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return currentState.IsKeyUp(key);
        }

        public bool WasKeyJustPressed(Keys key)
        {
            return currentState.IsKeyDown(key) && lastState.IsKeyUp(key);
        }

        public bool WasKeyJustReleased(Keys key)
        {
            return currentState.IsKeyUp(key) && lastState.IsKeyDown(key);
        }
    }
}
