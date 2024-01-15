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
    internal class Collider
    {
        // Vars for the Collider object
        private Vector2 colliderPos;

        // Rectangle Collider Vars
        public Rectangle container;

        /**
         * Collider Constructor, constructs either a rectangle or a circle collider depending on whether it is
         * a being (player/enemy) or not.
         */
        public Collider(Vector2 pos, int w, int h)
        {
            colliderPos = pos;
            container = new Rectangle((int)pos.X, (int)pos.Y, w, h);
        }

        /**
         * Position setter
         */
        public Vector2 Position
        {
            set { colliderPos = value; }
        }

        /**
         * Rectangle getter
         */
        public Rectangle Rectangle
        {
            get { return container; }
        }
    }
}
