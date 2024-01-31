using Microsoft.Xna.Framework;
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


        private State currentState = State.UpgradeMenu;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Texture2D green;
        public static Texture2D blue;
        public static Texture2D red;

        public static SpriteFont Title{get; set;}
        public static SpriteFont Text{get; set;}
        public static SpriteFont TextSmall{get; set;}
        public static SpriteFont TextLarge{get; set; }
        public static SpriteFont Faces{get; set;}

        private MainMenu mainMenu = null;
        private GameOver gameOverMenu = null;
        private UpgradeTree upgradeTree = null;

        private Cursor cursor;

        Camera camera;

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
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
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
            red = CreateColoredTexture(GraphicsDevice, Color.DarkRed, 16, 16);

            // Loads Fonts
            Title = Content.Load<SpriteFont>("Fonts/Title");
            TextSmall = Content.Load<SpriteFont>("Fonts/TextSmall");
            Text = Content.Load<SpriteFont>("Fonts/Text");
            TextLarge = Content.Load<SpriteFont>("Fonts/TextLarge");
            Faces = Content.Load<SpriteFont>("Fonts/Face");

            camera = new Camera(new Vector2(0, 0), 0, new Vector2(1f, 1f), (1920, 1080));

            cursor = new Cursor(blue);

            player.Load(_spriteBatch, Content); // Loads Player
        }
        
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            cursor.Update();

            // Updates depending on current state of game
            switch (currentState)
            {
                // MAIN MENU
                case State.MainMenu:
                    if (mainMenu == null)
                        mainMenu = new MainMenu(_graphics, camera); // Loads Main Menu

                    cursor.UpdateElements(mainMenu.elements, camera);

                    currentState = mainMenu.Update(currentState, mission, cursor, _graphics, camera);
                    break;
                // END OF MAIN MENU


                // BASE
                case State.Base:
                    currentState = player.Update(gameTime, _graphics, GraphicsDevice, currentState, enemies, camera, cursor, mission, upgradeTree);
                    break;
                // END OF BASE


                // MISSION
                case State.Mission:
                    if (mission == null)
                        mission = new Mission(player, _graphics, camera);
                    if (upgradeTree == null)
                        upgradeTree = new UpgradeTree(_graphics, camera);

                    currentState = player.Update(gameTime, _graphics, GraphicsDevice, currentState, enemies, camera, cursor, mission, upgradeTree);


                    if (player.health <= 0)
                    {
                        enemies.Clear();
                        player.initialize(_graphics);
                        currentState = State.GameOver; // Ends Mission and goes to GAME OVER scene
                    }

                    currentState = mission.Update(gameTime, _graphics, enemies, player, camera, cursor, currentState);
                    break;
                // END OF MISSION


                // UPGRADE MENU
                case State.UpgradeMenu:
                    if (mission == null)
                        mission = new Mission(player, _graphics, camera);
                    if (upgradeTree == null)
                        upgradeTree = new UpgradeTree(_graphics, camera);

                    cursor.UpdateElements(upgradeTree.elements, camera);

                    currentState = upgradeTree.Update(currentState, mission, cursor);
                    break;
                // END OF UPGRADE MENU


                // GAME OVER
                case State.GameOver:
                    if (gameOverMenu == null)
                        gameOverMenu = new GameOver(_graphics, camera); // Loads Main Menu
                    
                    cursor.UpdateElements(gameOverMenu.elements, camera);

                    currentState = gameOverMenu.Update(currentState);
                    break;
                // END OF GAME OVER


                // ERROR
                default:
                    Console.log("ERROR UPDATING!!!!!!!!!!!!!!!!!!!!!!!!");
                    break;
            }

            cursor.clicked = false;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.View(0f));


            switch (currentState)
            {
                // MAIN MENU
                case State.MainMenu:
                    if (mainMenu != null)
                    {
                        mainMenu.Draw(gameTime, _spriteBatch, _graphics);
                    }
                    break;
                // END OF MAIN MENU


                // BASE
                case State.Base:
                    //maps[0].DrawTilemap(_spriteBatch);
                    break;
                // END OF BASE


                // MISSION
                case State.Mission:
                    if (mission != null)
                    {
                        //_spriteBatch.Draw(red, mission.container, Color.White); // debug
                        mission.Draw(gameTime, _spriteBatch, _graphics, player, enemies);
                    }
                    break;
                // END OF MISSION


                // UPGRADE MENU
                case State.UpgradeMenu:
                    if (upgradeTree != null)
                    {
                        mission.DrawForUpgradeTree(gameTime, _spriteBatch, _graphics, player, enemies);
                        upgradeTree.Draw(gameTime, _spriteBatch, _graphics, cursor);
                    }
                    break;
                // END OF UPGRADE MENU


                // GAME OVER
                case State.GameOver:
                    if (gameOverMenu != null)
                    {
                        gameOverMenu.Draw(gameTime, _spriteBatch, _graphics);
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


        public static void DrawLine(SpriteBatch _spriteBatch, GraphicsDeviceManager graphics, Vector2 p1, Vector2 p2, int width, Color color)
        {
            var line = new Rectangle((int)p1.X, (int)p1.Y, width, (int)Vector2.Distance(p1, p2));
            float angle = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X) - MathHelper.PiOver2;

            _spriteBatch.Draw(blue, line, null, Color.White, angle, new Vector2(0, 0), SpriteEffects.None, 0f);
        }
    }
}
