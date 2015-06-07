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
        private float particlePower;

        public int CurrentParticle;
        public int PlayerNumber; 

        public ParticleEngine(World world, Texture2D particleTexture, Vector2 particleEmitterLocation, Vector2 particleVelocity, Color particleColor, int playerNumber)
        {
            ParticleEmitterLocation = particleEmitterLocation;
            //ParticleEmitterVelocity = particleVelocity;
            //PlayerLocation = playerLocation;
            PlayerNumber = playerNumber;
            
            this.particleTexture = particleTexture;
            particleOrigin = new Vector2(particleTexture.Width / 2f, particleTexture.Height / 2f);

            particles = new List<Body>();
            particlesTTL = new List<int>();
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

            

            particle = BodyFactory.CreateCircle(ThisWorld, ConvertUnits.ToSimUnits(particleTexture.Width / 2), particleDensity, ParticleEmitterLocation);
            particle.BodyType = BodyType.Dynamic;
            particle.Restitution = .1f;
            particle.Friction = .5f;

            particle.OnCollision += particleCollided;

            if (ParticleTTL > 50)
            {
                if (PlayerNumber == 1)
                    particle.CollisionCategories = Category.Cat3;
                else if (PlayerNumber == 2)
                    particle.CollisionCategories = Category.Cat13;
                particle.CollidesWith = Category.Cat4 | Category.Cat1 | Category.Cat2 | Category.Cat5;
                particle.Restitution = .01f;
                particle.Friction = 15f;
                particleDensity = .01f;
                particlePower = .000015f;                
            }
            else
            {
                particle.CollisionCategories = Category.Cat2;
                particle.CollidesWith = Category.Cat1 | Category.Cat4 | Category.Cat3 | Category.Cat13 | Category.Cat5;
                particleDensity = 1f;
                particlePower = .002f;
            }

            particleColors.Add(ParticleColor);
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

                CurrentParticle = i;

                
              
                particlesTTL[CurrentParticle] = particlesTTL[CurrentParticle] - 1;

                if (particlesTTL[CurrentParticle] > (ParticleTTL - 10))
                {
                    particles[CurrentParticle].ApplyLinearImpulse(ParticleVelocity); 
                }
                if (particlesTTL[CurrentParticle] < 10)
                {
                    particleDictionary[particles[CurrentParticle]] = new Color(ParticleColor, (.1f * CurrentParticle));
                    particles[CurrentParticle].Mass = .001f;
                }

                if (particlesTTL[CurrentParticle] < 0)
                {
                    particles[CurrentParticle].OnCollision -= particleCollided;
                    particles[CurrentParticle].Dispose();
                    particleDictionary.Remove(particles[CurrentParticle]);
                    particles.RemoveAt(CurrentParticle);
                    particlesTTL.RemoveAt(CurrentParticle);
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

            if (fixtureA.CollisionCategories == Category.Cat3)
            {
                fixtureA.Body.Awake = false;
                particlesTTL[CurrentParticle] = 0;
                return true;
            }
            if (fixtureB.CollisionCategories == Category.Cat5)
            {
                particlesTTL[CurrentParticle] = 0;
                return true;
            }
            else
            {
                return true;
            }


        }

    }
}
