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
        public float degree = 0f;
        public float degreeY = 20f;
        public float angle = 0f;

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
            /*
            if (controls.onPress(Keys.X, Buttons.RightThumbstickRight) || controls.isHeld(Keys.X, Buttons.RightThumbstickRight))
            {
                angle += 10;
            }
            if (controls.onPress(Keys.Z, Buttons.RightThumbstickRight) || controls.isHeld(Keys.Z, Buttons.RightThumbstickLeft))
            {
                angle -= 10;
            }

            degreeX = (float)Math.Cos((Math.PI / 180) * angle) * 60;
            degreeY = (float)Math.Sin((Math.PI / 180) * angle) * 60;
             * */
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            float maxSpeed = 1f;
            float scale = maxSpeed * 10;
            float changeInAngleX = gamePadState.ThumbSticks.Right.X * maxSpeed;
            float changeInAngleY = gamePadState.ThumbSticks.Right.Y * maxSpeed;


                degree = (float)Math.Atan(changeInAngleY / changeInAngleX)* 60; // gives us absolute controller direction in radians
                if (degree != angle)
                {
                    angle += scale; //have to change to plus or minus depending on where reticle is relative to controller input                
                }
                //then stop changing but we need degree and angle on the same metric
            degreeX = (float)Math.Cos((Math.PI / 180) * angle) * 60;
            degreeY = (float)Math.Sin((Math.PI / 180) * angle) * 60;
            
        }
        private void FireFlame(Controls controls, GameTime gameTime, FireParticleEngine fireParticleEngine, Player player1, FlameGun flameGun)
        {
            // Fire on button press
            if (controls.onPress(Keys.LeftControl, Buttons.RightThumbstickRight) || controls.isHeld(Keys.LeftControl, Buttons.RightThumbstickRight))
            {
                isFiringFlame = true;
                fireParticleEngine.FireEmitterLocation = new Vector2(this.FlameGunLocationX, this.FlameGunLocationY);
                fireParticleEngine.PlayerLocation = new Vector2(player1.PlayerLocationX, player1.PlayerLocationY);
                fireParticleEngine.Update(isFiringFlame);
            }

            //stop firing
            if (controls.onRelease(Keys.LeftControl, Buttons.RightThumbstickRight))
            {
                isFiringFlame = false;
                fireParticleEngine.FireEmitterLocation = new Vector2(this.FlameGunLocationX, this.FlameGunLocationY);
                fireParticleEngine.PlayerLocation = new Vector2(player1.PlayerLocationX, player1.PlayerLocationY);
                fireParticleEngine.Update(isFiringFlame);
            }
        }
    }
}
