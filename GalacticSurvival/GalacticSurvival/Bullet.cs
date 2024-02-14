using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public float damage;

        private UpgradeTree tree;

        public Bullet(Vector2 pos, float a, float s, UpgradeTree t, float weaponDamageMod)
        {
            position = pos;
            angle = a;
            speed = s;
            container = new Rectangle((int)pos.X, (int)pos.Y, 8, 8);
            rotationOrigin = new Vector2(4, 4);
            tree = t;

            switch (t.currentAmmo)
            {
                case "default":
                    damage = 25;
                    damage += damage * weaponDamageMod;
                    Console.log("Bullet Damage" + damage);
                    speed = 10;
                    break;



                case "chainingRounds":
                    damage = 200;
                    speed = 20;
                    break;



                case "piercingRounds":
                    damage = 10;
                    speed = 10;
                    break;



                case "causticRounds":
                    damage = 20;
                    speed = 15;
                    break;


                case "homingRounds":
                    damage = 20;
                    speed = 15;
                    break;


                default:
                    Console.log("ERROR UNKOWN CURRENT WEAPON FOR BULLET: " + t.currentAmmo);
                    break;
            }
        }

        public bool Update(GameTime gameTime, GraphicsDeviceManager graphics, Rectangle bulletBoundry, List<Enemy> enemies)
        {
            switch (tree.currentAmmo)
            {
                case "default":
                    position.X += (float)Math.Cos(angle) * speed;
                    position.Y += (float)Math.Sin(angle) * speed;

                    container.X = (int)position.X;
                    container.Y = (int)position.Y;
                    break;



                case "chainingRounds":
                    break;



                case "piercingRounds":
                    break;



                case "causticRounds":
                    break;


                case "homingRounds":
                    break;


                default:
                    Console.log("ERROR UNKOWN CURRENT WEAPON FOR BULLET: " + tree.currentAmmo);
                    break;
            }

            return container.Intersects(bulletBoundry);
        }


        public bool CheckEnemyCollision(List<Enemy> enemies, Mission mission)
        {
            bool collision = false;

            foreach (var e in enemies)
            {
                if (container.Intersects(e.collider.container))
                {
                    collision = true;

                    e.health -= damage;
                    Console.log(tree.weaponTree.children[1].title + "");

                    if (e.type == "Spawner")
                    {
                        mission.points += 10;
                    }

                    break;
                }
            }

            return collision;
        }



        public void Draw(Texture2D sprite, GameTime gameTime, SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
        {
            _spriteBatch.Draw(sprite, container, null, Color.White, angle, rotationOrigin, SpriteEffects.None, 0);
        }
    }
}
