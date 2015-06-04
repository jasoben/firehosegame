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
    class Particle
    {

        public Texture2D ParticleTexture { get; set; }
        public Vector2 ParticlePosition { get; set; }
        public Vector2 ParticleVelocity { get; set; }
        public Rectangle SourceRectangle;

        private Random randomDirection;
        private int r;
        
        //public float FireAngle { get; set; }
        //public float FireAngularVelocity { get; set; }
        public Color ParticleColor { get; set; }
        public float ParticleSize { get; set; }
        public int ParticleTTL { get; set; }

        public Particle(Texture2D particleTexture, Vector2 particlePosition, Vector2 particleVelocity, int particlePower, Color particleColor, float particleSize, int particleTTL)
        {

            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            ParticlePosition = ConvertUnits.ToDisplayUnits(particlePosition);
            ParticleTexture = particleTexture;
            ParticleColor = particleColor;
            ParticleSize = particleSize;
            ParticleTTL = particleTTL;
            
            
            SourceRectangle = new Rectangle(0, 0, ParticleTexture.Width, ParticleTexture.Height);
            randomDirection = new Random();
            r = randomDirection.Next(-8, 8);

            ParticleVelocity = (particleVelocity + new Vector2(r, r)) / particlePower;
        }

        public void Update()
        {
            ParticleTTL--;
            ParticlePosition += ParticleVelocity;

        }

        
        public void Draw(SpriteBatch particleSB)
        {
            Vector2 origin = new Vector2(ParticleTexture.Width / 2, ParticleTexture.Height / 2);
            particleSB.Draw(ParticleTexture, ParticlePosition, SourceRectangle, ParticleColor, 1f, origin, ParticleSize, SpriteEffects.None, 0f);
           
        }

    }
}
