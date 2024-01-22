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
        private int currentRound;

        public string enemyType = "";

        private Vector2 position;
        private Vector2 paddingCheck;
        private float angle;

        private int spawnerSize = 96;
        private double spawnTimer;
        private double spawnerInterval = 1.5;
        private Enemy spawnedEnemy = null;

        private int rusherSize = 16;
        private double attackTimer = 2;
        private double attackInterval = 2;

        public Collider collider;

        private float speed = 1.2f;

        public float health = 100;

        private Random random = new Random();


        // Spawns enemy Spawner
        public Enemy(Player player, GraphicsDeviceManager graphics, string type, Rectangle missionContainer, List<Enemy> enemies, int round)
        {
            enemyType = type;

            currentRound = round;

            health = round * 100f + round * 0.25f;

            spawnerInterval = 1.5 - currentRound * 0.05;
            if (spawnerInterval < 0.5)
                spawnerInterval = 0.5;

            var tempCollider = new Rectangle(0, 0, spawnerSize, spawnerSize);

            bool spawnCollides = false;

            while (Vector2.Distance(player.position, paddingCheck) < missionContainer.Height * 0.4 || Vector2.Distance(player.position, paddingCheck) > missionContainer.Height*0.75 || spawnCollides)
            {
                spawnCollides = false;

                // Decides where to spawn spawner
                switch (random.Next(1, 5))
                {
                    case 1: // Top
                        position = new Vector2(random.Next(missionContainer.X, missionContainer.X + missionContainer.Width - spawnerSize), random.Next(missionContainer.Y, (int)player.position.Y - spawnerSize * 3));
                        //position = new Vector2(random.Next((int)player.position.Y - missionContainer.Height/2, graphics.PreferredBackBufferWidth), random.Next(0, (int)player.position.Y - (int)(missionContainer.Width * 0.25)));
                        break;
                    case 2: // Down
                        position = new Vector2(random.Next(missionContainer.X, missionContainer.X + missionContainer.Width - spawnerSize), random.Next((int)player.position.Y + spawnerSize * 2, missionContainer.Y + missionContainer.Height - spawnerSize));
                        //position = new Vector2(random.Next((int)player.position.Y, graphics.PreferredBackBufferWidth), random.Next(((int)player.position.Y + (int)(missionContainer.Width * 0.25)), graphics.PreferredBackBufferHeight));
                        break;
                    case 3: // Left
                        position = new Vector2(random.Next(missionContainer.X, (int)player.position.X - spawnerSize * 3), random.Next(missionContainer.Y, missionContainer.Y + missionContainer.Height - spawnerSize));
                        //position = new Vector2(random.Next((int)player.position.X - missionContainer.Width/2, (int)player.position.X - (int)(missionContainer.Width * 0.25)), random.Next(0, graphics.PreferredBackBufferHeight));
                        break;
                    case 4: // Right
                        position = new Vector2(random.Next((int)player.position.X + spawnerSize * 2, missionContainer.X + missionContainer.Width - spawnerSize), random.Next(missionContainer.Y, missionContainer.Y + missionContainer.Height - spawnerSize));
                        //position = new Vector2(random.Next((int)player.position.X + (int)(missionContainer.Width * 0.25), graphics.PreferredBackBufferWidth), random.Next(0, graphics.PreferredBackBufferHeight));
                        break;
                    default:
                        Console.log("ERROR INVALID SIDE FOR SPAWN");
                        break;
                }

                // Updates the padding check
                paddingCheck.X = position.X + spawnerSize / 2;
                paddingCheck.Y = position.Y + spawnerSize / 2;

                tempCollider.X = (int)position.X;
                tempCollider.Y = (int)position.Y;

                foreach (var e in enemies)
                {
                    if (e.collider.container.Intersects(tempCollider))
                    {
                        spawnCollides = true;
                        break;
                    }
                    else 
                    {
                        spawnCollides = false;
                    }
                }
            }

            collider = new Collider(position, spawnerSize, spawnerSize);
        }


        public Enemy(Player player, GraphicsDeviceManager graphics, string type, Vector2 pos, int round)
        {
            switch (type)
            {
                case "Rusher":
                    health = currentRound * 0.25f;

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
                            spawnedEnemy = new Enemy(player, graphics, "Rusher", new Vector2(random.Next(collider.container.X, collider.container.X + spawnerSize), random.Next(collider.container.Y, collider.container.Y + spawnerSize)), currentRound);
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
