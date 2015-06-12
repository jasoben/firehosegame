#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics.Contacts;
using Tao.Sdl; 

#endregion

namespace FireHose_DirectX_
{
    class Player
    {
        public Body playerBody;
        Texture2D playerTexture;
        Texture2D playerDeadTexture;
        Texture2D damageTexture;
        Texture2D burnDamageTexture;
        Texture2D bumpDamageTexture;

        public Texture2D CurrentPlayerTexture;

        Vector2 playerOrigin;
        Gun fireGun;
        Gun waterGun;

        public int PlayerHealth;
        public Vector2 HealthScale;

        SoundEffect water;
        SoundItem waterSound;

        SoundEffect fire;
        SoundItem fireSound;

        SoundEffect ouch;
        SoundItem ouchSound;

        SoundEffect death;
        SoundItem deathSound;

        Vector2 flyDirection;
        public Vector2 PlayerPosition;
        public Vector2 PlayerStartPosition; 

        public int PlayerNumber;
        int BumpCount = 30;

        public World World;

        public Color PlayerColor;
        public Color OriginalPlayerColor;
        public Color DamageColor;
        public Color BurnDamageColor;

        private int DeltaColor;

        public int PlayerScore = 0;

        private Vector2 centorOfMass;

        public bool isGrounded = true;
        public bool BumpDamaged = false;

        private int deathTimer = 90;

        private int totalHealth = 100;

        public float WinPercent
        {
            get { return (float)PlayerScore / 1000000f; }
        }

        public int RestartTimer = 120;
        public int TotalRestartTime = 120;

        public Player(Vector2 playerStartPosition, World world, int playerNumber)
        {

            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            PlayerStartPosition = playerStartPosition;
            World = world;
            PlayerNumber = playerNumber;
            
            if (PlayerNumber == 1)
                PlayerColor = OriginalPlayerColor = Color.CornflowerBlue;
            if (PlayerNumber == 2)
                PlayerColor = OriginalPlayerColor = Color.GreenYellow;

            PlayerPosition = ConvertUnits.ToSimUnits(playerStartPosition + new Vector2(0, 1.25f));
            centorOfMass = new Vector2(ConvertUnits.ToSimUnits(0f), ConvertUnits.ToSimUnits(10f));

            CreatePlayer();

            fireGun = new Gun(this, PlayerPosition, world, true, playerNumber);
            waterGun = new Gun(this, PlayerPosition, world, false, playerNumber);

            
        }

        public void LoadContent(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("dude.png");
            damageTexture = content.Load<Texture2D>("dudedamage.png");
            burnDamageTexture = content.Load<Texture2D>("BurnDamage");
            playerDeadTexture = content.Load<Texture2D>("deadDude.png");
            bumpDamageTexture = content.Load<Texture2D>("BumpDamage");
            playerOrigin = new Vector2(playerTexture.Width / 2f, playerTexture.Height / 2f);
            fireGun.LoadContent(content);
            waterGun.LoadContent(content);

            water = content.Load<SoundEffect>("water-sound");
            fire = content.Load<SoundEffect>("fire-sound");
            ouch = content.Load<SoundEffect>("ouch-noise");
            death = content.Load<SoundEffect>("death-noise");

            CurrentPlayerTexture = playerTexture;

            waterSound = new SoundItem(water);
            fireSound = new SoundItem(fire);
            ouchSound = new SoundItem(ouch);
            deathSound = new SoundItem(death);
            
        }

        public void Update(Controls controls, GameTime gameTime, List<Keys> playerControls)
        {
            Move(controls, playerControls);
            LimitVelocity();
            fireGun.Update(playerBody.Position, controls, playerControls);
            waterGun.Update(playerBody.Position, controls, playerControls);
            PlayerPosition = ConvertUnits.ToDisplayUnits(playerBody.Position);
            CheckBounds();
            
            //Dummy method for pushing info to text box
            //Location = fireGun.GetLocation();
            

            DeltaColor--;
            if (DeltaColor < 0)
                PlayerColor = OriginalPlayerColor;

            if (PlayerHealth < 0)
            {
                DeadPlayer();
                
            }

            if (PlayerHealth < 80 && PlayerHealth >= 0)
            {
                
                HealthScale = new Vector2(.5f, (.5f * (1f - ((float)PlayerHealth / (float)totalHealth))));
            }
            else
            {
                DamageColor = new Color(Color.Black, 0f);
                BurnDamageColor = new Color(Color.Black, 0f);
            }

            if (BumpDamaged == true)
            {
                BumpCount--;
                Console.WriteLine(BumpCount);
                if (BumpCount < 0)
                {
                    BumpDamaged = false;
                    BumpCount = 30;
                }
            }
            
                        
        }

        public void Draw(SpriteBatch sb)
        {
            
            sb.Draw(CurrentPlayerTexture, ConvertUnits.ToDisplayUnits(playerBody.Position), null, PlayerColor, playerBody.Rotation, playerOrigin, .5f, SpriteEffects.None, 0f);
            sb.Draw(damageTexture,  ConvertUnits.ToDisplayUnits(playerBody.Position), null, DamageColor, playerBody.Rotation, playerOrigin, .5f, SpriteEffects.None, 1f);
            sb.Draw(burnDamageTexture, ConvertUnits.ToDisplayUnits(playerBody.Position), null, BurnDamageColor, playerBody.Rotation, playerOrigin, .5f, SpriteEffects.None, 1f);
            waterGun.Draw(sb);
            fireGun.Draw(sb);
            
            if (BumpDamaged == true)
            {
                sb.Draw(bumpDamageTexture, ConvertUnits.ToDisplayUnits(playerBody.Position), null, Color.White, playerBody.Rotation, playerOrigin, 1f, SpriteEffects.None, 1f);
            }
        }

        public void Move(Controls controls, List<Keys> playerControls)
        {

            if (playerBody.BodyType == BodyType.Dynamic)
            {

               

                if (controls.isHeld(Keys.U, Buttons.LeftShoulder) && (isGrounded == true))
                {
                    if (controls.isHeld(playerControls[0], Buttons.LeftThumbstickLeft))
                    {
                        playerBody.ApplyLinearImpulse(new Vector2(-.2f, 0));
                    }


                    if (controls.isHeld(playerControls[1], Buttons.LeftThumbstickRight))
                    {
                        playerBody.ApplyLinearImpulse(new Vector2(.2f, 0));
                    }

                    if (controls.onPress(playerControls[2], Buttons.A))
                    {
                        playerBody.ApplyLinearImpulse(new Vector2(0, -4));
                    }
                }



               
                if (controls.isHeld(playerControls[3], Buttons.B))
                {
                    playerBody.ApplyForce(new Vector2(0, -20f));
                }

                if (controls.isThumbStick(Buttons.RightThumbstickDown) || controls.isThumbStick(Buttons.RightThumbstickUp) || controls.isThumbStick(Buttons.RightThumbstickLeft) || controls.isThumbStick(Buttons.RightThumbstickRight))
                {
                    flyDirection = controls.Fly(true);
                    flyDirection = new Vector2(-1 * flyDirection.X, flyDirection.Y);
                    playerBody.ApplyForce(flyDirection);

                }
                if (controls.isPressed(Keys.A, Buttons.RightThumbstickDown) || controls.isPressed(Keys.A, Buttons.RightThumbstickLeft) || controls.isPressed(Keys.A, Buttons.RightThumbstickUp) || controls.isPressed(Keys.A, Buttons.RightThumbstickRight))
                {

                    fireSound.PlaySound();
                }
                if (controls.onRelease(Keys.A, Buttons.RightThumbstickDown) || controls.onRelease(Keys.A, Buttons.RightThumbstickLeft) || controls.onRelease(Keys.A, Buttons.RightThumbstickUp) || controls.onRelease(Keys.A, Buttons.RightThumbstickRight))
                {

                    fireSound.StopSound();
                }

                if (!controls.isHeld(Keys.U, Buttons.LeftShoulder))
                {
                    if (controls.isThumbStick(Buttons.LeftThumbstickDown) || controls.isThumbStick(Buttons.LeftThumbstickUp) || controls.isThumbStick(Buttons.LeftThumbstickLeft) || controls.isThumbStick(Buttons.LeftThumbstickRight))
                    {
                        flyDirection = controls.Fly(false);
                        flyDirection = new Vector2(-1 * flyDirection.X, flyDirection.Y);
                        playerBody.ApplyForce(flyDirection);

                    }
                    if (controls.isPressed(Keys.A, Buttons.LeftThumbstickDown) || controls.isPressed(Keys.A, Buttons.LeftThumbstickLeft) || controls.isPressed(Keys.A, Buttons.LeftThumbstickUp) || controls.isPressed(Keys.A, Buttons.LeftThumbstickRight))
                    {
                        waterSound.SoundVolume = .3f;
                        waterSound.PlaySound();
                    }
                    if (controls.onRelease(Keys.A, Buttons.LeftThumbstickDown) || controls.onRelease(Keys.A, Buttons.LeftThumbstickLeft) || controls.onRelease(Keys.A, Buttons.LeftThumbstickUp) || controls.onRelease(Keys.A, Buttons.LeftThumbstickRight))
                    {
                        waterSound.SoundVolume = .3f;
                        waterSound.StopSound();
                    }
                }
            }           
           

        }

        
        public void LimitVelocity()
        {
            //if (playerBody.LinearVelocity.X > 1)
            //{
            //    playerBody.LinearVelocity = new Vector2(1, playerBody.LinearVelocity.Y);
            //}
            //if (playerBody.LinearVelocity.X < -1)
            //{
            //    playerBody.LinearVelocity = new Vector2(-1, playerBody.LinearVelocity.Y);
            //}
        }

        public void Restart()
        {

            if (RestartTimer > (TotalRestartTime -2))
                deathSound.PlaySingleSound();

            RestartTimer -= 1;
            if (RestartTimer < 0)
            {
                
                playerBody.Dispose();
                CreatePlayer();
                RestartTimer = TotalRestartTime;
                
            }
            
           
            
        }

        public void CreatePlayer()
        {
            Vector2 playerPosition = ConvertUnits.ToSimUnits(PlayerStartPosition + new Vector2(0, 1.25f));

            CurrentPlayerTexture = playerTexture;

            playerBody = BodyFactory.CreateRectangle(World, ConvertUnits.ToSimUnits(50f), ConvertUnits.ToSimUnits(50f), 2f, playerPosition);
            playerBody.BodyType = BodyType.Dynamic;
            playerBody.AngularDamping = 4f;

            playerBody.Restitution = .3f;
            playerBody.Friction = .5f;
            playerBody.LocalCenter = centorOfMass;

            playerBody.OnCollision += playerCollided;
            playerBody.OnSeparation += playerSeparated;

            PlayerHealth = totalHealth;
        }

        public void CheckBounds()
        {
            if (PlayerPosition.X < -0)
            {
                playerBody.SetTransform(new Vector2(ConvertUnits.ToSimUnits(1700f), playerBody.Position.Y), 0f);
            }
            
            if (PlayerPosition.X > 1800)
            {
                playerBody.SetTransform(new Vector2(ConvertUnits.ToSimUnits(100f), playerBody.Position.Y), 0f);
            }
            
            if (PlayerPosition.Y > 1200)
            {
                Restart();
            }
        }

        public bool playerCollided(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (fixtureB.CollisionCategories == Category.Cat3)
            {
                DeltaColor = 20;
                PlayerHealth--;
                PlayerColor = Color.Orange;
                JointFactory.CreateWeldJoint(World, fixtureA.Body, fixtureB.Body, new Vector2(contact.Manifold.LocalPoint.X, contact.Manifold.LocalPoint.Y), Vector2.Zero);
                BurnDamageColor = new Color(Color.Black, (1f - (float)PlayerHealth / (float)totalHealth));
                ouchSound.PlaySingleSound();
                return true;
            }

            if (fixtureB.CollisionCategories == Category.Cat4)
            {
                
                playerBody.SetTransform(playerBody.Position, 0f);
                isGrounded = true;

                if (Math.Abs(playerBody.LinearVelocity.Y) > 10f)
                {
                    DeltaColor = 20;
                    PlayerHealth -= (int)Math.Abs(playerBody.LinearVelocity.Y);
                    PlayerColor = Color.Red;
                    ouchSound.PlaySingleSound();
                    BumpDamage(); 
                }

                if (Math.Abs(playerBody.LinearVelocity.X) > 10f)
                {
                    DeltaColor = 20;
                    PlayerHealth -= (int)Math.Abs(playerBody.LinearVelocity.X);
                    PlayerColor = Color.Red;
                    ouchSound.PlaySingleSound();
                    BumpDamage(); 
                }
                return true;
            }

            else
            {
                PlayerColor = Color.White;
                return true;
            }

        }

        public void playerSeparated(Fixture fixtureA, Fixture fixtureB)
        {
            
            if (fixtureB.CollisionCategories == Category.Cat4)
            {
                isGrounded = false;
            }
            

        }

        public void BumpDamage()
        {
            BumpDamaged = true;
            DamageColor = new Color(Color.White, (1f - ((float)PlayerHealth / (float)totalHealth)));
        }
        
        public void DeadPlayer()
        {
            fireSound.StopSound();
            waterSound.StopSound();
            DamageColor = new Color(Color.Black, 0f);
            BurnDamageColor = new Color(Color.Black, 0f);
            CurrentPlayerTexture = playerDeadTexture;
            deathTimer -= 1;
            playerBody.BodyType = BodyType.Kinematic;
            if (deathTimer > 85)
            {
                playerBody.LinearVelocity = new Vector2(0, 0);
                playerBody.AngularVelocity = 0f;
            }
            playerBody.LinearVelocity = new Vector2(0, 4f);
            playerBody.AngularVelocity = 1f;
            
            if (deathTimer < 0)
            {
                //playerBody.SetTransform(new Vector2(3000f, 3000f), 0f);
                deathTimer = 90;
            }
            Restart();
        }
    }
}
