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
    internal class Bullet
    {
        private Texture2D bulletSprite;

        private Vector2 position;

        private float angle;

        private float speed;

        public Rectangle container;

        private Vector2 rotationOrigin;

        public int damage = 25;

        public Bullet(Vector2 pos, float a, float s)
        {
            position = pos;
            angle = a;
            speed = s;
            container = new Rectangle((int)pos.X, (int)pos.Y, 8, 8);
            rotationOrigin = new Vector2(4, 4);
        }

        public bool Update(GameTime gameTime, GraphicsDeviceManager graphics, Rectangle bulletBoundry)
        {
            position.X += (float)Math.Cos(angle) * speed;
            position.Y += (float)Math.Sin(angle) * speed;
            
            container.X = (int)position.X;
            container.Y = (int)position.Y;

            if (container.Intersects(bulletBoundry))
                return true;
            else
                return false;
        }

        public void Draw(Texture2D sprite, GameTime gameTime, SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
        {
            _spriteBatch.Draw(sprite, container, null, Color.White, angle, rotationOrigin, SpriteEffects.None, 0);

        }
    }
}
