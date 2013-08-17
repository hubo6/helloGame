#region File Description
//-----------------------------------------------------------------------------
// MessageBoxScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace HelloGame
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    class MessageBoxScreen : MenuScreen//GameScreen
    {
        #region Fields

        string message;
        Texture2D gradientTexture;

        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        MenuEntry ok = new MenuEntry("OK");
        MenuEntry cancel = new MenuEntry("Cancel");

        // Hook up menu event handlers.
        #endregion

        #region Initialization
        
         void okSelected(object sender, PlayerIndexEventArgs e)
         {
             ScreenManager.Game.Exit();

         }
         void cancelSelected(object sender, PlayerIndexEventArgs e)
         {
             ExitScreen();
         }
        
        public MessageBoxScreen(string message)
            : base(message)
        {
            this.message = message;
            IsPopup = true;

            ok.Selected += okSelected;
            cancel.Selected += cancelSelected;
            // 
            // Add entries to the menu.
            MenuEntries.Add(ok);
            MenuEntries.Add(cancel);

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
            gradientTexture = content.Load<Texture2D>("Png\\gradient");
            base.LoadContent();
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>


        #endregion

        #region Draw
        /// <summary>
        /// Draws the message box.
        /// </summary>


        #endregion
    }
}
