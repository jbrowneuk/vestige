using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Vestige.Engine.Dialogue
{
    /// <summary>
    /// Used to represent speech (dialogue) in-game. Encapsulates the visuals and control of the conversation.
    /// </summary>
    internal class DialogueSystem
    {
        private const int visualAreaHeight = 240;
        private const float moveAnimationSeconds = 0.5f;
        private const float entryScalingFactor = 1.5f;
        private const int bubbleVerticalOffset = -80;

        private readonly Color shadeColor;

        // Variables to control dialog
        private IDialoguePart[] messages;
        private IDialoguePart currentDialogPart;
        private int currentMessageIndex;
        private string displayText;

        // Variables to control visuals
        private bool isShown;
        private bool isAnimating;
        private bool isUp;
        private float animationOffset;
        private float drawOffset;
        private float blackoutScale;

        internal DialogueSystem()
        {
            isShown = false;
            isAnimating = false;
            isUp = false;
            shadeColor = new Color(Color.Black, 0.5f);
        }

        /// <summary>
        /// Used for drawing the blank areas.
        /// </summary>
        internal Texture2D BlankTexture { get; set; }

        /// <summary>
        /// Used for drawing the speech bubble.
        /// </summary>
        internal Texture2D SpeechBubble { get; set; }

        /// <summary>
        /// Debug - remove when completed.
        /// </summary>
        internal Texture2D DebugCharacter { get; set; }

        /// <summary>
        /// The font used to draw the dialog.
        /// </summary>
        internal SpriteFont Font { get; set; }

        /// <summary>
        /// The game window viewport.
        /// </summary>
        internal Rectangle Viewport { get; set; }

        /// <summary>
        /// Initialises a dialog and shows the system.
        /// </summary>
        internal void ShowText()
        {
            if (isShown)
            {
                return;
            }

            LoadDialogue();

            // Reset control variables
            currentMessageIndex = 0;
            CalculateDialogueText();

            // Show visual area
            isShown = true;
            isAnimating = true;
            animationOffset = 0;
            drawOffset = visualAreaHeight;
            blackoutScale = 0;
        }

        /// <summary>
        /// Used to advance text if possible or closes the dialog if the messages have ended.
        /// </summary>
        internal void AdvanceText()
        {
            if (!isShown || isAnimating)
            {
                return;
            }

            if (currentMessageIndex < messages.Length - 1)
            {
                currentMessageIndex += 1;
                CalculateDialogueText();
            }
            else
            {
                isAnimating = true;
            }
        }

        /// <summary>
        /// Used to update the current internal state of the system.
        /// </summary>
        /// <param name="gameTime">Current GameTime value from game runner</param>
        internal void Update(GameTime gameTime)
        {
            if (!isShown)
            {
                return;
            }

            if (isAnimating && animationOffset < 1)
            {
                animationOffset += (float)gameTime.ElapsedGameTime.TotalSeconds / moveAnimationSeconds;

                // Control animation direction (i.e. fade in/out) based on position in dialog
                int from = 0;
                int to = 0;

                if (!isUp)
                {
                    from = 1;
                }
                else
                {
                    to = 1;
                }

                drawOffset = MathHelper.Lerp(from * visualAreaHeight, to * visualAreaHeight, animationOffset);
                blackoutScale = MathHelper.Lerp(to, from, animationOffset); // Shade effect is inversed
            }
            else
            {
                if (isAnimating)
                {
                    isUp = drawOffset <= 0;

                    if (!isUp)
                    {
                        // Clear state
                        messages = null;
                        isShown = false;
                    }
                }

                isAnimating = false;
                animationOffset = 0;
                drawOffset = 0;
                blackoutScale = 1;
            }
        }

        /// <summary>
        /// Draws the current system to screen.
        /// </summary>
        /// <param name="spriteBatch">Activated <see cref="SpriteBatch"/></param>
        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!isShown)
            {
                return;
            }

            // Darken play area
            spriteBatch.Draw(BlankTexture, Viewport, shadeColor * blackoutScale);

            // Base image area
            float yPosition = Viewport.Bottom - visualAreaHeight + drawOffset;
            Rectangle drawableArea = new Rectangle(0, (int)yPosition, Viewport.Width, visualAreaHeight);
            spriteBatch.Draw(BlankTexture, drawableArea, Color.SkyBlue);

            // Characters
            float characterTop = yPosition + drawOffset * entryScalingFactor;
            DrawLeftSideCharacter(spriteBatch, characterTop);
            DrawRightSideCharacter(spriteBatch, characterTop);

            // Text area
            if (currentDialogPart.BubbleDirection != DialogueDirection.None)
            {
                DrawSpeechBubble(spriteBatch, drawableArea, yPosition);
            }
        }

        /// <summary>
        /// Draws the speech bubble
        /// </summary>
        private void DrawSpeechBubble(SpriteBatch spriteBatch, Rectangle drawableArea, float yPosition)
        {
            DialogueDirection currentBubbleDirection = messages[currentMessageIndex].BubbleDirection;
            if (currentBubbleDirection == DialogueDirection.None)
            {
                return;
            }

            float centerY = yPosition + bubbleVerticalOffset + visualAreaHeight / 2;
            Vector2 drawCenter = new Vector2(drawableArea.Center.X, centerY + drawOffset * entryScalingFactor);
            Vector2 bubbleOffset = new Vector2(SpeechBubble.Width, SpeechBubble.Height) / 2;
            SpriteEffects effects = currentBubbleDirection == DialogueDirection.Left ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(SpeechBubble, drawCenter - bubbleOffset, null, Color.White, 0.0f, Vector2.Zero, 1f, effects, 0.0f);

            if (displayText.Length > 0)
            {
                Vector2 textCenter = Font.MeasureString(displayText) / 2;
                Vector2 drawPosition = drawCenter - textCenter;
                spriteBatch.DrawString(Font, displayText, drawPosition, Color.Black);
            }
        }

        /// <summary>
        /// Draws the characters on the left side of the bubble.
        /// </summary>
        private void DrawLeftSideCharacter(SpriteBatch spriteBatch, float yPosition)
        {
            DialogueDirection direction = currentDialogPart.LeftCharacterDirection;
            DrawCharacter(spriteBatch, (int)-drawOffset, yPosition, direction);
        }

        /// <summary>
        /// Draws the characters on the right side of the bubble.
        /// </summary>
        private void DrawRightSideCharacter(SpriteBatch spriteBatch, float yPosition)
        {
            DialogueDirection direction = currentDialogPart.RightCharacterDirection;
            DrawCharacter(spriteBatch, Viewport.Right - DebugCharacter.Width + (int)drawOffset, yPosition, direction);
        }

        /// <summary>
        /// Draws a character graphic at a specified position.
        /// </summary>
        private void DrawCharacter(SpriteBatch spriteBatch, int xPosition, float yPosition, DialogueDirection direction)
        {
            if (direction == DialogueDirection.None)
            {
                return;
            }

            Vector2 characterPos = new Vector2(xPosition, yPosition);
            SpriteEffects flip = direction == DialogueDirection.Left ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(DebugCharacter, characterPos, null, Color.White, 0f, Vector2.Zero, 1f, flip, 0);
        }

        /// <summary>
        /// Loads speech dialogue from file.
        /// </summary>
        private void LoadDialogue()
        {
            List<IDialoguePart> parsedMessages = new List<IDialoguePart>();
            const string filename = "Content/Dialogue/test.xml"; // Fixme

            XDocument document;
            try
            {
                document = XDocument.Load(filename);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Could not load dialogue: {0} thrown.\n{1}", exception.GetType(), exception.Message);
                return;
            }

            var parts = document.Root.Elements("Part");
            foreach (var partElement in parts)
            {
                // if part[type] == "message"…
                var messageElement = partElement.Element("Message");
                if (messageElement == null)
                {
                    continue;
                }

                var characterLeftDirection = ParseDirectionFromElement(partElement.Element("CharacterLeft"));
                var characterRightDirection = ParseDirectionFromElement(partElement.Element("CharacterRight"));
                var bubbleDirection = ParseDirectionFromElement(partElement.Element("Bubble"));

                parsedMessages.Add(new TextDialoguePart(bubbleDirection, characterLeftDirection, characterRightDirection, messageElement.Value));
            }

            messages = parsedMessages.ToArray();
        }

        private DialogueDirection ParseDirectionFromElement(XElement element)
        {
            if (element == null)
            {
                return DialogueDirection.None;
            }

            return Enum.TryParse(element.Value, out DialogueDirection parsedValue) ? parsedValue : DialogueDirection.None;
        }

        /// <summary>
        /// Calculates the string to display based on the drawable area.
        /// </summary>
        private void CalculateDialogueText()
        {
            currentDialogPart = messages[currentMessageIndex];
            if (currentDialogPart.GetType() == typeof(TextDialoguePart))
            {
                // todo - calc drawable area, control widths of strings based upon SpriteFont.MeasureString
                displayText = (currentDialogPart as TextDialoguePart).MessageText;
            }
            else
            {
                displayText = "";
            }
        }
    }
}
