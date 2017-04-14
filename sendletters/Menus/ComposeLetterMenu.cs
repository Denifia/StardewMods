using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using StardewValley;
//using StardewValley.Menus;

namespace denifia.stardew.sendletters.Menus
{
    internal class ComposeLetterMenu : IClickableMenu

    {
        private int questID = -1;
        private string learnedRecipe = "";
        private string cookingOrCrafting = "";
        private List<string> mailMessage = new List<string>();
        private List<ClickableComponent> itemsToGrab = new List<ClickableComponent>();
        public const int letterWidth = 320;
        public const int letterHeight = 180;
        public Texture2D letterTexture;
        private int moneyIncluded;
        private string mailTitle;
        private int page;
        private float scale;
        private bool isMail;
        private ClickableTextureComponent backButton;
        private ClickableTextureComponent forwardButton;
        private ClickableComponent acceptQuestButton;
        public const float scaleChange = 0.003f;

        public ComposeLetterMenu(string text)
      : base((int) Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).X, (int) Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).Y, 320 * Game1.pixelZoom, 180 * Game1.pixelZoom, true)
    {
            Game1.playSound("shwip");
            this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
            this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
            Game1.temporaryContent = Game1.content.CreateTemporary();
            this.letterTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\letterBG");
            this.mailMessage = SpriteText.getStringBrokenIntoSectionsOfHeight(text, this.width - Game1.tileSize / 2, this.height - Game1.tileSize * 2);
        }

        public ComposeLetterMenu(string mail, string mailTitle)
      : base((int) Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).X, (int) Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).Y, 320 * Game1.pixelZoom, 180 * Game1.pixelZoom, true)
    {
            this.isMail = true;
            Game1.playSound("shwip");
            this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
            this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
            this.acceptQuestButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - Game1.tileSize * 2, this.yPositionOnScreen + this.height - Game1.tileSize * 2, Game1.tileSize * 4, Game1.tileSize), "");
            this.mailTitle = mailTitle;
            Game1.temporaryContent = Game1.content.CreateTemporary();
            this.letterTexture = Game1.temporaryContent.Load<Texture2D>("LooseSprites\\letterBG");
            if (mail.Contains("%item"))
            {
                string oldValue = mail.Substring(mail.IndexOf("%item"), mail.IndexOf("%%") + 2 - mail.IndexOf("%item"));
                string[] strArray1 = oldValue.Split(' ');
                mail = mail.Replace(oldValue, "");
                if (strArray1[1].Equals("object"))
                {
                    int maxValue = strArray1.Length - 1;
                    int num = Game1.random.Next(2, maxValue);
                    int index = num - num % 2;
                    StardewValley.Object @object = new StardewValley.Object(Vector2.Zero, Convert.ToInt32(strArray1[index]), Convert.ToInt32(strArray1[index + 1]));
                    this.itemsToGrab.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom), (Item)@object));
                }
                else if (strArray1[1].Equals("tools"))
                {
                    for (int index = 2; index < strArray1.Length; ++index)
                    {
                        Item obj = (Item)null;
                        string str = strArray1[index];
                        if (!(str == "Axe"))
                        {
                            if (!(str == "Hoe"))
                            {
                                if (!(str == "Can"))
                                {
                                    if (!(str == "Scythe"))
                                    {
                                        if (str == "Pickaxe")
                                            obj = (Item)new Pickaxe();
                                    }
                                    else
                                        obj = (Item)new MeleeWeapon(47);
                                }
                                else
                                    obj = (Item)new WateringCan();
                            }
                            else
                                obj = (Item)new Hoe();
                        }
                        else
                            obj = (Item)new Axe();
                        if (obj != null)
                            this.itemsToGrab.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom), obj));
                    }
                }
                else if (strArray1[1].Equals("bigobject"))
                {
                    int maxValue = strArray1.Length - 1;
                    int index = Game1.random.Next(2, maxValue);
                    StardewValley.Object @object = new StardewValley.Object(Vector2.Zero, Convert.ToInt32(strArray1[index]), false);
                    this.itemsToGrab.Add(new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom), (Item)@object));
                }
                else if (strArray1[1].Equals("money"))
                {
                    int num1 = strArray1.Length > 4 ? Game1.random.Next(Convert.ToInt32(strArray1[2]), Convert.ToInt32(strArray1[3])) : Convert.ToInt32(strArray1[2]);
                    int num2 = num1 - num1 % 10;
                    Game1.player.Money += num2;
                    this.moneyIncluded = num2;
                }
                else if (strArray1[1].Equals("quest"))
                {
                    this.questID = Convert.ToInt32(strArray1[2]);
                    if (strArray1.Length > 4)
                    {
                        if (!Game1.player.mailReceived.Contains("NOQUEST_" + (object)this.questID))
                            Game1.player.addQuest(this.questID);
                        this.questID = -1;
                    }
                }
                else if (strArray1[1].Equals("cookingRecipe"))
                {
                    Dictionary<string, string> dictionary = Game1.content.Load<Dictionary<string, string>>("Data\\CookingRecipes");
                    foreach (string key in dictionary.Keys)
                    {
                        string[] strArray2 = dictionary[key].Split('/')[3].Split(' ');
                        if (strArray2[0].Equals("f") && strArray2[1].Equals(mailTitle.Replace("Cooking", "")) && (Game1.player.friendships[strArray2[1]][0] >= Convert.ToInt32(strArray2[2]) * 250 && !Game1.player.cookingRecipes.ContainsKey(key)))
                        {
                            Game1.player.cookingRecipes.Add(key, 0);
                            this.learnedRecipe = key;
                            this.cookingOrCrafting = Game1.content.LoadString("Strings\\UI:LearnedRecipe_cooking");
                            break;
                        }
                    }
                }
                else if (strArray1[1].Equals("craftingRecipe"))
                {
                    this.learnedRecipe = strArray1[2].Replace('_', ' ');
                    Game1.player.craftingRecipes.Add(this.learnedRecipe, 0);
                    this.cookingOrCrafting = Game1.content.LoadString("Strings\\UI:LearnedRecipe_crafting");
                }
            }
            Random r = new Random((int)(Game1.uniqueIDForThisGame / 2UL) - Game1.year);
            mail = mail.Replace("%secretsanta", Utility.getRandomTownNPC(r, Utility.getFarmerNumberFromFarmer(Game1.player)).name);
            this.mailMessage = SpriteText.getStringBrokenIntoSectionsOfHeight(mail, this.width - Game1.tileSize, this.height - Game1.tileSize * 2);
        }

        public override void gameWindowSizeChanged(Rectangle oldBounds, Rectangle newBounds)
        {
            this.xPositionOnScreen = (int)Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).X;
            this.yPositionOnScreen = (int)Utility.getTopLeftPositionForCenteringOnScreen(320 * Game1.pixelZoom, 180 * Game1.pixelZoom, 0, 0).Y;
            this.backButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(352, 495, 12, 11), (float)Game1.pixelZoom, false);
            this.forwardButton = new ClickableTextureComponent(new Rectangle(this.xPositionOnScreen + this.width - Game1.tileSize / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 16 * Game1.pixelZoom, 12 * Game1.pixelZoom, 11 * Game1.pixelZoom), Game1.mouseCursors, new Rectangle(365, 495, 12, 11), (float)Game1.pixelZoom, false);
            this.acceptQuestButton = new ClickableComponent(new Rectangle(this.xPositionOnScreen + this.width / 2 - Game1.tileSize * 2, this.yPositionOnScreen + this.height - Game1.tileSize * 2, Game1.tileSize * 4, Game1.tileSize), "");
            foreach (ClickableComponent clickableComponent in this.itemsToGrab)
                clickableComponent.bounds = new Rectangle(this.xPositionOnScreen + this.width / 2 - 12 * Game1.pixelZoom, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - 24 * Game1.pixelZoom, 24 * Game1.pixelZoom, 24 * Game1.pixelZoom);
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if ((double)this.scale < 1.0)
                return;
            base.receiveLeftClick(x, y, playSound);
            if (Game1.activeClickableMenu == null && Game1.currentMinigame == null)
            {
                this.unload();
            }
            else
            {
                foreach (ClickableComponent clickableComponent in this.itemsToGrab)
                {
                    if (clickableComponent.containsPoint(x, y) && clickableComponent.item != null)
                    {
                        Game1.playSound("coin");
                        Game1.player.addItemByMenuIfNecessary(clickableComponent.item, (ItemGrabMenu.behaviorOnItemSelect)null);
                        clickableComponent.item = (Item)null;
                        return;
                    }
                }
                if (this.backButton.containsPoint(x, y) && this.page > 0)
                {
                    this.page = this.page - 1;
                    Game1.playSound("shwip");
                }
                else if (this.forwardButton.containsPoint(x, y) && this.page < this.mailMessage.Count - 1)
                {
                    this.page = this.page + 1;
                    Game1.playSound("shwip");
                }
                else if (this.questID != -1 && this.acceptQuestButton.containsPoint(x, y))
                {
                    Game1.player.addQuest(this.questID);
                    this.questID = -1;
                    Game1.playSound("newArtifact");
                }
                else if (this.isWithinBounds(x, y))
                {
                    if (this.page < this.mailMessage.Count - 1)
                    {
                        this.page = this.page + 1;
                        Game1.playSound("shwip");
                    }
                    else if (this.page == this.mailMessage.Count - 1 && this.mailMessage.Count > 1)
                    {
                        this.page = 0;
                        Game1.playSound("shwip");
                    }
                    if (this.mailMessage.Count != 1 || this.isMail)
                        return;
                    this.exitThisMenuNoSound();
                    Game1.playSound("shwip");
                }
                else
                {
                    if (this.itemsLeftToGrab())
                        return;
                    this.exitThisMenuNoSound();
                    Game1.playSound("shwip");
                }
            }
        }

        public bool itemsLeftToGrab()
        {
            if (this.itemsToGrab == null)
                return false;
            foreach (ClickableComponent clickableComponent in this.itemsToGrab)
            {
                if (clickableComponent.item != null)
                    return true;
            }
            return false;
        }

        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);
            foreach (ClickableComponent clickableComponent in this.itemsToGrab)
                clickableComponent.scale = !clickableComponent.containsPoint(x, y) ? Math.Max(1f, clickableComponent.scale - 0.03f) : Math.Min(clickableComponent.scale + 0.03f, 1.1f);
            this.backButton.tryHover(x, y, 0.6f);
            this.forwardButton.tryHover(x, y, 0.6f);
            if (this.questID == -1)
                return;
            float scale = this.acceptQuestButton.scale;
            this.acceptQuestButton.scale = this.acceptQuestButton.bounds.Contains(x, y) ? 1.5f : 1f;
            if ((double)this.acceptQuestButton.scale <= (double)scale)
                return;
            Game1.playSound("Cowboy_gunshot");
        }

        public override void update(GameTime time)
        {
            base.update(time);
            TimeSpan timeSpan;
            if ((double)this.scale < 1.0)
            {
                double scale = (double)this.scale;
                timeSpan = time.ElapsedGameTime;
                double num = (double)timeSpan.Milliseconds * (3.0 / 1000.0);
                this.scale = (float)(scale + num);
                if ((double)this.scale >= 1.0)
                    this.scale = 1f;
            }
            if (this.page >= this.mailMessage.Count - 1 || this.forwardButton.containsPoint(Game1.getOldMouseX(), Game1.getOldMouseY()))
                return;
            ClickableTextureComponent forwardButton = this.forwardButton;
            double num1 = 4.0;
            timeSpan = time.TotalGameTime;
            double num2 = Math.Sin((double)timeSpan.Milliseconds / (64.0 * Math.PI)) / 1.5;
            double num3 = num1 + num2;
            forwardButton.scale = (float)num3;
        }

        public override void draw(SpriteBatch b)
        {
            b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.4f);
            b.Draw(this.letterTexture, new Vector2((float)(this.xPositionOnScreen + this.width / 2), (float)(this.yPositionOnScreen + this.height / 2)), new Rectangle?(new Rectangle(0, 0, 320, 180)), Color.White, 0.0f, new Vector2(160f, 90f), (float)Game1.pixelZoom * this.scale, SpriteEffects.None, 0.86f);
            if ((double)this.scale == 1.0)
            {
                SpriteText.drawString(b, this.mailMessage[this.page], this.xPositionOnScreen + Game1.tileSize / 2, this.yPositionOnScreen + Game1.tileSize / 2, 999999, this.width - Game1.tileSize, 999999, 0.75f, 0.865f, false, -1, "", -1);
                foreach (ClickableComponent clickableComponent in this.itemsToGrab)
                {
                    b.Draw(this.letterTexture, clickableComponent.bounds, new Rectangle?(new Rectangle(0, 180, 24, 24)), Color.White);
                    if (clickableComponent.item != null)
                        clickableComponent.item.drawInMenu(b, new Vector2((float)(clickableComponent.bounds.X + 4 * Game1.pixelZoom), (float)(clickableComponent.bounds.Y + 4 * Game1.pixelZoom)), clickableComponent.scale);
                }
                if (this.moneyIncluded > 0)
                {
                    string s = Game1.content.LoadString("Strings\\UI:LetterViewer_MoneyIncluded", (object)this.moneyIncluded);
                    SpriteText.drawString(b, s, this.xPositionOnScreen + this.width / 2 - SpriteText.getWidthOfString(s) / 2, this.yPositionOnScreen + this.height - Game1.tileSize * 3 / 2, 999999, -1, 9999, 0.75f, 0.865f, false, -1, "", -1);
                }
                else if (this.learnedRecipe != null && this.learnedRecipe.Length > 0)
                {
                    string s = Game1.content.LoadString("Strings\\UI:LetterViewer_LearnedRecipe", (object)this.cookingOrCrafting);
                    SpriteText.drawStringHorizontallyCenteredAt(b, s, this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - SpriteText.getHeightOfString(s, 999999) * 2, 999999, this.width - Game1.tileSize, 9999, 0.65f, 0.865f, false, -1);
                    SpriteText.drawStringHorizontallyCenteredAt(b, Game1.content.LoadString("Strings\\UI:LetterViewer_LearnedRecipeName", (object)this.learnedRecipe), this.xPositionOnScreen + this.width / 2, this.yPositionOnScreen + this.height - Game1.tileSize / 2 - SpriteText.getHeightOfString("t", 999999), 999999, this.width - Game1.tileSize, 9999, 0.9f, 0.865f, false, -1);
                }
                base.draw(b);
                if (this.page < this.mailMessage.Count - 1)
                    this.forwardButton.draw(b);
                if (this.page > 0)
                    this.backButton.draw(b);
                if (this.questID != -1)
                {
                    IClickableMenu.drawTextureBox(b, Game1.mouseCursors, new Rectangle(403, 373, 9, 9), this.acceptQuestButton.bounds.X, this.acceptQuestButton.bounds.Y, this.acceptQuestButton.bounds.Width, this.acceptQuestButton.bounds.Height, (double)this.acceptQuestButton.scale > 1.0 ? Color.LightPink : Color.White, (float)Game1.pixelZoom * this.acceptQuestButton.scale, true);
                    Utility.drawTextWithShadow(b, Game1.content.LoadString("Strings\\UI:AcceptQuest"), Game1.dialogueFont, new Vector2((float)(this.acceptQuestButton.bounds.X + Game1.pixelZoom * 3), (float)(this.acceptQuestButton.bounds.Y + Game1.pixelZoom * 3)), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);
                }
            }
            if (Game1.options.hardwareCursor)
                return;
            b.Draw(Game1.mouseCursors, new Vector2((float)Game1.getMouseX(), (float)Game1.getMouseY()), new Rectangle?(Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 0, 16, 16)), Color.White, 0.0f, Vector2.Zero, (float)Game1.pixelZoom + Game1.dialogueButtonScale / 150f, SpriteEffects.None, 1f);
        }

        public void unload()
        {
            Game1.temporaryContent.Unload();
            Game1.temporaryContent = (LocalizedContentManager)null;
        }

        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            this.receiveLeftClick(x, y, playSound);
        }
    }
}