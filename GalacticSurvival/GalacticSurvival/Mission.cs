using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GalacticSurvival
{
    internal class Mission
    {
        private int currentRound = 1;
        private bool betweenRounds = true;

        private int prevPoints = 0;
        public int points = 0;

        private int maxSpawners = 0;

        private double spawnTimer = 5;
        private double spawnerInterval = 6;

        public Rectangle container;
        int width = 1728;
        int height = 1024;

        List<Enemy> enemiesToAdd = new List<Enemy>();
        List<Enemy> enemiesToRemove = new List<Enemy>();
        Enemy currentEnemy = null;

        // Between Rounds UI VARS
        public Dictionary<string, UI> elements = new Dictionary<string, UI>();
        private UI UpgradeMenuButton;
        private UI StartNextRoundButton;

        private UI RoundText;
        private int roundPadding = 15;

        private UI PointsText;
        public int pointsPadding = 15;

        private bool upgrading = false;


        public Mission(Player player, GraphicsDeviceManager graphics, Camera camera)
        {
            // Mission Container and mission vars setup
            container = new Rectangle((int)player.position.X - width/2, (int)player.position.Y - height/2, width, height);
            maxSpawners = 4;

            upgrading = false;
            betweenRounds = false;

            points = 10000;

            // UI SETUP
            // Vars used for Main Menu UI Creation
            string text;
            Vector2 textSize;
            Vector2 textPos;
            Vector2 buttonPos;
            int buttonWidth;
            int buttonHeight;
            int buttonPadding = 50;


            // Upgrade Menu Button
            text = "UPGRADES";
            textSize = Game1.Text.MeasureString(text);
            buttonWidth = (int)textSize.X + buttonPadding;
            buttonHeight = 100;
            buttonPos.X = graphics.PreferredBackBufferWidth / 2 - buttonWidth / 2;
            buttonPos.Y = graphics.PreferredBackBufferHeight / 2 - buttonHeight / 2 - buttonHeight - buttonPadding;
            buttonPos = camera.ScreenToWorld(buttonPos, 0);
            textPos.X = buttonPos.X + buttonWidth / 2 - textSize.X / 2;
            textPos.Y = buttonPos.Y + buttonHeight / 2 - textSize.Y / 2;
            UpgradeMenuButton = new UI(buttonPos, buttonWidth, buttonHeight, text, Color.White, textPos, Game1.Text);
            elements["upgrade"] = UpgradeMenuButton; // Adds created element to elements list


            // Next Round Button
            text = "NEXT ROUND";
            textSize = Game1.Text.MeasureString(text);
            buttonWidth = (int)textSize.X + buttonPadding;
            buttonHeight = 100;
            buttonPos.X = graphics.PreferredBackBufferWidth / 2 - buttonWidth / 2;
            buttonPos.Y = graphics.PreferredBackBufferHeight / 2 - buttonHeight / 2 + buttonHeight + buttonPadding;
            buttonPos = camera.ScreenToWorld(buttonPos, 0);
            textPos.X = buttonPos.X + buttonWidth / 2 - textSize.X / 2;
            textPos.Y = buttonPos.Y + buttonHeight / 2 - textSize.Y / 2;
            StartNextRoundButton = new UI(buttonPos, buttonWidth, buttonHeight, text, Color.White, textPos, Game1.Text);
            elements["nextRound"] = StartNextRoundButton; // Adds created element to elements list


            // Creates Points Text
            text = points + "";
            textSize = Game1.Text.MeasureString(text);
            textPos = new Vector2(pointsPadding, graphics.PreferredBackBufferHeight - textSize.Y);
            textPos = camera.ScreenToWorld(textPos, 0); // Converts position to world coordinates for camera
            PointsText = new UI(text, Color.Gold, textPos, Game1.Text);
            elements["points"] = PointsText; // Adds created element to elements list


            // Creates Points Text
            text = "ROUND: \n" + currentRound;
            textSize = Game1.Text.MeasureString(text);
            textPos = new Vector2(pointsPadding, roundPadding);
            textPos = camera.ScreenToWorld(textPos, 0); // Converts position to world coordinates for camera
            PointsText = new UI(text, Color.White, textPos, Game1.Text);
            elements["round"] = PointsText; // Adds created element to elements list
            elements["oldRound"] = PointsText;
        }


        public Game1.State Update(GameTime gameTime, GraphicsDeviceManager graphics, List<Enemy> enemies, Player player, Camera camera, Cursor cursor, Game1.State currentState)
        {
            if (maxSpawners == 0 && enemies.Count == 0)
            {
                betweenRounds = true;
            }

            if (prevPoints != points)
            {
                prevPoints = points;

                // Updates Main Title
                var text = points+"";
                var textSize = Game1.Text.MeasureString(text);
                var textPos = new Vector2(pointsPadding, graphics.PreferredBackBufferHeight - textSize.Y);
                textPos = camera.ScreenToWorld(textPos, 0); // Converts position to world coordinates for cameras
                PointsText = new UI(text, Color.Gold, textPos, Game1.Text);
                elements["points"] = PointsText; // Adds created element to elements list
            }

            if (betweenRounds)
            {
                cursor.UpdateElements(elements, camera);

                if (elements["nextRound"].pressed || elements["upgrade"].pressed)
                {
                    currentRound += 1;

                    // Updates Points Text
                    var text = "ROUND: \n" + currentRound;
                    var textSize = Game1.Text.MeasureString(text);
                    var textPos = new Vector2(pointsPadding, roundPadding);
                    textPos = camera.ScreenToWorld(textPos, 0); // Converts position to world coordinates for camera
                    PointsText = new UI(text, Color.White, textPos, Game1.Text);
                    elements["round"] = PointsText; // Adds created element to elements list

                    text = "ROUND: \n" + (currentRound - 1);
                    textSize = Game1.Text.MeasureString(text);
                    textPos = new Vector2(pointsPadding, roundPadding);
                    textPos = camera.ScreenToWorld(textPos, 0); // Converts position to world coordinates for camera
                    PointsText = new UI(text, Color.White, textPos, Game1.Text);
                    elements["oldRound"] = PointsText; // Adds created element to elements list

                    // Hastens Spawn Interval per round
                    if (spawnerInterval - 0.24 > 1)
                        spawnerInterval -= 0.24;
                    else
                        spawnerInterval = 1;

                    spawnTimer = spawnerInterval - 2;

                    upgrading = false;
                    maxSpawners = (int)(currentRound * 2.5f);
                    betweenRounds = false;
                    
                    cursor.clicked = false;

                    if (elements["upgrade"].pressed)
                    {
                        return Game1.State.UpgradeMenu;
                    }
                }
            }
            else
            {
                cursor.UpdateElements(null, camera);

                spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;

                // Spawns enemy spawner every n seconds
                if (spawnTimer >= spawnerInterval && enemies.Count < 24 && maxSpawners != 0)
                {
                    currentEnemy = new Enemy(player, graphics, "Spawner", container, enemies, currentRound);

                    enemies.Add(currentEnemy);

                    spawnTimer = 0;

                    maxSpawners -= 1;
                }

                // Updates spawned enemies
                foreach (var e in enemies)
                {
                    currentEnemy = e.Update(gameTime, graphics, player, enemiesToRemove, this); // Updates current enemy

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
            }

            return currentState;
        }


        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch, GraphicsDeviceManager graphics, Player player, List<Enemy> enemies)
        {
            if (betweenRounds)
            {
                player.BetweenRoundDraw(gameTime, _spriteBatch, graphics);// Draws player in between rounds

                foreach (var e in elements) // Draws UI elements attached to Main Menu
                {
                    if (e.Key != "oldRound")
                        e.Value.Draw(gameTime, _spriteBatch, graphics);
                }
            }
            else
            {
                player.Draw(gameTime, _spriteBatch, graphics); // Draws player

                foreach (var e in enemies) // Draws enemies
                {
                    if (e.type == "Spawner")
                    {
                        _spriteBatch.Draw(Game1.green, e.collider.container, Color.White);
                    }
                    else if (e.type == "Rusher")
                    {
                        _spriteBatch.Draw(Game1.blue, e.collider.container, Color.White);
                    }
                }
            }

            elements["round"].Draw(gameTime, _spriteBatch, graphics);
            elements["points"].Draw(gameTime, _spriteBatch, graphics);
        }


        public void DrawForUpgradeTree(GameTime gameTime, SpriteBatch _spriteBatch, GraphicsDeviceManager graphics, Player player, List<Enemy> enemies)
        {
            elements["oldRound"].Draw(gameTime, _spriteBatch, graphics);
            elements["points"].Draw(gameTime, _spriteBatch, graphics);
        }


        public void Init(GraphicsDeviceManager graphics, Camera camera)
        {
            points = 0;
            currentRound = 1;
            maxSpawners = 3;
            upgrading = false;
            betweenRounds = false;
            spawnTimer = 1;
            spawnerInterval = 4;

            enemiesToAdd.Clear();
            enemiesToRemove.Clear();
            currentEnemy = null;

            // Re-Creates Current Roud Text
            var text = "ROUND: \n" + currentRound;
            var textPos = new Vector2(pointsPadding, roundPadding);
            textPos = camera.ScreenToWorld(textPos, 0); // Converts position to world coordinates for camera
            PointsText = new UI(text, Color.White, textPos, Game1.Text);
            elements["round"] = PointsText; // Adds created element to elements list
            elements["oldRound"] = PointsText;
        }

    }
}