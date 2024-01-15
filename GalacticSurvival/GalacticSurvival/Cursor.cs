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
    internal class Cursor
    {
        public Collider collider;
        private Texture2D texture;
        private MouseState mouseState;

        private Vector2 mousePosition = new Vector2();

        private bool clicked = false;
        private bool released = true;


        public Cursor(Texture2D t)
        {
            texture = t;
            collider = new Collider(Vector2.Zero, t.Width, t.Height);
        }


        // Updates Cursor
        public void Update(Dictionary<string, UI> elements, Camera camera)
        {
            // Converts screen mouse pos
            mouseState = Mouse.GetState();
            mousePosition.X = mouseState.X;
            mousePosition.Y = mouseState.Y;
            mousePosition = camera.ScreenToWorld(mousePosition, 0);


            collider.container.X = (int)mousePosition.X;
            collider.container.Y = (int)mousePosition.Y;

            if (mouseState.LeftButton == ButtonState.Pressed && released)
            {
                clicked = true;
                released = false;
            }

            if (clicked == false && mouseState.LeftButton == ButtonState.Released)
            {
                clicked = false;
                released = true;
            }

            if (elements != null)
            {
                foreach (var e in elements)
                {
                    // if Element is a button AND it's collider interects with the cursor AND LMB is pressed
                    if (e.Value.button != null && e.Value.button.container.Intersects(collider.container) && clicked)
                    {
                        e.Value.pressed = true;
                    }
                    else
                    {
                        e.Value.pressed = false;
                    }
                }
            }

            clicked = false;
        }


        // Draws Cursor
        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture, collider.container, Color.White);
        }
    }
}
