using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GalacticSurvival
{
    internal class UpgradeTree
    {
        private MouseState mouseState;

        private TreeNode weaponTree;
        private TreeNode ammoTree;
        private TreeNode minionTree;
        private TreeNode shipTree;

        private bool weaponChoosing;
        private bool ammoChoosing;
        private Dictionary<int, bool> minionChoosing = new Dictionary<int, bool>();

        private Dictionary<string, TreeNode> treeNodes = new Dictionary<string, TreeNode>();

        private int choicePadding = 84;
        private int nodePadding = 96;

        public string currentWeapon = "default";
        public string currentAmmo = "default";

        private UI NextRoundButton;
        public Dictionary<string, UI> elements = new Dictionary<string, UI>();



        public UpgradeTree(GraphicsDeviceManager graphics, Camera camera)
        {
            CreateWeaponTree();

            CreateAmmoTree();

            CreateMinionTree();

            CreateShipTree();



            string text;
            Vector2 textSize;
            Vector2 textPos;
            Vector2 buttonPos;
            int buttonWidth;
            int buttonHeight;

            // Creates Start Next Round Button
            buttonWidth = 364;
            buttonHeight = 80;
            buttonPos.X = graphics.PreferredBackBufferWidth/2 - buttonWidth/2;
            buttonPos.Y = graphics.PreferredBackBufferHeight - buttonHeight - buttonHeight/2;
            buttonPos = camera.ScreenToWorld(buttonPos, 0);
            text = "Begin Next Round";
            textSize = Game1.Text.MeasureString(text);
            textPos.X = buttonPos.X + buttonWidth / 2 - textSize.X / 2;
            textPos.Y = buttonPos.Y + buttonHeight / 2 - textSize.Y / 2;
            NextRoundButton = new UI(buttonPos, buttonWidth, buttonHeight, text, Color.White, textPos, Game1.Text);
            elements["nextLevel"] = NextRoundButton; // Adds created element to elements list
        }


        // Weapon Tree Initialization
        private void CreateWeaponTree()
        {
            TreeNode currentSubNode;

            // Creates Weapon Tree
            weaponTree = new TreeNode("weapon", new Vector2(-32, -nodePadding*1.25f - 96), 64, 64, Game1.green, "Weapon Selection", "Select your main weapon of choice\nto protect yourself from the \nonslaught of enemies.", 0, Game1.Text, Game1.TextSmall);
            treeNodes["weapon"] = weaponTree;

            // Creates Weapon Tree Choices
            weaponTree.choiceNodes.Add(new TreeNode("defaultGun", new Vector2(weaponTree.position.X, weaponTree.position.Y + choicePadding), 64, 64, Game1.green, "Simple Gun", "Base weapon attached to all factory\ndefault gun ships.\nThis will only get you so far, in terms\nof power.", 0, Game1.Text, Game1.TextSmall));
            weaponTree.choiceNodes.Add(new TreeNode("railGun", new Vector2(weaponTree.position.X - choicePadding, weaponTree.position.Y), 64, 64, Game1.green, "Rail Gun", "Strong precision based weapon. Has a\nslight ammount of charge time and a\nlow fire rate, but makes up for it with\nits devastating power, perfect accuracy,\nand built in ammo piercing capabilities.", 0, Game1.Text, Game1.TextSmall));
            weaponTree.choiceNodes.Add(new TreeNode("scatterGun", new Vector2(weaponTree.position.X, weaponTree.position.Y - choicePadding), 64, 64, Game1.green, "Scatter Gun", "Powerful weapon that closely resembles the\ncommon \"shotgun\".\nVery useful for clearing out large groups of\nenemies.", 0, Game1.Text, Game1.TextSmall));
            weaponTree.choiceNodes.Add(new TreeNode("barrageGun", new Vector2(weaponTree.position.X + choicePadding, weaponTree.position.Y), 64, 64, Game1.green, "Barrage Gun", "A devastating weapon with a fire\nrate so high that it easily compansates\nfor its low accuracy and low damage\nper shot fired.", 0, Game1.Text, Game1.TextSmall));

            // railGun Upgrades
            currentSubNode = new TreeNode("fireRate", new Vector2(weaponTree.position.X - nodePadding, weaponTree.position.Y - nodePadding * 2.25f), 64, 64, Game1.green, "Fire Rate", "", 500, Game1.Text, Game1.Text);
            currentSubNode.weapon = "railGun";
            weaponTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("damage", new Vector2(weaponTree.position.X, weaponTree.position.Y - nodePadding * 2.25f), 64, 64, Game1.green, "Damage", "Damage MULTIPLIER to ammo damage", 500, Game1.Text, Game1.Text);
            currentSubNode.weapon = "railGun";
            weaponTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("piercing", new Vector2(weaponTree.position.X + nodePadding, weaponTree.position.Y - nodePadding * 2.25f), 64, 64, Game1.green, "Piercing", "", 500, Game1.Text, Game1.Text);
            currentSubNode.weapon = "railGun";
            weaponTree.children.Add(currentSubNode);

            // scatterGun Upgrades
            currentSubNode = new TreeNode("fireRate", new Vector2(weaponTree.position.X - nodePadding, weaponTree.position.Y - nodePadding * 2.25f), 64, 64, Game1.green, "Fire Rate", "", 500, Game1.Text, Game1.Text);
            currentSubNode.weapon = "scatterGun";
            weaponTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("damage", new Vector2(weaponTree.position.X, weaponTree.position.Y - nodePadding * 2.25f), 64, 64, Game1.green, "Damage", "Damage MULTIPLIER to ammo damage", 500, Game1.Text, Game1.Text);
            currentSubNode.weapon = "scatterGun";
            weaponTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("bulletSpread", new Vector2(weaponTree.position.X + nodePadding, weaponTree.position.Y - nodePadding * 2.25f), 64, 64, Game1.green, "Bullet Spread", "", 500, Game1.Text, Game1.Text);
            currentSubNode.weapon = "scatterGun";
            weaponTree.children.Add(currentSubNode);

            // barrageGun Upgrades
            currentSubNode = new TreeNode("fireRate", new Vector2(weaponTree.position.X - nodePadding, weaponTree.position.Y - nodePadding * 2.25f), 64, 64, Game1.green, "Fire Rate", "", 500, Game1.Text, Game1.Text);
            currentSubNode.weapon = "barrageGun";
            weaponTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("damage", new Vector2(weaponTree.position.X, weaponTree.position.Y - nodePadding * 2.25f), 64, 64, Game1.green, "Damage", "Damage MULTIPLIER to ammo damage", 500, Game1.Text, Game1.Text);
            currentSubNode.weapon = "barrageGun";
            weaponTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("accuracy", new Vector2(weaponTree.position.X + nodePadding, weaponTree.position.Y - nodePadding * 2.25f), 64, 64, Game1.green, "Accuracy", "", 500, Game1.Text, Game1.Text);
            currentSubNode.weapon = "barrageGun";
            weaponTree.children.Add(currentSubNode);
        }

        // Ammo Tree Initialization
        private void CreateAmmoTree()
        {
            TreeNode currentSubNode;

            // Creates Ammo Tree
            ammoTree = new TreeNode("ammo", new Vector2(-nodePadding * 1.25f - 32, -96), 64, 64, Game1.green, "Ammo Selection", "Select your Ammo Type.\nAmmo can be upgraded and\nchanged to be more destructive.", 0, Game1.Text, Game1.TextSmall);
            treeNodes["ammo"] = ammoTree;

            // Creates Ammo Tree Choices
            ammoTree.choiceNodes.Add(new TreeNode("defaultRounds", new Vector2(ammoTree.position.X, ammoTree.position.Y + choicePadding), 64, 64, Game1.green, "Simple Rounds", "", 0, Game1.Text, Game1.Text));
            ammoTree.choiceNodes.Add(new TreeNode("chainingRounds", new Vector2(ammoTree.position.X - choicePadding, ammoTree.position.Y + choicePadding), 64, 64, Game1.green, "Chaining Rounds", "", 0, Game1.Text, Game1.Text));
            ammoTree.choiceNodes.Add(new TreeNode("piercingRounds", new Vector2(ammoTree.position.X - choicePadding, ammoTree.position.Y), 64, 64, Game1.green, "Piercing Rounds", "", 0, Game1.Text, Game1.Text));
            ammoTree.choiceNodes.Add(new TreeNode("causticRounds", new Vector2(ammoTree.position.X - choicePadding, ammoTree.position.Y - choicePadding), 64, 64, Game1.green, "Caustic Rounds", "", 0, Game1.Text, Game1.Text));
            ammoTree.choiceNodes.Add(new TreeNode("homingRounds", new Vector2(ammoTree.position.X, ammoTree.position.Y - choicePadding), 64, 64, Game1.green, "Homing Rounds", "", 0, Game1.Text, Game1.Text));

            // Chaining Rounds Upgrades
            currentSubNode = new TreeNode("velocity", new Vector2(ammoTree.position.X - nodePadding * 2.25f, ammoTree.position.Y - nodePadding), 64, 64, Game1.green, "Round Velocity", "", 500, Game1.Text, Game1.Text);
            currentSubNode.ammo = "chainingRounds";
            ammoTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("damage", new Vector2(ammoTree.position.X - nodePadding * 2.25f, ammoTree.position.Y), 64, 64, Game1.green, "Damage", "Accumulative Damage", 500, Game1.Text, Game1.Text);
            currentSubNode.ammo = "chainingRounds";
            ammoTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("chainAmmount", new Vector2(ammoTree.position.X - nodePadding * 2.25f, ammoTree.position.Y + nodePadding), 64, 64, Game1.green, "Chain Ammount", "", 500, Game1.Text, Game1.Text);
            currentSubNode.ammo = "chainingRounds";
            ammoTree.children.Add(currentSubNode);

            // Piercing Rounds Upgrades
            currentSubNode = new TreeNode("velocity", new Vector2(ammoTree.position.X - nodePadding * 2.25f, ammoTree.position.Y - nodePadding), 64, 64, Game1.green, "Round Velocity", "", 500, Game1.Text, Game1.Text);
            currentSubNode.ammo = "piercingRounds";
            ammoTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("damage", new Vector2(ammoTree.position.X - nodePadding * 2.25f, ammoTree.position.Y), 64, 64, Game1.green, "Damage", "Accumulative Damage", 500, Game1.Text, Game1.Text);
            currentSubNode.ammo = "piercingRounds";
            ammoTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("pierceAmmount", new Vector2(ammoTree.position.X - nodePadding * 2.25f, ammoTree.position.Y + nodePadding), 64, 64, Game1.green, "Pierce Ammount", "", 500, Game1.Text, Game1.Text);
            currentSubNode.ammo = "piercingRounds";
            ammoTree.children.Add(currentSubNode);

            // Caustic Rounds Upgrades
            currentSubNode = new TreeNode("velocity", new Vector2(ammoTree.position.X - nodePadding * 2.25f, ammoTree.position.Y - nodePadding), 64, 64, Game1.green, "Round Velocity", "", 500, Game1.Text, Game1.Text);
            currentSubNode.ammo = "causticRounds";
            ammoTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("damage", new Vector2(ammoTree.position.X - nodePadding * 2.25f, ammoTree.position.Y), 64, 64, Game1.green, "Damage", "Accumulative Damage", 500, Game1.Text, Game1.Text);
            currentSubNode.ammo = "causticRounds";
            ammoTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("causticTick", new Vector2(ammoTree.position.X - nodePadding * 2.25f, ammoTree.position.Y + nodePadding), 64, 64, Game1.green, "Caustic Tick", "", 500, Game1.Text, Game1.Text);
            currentSubNode.ammo = "causticRounds";
            ammoTree.children.Add(currentSubNode);

            // Homing Rounds Upgrades
            currentSubNode = new TreeNode("velocity", new Vector2(ammoTree.position.X - nodePadding * 2.25f, ammoTree.position.Y - nodePadding), 64, 64, Game1.green, "Round Velocity", "", 500, Game1.Text, Game1.Text);
            currentSubNode.ammo = "homingRounds";
            ammoTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("damage", new Vector2(ammoTree.position.X - nodePadding * 2.25f, ammoTree.position.Y), 64, 64, Game1.green, "Damage", "Accumulative Damage", 500, Game1.Text, Game1.Text);
            currentSubNode.ammo = "homingRounds";
            ammoTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("trackTime", new Vector2(ammoTree.position.X - nodePadding * 2.25f, ammoTree.position.Y + nodePadding), 64, 64, Game1.green, "Track Time", "", 500, Game1.Text, Game1.Text);
            currentSubNode.ammo = "homingRounds";
            ammoTree.children.Add(currentSubNode);
        }

        // Minion Tree Initialization
        private void CreateMinionTree()
        {
            TreeNode currentSubNode;

            // Creates Minion Tree
            minionTree = new TreeNode("minion", new Vector2(nodePadding * 1.25f - 32, -96), 64, 64, Game1.green, "Increase Supporting Ship Count", "Purchasing grants an additional Support Ship\nthat can be upgraded and modified.", 1500, Game1.Text, Game1.TextSmall);
            treeNodes["minion"] = minionTree;

            currentSubNode = new TreeNode("1", new Vector2(minionTree.position.X + nodePadding * 2f, minionTree.position.Y), 64, 64, Game1.green, "", "", 500, Game1.Text, Game1.Text);
            minionTree.minions[1] = currentSubNode;
            currentSubNode = new TreeNode("2", new Vector2(minionTree.position.X + nodePadding * 2f, minionTree.position.Y), 64, 64, Game1.green, "", "", 500, Game1.Text, Game1.Text);
            minionTree.minions[2] = currentSubNode;
            currentSubNode = new TreeNode("3", new Vector2(minionTree.position.X + nodePadding * 2f, minionTree.position.Y), 64, 64, Game1.green, "", "", 500, Game1.Text, Game1.Text);
            minionTree.minions[3] = currentSubNode;
            currentSubNode = new TreeNode("4", new Vector2(minionTree.position.X + nodePadding * 2f, minionTree.position.Y), 64, 64, Game1.green, "", "", 500, Game1.Text, Game1.Text);
            minionTree.minions[4] = currentSubNode;


            minionChoosing[1] = false;
            minionChoosing[2] = false;
            minionChoosing[3] = false;
            minionChoosing[4] = false;
        }

        // Ship Tree Initialization
        private void CreateShipTree()
        {
            TreeNode currentSubNode;

            // Creates Ship Tree
            shipTree = new TreeNode("ship", new Vector2(-32, nodePadding * 1.25f - 96), 64, 64, Game1.green, "Ship Upgrades", "Basic, yet essential upgrades\nto your ship", 0, Game1.Text, Game1.TextSmall);
            treeNodes["ship"] = shipTree;

            
            // shipUpgrades
            currentSubNode = new TreeNode("health", new Vector2(shipTree.position.X - nodePadding, shipTree.position.Y + nodePadding * 2.25f), 64, 64, Game1.green, "Health", "", 500, Game1.Text, Game1.Text);
            shipTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("shield", new Vector2(shipTree.position.X, shipTree.position.Y + nodePadding * 2.25f), 64, 64, Game1.green, "Shield", "", 500, Game1.Text, Game1.Text);
            shipTree.children.Add(currentSubNode);
            currentSubNode = new TreeNode("repairRate", new Vector2(shipTree.position.X + nodePadding, shipTree.position.Y + nodePadding * 2.25f), 64, 64, Game1.green, "Repair Rate", "", 500, Game1.Text, Game1.Text);
            shipTree.children.Add(currentSubNode);
        }


        public Game1.State Update(Game1.State currentState, Mission mission, Cursor cursor)
        {
            mouseState = Mouse.GetState();


            if (elements["nextLevel"].pressed)
            {
                elements["nextLevel"].pressed = false;
                Console.log("???");

                return Game1.State.Mission;
            }


            // Checks weapon node for clicked and updates SHOW CHOICES
            if (treeNodes["weapon"].IsNodeHighlighted(cursor) && cursor.clicked)
            {
                if (treeNodes["weapon"].showChoices)
                {
                    treeNodes["weapon"].showChoices = false;
                    weaponChoosing = false;
                }
                else
                {
                    treeNodes["weapon"].showChoices = true;
                    weaponChoosing = true;
                }
            }

            // Checks ammo node for clicked and updates SHOW CHOICES
            if (treeNodes["ammo"].IsNodeHighlighted(cursor) && cursor.clicked)
            {
                if (treeNodes["ammo"].showChoices)
                {
                    treeNodes["ammo"].showChoices = false;
                    ammoChoosing = false;
                }
                else
                {
                    treeNodes["ammo"].showChoices = true;
                    ammoChoosing = true;
                }
            }

            // Checks minion node for click
            if (treeNodes["minion"].IsNodeHighlighted(cursor) && cursor.clicked && minionTree.minionCount < 4)
            {
                minionTree.minionCount += 1;
                minionTree.minionUpdate = false;
            }

            // Checks Minion Nodes for clicked and updates SHOW CHOICES
            if (minionTree.minionCount > 0)
            {
                for (var i = 1; i <= minionTree.minionCount; i++)
                {
                    if (minionTree.minions[i].IsNodeHighlighted(cursor) && cursor.clicked)
                    {
                        if (minionTree.minions[i].showChoices)
                        {
                            minionTree.minions[i].showChoices = false;
                            minionChoosing[i] = false;
                        }
                        else
                        {
                            minionTree.minions[i].showChoices = true;
                            minionChoosing[i] = true;
                        }
                    }
                }
            }





            // Handles clicking for all weapon choosing nodes
            if (weaponChoosing)
            {
                foreach (var t in weaponTree.choiceNodes)
                {
                    if (t.IsNodeHighlighted(cursor) && cursor.clicked)
                    {
                        currentWeapon = t.type;
                    }
                }
            }

            // Handles clicking for all ammo choosing nodes
            if (ammoChoosing)
            {
                foreach (var t in ammoTree.choiceNodes)
                {
                    if (t.IsNodeHighlighted(cursor) && cursor.clicked)
                    {
                        currentAmmo = t.type;
                    }
                }
            }

            // Handles clicking for all minion choice nodes
            for (var i = 1; i <= minionTree.minionCount; i++)
            {
                if (minionChoosing[i])
                {
                    foreach (var t in minionTree.minions[i].choiceNodes)
                    {
                        if (t.IsNodeHighlighted(cursor) && cursor.clicked)
                        {
                            minionTree.minions[i].minion = t.type;
                            minionTree.minions[i].showChildren = true;
                        }
                    }
                }
            }





            // Handles Clicking for Weapon Upgrade Nodes
            foreach(var c in weaponTree.children)
            {
                if (c.IsNodeHighlighted(cursor) && cursor.clicked && currentWeapon == c.weapon)
                {
                    Console.log(c.type);
                }
            }

            // Handles Clicking for Ammo Upgrade Nodes
            foreach (var c in ammoTree.children)
            {
                if (c.IsNodeHighlighted(cursor) && cursor.clicked && currentAmmo == c.ammo)
                {
                    Console.log(c.type);
                }
            }

            // Handles Clicking for Minion Upgrade Nodes
            for (var i = 1; i <= minionTree.minionCount; i++)
            {
                foreach(var c in minionTree.minions[i].children)
                {
                    if (c.IsNodeHighlighted(cursor) && cursor.clicked && c.minion == minionTree.minions[i].minion)
                    {
                        Console.log(c.type + "");
                    }
                }
            }

            // Handles Clicking for Ship Upgrade Nodes
            foreach (var c in shipTree.children)
            {
                if (c.IsNodeHighlighted(cursor) && cursor.clicked)
                {
                    Console.log(c.type);
                }
            }





            // Closes choices if their nodes are not clicked on
            if (cursor.clicked)
            {
                // Check to see if weapon is clicked
                if (!treeNodes["weapon"].IsNodeHighlighted(cursor))
                {
                    treeNodes["weapon"].showChoices = false;
                    weaponChoosing = false;
                }

                // Check to see if ammo is cliced
                if (!treeNodes["ammo"].IsNodeHighlighted(cursor))
                {
                    treeNodes["ammo"].showChoices = false;
                    ammoChoosing = false;
                }

                // Check to see if a minion is clicked
                for (var i = 1; i <= minionTree.minionCount; i++)
                {
                    if (!minionTree.minions[i].IsNodeHighlighted(cursor))
                    {
                        minionTree.minions[i].showChoices = false;
                        minionChoosing[i] = false;
                    }
                }
            }

            // resets clicked
            cursor.clicked = false;

            return currentState;
        }


        public void Draw(GameTime gameTime, SpriteBatch _spriteBatch, GraphicsDeviceManager graphics, Cursor cursor)
        {
            foreach (var e in elements) // Draws UI elements attached to Main Menu
            {
                e.Value.Draw(gameTime, _spriteBatch, graphics);
            }


            // Draws trees
            weaponTree.Draw(gameTime, _spriteBatch, graphics, cursor, currentWeapon, null);
            ammoTree.Draw(gameTime, _spriteBatch, graphics, cursor, null, currentAmmo);
            minionTree.Draw(gameTime, _spriteBatch, graphics, cursor, null, null);
            shipTree.Draw(gameTime, _spriteBatch, graphics, cursor, null, null);

            
            // Handles Highlighting for Main Nodes
            if (treeNodes["weapon"].IsNodeHighlighted(cursor))
                treeNodes["weapon"].DrawDescriptor(gameTime, _spriteBatch, graphics, cursor);
            if (treeNodes["ammo"].IsNodeHighlighted(cursor))
                treeNodes["ammo"].DrawDescriptor(gameTime, _spriteBatch, graphics, cursor);
            if (treeNodes["minion"].IsNodeHighlighted(cursor))
                treeNodes["minion"].DrawDescriptor(gameTime, _spriteBatch, graphics, cursor);
            if (treeNodes["ship"].IsNodeHighlighted(cursor))
                treeNodes["ship"].DrawDescriptor(gameTime, _spriteBatch, graphics, cursor);


            // Handles Highlighting for Weapon and Ammo Child Nodes
            foreach(var c in weaponTree.children)
            {
                if (c.IsNodeHighlighted(cursor) && c.weapon == currentWeapon)
                {
                    c.DrawDescriptor(gameTime, _spriteBatch, graphics, cursor);
                }
            }
            foreach (var c in ammoTree.children)
            {
                if (c.IsNodeHighlighted(cursor) && c.ammo == currentAmmo)
                {
                    c.DrawDescriptor(gameTime, _spriteBatch, graphics, cursor);
                }
            }


            // Handles Highlighting for Minion Nodes
            if (minionTree.minionCount > 0)
            {
                for (var i = 1; i <= minionTree.minionCount; i++)
                {
                    if (minionTree.minions[i].IsNodeHighlighted(cursor))
                        minionTree.minions[i].DrawDescriptor(gameTime, _spriteBatch, graphics, cursor);

                    // Handles Highlighting for Minion Children
                    if (minionTree.minions[i].showChildren && minionTree.minions[i].minion != "simpleMinion")
                    {
                        foreach(var c in minionTree.minions[i].children)
                        {
                            if (c.IsNodeHighlighted(cursor) && c.minion == minionTree.minions[i].minion)
                                c.DrawDescriptor(gameTime, _spriteBatch, graphics, cursor);
                        }
                    }
                }
            }

            // Handles Weapon Choice Highlighting
            if (weaponChoosing)
            {
                foreach (var t in weaponTree.choiceNodes)
                {
                    if (t.IsNodeHighlighted(cursor))
                        t.DrawDescriptor(gameTime, _spriteBatch, graphics, cursor);
                }
            }


            // Handles Ammo Choice Highlighting
            if (ammoChoosing)
            {
                foreach (var t in ammoTree.choiceNodes)
                {
                    if (t.IsNodeHighlighted(cursor))
                        t.DrawDescriptor(gameTime, _spriteBatch, graphics, cursor);
                }
            }


            // Handles Minions Choice Highlighting
            for (var i = 1; i <= minionTree.minionCount; i++)
            {
                if (minionChoosing[i])
                {
                    foreach(var t in minionTree.minions[i].choiceNodes)
                    {
                        if (t.IsNodeHighlighted(cursor))
                        {
                            t.DrawDescriptor(gameTime, _spriteBatch, graphics, cursor);
                        }
                    }
                }
            }


            // Handles Ship Upgrade Highlighting
            foreach(var c in shipTree.children)
            {
                if (c.IsNodeHighlighted(cursor))
                    c.DrawDescriptor(gameTime, _spriteBatch, graphics, cursor);
            }
        }
    }
}
