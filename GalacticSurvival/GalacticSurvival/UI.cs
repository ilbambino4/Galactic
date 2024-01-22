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
    internal class UI
    {
        private SpriteFont font;
        private string text;
        private Color textColor;
        private Vector2 textSize;
        private Vector2 textPosition;

        public Collider button = null;
        public bool pressed = false;


        // Constructor for UI Text Element
        public UI(string t, Color c, Vector2 pos, SpriteFont f)
        {
            font = f;
            text = t;
            textColor = c;
            textSize = f.MeasureString(text);
            textPosition = pos;
        }


        // Constructor for UI Button Element
        public UI(Vector2 pos, int width, int height, string t, Color c, Vector2 textPos, SpriteFont f)
        {
            button = new Collider(pos, width, height);
            font = f;
            text = t;
            textColor = c;
            textSize = f.MeasureString(text);
            textPosition = textPos;
        }

        // Draws a UI Element
        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
        {
            if (button == null)
            {
                _spriteBatch.DrawString(font, text, textPosition, textColor);
            } 
            else
            {
                _spriteBatch.Draw(Game1.green, button.container, Color.White);
                _spriteBatch.DrawString(font, text, textPosition, textColor);
            }
        }
    }
}
