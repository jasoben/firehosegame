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

namespace FireHoseTake2_DirectX_
{
    class WaterGun
    {

        //Player player;
        public Vector2 PlayerPosition;
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
            waterGunTexture = content.Load<Texture2D>("waterdrop.png");
            waterDropTexture = content.Load<Texture2D>("waterdrop.png");
            waterParticleEngine = new ParticleEngine(world, waterDropTexture, PlayerPosition, PlayerPosition);
                
        }

        public void Update(Vector2 playerPosition, Controls controls, List<Keys> playerControls)
        {
            PlayerPosition = playerPosition;
            BlastWater(controls, playerControls);
            if (isFiring == false)
            {
                waterParticleEngine.Update(isFiring);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(waterGunTexture, ConvertUnits.ToDisplayUnits(PlayerPosition), null, Color.White);
            waterParticleEngine.Draw(sb);
        }

        public void BlastWater(Controls controls, List<Keys> playerControls)
        {
            if (controls.onPress(playerControls[4], Buttons.X))
            {
                isFiring = true;
                waterParticleEngine.ParticleEmitterLocation = PlayerPosition;
                waterParticleEngine.PlayerLocation = PlayerPosition;
                waterParticleEngine.Update(isFiring);
            }
            if (controls.onRelease(playerControls[4], Buttons.X))
            {
                isFiring = false;
                waterParticleEngine.ParticleEmitterLocation = PlayerPosition;
                waterParticleEngine.PlayerLocation = PlayerPosition;
                waterParticleEngine.Update(isFiring);
            }

        }
    }
}
