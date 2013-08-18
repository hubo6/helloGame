//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;



namespace HelloGame
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
            : base("Main Menu")
        {
            MenuEntry levelSelect = new MenuEntry("New Game");
            levelSelect.Selected += newGamePressed;
            MenuEntries.Add(levelSelect);

            MenuEntry highScores = new MenuEntry("Exit");
            highScores.Selected += exitCancel;
            MenuEntries.Add(highScores);
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// Event handler for our Select Level button.
        /// </summary>
        private void newGamePressed(object sender, PlayerIndexEventArgs e)
        {
            // We use the loading screen to move to our level selection screen because the
            // level selection screen needs to load in a decent amount of level art. The Load
            // method will cause all current screens to exit, so to enable us to be able to
            // easily come back from the level select screen, we must also pass down the
            // background and main menu screens.

            LoadingScreen.Load(
                ScreenManager, 
                true, 
                e.PlayerIndex, 
                new BackgroundScreen(), new MainMenuScreen(), new LevelSelectScreen());
        }

        private void alertOk(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }

        private void alertCancel(object sender, PlayerIndexEventArgs e)
        {}
        private void exitCancel(object sender, PlayerIndexEventArgs e)
        {
           OnCancel(e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, we exit the game.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.Game.Exit();
        }

        public override void HandleInput(InputState input)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                MessageBoxScreen alert = new MessageBoxScreen("Warnning");
                alert.ok.Selected += alertOk;
                alert.cancel.Selected += alertCancel;
                ScreenManager.AddScreen(alert, null);
            }
            else
                base.HandleInput(input);
        }
    }
}
