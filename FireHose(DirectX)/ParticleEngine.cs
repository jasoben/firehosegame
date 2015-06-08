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

        private List<Particle> particles;

        private Texture2D particleTexture;
        private Texture2D steamTexture;
        public Color ParticleColor;

        public World ThisWorld;

        Body particle;

        Particle thatParticle;

        float drawScale = 1f;

        private float particleDensity;
        private float particlePower;

        public int CurrentParticle;
        public int PlayerNumber;

        //private float drawScale = 1f;

        public ParticleEngine(World world, Texture2D particleTexture, Texture2D steamTexture, Vector2 particleEmitterLocation, Vector2 particleVelocity, Color particleColor, int playerNumber)
        {
            ParticleEmitterLocation = particleEmitterLocation;
            PlayerNumber = playerNumber;
            
            this.particleTexture = particleTexture;
            this.steamTexture = steamTexture;
            particleOrigin = new Vector2(particleTexture.Width / 2f, particleTexture.Height / 2f);

            particles = new List<Particle>();
        
            ParticleColor = particleColor;
            ParticleVelocity = particleVelocity;
            ThisWorld = world;
            
            
        }

        public Body GenerateNewParticle(int particleTTL)
        {
            
            Texture2D theParticleTexture = particleTexture;
            
            particle = BodyFactory.CreateCircle(ThisWorld, ConvertUnits.ToSimUnits(particleTexture.Width / 2), particleDensity, ParticleEmitterLocation);
            particle.BodyType = BodyType.Dynamic;
            particle.Restitution = .1f;
            particle.Friction = .5f;
            
            particle.OnCollision += particleCollided;

            if (particleTTL > 50)
            {
                if (PlayerNumber == 1)
                    particle.CollisionCategories = Category.Cat3;
                else if (PlayerNumber == 2)
                    particle.CollisionCategories = Category.Cat13;
                particle.CollidesWith = Category.Cat4 | Category.Cat1 | Category.Cat5 | Category.Cat2;
                particle.Restitution = .01f;
                particle.Friction = 15f;
                particleDensity = .01f;
                particlePower = .000015f;                
            }
            else
            {
                particle.CollisionCategories = Category.Cat2;
                particle.CollidesWith = Category.Cat1 | Category.Cat4 | Category.Cat13 | Category.Cat5 | Category.Cat3; 
                particleDensity = 1f;
                particlePower = .002f;
            }


            thatParticle = new Particle(particle, ParticleColor, drawScale, particleTTL, particleTexture);
            
            particles.Add(thatParticle);
         
            return particle;
        }

        public void Update(Boolean isFiring, Vector2 particleEmitterLocation, Vector2 particleVelocity)
        {

            ParticleEmitterLocation = particleEmitterLocation;
            ParticleVelocity = particleVelocity * particlePower;
            

            for (int i = 0; i < particles.Count; i++)
            {

                CurrentParticle = i;
                particles[CurrentParticle].Update();

                particles[CurrentParticle].ParticleTTL = particles[CurrentParticle].ParticleTTL - 1;

               

                if (particles[CurrentParticle].ParticleTTL > (particles[CurrentParticle].InitialTTL - 10))
                {
                    particles[CurrentParticle].ParticleBody.ApplyLinearImpulse(ParticleVelocity);
                   
                }
                if ((particles[CurrentParticle].ParticleTTL < 10) && (particles[CurrentParticle].ParticleTTL > 0))
                {
                    particles[CurrentParticle].ParticleColor = new Color(particles[CurrentParticle].ParticleColor, (.1f * CurrentParticle));
                    particles[CurrentParticle].ParticleBody.Mass = .001f;
                 
                }

                if (particles[CurrentParticle].ParticleTTL == 0)
                {
                   
                    particles[CurrentParticle].ParticleBody.OnCollision -= particleCollided;
                    particles[CurrentParticle].ParticleBody.Dispose();
                    particles.RemoveAt(CurrentParticle);
                    
                }

               
                
            }

            
            

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //    spriteBatch.Begin();
            //foreach (Body particle in particles)
            //{
            //    spriteBatch.Draw(particleTexture, ConvertUnits.ToDisplayUnits(particle.Position), null, ParticleColor, 0f, particleOrigin, 1f, SpriteEffects.None, 0f);
            //}

            foreach (Particle particle in particles)
            {
                spriteBatch.Draw(particle.CurrentTexture, ConvertUnits.ToDisplayUnits(particle.ParticleBody.Position), null, particle.ParticleColor, particle.RandomRotation, particleOrigin, particle.DrawScale, SpriteEffects.None, 0f);
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
            
            
            if (fixtureB.CollisionCategories == Category.Cat3)
            {
                fixtureB.IsSensor = true;
                fixtureB.Body.Awake = false;
                fixtureB.CollidesWith = Category.Cat2;
                
                return true;
            }
            if (fixtureB.CollisionCategories == Category.Cat13)
            {
                fixtureB.IsSensor = true;
                fixtureB.Body.Awake = false;
                fixtureB.CollidesWith = Category.Cat2;

                return true;
            }
            if (fixtureB.CollisionCategories == Category.Cat2)
            {
                particles[CurrentParticle].DrawScale = 5f;
                particles[CurrentParticle].CurrentTexture = steamTexture;
                particles[CurrentParticle].ParticleColor = new Color(Color.Gray, 1f);
                return true;
            }
            

            else
            {
                return true;
            }


        }

    }
}
