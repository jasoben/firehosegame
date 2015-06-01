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
    class ParticleEngine
    {
        public Vector2 ParticleEmitterLocation { get; set; }
        public Vector2 ParticleEmitterVelocity { get; set; }
        private List<Particle> particles;
        private Texture2D particleTexture;
        public Color ParticleColor;

        public ParticleEngine(World world, Texture2D particleTexture, Vector2 particleEmitterLocation, Color particleColor)
        {
            ParticleEmitterLocation = particleEmitterLocation;
            //ParticleEmitterVelocity = particleVelocity;
            //PlayerLocation = playerLocation;
            this.particleTexture = particleTexture;
            this.particles = new List<Particle>();
            ParticleColor = particleColor;
        }

        private Particle GenerateNewParticle()
        {
            Texture2D theParticleTexture = particleTexture;
            
            Vector2 particleLocation  = ParticleEmitterLocation;
            Vector2 particleVelocity = ParticleEmitterVelocity;
            
            Color particleColor = new Color(255, 255, 255);
            float particleSize = .2f;
            int particleTTL = 20;

            return new Particle(theParticleTexture, particleLocation, particleVelocity, ParticleColor, particleSize, particleTTL);
        }

        public void Update(Boolean isFiring)
        {
            int particleTotal = 30;

            if (isFiring == true)
            {
                
                for (int i = 0; i < particleTotal; i++)
                {
                    particles.Add(GenerateNewParticle());
                }
            }

            for (int currentParticle = 0; currentParticle< particles.Count; currentParticle++)
            {
                particles[currentParticle].Update();
                if (particles[currentParticle].ParticleTTL <= 0)
                {
                    particles.RemoveAt(currentParticle);
                    currentParticle--;
                }
            }



        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //    spriteBatch.Begin();
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
            //    spriteBatch.End();
        }

    }
}
