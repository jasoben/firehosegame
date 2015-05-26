#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

#endregion

namespace Firehose
{
    public class FireParticle
    {

        public Texture2D FireTexture { get; set; }
        public Vector2 FirePosition { get; set; }
        public Vector2 FireVelocity { get; set; }
        public float FireAngle { get; set; }
        public float FireAngularVelocity { get; set; }
        public Color FireColor { get; set; }
        public float FireSize { get; set; }
        public int FireTTL { get; set; }

        public FireParticle(Texture2D fireTexture, Vector2 firePosition, Vector2 fireVelocity, float fireAngle, float fireAngularVelocity, Color fireColor, float fireSize, int fireTTL)
        {
            FireTexture = fireTexture;
            FirePosition = firePosition;
            FireVelocity = fireVelocity;
            FireAngle = fireAngle;
            FireAngularVelocity = fireAngularVelocity;
            FireColor = fireColor;
            FireSize = fireSize;
            FireTTL = fireTTL;
        }

        public void Update() 
        {
            FireTTL--;
            FirePosition += FireVelocity;
            FireAngle += FireAngularVelocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, FireTexture.Width, FireTexture.Height);
            Vector2 origin = new Vector2(FireTexture.Width / 2, FireTexture.Height / 2);
 
            spriteBatch.Draw(FireTexture, FirePosition, sourceRectangle, FireColor, FireAngle, origin, FireSize, SpriteEffects.None, 0f);
        }
    }

}
