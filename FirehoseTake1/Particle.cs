#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using FarseerPhysics.Common.PhysicsLogic;
using FarseerPhysics.Dynamics;

#endregion

namespace Firehose
{
    
    /// <summary>
    /// This class defines what the fire particle looks like and what it does
    /// </summary>
    public class Particle : PhysicsObject
    {
                

        #region old particle engine stuff
        //public Texture2D FireTexture { get; set; } 
        //public Vector2 FirePosition { get; set; } 
        //public Vector2 FireVelocity { get; set; }
        //public float FireAngle { get; set; } 
        //public float FireAngularVelocity { get; set; }
        //public Color FireColor { get; set; }
        //public float FireSize { get; set; }
        //public int FireTTL { get; set; } //TTL = Time To Live = how long the particle sticks around
        #endregion
        #region old constructor pre-Farseer
        /// <summary>
        /// This is the old constructor before I started using the Farseer Engine; keeping for posterity... and in case I bork this thing
        /// </summary>
        /// <param name="fireTexture"></param>
        /// <param name="firePosition"></param>
        /// <param name="fireVelocity"></param>
        /// <param name="fireAngle"></param>
        /// <param name="fireAngularVelocity"></param>
        /// <param name="fireColor"></param>
        /// <param name="fireSize"></param>
        /// <param name="fireTTL"></param> explained in field definition
        
        //public FireParticle(Texture2D fireTexture, Vector2 firePosition, Vector2 fireVelocity, float fireAngle, float fireAngularVelocity, Color fireColor, float fireSize, int fireTTL)
        //{
        //    FireTexture = fireTexture;
        //    FirePosition = firePosition;
        //    FireVelocity = fireVelocity;
        //    FireAngle = fireAngle;
        //    FireAngularVelocity = fireAngularVelocity;
        //    FireColor = fireColor;
        //    FireSize = fireSize;
        //    FireTTL = fireTTL;

        //}
        #endregion
        #region old update method
        /// <summary>
        /// The old Update method pre-Farseer
        /// </summary>
        //public void Update() 
        //{
        //    FireTTL--;
        //    FirePosition += FireVelocity;
        //    FireAngle += FireAngularVelocity;
        //}
        #endregion
        #region old Draw method
        /// <summary>
        /// pre-Farseer Draw Method
        /// </summary>
        /// <param name="spriteBatch"></param>
        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    Rectangle sourceRectangle = new Rectangle(0, 0, FireTexture.Width, FireTexture.Height);
        //    Vector2 origin = new Vector2(FireTexture.Width / 2, FireTexture.Height / 2);
 
        //    spriteBatch.Draw(FireTexture, FirePosition, sourceRectangle, FireColor, FireAngle, origin, FireSize, SpriteEffects.None, 0f);
        //}

        #endregion

        #region Farseer fields
        // Farseer Physics Engine fields

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _batch;

        private World _firehoseWorld;

        private Body _fireParticleBody;
        private Texture2D _fireSprite;

        #endregion

        #region new constructor using Farseer Physics Engine
        /// <summary>
        /// This is the new constructor using the Farseer Physics Engine
        /// </summary>
        public Particle()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            

            _firehoseWorld = new World(new Vector2(0, 9.82f));
            _fireSprite = 
        }
        #endregion

        #region new update method
        /// <summary>
        /// The Update method using Farseer code
        /// </summary>
        public void Update()
        {

        }
        #endregion

        #region the new Draw method
        /// <summary>
        /// The new Draw method using Farseer
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {

        }
        #endregion
    }

}
