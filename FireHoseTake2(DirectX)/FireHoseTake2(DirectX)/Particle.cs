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
    class Particle
    {

        Body particleBody;
        World world;

        public Texture2D ParticleTexture { get; set; }
        public Vector2 ParticlePosition { get; set; }
        
        //public float FireAngle { get; set; }
        //public float FireAngularVelocity { get; set; }
        public Color ParticleColor { get; set; }
        public float ParticleSize { get; set; }
        public int ParticleTTL { get; set; }

        public Particle(World world, Texture2D particleTexture, Vector2 particlePosition, Color particleColor, float particleSize, int particleTTL)
        {
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);

            ParticlePosition = particlePosition;
            ParticleTexture = particleTexture;
            ParticleColor = particleColor;
            ParticleSize = particleSize;
            ParticleTTL = particleTTL;
            this.world = world;

            ParticlePosition  = ConvertUnits.ToSimUnits(ParticlePosition + new Vector2(0, 1.25f));

            particleBody = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(2f), 20f, ParticlePosition);
            particleBody.BodyType = BodyType.Dynamic;

            particleBody.Restitution = .3f;
            particleBody.Friction = .5f;

            //particleBody.ApplyForce(new Vector2(100, 0));

            
        }

        public void Update()
        {
            ParticleTTL--;


        }

        
        public void Draw(SpriteBatch particleSB)
        {
            particleSB.Draw(ParticleTexture, ConvertUnits.ToDisplayUnits(ParticlePosition), null, Color.White); 
        }
    }
}
