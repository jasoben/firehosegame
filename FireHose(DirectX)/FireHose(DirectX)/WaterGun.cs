#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics.Contacts;

#endregion

namespace FireHose_DirectX_
{
    class WaterGun
    {

        //Player player;
        public Vector2 PlayerPosition;
        

        public Vector2 GunOrigin;
        public float GunRotation;

        Texture2D waterGunTexture;
        Texture2D waterDropTexture; 
        ParticleEngine waterParticleEngine;
        

        World world;

        public bool isFiring = false;

        public WaterGun(Player player, Vector2 playerPosition, World world)
        {
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            PlayerPosition = playerPosition;

            this.world = world;
            
            
        }

        public void LoadContent(ContentManager content)
        {
            waterGunTexture = content.Load<Texture2D>("watergun.png");
            waterDropTexture = content.Load<Texture2D>("particle.png");
            waterParticleEngine = new ParticleEngine(world, waterDropTexture, PlayerPosition, Color.Blue);
                
        }

        public void Update(Vector2 playerPosition, Controls controls, List<Keys> playerControls)
        {
            PlayerPosition = playerPosition;
            BlastWater(controls, playerControls);
            if (isFiring == false)
            {
                waterParticleEngine.Update(isFiring);
            }

            GunOrigin = controls.FlyWater() * 10;
            GunOrigin = new Vector2(GunOrigin.X, -1 * GunOrigin.Y);
            GunRotation = (float)(Math.Atan2(GunOrigin.Y, GunOrigin.X));

            //testing

            
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(waterGunTexture, ConvertUnits.ToDisplayUnits(PlayerPosition), null, Color.White, GunRotation, new Vector2 (0,0), .2f, SpriteEffects.None, 0f);
            waterParticleEngine.Draw(sb);
        }

        public void BlastWater(Controls controls, List<Keys> playerControls)
        {
            if (!controls.isHeld(Keys.U, Buttons.LeftShoulder))
            {
                if (controls.isThumbStick(Buttons.LeftThumbstickDown) || controls.isThumbStick(Buttons.LeftThumbstickUp) || controls.isThumbStick(Buttons.LeftThumbstickLeft) || controls.isThumbStick(Buttons.LeftThumbstickRight))
                {
                    isFiring = true;
                    waterParticleEngine.ParticleEmitterLocation = PlayerPosition;
                    waterParticleEngine.ParticleEmitterVelocity = controls.FlyWater();
                    waterParticleEngine.Update(isFiring);


                }
                else
                {
                    isFiring = false;
                    waterParticleEngine.ParticleEmitterLocation = PlayerPosition;
                    waterParticleEngine.ParticleEmitterVelocity = controls.FlyWater();
                    waterParticleEngine.Update(isFiring);

                }
            }
        }
    }
}
