using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalacticSurvival
{
    internal class GameOver
    {
        private UI continueButton;

        public Dictionary<string, UI> elements = new Dictionary<string, UI>();

        public GameOver(GraphicsDeviceManager graphics)
        {
            // Vars used for Main Menu UI Creation
            string text;
            Vector2 textSize;
            Vector2 textPos;
            Vector2 buttonPos;
            int buttonWidth = 0;
            int buttonHeight = 0;

            // Creates Play Button
            buttonWidth = 264;
            buttonHeight = 130;
            buttonPos.X = graphics.PreferredBackBufferWidth / 2 - buttonWidth / 2;
            buttonPos.Y = graphics.PreferredBackBufferHeight / 2 - buttonHeight / 2 + 150;
            text = "Continue";
            textSize = Game1.Text.MeasureString(text);
            textPos.X = buttonPos.X + buttonWidth / 2 - textSize.X / 2;
            textPos.Y = buttonPos.Y + buttonHeight / 2 - textSize.Y / 2;
            continueButton = new UI(buttonPos, buttonWidth, buttonHeight, text, Color.White, textPos, Game1.Text);
            elements["continue"] = continueButton; // Adds created element to elements list
        }

        public Game1.State Update(Game1.State currentState)
        {
            if (UpdateContinueButton())
            {
                elements["continue"].pressed = false;
                return Game1.State.MainMenu;
            }

            return currentState;
        }


        public bool UpdateContinueButton()
        {
            return elements["continue"].pressed;
        }
    }
}
