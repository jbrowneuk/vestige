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
        private DialoguePart[] messages;
        private DialoguePart currentDialogPart;
        private int currentMessageIndex;

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
            currentDialogPart = messages[currentMessageIndex];

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
                currentDialogPart = messages[currentMessageIndex];
            }
            else
            {
                isAnimating = true;
            }
        }

        internal void HandleMoveUpInteraction()
        {
            if (currentDialogPart is InputDialoguePart interactiveDialogPart)
            {
                interactiveDialogPart.SelectPreviousOption();
            }
        }

        internal void HandleMoveDownInteraction()
        {
            if (currentDialogPart is InputDialoguePart interactiveDialogPart)
            {
                interactiveDialogPart.SelectNextOption();
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
            DrawSpeechBubble(spriteBatch, drawableArea, yPosition);
            DrawIndicator(spriteBatch);
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

            currentDialogPart.Draw(spriteBatch, Font, drawCenter);
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
        /// Draws the indicator to advance the conversation.
        /// </summary>
        /// <param name="spriteBatch">Activated SpriteBatch</param>
        private void DrawIndicator(SpriteBatch spriteBatch)
        {
            string advanceMessage = currentMessageIndex < messages.Length - 1 ? "Next" : "End";
            Vector2 stringDimensions = Font.MeasureString(advanceMessage);
            Vector2 drawOrigin = new Vector2(Viewport.Width / 2, Viewport.Height);

            Rectangle backgroundRectangle = new Rectangle((int)(drawOrigin.X - stringDimensions.X / 2), (int)(drawOrigin.Y - stringDimensions.Y), (int)stringDimensions.X, (int)stringDimensions.Y);
            spriteBatch.Draw(BlankTexture, backgroundRectangle, Color.White);

            Vector2 position = drawOrigin - new Vector2(stringDimensions.X / 2, stringDimensions.Y);
            spriteBatch.DrawString(Font, advanceMessage, position, Color.Black);
        }

        /// <summary>
        /// Loads speech dialogue from file.
        /// </summary>
        private void LoadDialogue()
        {
            List<DialoguePart> parsedMessages = new List<DialoguePart>();
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
                // Todo: this should not be string-based
                DialoguePart parsedPart = null;
                string partType = "Message";
                var typeAttribute = partElement.Attribute("Type");
                if (typeAttribute != null)
                {
                    partType = typeAttribute.Value;
                }

                switch (partType)
                {
                    case "Choice":
                        parsedPart = ParseInputDialogPart(partElement);
                        break;

                    default:
                        parsedPart = ParseTextDialogPart(partElement);
                        break;
                }

                if (parsedPart != null)
                {
                    parsedMessages.Add(parsedPart);
                }
            }

            messages = parsedMessages.ToArray();
        }

        /// <summary>
        /// Used to parse a standard message dialogue element.
        /// </summary>
        /// <param name="partElement">The relevant XML element containing data</param>
        /// <returns>The parsed element, or null if not possible</returns>
        private DialoguePart ParseTextDialogPart(XElement partElement)
        {
            var messageElement = partElement.Element("Message");
            if (messageElement == null)
            {
                return null;
            }

            var characterLeftDirection = ParseDirectionFromElement(partElement.Element("CharacterLeft"));
            var characterRightDirection = ParseDirectionFromElement(partElement.Element("CharacterRight"));
            var bubbleDirection = ParseDirectionFromElement(partElement.Element("Bubble"));

            return new TextDialoguePart(bubbleDirection, characterLeftDirection, characterRightDirection, messageElement.Value);
        }

        /// <summary>
        /// Used to parse a multiple choice dialogue element.
        /// </summary>
        /// <param name="partElement">The relevant XML element containing data</param>
        /// <returns>The parsed element, or null if not possible</returns>
        private DialoguePart ParseInputDialogPart(XElement partElement)
        {
            var optionsContainerElement = partElement.Element("Options");
            if (optionsContainerElement == null)
            {
                return null;
            }

            List<string> choices = new List<string>();
            var optionElements = optionsContainerElement.Elements("Option");
            foreach (var optionElement in optionElements)
            {
                choices.Add(optionElement.Value);
            }

            return new InputDialoguePart(DialogueDirection.Left, DialogueDirection.Right, DialogueDirection.None, choices);
        }

        private DialogueDirection ParseDirectionFromElement(XElement element)
        {
            if (element == null)
            {
                return DialogueDirection.None;
            }

            return Enum.TryParse(element.Value, out DialogueDirection parsedValue) ? parsedValue : DialogueDirection.None;
        }
    }
}
