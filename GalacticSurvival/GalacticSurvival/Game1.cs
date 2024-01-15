﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System;

namespace GalacticSurvival
{
    public class Game1 : Game
    {
        public enum State
        {
            MainMenu,
            Base,
            UpgradeMenu,
            Mission,
            GameOver
        }


        private State currentState = State.Mission;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Texture2D green;
        private Texture2D blue;
        private Texture2D red;

        public static SpriteFont Title{get; set;}
        public static SpriteFont Text{get; set;}
        public static SpriteFont Faces{get; set;}

        private MainMenu mainMenu;
        private GameOver gameOverMenu;

        private Cursor cursor;

        private Mission mission = null;

        Player player = new Player();
        List<Enemy> enemies = new List<Enemy>();

        private Random random = new Random();

        //private List<TiledMap> maps = new List<TiledMap>();

        // Vars for a tile map
        //private TiledMap tiledMap;
        //private TiledMapRenderer tiledMapRenderer;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1920/4;
            _graphics.PreferredBackBufferHeight = 1080/4;
            _graphics.ApplyChanges();

            base.Initialize(); // All other functions like Load Content for instance
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Debug Textures
            green = CreateColoredTexture(GraphicsDevice, Color.Green, 16, 16);
            blue = CreateColoredTexture(GraphicsDevice, Color.Blue, 16, 16);
            red = CreateColoredTexture(GraphicsDevice, Color.Red, 16, 16);

            // Loads Fonts
            Title = Content.Load<SpriteFont>("Fonts/Title");
            Text = Content.Load<SpriteFont>("Fonts/Text");
            Faces = Content.Load<SpriteFont>("Fonts/Face");

            mainMenu = new MainMenu(_graphics); // Loads Main Menu
            gameOverMenu = new GameOver(_graphics); // Loads Main Menu

            cursor = new Cursor(blue);

            player.Load(_spriteBatch, Content); // Loads Player
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Updates depending on current state of game
            switch (currentState)
            {
                // MAIN MENU
                case State.MainMenu:
                    cursor.Update(mainMenu.elements);

                    currentState = mainMenu.Update(currentState);
                    break;
                // END OF MAIN MENU


                // BASE
                case State.Base:
                    currentState = player.Update(gameTime, _graphics, GraphicsDevice, currentState, enemies);
                    break;
                // END OF BASE


                // MISSION
                case State.Mission:
                    if (mission == null)
                    {
                        mission = new Mission(player);
                    }

                    cursor.Update(null);

                    currentState = player.Update(gameTime, _graphics, GraphicsDevice, currentState, enemies);

                    if (player.health <= 0)
                    {
                        enemies.Clear();
                        player.initialize(_graphics);
                        currentState = State.GameOver; // Ends Mission and goes to GAME OVER scene
                    }

                    //mission.Update(gameTime, _graphics, enemies, player, camera);
                    break;
                // END OF MISSION


                // UPGRADE MENU
                case State.UpgradeMenu:
                    break;
                // END OF UPGRADE MENU


                // GAME OVER
                case State.GameOver:
                    cursor.Update(gameOverMenu.elements);

                    currentState = gameOverMenu.Update(currentState);
                    break;
                // END OF GAME OVER


                // ERROR
                default:
                    Console.log("ERROR UPDATING!!!!!!!!!!!!!!!!!!!!!!!!");
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            switch (currentState)
            {
                // MAIN MENU
                case State.MainMenu:
                    foreach (var e in mainMenu.elements) // Draws UI elements attached to Main Menu
                    {
                        e.Value.Draw(gameTime, _spriteBatch, _graphics);
                    }
                    break;
                // END OF MAIN MENU


                // BASE
                case State.Base:
                    //maps[0].DrawTilemap(_spriteBatch);
                    _spriteBatch.Draw(green, new Rectangle(16 * 11, 16 * 16, 16, 16), Color.White);
                    break;
                // END OF BASE


                // MISSION
                case State.Mission:
                    player.Draw(gameTime, _spriteBatch, _graphics); // Draws player

                    foreach (var e in enemies) // Draws enemies
                    {
                        if (e.enemyType == "Spawner")
                        {
                            _spriteBatch.Draw(green, e.collider.container, Color.White);
                        }
                        else if (e.enemyType == "Rusher")
                        {
                            _spriteBatch.Draw(blue, e.collider.container, Color.White);
                        }
                    }
                    break;
                // END OF MISSION


                // UPGRADE MENU
                case State.UpgradeMenu:
                    break;
                // END OF UPGRADE MENU


                // GAME OVER
                case State.GameOver:
                    foreach (var e in gameOverMenu.elements) // Draws UI elements attached to Main Menu
                    {
                        e.Value.Draw(gameTime, _spriteBatch, _graphics);
                    }
                    break;
                // END OF GAME OVER


                default:
                    Console.log("ERROR DRAWING!!!!!!!!!!!!!!!!!!!!!");
                    break;
            }

            cursor.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public Texture2D CreateColoredTexture(GraphicsDevice graphicsDevice, Color color, int width, int height)
        {
            Texture2D texture = new Texture2D(graphicsDevice, width, height);

            Color[] colorData = new Color[width * height];
            for (int i = 0; i < colorData.Length; i++)
            {
                colorData[i] = color;
            }

            texture.SetData(colorData);

            return texture;
        }
    }
}
