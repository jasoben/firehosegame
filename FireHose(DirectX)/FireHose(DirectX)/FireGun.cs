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
    class FireGun
    {

        //Player player;
        public Vector2 PlayerPosition;
        

        public Vector2 GunOrigin;
        public float GunRotation;

        Texture2D fireGunTexture;
        Texture2D fireDropTexture; 
        ParticleEngine fireParticleEngine;

        public List<Rectangle> FireRectangles; 
       

        World world;

        public bool isFiring = false;

        public FireGun(Player player, Vector2 playerPosition, World world)
        {
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            PlayerPosition = playerPosition;

            this.world = world;
        }

        public void LoadContent(ContentManager content)
        {
            fireGunTexture = content.Load<Texture2D>("firegun.png");
            fireDropTexture = content.Load<Texture2D>("particle.png");
            fireParticleEngine = new ParticleEngine(world, fireDropTexture, PlayerPosition, Color.Orange);
                
        }

        public void Update(Vector2 playerPosition, Controls controls, List<Keys> playerControls)
        {
            PlayerPosition = playerPosition;
            BlastFire(controls, playerControls);
            if (isFiring == false)
            {
                fireParticleEngine.Update(isFiring);
                FireRectangles = fireParticleEngine.GetRectangles();
            }

            GunOrigin = controls.FlyFire() * 10;
            GunOrigin = new Vector2(GunOrigin.X, -1 * GunOrigin.Y);
            GunRotation = (float)(Math.Atan2(GunOrigin.Y, GunOrigin.X));

            //testing

            
            
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(fireGunTexture, ConvertUnits.ToDisplayUnits(PlayerPosition), null, Color.White, GunRotation, new Vector2 (0,0), .2f, SpriteEffects.None, 0f);
            fireParticleEngine.Draw(sb);
        }

        public void BlastFire(Controls controls, List<Keys> playerControls)
        {
            if (controls.isThumbStick(Buttons.RightThumbstickDown) || controls.isThumbStick(Buttons.RightThumbstickUp) || controls.isThumbStick(Buttons.RightThumbstickLeft) || controls.isThumbStick(Buttons.RightThumbstickRight))
            {
                isFiring = true;
                fireParticleEngine.ParticleEmitterLocation = PlayerPosition;
                fireParticleEngine.ParticleEmitterVelocity = controls.FlyFire() * 2;
                fireParticleEngine.Update(isFiring);
                FireRectangles = fireParticleEngine.GetRectangles();
                
            } else         
            {
                isFiring = false;
                fireParticleEngine.ParticleEmitterLocation = PlayerPosition;
                fireParticleEngine.ParticleEmitterVelocity = controls.FlyFire() * 2;
                fireParticleEngine.Update(isFiring);
                FireRectangles = fireParticleEngine.GetRectangles();
            }

        }

        public List<Rectangle> GetRectangles()
        {
            return FireRectangles; 
        }
    }
}
