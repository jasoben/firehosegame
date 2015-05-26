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
    class Fire : Sprite
    {
        public int fireVelocityX;
        public int fireVelocityY;

        public Fire(int x, int y)
        {
            this.spriteWidth = 20;
            this.spriteHeight = 20;
            this.spriteX = 20;
            this.spriteY = 20;

            Console.WriteLine("Hello Mr. Squishy.");
        }

        public void LoadContent(ContentManager content)
        {
            fireImage = content.Load<Texture2D>("fire.png");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(fireImage, new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight), Color.White);
        }

        public void Update(Player player1, FlameGun flameGun, Controls controls, GameTime gameTime)
        {
            //fireVelocityX = flameGun.flameGunLocationX - player1.playerLocationX;
            //fireVelocityY = flameGun.flameGunLocationY - player1.playerLocationY;

            //this.spriteX += fireVelocityX / 5;
            //this.spriteY += fireVelocityY / 5;
        }

    }

}
