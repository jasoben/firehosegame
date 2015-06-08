#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
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
    class Altar
    {
        public Body altar;
        private Vector2 SimPosition;
        private Vector2 DrawPosition;
        private Vector2 DrawOrigin; 

        private Texture2D altarTexture;

        public float AltarAmount;
        public int DrenchedAmount;
        public bool AltarIsLit = false;

        SoundEffect lightingFire;
        SoundItem lightingFireSound;

        SoundEffect capture;
        SoundItem captureSound;

        public float DrawScale;

        public int PlayerNumber;

        public bool playSound = true;

        public Altar(World world, Vector2 position)
        {
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            SimPosition = new Vector2(ConvertUnits.ToSimUnits(position.X), ConvertUnits.ToSimUnits(position.Y));
            DrawPosition = position;
            
            altar = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(5f), ConvertUnits.ToSimUnits(100f), 1f, SimPosition);
            altar.IsStatic = true;
            altar.CollisionCategories = Category.Cat5;
            altar.CollidesWith = Category.Cat2 | Category.Cat3 | Category.Cat13;
            altar.OnCollision += altarCollision;

            AltarAmount = 0;
            DrenchedAmount = 0;
            DrawScale = AltarAmount / 100500;
        }
    
        public void LoadContent(ContentManager content)
        {
            altarTexture = content.Load<Texture2D>("altar.png");
            DrawOrigin = new Vector2(altarTexture.Width / 2, altarTexture.Height / 2);

            lightingFire = content.Load<SoundEffect>("lighting-fire-sound");
            lightingFireSound = new SoundItem(lightingFire);

            capture = content.Load<SoundEffect>("capture-noise");
            captureSound = new SoundItem(capture);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(altarTexture, DrawPosition, null, Color.Brown, 0f, DrawOrigin, 1f, SpriteEffects.None, 1f);
            sb.Draw(altarTexture, DrawPosition, null, Color.Orange, 0f, DrawOrigin, DrawScale, SpriteEffects.None, 1f);
            if (AltarIsLit)
            {
                if (PlayerNumber == 1)
                {
                    sb.Draw(altarTexture, DrawPosition + new Vector2(0f, -60f), null, Color.CornflowerBlue, 0f, DrawOrigin, .3f, SpriteEffects.None, 1f);
                } else if (PlayerNumber == 2)
                {
                    sb.Draw(altarTexture, DrawPosition + new Vector2(0f, -60f), null, Color.GreenYellow, 0f, DrawOrigin, .3f, SpriteEffects.None, 1f);
                }
            }

        }

        public void Update()
        {
            
            LightAltar(-400);
            DrenchedAmount -= 400;

            DrawScale = AltarAmount / 100500;
        }

        public bool altarCollision (Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == Category.Cat3)
            {
                if (!AltarIsLit)
                {
                    PlayerNumber = 1;
                    LightAltar(2000f);
                    lightingFireSound.PlaySound();
                }

              //  Console.WriteLine("testing");
                return true;
            }
            if (fixtureB.CollisionCategories == Category.Cat13)
            {
                if (!AltarIsLit)
                {
                    PlayerNumber = 2;
                    LightAltar(2000f);
                    lightingFireSound.PlaySound();
                }

                //  Console.WriteLine("testing");
                return true;
            }
            if (fixtureB.CollisionCategories == Category.Cat2)
            {
                DrenchedAmount += 2000;
                if (DrenchedAmount > 50000)
                {
                    LightAltar(-5000);
                }
                return true;
            }
            else
            {
                return true;
            }
        }

        public float LightAltar(float amount)
        {
            AltarAmount += amount;
            if (AltarAmount > 100000)
            {
                AltarIsLit = true;
                AltarAmount = 100500;
                lightingFireSound.StopSound();
                PlaySound(playSound);
            }
                        
            if (AltarAmount < 0)
            {
                AltarAmount = 0;
                DrenchedAmount = 0;
                AltarIsLit = false;
                playSound = true;
                lightingFireSound.StopSound();
            }
            if (DrenchedAmount < 0)
                DrenchedAmount = 500;

            return AltarAmount;

               
        }

        public void PlaySound(bool playTheSound)
        {
            if (playTheSound == true)
            {
                playSound = false;
                captureSound.PlaySingleSound();
            }
        }
        
    }
}
