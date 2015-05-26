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
    class FlameGun : Sprite
    {
        FireParticle fire;
        
        public float degreeX = 60f;
        public float degreeY = 20f;
        public int angle = 0;

        public int flameGunLocationX;
        public int flameGunLocationY;

        public FlameGun(int width, int height)
        {
            this.spriteWidth = width;
            this.spriteHeight = height;			
        }

        public void LoadContent(ContentManager content)
        {
            crossHairImage = content.Load<Texture2D>("aiming_reticule.png");
        }
        
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(crossHairImage, new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight), Color.White);
        }

        public void Update(Player player1, Controls controls, GameTime gameTime)
        {
            this.spriteX = flameGunLocationX = player1.playerLocationX + (int) degreeX + 25;
            this.spriteY = flameGunLocationY = player1.playerLocationY + (int) degreeY + 25;

            FireFlame(flameGunLocationX, flameGunLocationY, controls, gameTime); 
            AimFlame(controls, gameTime);
        }

        private void AimFlame(Controls controls, GameTime gameTime)
        {
            //Aim Gun
            
            if (controls.onPress(Keys.X, Buttons.Y) || controls.isHeld(Keys.X, Buttons.Y))
            {
                angle += 10;
            }
            if (controls.onPress(Keys.Z, Buttons.Y) || controls.isHeld(Keys.Z, Buttons.Y))
            {
                angle -= 10;
            }

            degreeX = (float)Math.Cos((Math.PI / 180) * angle) * 60;
            degreeY = (float)Math.Sin((Math.PI / 180) * angle) * 60;
        }
        private void FireFlame(int x, int y, Controls controls, GameTime gameTime)
        {
            // Fire on button press
            if (controls.onPress(Keys.LeftControl, Buttons.B) || controls.isHeld(Keys.LeftControl, Buttons.B))
            {
             //   fire = new FireParticle(x, y);
            }
        }
    }
}
