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
    public class WaterParticleEngine
    {
        private Random waterRandom;
        public Vector2 WaterEmitterLocation { get; set; }
        public Vector2 PlayerLocation { get; set; }
        private List<Particle> waterParticles;
        private List<Texture2D> waterTextures;


        public WaterParticleEngine(List<Texture2D> waterTextures, Vector2 waterLocation, Vector2 playerLocation)
        {
            WaterEmitterLocation = waterLocation;
            PlayerLocation = playerLocation;
            this.waterTextures = waterTextures;
            this.waterParticles = new List<Particle>();
            waterRandom = new Random();
        }

        private Particle GenerateNewwaterParticle()
        {
            Texture2D waterTexture = waterTextures[waterRandom.Next(waterTextures.Count)];
            Vector2 waterPosition = WaterEmitterLocation;
            Vector2 waterVelocity = (WaterEmitterLocation - PlayerLocation) / 5;
            float waterAngle = 0;
            float waterAngularVelocity = 0.1f * (float)(waterRandom.NextDouble() * 2 - 1);
            Color waterColor = new Color(255, 255, 255);
            //(float)waterRandom.NextDouble(),
            //(float)waterRandom.NextDouble(),
            //(float)waterRandom.NextDouble());
            float waterSize = (float)waterRandom.NextDouble();
            int waterTTL = 20 + waterRandom.Next(40);

            return new Particle(waterTexture, waterPosition, waterVelocity, waterAngle, waterAngularVelocity, waterColor, waterSize, waterTTL);
        }

        public void Update(Boolean isFiringFlame)
        {
            int waterTotal = 30;

            if (isFiringFlame == true)
            {
                for (int i = 0; i < waterTotal; i++)
                {
                    waterParticles.Add(GenerateNewwaterParticle());
                }
            }

            for (int waterParticle = 0; waterParticle < waterParticles.Count; waterParticle++)
            {
                waterParticles[waterParticle].Update();
                if (waterParticles[waterParticle].waterTTL <= 0)
                {
                    waterParticles.RemoveAt(waterParticle);
                    waterParticle--;
                }
            }



        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //    spriteBatch.Begin();
            for (int index = 0; index < waterParticles.Count; index++)
            {
                waterParticles[index].Draw(spriteBatch);
            }
            //    spriteBatch.End();
        }
    }


}
