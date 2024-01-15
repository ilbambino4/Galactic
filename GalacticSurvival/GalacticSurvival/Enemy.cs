using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalacticSurvival
{
    internal class Enemy
    {
        public string enemyType = "";

        private Vector2 position;
        private float angle;

        private int spawnerSize = 128;
        private double spawnTimer;
        private double spawnerInterval = 0.1;
        private Enemy spawnedEnemy = null;

        private int rusherSize = 16;
        private double attackTimer = 2;
        private double attackInterval = 2;

        public Collider collider;

        private float speed = 1.2f;

        public int health = 5;

        private Random random = new Random();


        // Spawns enemy Spawner
        public Enemy(Player player, GraphicsDeviceManager graphics, string type, Rectangle missionContainer)
        {
            enemyType = type;

            health = 400;

            // Decides where to spawn spawner
            switch (random.Next(1, 5))
            {
                case 1: // Top
                    //position = new Vector2(random.Next((int)player.position.Y - missionContainer.Height/2, graphics.PreferredBackBufferWidth), random.Next(0, (int)player.position.Y - (int)(missionContainer.Width * 0.25)));
                    break;
                case 2: // Down
                    //position = new Vector2(random.Next((int)player.position.Y, graphics.PreferredBackBufferWidth), random.Next(((int)player.position.Y + (int)(missionContainer.Width * 0.25)), graphics.PreferredBackBufferHeight));
                    break;
                case 3: // Left
                    //position = new Vector2(random.Next((int)player.position.X - missionContainer.Width/2, (int)player.position.X - (int)(missionContainer.Width * 0.25)), random.Next(0, graphics.PreferredBackBufferHeight));
                    break;
                case 4: // Right
                    //position = new Vector2(random.Next((int)player.position.X + (int)(missionContainer.Width * 0.25), graphics.PreferredBackBufferWidth), random.Next(0, graphics.PreferredBackBufferHeight));
                    break;
                default:
                    Console.log("ERROR INVALID SIDE FOR SPAWN");
                    break;
            }

            position = new Vector2(0, 0);

            //position = new Vector2(missionContainer.X, missionContainer.Y);

            /*// Check for off screen spawns
            if (position.Y - spawnerSize < 0) // Top
            {
                position.Y = 0 + random.Next(1, spawnerSize+1);
            }
            if (position.Y + spawnerSize > graphics.PreferredBackBufferHeight) // Bottom
            {
                position.Y = (graphics.PreferredBackBufferHeight - spawnerSize) - random.Next(1, spawnerSize);
            }
            if (position.X - spawnerSize < 0) // Left
            {
                position.X  = 0 + random.Next(1, spawnerSize+1);
            }
            if (position.X + spawnerSize > graphics.PreferredBackBufferWidth) // Right
            {
                position.X = (graphics.PreferredBackBufferWidth - spawnerSize) - random.Next(1, spawnerSize);
            }*/

            collider = new Collider(position, spawnerSize, spawnerSize);
        }


        public Enemy(Player player, GraphicsDeviceManager graphics, string type, Vector2 pos)
        {
            switch (type)
            {
                case "Rusher":
                    enemyType = type;

                    position = pos;

                    collider = new Collider(pos, rusherSize, rusherSize);
                    break;

                default:
                    Console.log("ERROR: UNKOWN ENEMY SPAWN TYPE");
                    break;
            }
        }


        public Enemy Update(GameTime gameTime, GraphicsDeviceManager graphics, Player player, List<Enemy> deadEnemies)
        {
            if (health < 0)
            {
                deadEnemies.Add(this); // adds this enemy to remove, when it has died
            }
            else
            {
                switch (enemyType)
                {
                    case "Spawner":
                        spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;

                        // Spawns enemy from spawner every n seconds
                        if (spawnTimer >= spawnerInterval)
                        {
                            spawnTimer = 0;
                            spawnedEnemy = new Enemy(player, graphics, "Rusher", new Vector2(random.Next(collider.container.X, collider.container.X + spawnerSize), random.Next(collider.container.Y, collider.container.Y + spawnerSize)));
                            return spawnedEnemy; // returns spawner spawned enemy
                        }
                    break;


                    case "Rusher":
                        angle = (float)Math.Atan2(player.position.Y - position.Y, player.position.X - position.X);


                        if (Vector2.Distance(position, player.position) > 50)
                        {
                            position.X += (float)Math.Cos(angle) * speed;
                            position.Y += (float)Math.Sin(angle) * speed;
                        }
                        else
                        {
                            attackTimer += gameTime.ElapsedGameTime.TotalSeconds;

                            if (attackTimer >= attackInterval)
                            {
                                player.health -= 10;
                                attackTimer = 0;
                            }
                        }

                        collider.container.X = (int)position.X - collider.container.Width / 2;
                        collider.container.Y = (int)position.Y - collider.container.Height / 2;
                        break;

                    default:
                        Console.log("ERROR: UNKOWN ENEMY SPAWN TYPE");
                        break;
                }
            }

            return null;
        }
    }
}
