using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GalacticSurvival
{
    internal class Mission
    {
        private int currentRound = 1;

        private double spawnTimer;
        private double spawnerInterval = 1;

        public Rectangle container;
        int width = 1728;
        int height = 1024;

        List<Enemy> enemiesToAdd = new List<Enemy>();
        List<Enemy> enemiesToRemove = new List<Enemy>();
        Enemy currentEnemy = null;


        public Mission(Player player)
        {
            container = new Rectangle((int)player.position.X - width/2, (int)player.position.Y - height/2, width, height);
        }


        public void Update(GameTime gameTime, GraphicsDeviceManager graphics, List<Enemy> enemies, Player player)
        {
            spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;

            // Spawns enemy spawner every n seconds
            if (spawnTimer >= spawnerInterval)
            {
                currentEnemy = new Enemy(player, graphics, "Spawner", container);

                enemies.Add(currentEnemy);

                spawnTimer = 0;
            }


            /*
            // Updates spawned enemies
            foreach (var e in enemies)
            {
                currentEnemy = e.Update(gameTime, graphics, player, enemiesToRemove); // Updates current enemy

                if (currentEnemy != null)
                {
                    enemiesToAdd.Add(currentEnemy); // adds spawned enemies from spawner to main list of enemies
                }
            }

            foreach (var e in enemiesToAdd)
            {
                enemies.Add(e);
            }
            enemiesToAdd.Clear();

            if (enemiesToRemove.Count != 0)
            {
                foreach (var e in enemiesToRemove)
                {
                    enemies.Remove(e);
                }
            }
            enemiesToRemove.Clear();
        }*/

        }
    }
}
