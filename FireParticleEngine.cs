#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Tao.Sdl;
#endregion

namespace Firehose
{
    public class FireParticleEngine
    {
        private Random fireRandom;
        public Vector2 FireEmitterLocation { get; set; }
        public Vector2 PlayerLocation { get; set; }
        private List<FireParticle> fireParticles;
        private List<Texture2D> fireTextures;
        

        public FireParticleEngine(List<Texture2D> fireTextures, Vector2 fireLocation, Vector2 playerLocation)
        {
            FireEmitterLocation = fireLocation;
            PlayerLocation = playerLocation;
            this.fireTextures = fireTextures;
            this.fireParticles = new List<FireParticle>();
            fireRandom = new Random();
        }

        private FireParticle GenerateNewFireParticle()
        {
            Texture2D fireTexture = fireTextures[fireRandom.Next(fireTextures.Count)];
            Vector2 firePosition = FireEmitterLocation;
            Vector2 fireVelocity = (FireEmitterLocation - PlayerLocation) / 5;
            float fireAngle = 0;
            float fireAngularVelocity = 0.1f * (float)(fireRandom.NextDouble() * 2 - 1);
            Color fireColor = new Color(255, 255, 255);
                    //(float)fireRandom.NextDouble(),
                    //(float)fireRandom.NextDouble(),
                    //(float)fireRandom.NextDouble());
            float fireSize = (float)fireRandom.NextDouble();
            int fireTTL = 20 + fireRandom.Next(40);

            return new FireParticle(fireTexture, firePosition, fireVelocity, fireAngle, fireAngularVelocity, fireColor, fireSize, fireTTL);
        }

        public void Update(Boolean isFiringFlame)
        {
            int fireTotal = 30;

            if (isFiringFlame == true)
            {
                for (int i = 0; i < fireTotal; i++)
                {
                    fireParticles.Add(GenerateNewFireParticle());
                }
            }
            
            for (int fireParticle = 0; fireParticle < fireParticles.Count; fireParticle++)
            {
                fireParticles[fireParticle].Update();
                if (fireParticles[fireParticle].FireTTL <= 0)
                {
                    fireParticles.RemoveAt(fireParticle);
                    fireParticle--;
                }
            }

            
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        //    spriteBatch.Begin();
            for (int index = 0; index < fireParticles.Count; index++)
            {
                fireParticles[index].Draw(spriteBatch);
            }
        //    spriteBatch.End();
        }
    }


}
