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

        public int FlameGunLocationX { get; set; }
        public int FlameGunLocationY { get; set; }

        public bool isFiringFlame = false;

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

        public void Update(Player player1, Controls controls, GameTime gameTime, FireParticleEngine fireParticleEngine)
        {
            this.spriteX = FlameGunLocationX = player1.PlayerLocationX + (int) degreeX;
            this.spriteY = FlameGunLocationY = player1.PlayerLocationY + (int) degreeY;

            FireFlame(controls, gameTime, fireParticleEngine, player1, this); 
            AimFlame(controls, gameTime);

            if (isFiringFlame == false)
            {
                fireParticleEngine.Update(isFiringFlame);
            } 
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
        private void FireFlame(Controls controls, GameTime gameTime, FireParticleEngine fireParticleEngine, Player player1, FlameGun flameGun)
        {
            // Fire on button press
            if (controls.onPress(Keys.LeftControl, Buttons.B) || controls.isHeld(Keys.LeftControl, Buttons.B))
            {
                isFiringFlame = true;
                fireParticleEngine.FireEmitterLocation = new Vector2(this.FlameGunLocationX, this.FlameGunLocationY);
                fireParticleEngine.PlayerLocation = new Vector2(player1.PlayerLocationX, player1.PlayerLocationY);
                fireParticleEngine.Update(isFiringFlame);
            }

            //stop firing
            if (controls.onRelease(Keys.LeftControl, Buttons.B))
            {
                isFiringFlame = false;
                fireParticleEngine.FireEmitterLocation = new Vector2(this.FlameGunLocationX, this.FlameGunLocationY);
                fireParticleEngine.PlayerLocation = new Vector2(player1.PlayerLocationX, player1.PlayerLocationY);
                fireParticleEngine.Update(isFiringFlame);
            }
        }
    }
}
