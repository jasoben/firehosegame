#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

#endregion

namespace Firehose
{
    class Gun : Sprite
    {
        public int degree = 1;

        public Gun(int width, int height)
        {
            this.spriteWidth = width;
            this.spriteHeight = height;			
        }

        public void LoadContent(ContentManager content)
        {
            flameImage = content.Load<Texture2D>("aiming_reticule.png");
        }
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(flameImage, new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight), Color.White);
        }

        public void Update(Player player1, GameTime gameTime)
        {
            this.spriteX = player1.playerLocationX + 60 * degree;
            this.spriteY = player1.playerLocationY + 20;
        }
    }
}
