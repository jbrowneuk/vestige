using System;
using Microsoft.Xna.Framework;

namespace Vestige.Engine.Core
{
    /// <summary>
    /// Generic class used to represent any object in the game's overworld (levels).
    /// </summary>
    internal class OverworldObject
    {
        private const float moveSpeed = Constants.GridSize * 8f;
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

        /// <summary>
        /// If set, the sprite will become the visual representation of this object
        /// and is kept updated with this object's current location.
        /// </summary>
        internal AnimatedObject Sprite { get; set; } = null;

        /// <summary>
        /// An optional offset that is added to the <see cref="Sprite"/>.
        /// </summary>
        internal Vector2 DrawOffset { get; set; } = Vector2.Zero;

        /// <summary>
        /// Used to update the current internal state of the object.
        /// </summary>
        /// <param name="gameTime">Current GameTime value from game runner</param>
        internal void Update(GameTime time)
        {
            if (startPosition != endPosition && movement < 1)
            {
                movement += (float)time.ElapsedGameTime.TotalSeconds * (moveSpeed / Constants.GridSize);
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

        /// <summary>
        /// Used to move the object on the grid.
        /// </summary>
        /// <param name="direction">A vector containing the direction to move in</param>
        internal void Move(Vector2 direction)
        {
            // Check if on grid
            if (Math.Abs(currentPosition.X % Constants.GridSize) > float.Epsilon || Math.Abs(currentPosition.Y % Constants.GridSize) > float.Epsilon)
            {
                return;
            }

            // Todo: handle diagonals
            startPosition = currentPosition;
            endPosition = startPosition + (Vector2.Normalize(direction) * Constants.GridSize);
        }
    }
}
