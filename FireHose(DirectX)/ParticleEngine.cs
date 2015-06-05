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
        public Vector2 ParticleEmitterLocation;
        public Vector2 ParticleVelocity;
        Vector2 particleOrigin;

        private List<Body> particles;
        private List<Color> particleColors;
        private List<int> particlesTTL;
        private Dictionary<Body, Color> particleDictionary = new Dictionary<Body,Color>(); 

        private Texture2D particleTexture;
        public Color ParticleColor;

        public World ThisWorld;

        Body particle;

        public int ParticleTTL;
        private float particleDensity;
        private float particlePower 

        Random randomVelocity;
        private List<float> particleVelocities;

        public ParticleEngine(World world, Texture2D particleTexture, Vector2 particleEmitterLocation, Vector2 particleVelocity, Color particleColor, int particlePower)
        {
            ParticleEmitterLocation = particleEmitterLocation;
            //ParticleEmitterVelocity = particleVelocity;
            //PlayerLocation = playerLocation;
            
            this.particleTexture = particleTexture;
            particleOrigin = new Vector2(particleTexture.Width / 2f, particleTexture.Height / 2f);

            particles = new List<Body>();
            particlesTTL = new List<int>();
            particleVelocities = new List<float>();
            particleColors = new List<Color>();

            ParticleColor = particleColor;
            ParticleVelocity = particleVelocity;
            ThisWorld = world;
            
            
        }

        public Body GenerateNewParticle(int particleTTL)
        {
            Texture2D theParticleTexture = particleTexture;
            
            Color particleColor = new Color(255, 255, 255);
            ParticleTTL = particleTTL; 

            randomVelocity = new Random();

            particle = BodyFactory.CreateCircle(ThisWorld, ConvertUnits.ToSimUnits(particleTexture.Width / 2), particleDensity, ParticleEmitterLocation);
            particle.BodyType = BodyType.Dynamic;
            particle.Restitution = .1f;
            particle.Friction = .5f;

            
            if (ParticleTTL > 50)
            {
                particle.CollisionCategories = Category.Cat3;
                particle.CollidesWith = Category.Cat4 | Category.Cat1;
                particle.Restitution = .05f;
               // particle.IsSensor = true;
                particleVelocities.Add(-100);
                particleColors.Add(ParticleColor);
                particleDensity = .01f;
                particlePower = .003f;                
            }
            else
            {
                particle.CollisionCategories = Category.Cat2;
                particle.CollidesWith = Category.Cat1 | Category.Cat4;
                particleVelocities.Add(randomVelocity.Next(1, 20));
                particleColors.Add(ParticleColor);
                particleDensity = 1f;
                particlePower = .2f;
            }
            
            particles.Add(particle);
            particlesTTL.Add(ParticleTTL);

            particleDictionary.Add(particle, ParticleColor);

            return particle;
        }

        public void Update(Boolean isFiring, Vector2 particleEmitterLocation, Vector2 particleVelocity)
        {

            ParticleEmitterLocation = particleEmitterLocation;
            ParticleVelocity = particleVelocity * particlePower;
            
            for (int i = 0; i < particles.Count; i++)
            {
                particlesTTL[i] = particlesTTL[i] - 1;
                
                if (particlesTTL[i] > (ParticleTTL - 10))
                {
                    particles[i].ApplyLinearImpulse((ParticleVelocity / 100)+ (ParticleVelocity / (100 * (particleVelocities[i]))));
                }
                if (particlesTTL[i] < 10)
                {
                    particleDictionary[particles[i]] = new Color(ParticleColor, (.1f * i));
                    particles[i].Mass = .001f;
                }
                if (particlesTTL[i] < 0)
                {
                    
                    particles[i].Dispose();
                    particleDictionary.Remove(particles[i]);
                    particles.RemoveAt(i);
                    particlesTTL.RemoveAt(i);
                    particleVelocities.RemoveAt(i);
                    
                }

                particles[i].OnCollision += particleCollided;
                
            }

            
            

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //    spriteBatch.Begin();
            //foreach (Body particle in particles)
            //{
            //    spriteBatch.Draw(particleTexture, ConvertUnits.ToDisplayUnits(particle.Position), null, ParticleColor, 0f, particleOrigin, 1f, SpriteEffects.None, 0f);
            //}

            foreach (KeyValuePair<Body,Color> entry in particleDictionary)
            {
                spriteBatch.Draw(particleTexture, ConvertUnits.ToDisplayUnits(entry.Key.Position), null, entry.Value, 0f, particleOrigin, 1f, SpriteEffects.None, 0f);
            }

            //    spriteBatch.End();
        }

        //Dummy method for pushing information to the text box

        //public string GetInfo()
        //{
        //    //return MyString = ConvertUnits.ToDisplayUnits(particleEngine.ParticleEmitterLocation).ToString();
        //    return MyString = ConvertUnits.ToSimUnits(particleTexture.Width - 2).ToString();

        //}

        public bool particleCollided(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == Category.Cat4)
            { 
                return true;
            }
            else
            { 
                return true;
            }
        }

        

    }
}
