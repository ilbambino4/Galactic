using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Cryptography;
using System.Diagnostics.CodeAnalysis;
using static GalacticSurvival.Game1;

namespace GalacticSurvival
{
    internal class Player
    {
        public bool init = false;

        public int health;

        private Texture2D playerSprite;
        private Texture2D bulletSprite;
        private Rectangle bulletBoundry;

        public Vector2 position = new Vector2(0, 0);
        private Rectangle playerContainer;

        private MouseState mouseState;
        private Vector2 mousePosition;
        bool shot = false;

        private float angle;
        private float currentBulletAngle;
        private Vector2 rotationOrigin;

        private List<Bullet> bullets = new List<Bullet>();
        private List<Bullet> bulletsToRemove = new List<Bullet>();

        private Random random = new Random();

        private int tileSize = 32;





        public void Load(SpriteBatch _spriteBatch, Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            playerSprite = Content.Load<Texture2D>("AR");
            bulletSprite = Content.Load<Texture2D>("bullet");
        }


        public Game1.State Update(GameTime gameTime, GraphicsDeviceManager graphics, GraphicsDevice graphicsDevice, Game1.State currentState, List<Enemy> enemies, Camera camera)
        {
            if (!init)
                initialize(graphics);

            //camera.Update(new Vector2(0, 0), graphicsDevice);

            switch (currentState)
            {
                // MAIN MENU
                case Game1.State.MainMenu:
                    return Game1.State.MainMenu;
                // END OF MAIN MENU


                // BASE
                case Game1.State.Base:
                    return currentState;
                // END OF BASE


                // MISSION
                case Game1.State.Mission:
                    
                    angle = getAngle(camera);

                    if (shot)
                    {
                        if (mouseState.LeftButton == ButtonState.Released)
                            shot = false;
                    }
                    else
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed)
                        {
                            currentBulletAngle = angle * (float)(180 / Math.PI);
                            currentBulletAngle += -4 + (random.Next(9));
                            currentBulletAngle = currentBulletAngle * (float)(Math.PI / 180);


                            bullets.Add(new Bullet(position, currentBulletAngle, 10));
                            shot = true;
                        }
                    }


                    foreach (var b in bullets)
                    {
                        if (!b.Update(gameTime, graphics, bulletBoundry))
                            bulletsToRemove.Add(b);

                        foreach (var e in enemies)
                        {
                            if (b.container.Intersects(e.collider.container))
                            {
                                bulletsToRemove.Add(b);

                                e.health -= b.damage;

                                break;
                            }
                        }
                    }


                    foreach (var b in bulletsToRemove)
                        bullets.Remove(b);
                    

                    return currentState;
                // END OF MISSION


                // UPGRADE MENU
                case Game1.State.UpgradeMenu:
                    return currentState;
                // END OF UPGRADE MENU

                    
                // GAME OVER
                case Game1.State.GameOver:
                    return currentState;
                // END OF GAME OVER


                // ERROR
                default:
                    Console.log("ERROR DRAWING!!!!!!!!!!!!!!!!!!!!!");
                    return currentState;
            }
        }


        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
        {
            foreach (var b in bullets)
            {
                b.Draw(bulletSprite, gameTime, _spriteBatch, graphics);
            }

            _spriteBatch.Draw(playerSprite, playerContainer, null, Color.White, angle, rotationOrigin, SpriteEffects.None, 0);
        }


        private float getAngle(Camera camera)
        {
            // Gets Mouse state and converts position to the real world from the screen's camera position
            mouseState = Mouse.GetState();
            mousePosition.X = mouseState.X;
            mousePosition.Y = mouseState.Y;

            mousePosition = camera.ScreenToWorld(mousePosition, 0);

            // Returns the angle
            return (float)Math.Atan2(mousePosition.Y - position.Y, mousePosition.X - position.X);
        }


        public void initialize(GraphicsDeviceManager graphics)
        {
            health = 1;
            //position = new Vector2((graphics.PreferredBackBufferWidth / 2) - (playerSprite.Width / 2), (graphics.PreferredBackBufferHeight / 2) - (playerSprite.Height / 2));
            playerContainer = new Rectangle((int)position.X, (int)position.Y, playerSprite.Width, playerSprite.Height);
            rotationOrigin = new Vector2(playerSprite.Width / 2, playerSprite.Height / 2);
            bulletBoundry = new Rectangle(-graphics.PreferredBackBufferWidth/2, -graphics.PreferredBackBufferHeight/2, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            bullets.Clear();
            bulletsToRemove.Clear();
            init = true;
        }
    }
}