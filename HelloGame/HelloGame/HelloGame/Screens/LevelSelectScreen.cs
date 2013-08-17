//-----------------------------------------------------------------------------
// LevelSelectScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using HelloGame.Controls;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;


namespace HelloGame
{
    public class LevelInfo
    {
        public string Name;
        public string Description;
        public string Image;
    }

    // This class demonstrates the PageFlipControl, by letting the player choose from a
    // set of game levels. Each level is shown with an 
    public class LevelSelectScreen : SingleControlScreen
    {
        // Descriptions of the different levels.
        LevelInfo[] LevelInfos = new LevelInfo[] {
            new LevelInfo
            {
                Name="Easy",
                Description="Easy level",
                Image="Png\\House"
            },
            new LevelInfo
            {
                Name="Normal",
                Description="Normal level",
                Image="Png\\Pasture"
            },
            new LevelInfo
            {
                Name="Hard",
                Description="Hard level",
                Image="Png\\Hills"
            }
        };


        public buttonTexture buttonGo { get; set; }
        public void GoButtonLocation()
        {
            buttonGo.rect = new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width - buttonGo.img.Width,
                ScreenManager.GraphicsDevice.Viewport.Height / 2 - buttonGo.img.Height / 2, buttonGo.img.Width, buttonGo.img.Height);
        }

        public override void LoadContent()
        {
            EnabledGestures = PageFlipTracker.GesturesNeeded;
            ContentManager content = ScreenManager.Game.Content;
            
            buttonGo = new buttonTexture();
            buttonGo.img = content.Load<Texture2D>("Png\\buttonGo");
            buttonGo.Selected += goPressed;
            //buttonGo.img = img;
            GoButtonLocation();
            RootControl = new PageFlipControl();

            foreach (LevelInfo info in LevelInfos)
            {
                RootControl.AddChild(new LevelDescriptionPanel(content, info));
            }

            EnabledGestures = EnabledGestures | GestureType.Tap;
        }

        private void goPressed(object sender, PlayerIndexEventArgs e)
        {
            //add game screen
            int n = 0;
        }

        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(buttonGo.img, new Vector2(buttonGo.rect.Left,buttonGo.rect.Top), Color.White);
            ScreenManager.SpriteBatch.End();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput(InputState input)
        {
            // cancel the current screen if the user presses the back button
            bool pressedGoButton = false;
            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    //start new game
                    if (GetButtonHitBounds(buttonGo).Contains(new Point((int)gesture.Position.X, (int)gesture.Position.Y)))
                    {
                        buttonGo.OnSelectEntry();
                        pressedGoButton = true;  
                    }
                }
            }
            if (!pressedGoButton)
                base.HandleInput(input);
        }

        protected virtual Rectangle GetButtonHitBounds(buttonTexture button)
        {
            return button.rect;
        }
    }

    

    public class LevelDescriptionPanel : PanelControl
    {
        const float MarginLeft = 20;
        const float MarginTop = 20;
        const float DescriptionTop = 440;

        public LevelDescriptionPanel(ContentManager content, LevelInfo info)
        {
            Texture2D backgroundTexture = content.Load<Texture2D>(info.Image);
            ImageControl background = new ImageControl(backgroundTexture, Vector2.Zero);
            AddChild(background);

            SpriteFont titleFont = content.Load<SpriteFont>("Font\\MenuTitle");
            TextControl title = new TextControl(info.Name, titleFont, Color.Black, new Vector2(MarginLeft, MarginTop));
            AddChild(title);

            SpriteFont descriptionFont = content.Load<SpriteFont>("Font\\MenuDetail");
            TextControl description = new TextControl(info.Description, descriptionFont, Color.Black, new Vector2(MarginLeft, DescriptionTop));
            AddChild(description);
        }
    }
}
