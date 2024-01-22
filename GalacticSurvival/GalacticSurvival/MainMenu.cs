using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalacticSurvival
{
    internal class MainMenu
    {
        private UI Title;

        private UI PlayButton;

        public Dictionary<string, UI> elements = new Dictionary<string, UI>();


        public MainMenu(GraphicsDeviceManager graphics, Camera camera)
        {
            // Vars used for Main Menu UI Creation
            string text;
            Vector2 textSize;
            Vector2 textPos;
            Vector2 buttonPos;
            int buttonWidth;
            int buttonHeight;


            // Creates Main Title
            text = "GALACTIC";
            textSize = Game1.Title.MeasureString(text);
            textPos = new Vector2(graphics.PreferredBackBufferWidth / 2 - textSize.X / 2, graphics.PreferredBackBufferHeight / 2 - textSize.Y / 2 - 200);
            textPos = camera.ScreenToWorld(textPos, 0); // Converts position to world coordinates for camera
            Title = new UI(text, Color.Red, textPos, Game1.Title);
            elements["title"] = Title; // Adds created element to elements list


            // Creates Play Button
            buttonWidth = 264;
            buttonHeight = 130;
            buttonPos.X = graphics.PreferredBackBufferWidth / 2 - buttonWidth / 2;
            buttonPos.Y = graphics.PreferredBackBufferHeight / 2 - buttonHeight / 2 + 150;
            buttonPos = camera.ScreenToWorld(buttonPos, 0);
            text = "Play";
            textSize = Game1.Text.MeasureString(text);
            textPos.X = buttonPos.X + buttonWidth / 2 - textSize.X / 2;
            textPos.Y = buttonPos.Y + buttonHeight / 2 - textSize.Y / 2;
            PlayButton = new UI(buttonPos, buttonWidth, buttonHeight, text, Color.White, textPos, Game1.Text);
            elements["play"] = PlayButton; // Adds created element to elements list
        }

        public Game1.State Update(Game1.State currentState, Mission mission)
        {
            if (UpdatePlayButton())
            {
                elements["play"].pressed = false;

                if (mission != null)
                {
                    mission.Init();
                }

                return Game1.State.Mission;
            }


            return currentState;
        }

        
        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
        {
            foreach (var e in elements) // Draws UI elements attached to Main Menu
            {
                e.Value.Draw(gameTime, _spriteBatch, graphics);
            }
        }


        public bool UpdatePlayButton()
        {

            return elements["play"].pressed;
        }

    }
}
