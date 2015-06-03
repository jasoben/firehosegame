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
    /// <summary>
    /// This is the class for the flamethrower. 
    /// </summary>
    class FireGun
    {

        public Vector2 PlayerPosition;
        
        public Vector2 GunOrigin;
        public float GunRotation;

        Texture2D fireGunTexture;
        Texture2D fireDropTexture; 
        ParticleEngine fireParticleEngine;

        //this list passes the particle rectangles down into the base class to check
        public List<Rectangle> FireRectangles; 
       
        //world field to receive world from the base class
        World world;

        //this property is to adjust the update for the particle emitter
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

            //load the particle emitter object that activates when we fire
            //note the color orange for fire!

            fireParticleEngine = new ParticleEngine(world, fireDropTexture, PlayerPosition, Color.Orange);
                
        }

        public void Update(Vector2 playerPosition, Controls controls, List<Keys> playerControls)
        {
            PlayerPosition = playerPosition;
            BlastFire(controls, playerControls);
            
            //this conditional continues to update the particles after we stop firing
            if (isFiring == false)
            {
                fireParticleEngine.Update(isFiring);

                //this gets all the rectangles of the particles that have been emitted and passes them to the list, which gets passed down to the base class
                FireRectangles = fireParticleEngine.GetRectangles();
            }

            //compute where to place the gun in relation to the body based on inputs (called down below)
            GunOrigin = controls.FlyFire() * 10;
            GunOrigin = new Vector2(GunOrigin.X, -1 * GunOrigin.Y);
            GunRotation = (float)(Math.Atan2(GunOrigin.Y, GunOrigin.X));

            
            
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
                
                //this gets all the rectangles of the particles that have been emitted and passes them to the list, which gets passed down to the base class
                FireRectangles = fireParticleEngine.GetRectangles();
                
            } else         
            {
                isFiring = false;
                fireParticleEngine.ParticleEmitterLocation = PlayerPosition;
                fireParticleEngine.ParticleEmitterVelocity = controls.FlyFire() * 2;
                fireParticleEngine.Update(isFiring);

                //this gets all the rectangles of the particles that have been emitted and passes them to the list, which gets passed down to the base class
                FireRectangles = fireParticleEngine.GetRectangles();
            }

        }

        //method called by player class to get particle rectangles and pass them down to the base class
        public List<Rectangle> GetRectangles()
        {
            return FireRectangles; 
        }
    }
}
