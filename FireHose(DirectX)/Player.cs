﻿#region Using Statements
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
using Tao.Sdl; 

#endregion

namespace FireHose_DirectX_
{
    class Player
    {
        public Body playerBody;
        Texture2D playerTexture;
        Texture2D damageTexture;
        Vector2 playerOrigin;
        Gun fireGun;
        Gun waterGun;

        public int PlayerHealth;

        Vector2 flyDirection;
        public Vector2 PlayerPosition;
        public Vector2 PlayerStartPosition; 

        public int PlayerNumber;

        public World World;

        public Color PlayerColor;

        private int DeltaColor;

        private Vector2 centorOfMass;

        public bool isGrounded = true;

        private int totalHealth = 100;

        public Player(Vector2 playerStartPosition, World world, int playerNumber)
        {

            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            PlayerStartPosition = playerStartPosition;
            World = world;
            PlayerNumber = playerNumber;
            PlayerColor = Color.White;

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
            playerOrigin = new Vector2(playerTexture.Width / 2f, playerTexture.Height / 2f);
            fireGun.LoadContent(content);
            waterGun.LoadContent(content);
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
                PlayerColor = Color.White;

            if (PlayerHealth < 0)
            {
                PlayerHealth = totalHealth;
                Restart();
            }

         
                        
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(playerTexture, ConvertUnits.ToDisplayUnits(playerBody.Position), null, PlayerColor, playerBody.Rotation, playerOrigin, .5f, SpriteEffects.None, 0f);
            sb.Draw(damageTexture, ConvertUnits.ToDisplayUnits(playerBody.Position), null, Color.Red, playerBody.Rotation, playerOrigin, .5f, SpriteEffects.None, 0f);
            fireGun.Draw(sb);
            waterGun.Draw(sb);
        }

        public void Move(Controls controls, List<Keys> playerControls)
        {


           

            //if (player.ContactList != null)
            //{

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

            
             
            //} else {
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

            if (!controls.isHeld(Keys.U, Buttons.LeftShoulder))
            {
                if (controls.isThumbStick(Buttons.LeftThumbstickDown) || controls.isThumbStick(Buttons.LeftThumbstickUp) || controls.isThumbStick(Buttons.LeftThumbstickLeft) || controls.isThumbStick(Buttons.LeftThumbstickRight))
                {
                    flyDirection = controls.Fly(false);
                    flyDirection = new Vector2(-1* flyDirection.X, flyDirection.Y);
                    playerBody.ApplyForce(flyDirection);
                }
            }
           
            //}

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

            playerBody.Dispose();
            CreatePlayer();
            
        }

        public void CreatePlayer()
        {
            Vector2 playerPosition = ConvertUnits.ToSimUnits(PlayerStartPosition + new Vector2(0, 1.25f));

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
            
            if (PlayerPosition.Y > 1100)
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
                return true;
            }
            if (fixtureB.CollisionCategories == Category.Cat4)
            {
                playerBody.SetTransform(playerBody.Position, 0f);
                isGrounded = true;
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


    }
}
