using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HelloGame.Controls
{
    public class buttonTexture
    {
        public Texture2D img {get;set;}
       
        public Rectangle rect { get; set; } 
        public buttonTexture(){}
        public event EventHandler<PlayerIndexEventArgs> Selected;
        protected internal virtual void OnSelectEntry(/*PlayerIndex playerIndex*/)
        {
            if (Selected != null)
                Selected(this, new PlayerIndexEventArgs(/*playerIndex*/));
        }
    }
}
