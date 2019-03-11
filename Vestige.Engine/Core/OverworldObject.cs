using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Core
{
    public class OverworldObject
    {
        private const int gridSize = 12;
        private const float moveSpeed = gridSize * 8f;
        private Vector2 currentPosition;
        private Vector2 startPosition;
        private Vector2 endPosition;
        private float movement;

        internal OverworldObject()
        {
            currentPosition = Vector2.Zero;
            startPosition = currentPosition;
            endPosition = currentPosition;
            movement = 0;
        }

        public AnimatedObject Sprite { get; set; } = null;

        public Vector2 DrawOffset { get; set; } = Vector2.Zero;

        internal void Update(GameTime time)
        {
            if (startPosition != endPosition && movement < 1)
            {
                movement += (float)time.ElapsedGameTime.TotalSeconds * (moveSpeed / gridSize);
                currentPosition = Vector2.Lerp(startPosition, endPosition, movement);
            }
            else
            {
                movement = 0;
                currentPosition = endPosition;
                startPosition = endPosition;
            }

            if (Sprite != null)
            {
                Sprite.Position = currentPosition + DrawOffset;
            }
        }

        internal void Move(Vector2 keyboardMovement)
        {
            // Check if on grid
            if (Math.Abs(currentPosition.X % gridSize) > float.Epsilon || Math.Abs(currentPosition.Y % gridSize) > float.Epsilon)
            {
                return;
            }

            startPosition = currentPosition;
            endPosition = startPosition + (Vector2.Normalize(keyboardMovement) * gridSize);
        }
    }
}
