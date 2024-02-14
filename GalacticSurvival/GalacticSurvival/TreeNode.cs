using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;
using System.Timers;


namespace GalacticSurvival
{
    internal class TreeNode
    {
        public string type;
        public string weapon = null;
        public string ammo = null;
        public string minion = "simpleMinion";

        public Vector2 position;
        public Vector2 centerPosition;

        public Collider collider;

        public int width;
        public int height;

        public Texture2D sprite;

        public string title;
        public string description;

        public int level;
        public int cost;
        private string levelText;
        private string costText;

        // Descriptor Vars
        public SpriteFont titleFont;
        public SpriteFont descriptionFont;

        private Rectangle textBox;

        private Vector2 titleSize = Vector2.Zero;
        private Vector2 descriptionSize = Vector2.Zero;
        private Vector2 levelSize = Vector2.Zero;
        private Vector2 costSize = Vector2.Zero;

        private int padding = 15;
        private int titleDescriptionPadding = 20;
        private int descriptionLevelPadding = 20;
        private int levelCostPadding = 20;

        private Vector2 titlePosition = new Vector2(0, 0);
        private Vector2 descriptionPosition = new Vector2(0, 0);
        private Vector2 costPosition = new Vector2(0, 0);
        private Vector2 levelPosition = new Vector2(0, 0);


        public List<TreeNode> children = new List<TreeNode>();
        public List<TreeNode> choiceNodes = new List<TreeNode>();
        public Dictionary<int, TreeNode> minions = new Dictionary<int, TreeNode>();

        public bool highlighted = false;

        public bool showChoices = false;
        public bool showChildren = false;

        public int minionCount = 0;
        public bool minionUpdate = true;

        public bool choiceNode = false;

        public bool broke = false;
        private double brokeTimer = 0;
        private double brokeInterval = 1;


        public TreeNode(string t, Vector2 pos, int w, int h, Texture2D s, string tit, string desc, int c, bool choice, SpriteFont tf, SpriteFont df)
        {
            type = t;

            collider = new Collider(pos, w, h);

            // Sets node's values from the given values
            position = pos;
            centerPosition = new Vector2(pos.X + w/2, pos.Y + h/2);
            width = w;
            height = h;
            sprite = s;
            title = tit;
            description = desc;
            cost = c;
            level = 0;
            titleFont = tf;
            descriptionFont = df;

            choiceNode = choice;

            titleSize = titleFont.MeasureString(title);
            descriptionSize = descriptionFont.MeasureString(description);

            if (cost > 0)
            {
                levelSize = descriptionFont.MeasureString("Current Level: " + level);
                costSize = descriptionFont.MeasureString("Upgrade Cost: $" + cost);
            }
            else
            {
                levelSize = Vector2.Zero;
                costSize = Vector2.Zero;
            }

            if (c > 0) // Init for Text Box of Upgradable Node
                textBox = new Rectangle(0, 0, (int)(titleSize.X + descriptionSize.X + padding), (int)(titleSize.Y + descriptionSize.Y + levelSize.Y + costSize.Y + titleDescriptionPadding + descriptionLevelPadding));
            else // Init for Text Box of Title Node or Choice Node
                textBox = new Rectangle(0, 0, (int)(titleSize.X + padding), (int)(titleSize.Y + padding));

        }


        public void Update(GameTime gameTime, Game1.State currentState, Mission mission, Cursor cursor, GraphicsDeviceManager graphics, Camera camera)
        {
            // Resets points text after a set interval of time
            if (broke)
            {
                brokeTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (brokeTimer >= brokeInterval)
                {
                    brokeTimer = 0;
                    broke = false;
                }
            }
        }


        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch, GraphicsDeviceManager graphics, Cursor cursor, string currentWeapon, string currentAmmo)
        {
            // Draws Children Weapon Upgrades based on currently choses weapon
            if (currentWeapon != null)
            {
                foreach (var c in children)
                {
                    if (c.weapon == currentWeapon)
                    {
                        Game1.DrawLine(_spriteBatch, graphics, centerPosition, c.centerPosition, 4, Color.Yellow);
                        c.Draw(gameTime, _spriteBatch, graphics, cursor, currentWeapon, currentAmmo);
                    }
                }
            }

            // Draws Children Ammo Upgrades based on currently choses weapon
            if (currentAmmo != null)
            {
                foreach (var c in children)
                {
                    if (c.ammo == currentAmmo)
                    {
                        Game1.DrawLine(_spriteBatch, graphics, centerPosition, c.centerPosition, 4, Color.Yellow);
                        c.Draw(gameTime, _spriteBatch, graphics, cursor, currentWeapon, currentAmmo);
                    }
                }
            }

            // Draws Children Ship Upgrades
            if (type == "ship")
            {
                foreach (var c in children)
                {
                    Game1.DrawLine(_spriteBatch, graphics, centerPosition, c.centerPosition, 4, Color.Yellow);
                    c.Draw(gameTime, _spriteBatch, graphics, cursor, currentWeapon, currentAmmo);
                }
            }

            // Drawns Minion Sub Trees and Children
            if (type == "minion")
            {
                DrawMinionTrees(_spriteBatch, graphics);
            }

            _spriteBatch.Draw(sprite, collider.container, Color.White);


            if (showChoices)
                foreach(var n in choiceNodes)
                    n.Draw(gameTime, _spriteBatch, graphics, cursor, currentWeapon, currentAmmo);
        }


        public bool IsNodeHighlighted(Cursor cursor)
        {
            return cursor.collider.container.Intersects(collider.container);
        }


        public void DrawDescriptor(GameTime gameTime, SpriteBatch _spriteBatch, GraphicsDeviceManager graphics, Cursor cursor, Camera camera)
        {
            if (titleSize.X >= descriptionSize.X &&
                titleSize.X >= levelSize.X &&
                titleSize.X >= costSize.X)
                     textBox.Width = (int)(padding + titleSize.X + padding);

            else if (descriptionSize.X >= titleSize.X &&
                     descriptionSize.X >= levelSize.X &&
                     descriptionSize.X >= costSize.X)
                     textBox.Width = (int)(padding + descriptionSize.X + padding);

            else if (levelSize.X >= titleSize.X &&
                     levelSize.X >= descriptionSize.X &&
                     levelSize.X >= costSize.X)
                     textBox.Width = (int)(padding + levelSize.X + padding);

            else
                textBox.Width = (int)(padding + costSize.X + padding);


            // Resizes textbox based on the kind of tree node it is.
            if (cost > 0 && choiceNode)
            {
                costText = "$" + cost;
                costSize = descriptionFont.MeasureString(costText);

                textBox.Height = (int)(padding + titleSize.Y + descriptionSize.Y + costSize.Y + padding + padding + padding);
            }
            else if (cost > 0)
            {
                costText = "$" + cost;
                costSize = descriptionFont.MeasureString(costText);

                levelText = "Level: " + level;
                levelSize = descriptionFont.MeasureString(levelText);

                textBox.Height = (int)(padding + titleSize.Y + descriptionSize.Y + levelSize.Y + costSize.Y + descriptionLevelPadding + levelCostPadding + padding + padding);
            }
            else
            {
                if (description != "")
                    textBox.Height = (int)(padding + titleSize.Y + titleDescriptionPadding + descriptionSize.Y + padding);
                else
                    textBox.Height = (int)(padding + titleSize.Y + padding);
            }



            textBox.X = cursor.collider.container.X + cursor.collider.container.Width;
            textBox.Y = cursor.collider.container.Y + cursor.collider.container.Height;

            if (textBox.X + textBox.Width > camera.VirtualRes.Width / 2) 
                textBox.X = camera.VirtualRes.Width / 2 - textBox.Width;

            if (textBox.X < -camera.VirtualRes.Width / 2)
                textBox.X = -camera.VirtualRes.Width / 2;

            if (textBox.Y + textBox.Height > camera.VirtualRes.Height / 2)
                textBox.Y = camera.VirtualRes.Height / 2 - textBox.Height;

            if (textBox.Y < -camera.VirtualRes.Height / 2)
                textBox.Y = -camera.VirtualRes.Height / 2;



            if (cost > 0) // Draws Desctiption of Nodes that can be upgraded or bought
            {
                if (choiceNode)
                {
                    // Draws Text Box
                    _spriteBatch.Draw(Game1.red, textBox, Color.White);

                    // Update's text positions
                    titlePosition.X = textBox.X + padding;
                    titlePosition.Y = textBox.Y + padding;
                    descriptionPosition.X = titlePosition.X;
                    descriptionPosition.Y = titlePosition.Y + titleSize.Y + titleDescriptionPadding;
                    costPosition.X = descriptionPosition.X;
                    costPosition.Y = descriptionPosition.Y + descriptionSize.Y + padding;
                    _spriteBatch.DrawString(titleFont, title, titlePosition, Color.White);
                    _spriteBatch.DrawString(descriptionFont, description, descriptionPosition, Color.White);

                    if (broke)
                        costText = "Not Enough Points";
                    else
                        costText = "$" + cost;

                    _spriteBatch.DrawString(descriptionFont, costText, costPosition, Color.White);
                }
                else
                {
                    // Draws Text Box
                    _spriteBatch.Draw(Game1.red, textBox, Color.White);


                    // Update's text positions
                    titlePosition.X = textBox.X + padding;
                    titlePosition.Y = textBox.Y + padding;
                    descriptionPosition.X = titlePosition.X;
                    descriptionPosition.Y = titlePosition.Y + titleSize.Y + titleDescriptionPadding;
                    levelPosition.X = descriptionPosition.X;
                    levelPosition.Y = descriptionPosition.Y + descriptionSize.Y + descriptionLevelPadding;
                    costPosition.X = levelPosition.X;
                    costPosition.Y = levelPosition.Y + levelSize.Y + levelCostPadding;

                    _spriteBatch.DrawString(titleFont, title, titlePosition, Color.White);
                    _spriteBatch.DrawString(descriptionFont, description, descriptionPosition, Color.White);
                    _spriteBatch.DrawString(descriptionFont, levelText, levelPosition, Color.White);

                    if (broke)
                        costText = "Not Enough Points";
                    else
                        costText = "$" + cost;
                    _spriteBatch.DrawString(descriptionFont, costText, costPosition, Color.White);
                }
            }
            else // Draws Title and Choice Node Descriptions
            {
                // Draws Text Box
                _spriteBatch.Draw(Game1.red, textBox, Color.White);

                titlePosition.X = textBox.X + padding;
                titlePosition.Y = textBox.Y + padding;

                descriptionPosition.X = titlePosition.X;
                descriptionPosition.Y = titlePosition.Y + titleSize.Y + titleDescriptionPadding;

                _spriteBatch.DrawString(titleFont, title, titlePosition, Color.White);
                _spriteBatch.DrawString(descriptionFont, description, descriptionPosition, Color.White);
            }
        }


        public void DrawMinionTrees(SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
        {
            if (!minionUpdate)
            {
                TreeNode minionChoice;
                TreeNode currentUpgrade;
                var choicePadding = 84;
                var nodePadding = 96;

                // Resets minion positions before changing them
                minions[1].position = new Vector2(position.X + nodePadding * 2f, position.Y);
                minions[2].position = new Vector2(position.X + nodePadding * 2f, position.Y);
                minions[3].position = new Vector2(position.X + nodePadding * 2f, position.Y);
                minions[4].position = new Vector2(position.X + nodePadding * 2f, position.Y);

                switch (minionCount)
                {
                    case 0:
                        // DO NOTHING
                        break;


                    case 1:
                        minionUpdate = true;
                        break;


                    case 2:
                        // Update Minion node container positions
                        minionUpdate = true;

                        minions[1].position = new Vector2(minions[1].position.X, minions[1].position.Y - nodePadding*.875f);
                        minions[2].position = new Vector2(minions[2].position.X, minions[2].position.Y + nodePadding*.875f);

                        minions[1].centerPosition.X = minions[1].position.X + minions[1].width / 2;
                        minions[1].centerPosition.Y = minions[1].position.Y + minions[1].height / 2;
                        minions[2].centerPosition.X = minions[2].position.X + minions[2].width / 2;
                        minions[2].centerPosition.Y = minions[2].position.Y + minions[2].height / 2;

                        minions[1].collider = new Collider(minions[1].position, minions[1].width, minions[1].height);
                        minions[2].collider = new Collider(minions[2].position, minions[2].width, minions[2].height);
                        break;


                    case 3:
                        // Update Minion node container positions
                        minionUpdate = true;

                        minions[1].position = new Vector2(minions[1].position.X, minions[1].position.Y - nodePadding*.875f);
                        minions[2].position = new Vector2(minions[2].position.X, minions[2].position.Y + nodePadding*.875f);
                        minions[3].position = new Vector2(minions[3].position.X + nodePadding * 2.625f, minions[3].position.Y);

                        minions[1].centerPosition.X = minions[1].position.X + minions[1].width / 2;
                        minions[1].centerPosition.Y = minions[1].position.Y + minions[1].height / 2;
                        minions[2].centerPosition.X = minions[2].position.X + minions[2].width / 2;
                        minions[2].centerPosition.Y = minions[2].position.Y + minions[2].height / 2;
                        minions[3].centerPosition.X = minions[3].position.X + minions[3].width / 2;
                        minions[3].centerPosition.Y = minions[3].position.Y + minions[3].height / 2;

                        minions[1].collider = new Collider(minions[1].position, minions[1].width, minions[1].height);
                        minions[2].collider = new Collider(minions[2].position, minions[2].width, minions[2].height);
                        minions[3].collider = new Collider(minions[3].position, minions[3].width, minions[3].height);
                        break;


                    case 4:
                        // Update Minion node container positions
                        minionUpdate = true;

                        minions[1].position = new Vector2(minions[1].position.X - nodePadding, minions[1].position.Y - nodePadding*1.75f);
                        minions[2].position = new Vector2(minions[2].position.X - nodePadding, minions[2].position.Y + nodePadding*1.75f);
                        minions[3].position = new Vector2(minions[3].position.X + nodePadding*1.625f, minions[3].position.Y - nodePadding*.875f);
                        minions[4].position = new Vector2(minions[4].position.X + nodePadding*1.625f, minions[4].position.Y + nodePadding*.875f);

                        minions[1].centerPosition.X = minions[1].position.X + minions[1].width / 2;
                        minions[1].centerPosition.Y = minions[1].position.Y + minions[1].height / 2;
                        minions[2].centerPosition.X = minions[2].position.X + minions[2].width / 2;
                        minions[2].centerPosition.Y = minions[2].position.Y + minions[2].height / 2;
                        minions[3].centerPosition.X = minions[3].position.X + minions[3].width / 2;
                        minions[3].centerPosition.Y = minions[3].position.Y + minions[3].height / 2;
                        minions[4].centerPosition.X = minions[4].position.X + minions[4].width / 2;
                        minions[4].centerPosition.Y = minions[4].position.Y + minions[4].height / 2;

                        minions[1].collider = new Collider(minions[1].position, minions[1].width, minions[1].height);
                        minions[2].collider = new Collider(minions[2].position, minions[2].width, minions[2].height);
                        minions[3].collider = new Collider(minions[3].position, minions[3].width, minions[3].height);
                        minions[4].collider = new Collider(minions[4].position, minions[4].width, minions[4].height);
                        break;

                    default:
                        Console.log("ERROR INVALID MINION COUNT: " + minionCount);
                        break;
                }


                if (minionCount > 0)
                {
                    for (var i = 1; i <= minionCount; i++)
                    {
                        if (minionCount == i)
                        {
                            // Adds Choices for a minion
                            minions[i].choiceNodes.Clear();

                            minionChoice = new TreeNode("simpleMinion", new Vector2(minions[i].position.X, minions[i].position.Y + choicePadding), 64, 64, Game1.green, "Simple Support", "Simple support ship that will fire at enemies.", 0, true, Game1.Text, Game1.TextSmall);
                            minionChoice.minion = "";
                            minions[i].choiceNodes.Add(minionChoice);

                            minionChoice = new TreeNode("scatterMinion", new Vector2(minions[i].position.X - choicePadding, minions[i].position.Y), 64, 64, Game1.green, "Scatter Support", "A support ship with a built in Scatter Gun.\nAble to be upgraded.", 500, true, Game1.Text, Game1.TextSmall);
                            minionChoice.minion = "";
                            minions[i].choiceNodes.Add(minionChoice);

                            minionChoice = new TreeNode("barrageMinion", new Vector2(minions[i].position.X + choicePadding, minions[i].position.Y), 64, 64, Game1.green, "Barrage Support", "A support ship with a built in Barrage Gun.\nAble to be upgraded.", 500, true, Game1.Text, Game1.TextSmall);
                            minionChoice.minion = "";
                            minions[i].choiceNodes.Add(minionChoice);

                            minionChoice = new TreeNode("shieldMinion", new Vector2(minions[i].position.X, minions[i].position.Y - choicePadding), 64, 64, Game1.green, "Shield Support", "A support ship that adds another shield layer to your ship.\nThe shield can be upgraded.", 500, true, Game1.Text, Game1.TextSmall);
                            minionChoice.minion = "";
                            minions[i].choiceNodes.Add(minionChoice);




                            // Adds Children Upgrades for a minion

                            // SCATTER MINION UPGRADES 
                            currentUpgrade = new TreeNode("fireRate", new Vector2(minions[i].position.X + nodePadding * 2, minions[i].position.Y - nodePadding), 64, 64, Game1.green, "Fire Rate", "Upgrades this Scatter Support's fire rate.", 500, false, Game1.Text, Game1.TextSmall);
                            currentUpgrade.minion = "scatterMinion";
                            minions[i].children.Add(currentUpgrade);

                            currentUpgrade = new TreeNode("damage", new Vector2(minions[i].position.X + nodePadding * 2, minions[i].position.Y), 64, 64, Game1.green, "Damage", "Upgrades this Scatter Support's base damage.", 500, false, Game1.Text, Game1.TextSmall);
                            currentUpgrade.minion = "scatterMinion";
                            minions[i].children.Add(currentUpgrade);

                            currentUpgrade = new TreeNode("bulletSpread", new Vector2(minions[i].position.X + nodePadding * 2, minions[i].position.Y + nodePadding), 64, 64, Game1.green, "Bullet Spread", "Upgrades this Scatter Support's bullet spread making it more accurate.", 500, false, Game1.Text, Game1.TextSmall);
                            currentUpgrade.minion = "scatterMinion";
                            minions[i].children.Add(currentUpgrade);

                            // BARRAGE MINION UPGRADES
                            currentUpgrade = new TreeNode("fireRate", new Vector2(minions[i].position.X + nodePadding * 2, minions[i].position.Y - nodePadding), 64, 64, Game1.green, "Fire Rate", "Upgrades this Barrage Support's fire rate.", 500, false, Game1.Text, Game1.TextSmall);
                            currentUpgrade.minion = "barrageMinion";
                            minions[i].children.Add(currentUpgrade);

                            currentUpgrade = new TreeNode("damage", new Vector2(minions[i].position.X + nodePadding * 2, minions[i].position.Y), 64, 64, Game1.green, "Damage", "Upgrades this Barrage Support's base damage.", 500, false, Game1.Text, Game1.TextSmall);
                            currentUpgrade.minion = "barrageMinion";
                            minions[i].children.Add(currentUpgrade);

                            currentUpgrade = new TreeNode("accuracy", new Vector2(minions[i].position.X + nodePadding * 2, minions[i].position.Y + nodePadding), 64, 64, Game1.green, "Accuracy", "Upgrades this Barrage Support's accuracy.", 500, false, Game1.Text, Game1.TextSmall);
                            currentUpgrade.minion = "barrageMinion";
                            minions[i].children.Add(currentUpgrade);

                            //SHIELD MINION UPGRADES
                            currentUpgrade = new TreeNode("rechargeRate", new Vector2(minions[i].position.X + nodePadding * 2, minions[i].position.Y - nodePadding), 64, 64, Game1.green, "Recharge Rate", "Upgrades this Shield Support's shield recharge rate.", 500, false, Game1.Text, Game1.TextSmall);
                            currentUpgrade.minion = "shieldMinion";
                            minions[i].children.Add(currentUpgrade);

                            currentUpgrade = new TreeNode("capacity", new Vector2(minions[i].position.X + nodePadding * 2, minions[i].position.Y), 64, 64, Game1.green, "Shield Capacity", "Upgrades this Shield Support's shield capacity.", 500, false, Game1.Text, Game1.TextSmall);
                            currentUpgrade.minion = "shieldMinion";
                            minions[i].children.Add(currentUpgrade);

                            currentUpgrade = new TreeNode("discharge", new Vector2(minions[i].position.X + nodePadding * 2, minions[i].position.Y + nodePadding), 64, 64, Game1.green, "Discharge Damage", "Upgrades the ammount of damage this Shield Support\nwill do to surrounding enemies on\na shield break.", 500, false, Game1.Text, Game1.TextSmall);
                            currentUpgrade.minion = "shieldMinion";
                            minions[i].children.Add(currentUpgrade);
                        }
                        else
                        {
                            // Repositions Choice Nodes when a new minion is added
                            foreach(var c in minions[i].choiceNodes)
                            {
                                switch (c.type)
                                {
                                    case "simpleMinion":
                                        c.position = new Vector2(minions[i].position.X, minions[i].position.Y + choicePadding);
                                        c.collider = new Collider(c.position, c.width, c.height);
                                        break;


                                    case "scatterMinion":
                                        c.position = new Vector2(minions[i].position.X - choicePadding, minions[i].position.Y);
                                        c.collider = new Collider(c.position, c.width, c.height);
                                        break;


                                    case "barrageMinion":
                                        c.position = new Vector2(minions[i].position.X + choicePadding, minions[i].position.Y);
                                        c.collider = new Collider(c.position, c.width, c.height);
                                        break;


                                    case "shieldMinion":
                                        c.position = new Vector2(minions[i].position.X, minions[i].position.Y - choicePadding);
                                        c.collider = new Collider(c.position, c.width, c.height);
                                        break;


                                    default:
                                        Console.log("ERROR INVALID MINION CHILD TYPE: " + c.type);
                                        break;
                                }
                            }
                        }
                    }
                }


                // Repositions Upgrade Child Nodes depending on their height in the skill tree
                if (minionCount > 0)
                {
                    for (var i = 1; i <= minionCount; i++)
                    {
                        // MIDDLE MINION SLOT (1, 3 MINION)
                        if (position.Y == minions[i].position.Y)
                        {
                            foreach (var c in minions[i].children)
                            {
                                // Repositions top upgrades
                                if (c.type == "fireRate" || c.type == "rechargeRate")
                                {
                                    c.position = new Vector2(minions[i].position.X, minions[i].position.Y - nodePadding * 1.75f);
                                    c.centerPosition = new Vector2(c.position.X + c.width/2, c.position.Y + c.height/2);
                                    c.collider = new Collider(c.position, c.width,c.height);
                                }

                                // Repositions middle upgrades
                                if (c.type == "damage" || c.type == "capacity")
                                {
                                    c.position = new Vector2(minions[i].position.X + nodePadding * 1.75f, minions[i].position.Y);
                                    c.centerPosition = new Vector2(c.position.X + c.width / 2, c.position.Y + c.height / 2);
                                    c.collider = new Collider(c.position, c.width, c.height);
                                }

                                // Repositions bottom upgrades
                                if (c.type == "bulletSpread" || c.type == "accuracy" || c.type == "discharge")
                                {
                                    c.position = new Vector2(minions[i].position.X, minions[i].position.Y + nodePadding * 1.75f);
                                    c.centerPosition = new Vector2(c.position.X + c.width / 2, c.position.Y + c.height / 2);
                                    c.collider = new Collider(c.position, c.width, c.height);
                                }
                            }
                        }

                        // MIDDLE UP MINION SLOT (2, 3, 4 MINIONS)
                        if (position.Y - nodePadding*.875f == minions[i].position.Y || position.Y - nodePadding == minions[i].position.Y)
                        {
                            foreach (var c in minions[i].children)
                            {
                                // Repositions top upgrades
                                if (c.type == "fireRate" || c.type == "rechargeRate")
                                {
                                    c.position = new Vector2(minions[i].position.X, minions[i].position.Y - nodePadding * 1.75f);
                                    c.centerPosition = new Vector2(c.position.X + c.width / 2, c.position.Y + c.height / 2);
                                    c.collider = new Collider(c.position, c.width, c.height);
                                }

                                // Repositions middle upgrades
                                if (c.type == "damage" || c.type == "capacity")
                                {
                                    c.position = new Vector2(minions[i].position.X + nodePadding*.875f, minions[i].position.Y - nodePadding * .875f);
                                    c.centerPosition = new Vector2(c.position.X + c.width / 2, c.position.Y + c.height / 2);
                                    c.collider = new Collider(c.position, c.width, c.height);
                                }

                                // Repositions bottom upgrades
                                if (c.type == "bulletSpread" || c.type == "accuracy" || c.type == "discharge")
                                {
                                    c.position = new Vector2(minions[i].position.X + nodePadding*1.75f, minions[i].position.Y);
                                    c.centerPosition = new Vector2(c.position.X + c.width / 2, c.position.Y + c.height / 2);
                                    c.collider = new Collider(c.position, c.width, c.height);
                                }
                            }
                        }

                        // MIDDLE DOWN MINION SLOT (2, 3, 4 MINIONS)
                        if (position.Y + nodePadding * .875f == minions[i].position.Y || position.Y + nodePadding == minions[i].position.Y)
                        {
                            foreach (var c in minions[i].children)
                            {
                                // Repositions top upgrades
                                if (c.type == "fireRate" || c.type == "rechargeRate")
                                {
                                    c.position = new Vector2(minions[i].position.X + nodePadding*1.75f, minions[i].position.Y);
                                    c.centerPosition = new Vector2(c.position.X + c.width/2, c.position.Y + c.height/2);
                                    c.collider = new Collider(c.position, c.width,c.height);
                                }

                                // Repositions middle upgrades
                                if (c.type == "damage" || c.type == "capacity")
                                {
                                    c.position = new Vector2(minions[i].position.X + nodePadding*.875f, minions[i].position.Y + nodePadding * .875f);
                                    c.centerPosition = new Vector2(c.position.X + c.width / 2, c.position.Y + c.height / 2);
                                    c.collider = new Collider(c.position, c.width, c.height);
                                }

                                // Repositions bottom upgrades
                                if (c.type == "bulletSpread" || c.type == "accuracy" || c.type == "discharge")
                                {
                                    c.position = new Vector2(minions[i].position.X, minions[i].position.Y + nodePadding * 1.75f);
                                    c.centerPosition = new Vector2(c.position.X + c.width / 2, c.position.Y + c.height / 2);
                                    c.collider = new Collider(c.position, c.width, c.height);
                                }
                            }
                        }

                        // UP MINION SLOT (4 MINIONS)
                        if (position.Y - nodePadding * 1.75f == minions[i].position.Y)
                        {
                            foreach (var c in minions[i].children)
                            {
                                // Repositions top upgrades
                                if (c.type == "fireRate" || c.type == "rechargeRate")
                                {
                                    c.position = new Vector2(minions[i].position.X, minions[i].position.Y - nodePadding * 1.75f);
                                    c.centerPosition = new Vector2(c.position.X + c.width / 2, c.position.Y + c.height / 2);
                                    c.collider = new Collider(c.position, c.width, c.height);
                                }

                                // Repositions middle upgrades
                                if (c.type == "damage" || c.type == "capacity")
                                {
                                    c.position = new Vector2(minions[i].position.X + nodePadding * .875f, minions[i].position.Y - nodePadding * .875f);
                                    c.centerPosition = new Vector2(c.position.X + c.width / 2, c.position.Y + c.height / 2);
                                    c.collider = new Collider(c.position, c.width, c.height);
                                }

                                // Repositions bottom upgrades
                                if (c.type == "bulletSpread" || c.type == "accuracy" || c.type == "discharge")
                                {
                                    c.position = new Vector2(minions[i].position.X + nodePadding * 1.75f, minions[i].position.Y);
                                    c.centerPosition = new Vector2(c.position.X + c.width / 2, c.position.Y + c.height / 2);
                                    c.collider = new Collider(c.position, c.width, c.height);
                                }
                            }
                        }

                        // DOWN MINION SLOT (4 MINIONS)
                        if (position.Y + nodePadding * 1.75f == minions[i].position.Y)
                        {
                            foreach (var c in minions[i].children)
                            {
                                // Repositions top upgrades
                                if (c.type == "fireRate" || c.type == "rechargeRate")
                                {
                                    c.position = new Vector2(minions[i].position.X + nodePadding * 1.75f, minions[i].position.Y);
                                    c.centerPosition = new Vector2(c.position.X + c.width / 2, c.position.Y + c.height / 2);
                                    c.collider = new Collider(c.position, c.width, c.height);
                                }

                                // Repositions middle upgrades
                                if (c.type == "damage" || c.type == "capacity")
                                {
                                    c.position = new Vector2(minions[i].position.X + nodePadding * .875f, minions[i].position.Y + nodePadding * .875f);
                                    c.centerPosition = new Vector2(c.position.X + c.width / 2, c.position.Y + c.height / 2);
                                    c.collider = new Collider(c.position, c.width, c.height);
                                }

                                // Repositions bottom upgrades
                                if (c.type == "bulletSpread" || c.type == "accuracy" || c.type == "discharge")
                                {
                                    c.position = new Vector2(minions[i].position.X, minions[i].position.Y + nodePadding * 1.75f);
                                    c.centerPosition = new Vector2(c.position.X + c.width / 2, c.position.Y + c.height / 2);
                                    c.collider = new Collider(c.position, c.width, c.height);
                                }
                            }
                        }
                    }
                }
            }


            // Draw Minion Nodes
            for (var i = 1; i <= minionCount; i++)
            {
                if (minions[i].showChildren)
                {
                    foreach (var c in minions[i].children)
                    {
                        if (c.minion == minions[i].minion)
                        {
                            Game1.DrawLine(_spriteBatch, graphics, minions[i].centerPosition, c.centerPosition, 4, Color.Yellow);
                            _spriteBatch.Draw(c.sprite, c.collider.container, Color.White);
                        }
                    }
                }
            }

            for (var i = 1; i <= minionCount; i++)
            {
                Game1.DrawLine(_spriteBatch, graphics, centerPosition, minions[i].centerPosition, 4, Color.Yellow);
                _spriteBatch.Draw(minions[i].sprite, minions[i].collider.container, Color.White);

                if (minions[i].showChoices)
                {
                    minions[i].MinionChoiceDrawing(_spriteBatch, graphics);
                }
            }
        }


        public void MinionChoiceDrawing(SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
        {
            foreach(var c in choiceNodes)
            {
                _spriteBatch.Draw(c.sprite, c.collider.container, Color.White);
            }
        }
    }
}
