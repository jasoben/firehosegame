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
using Tao.Sdl; 

#endregion

namespace FireHose_DirectX_
{
    class Player
    {
        Body playerBody;
        Texture2D playerTexture;
        Vector2 playerOrigin;
        FireGun fireGun;

        Vector2 flyDirection;

        public int PlayerNumber;

        public Player(Vector2 playerStartPosition, World world, int playerNumber)
        {

            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);

            Vector2 playerPosition = ConvertUnits.ToSimUnits(playerStartPosition + new Vector2(0, 1.25f));
            
            playerBody = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(50f), ConvertUnits.ToSimUnits(50f), 2f, playerPosition);
            playerBody.BodyType = BodyType.Dynamic;

            playerBody.Restitution = .3f;
            playerBody.Friction = .5f;

            fireGun = new FireGun(this, playerPosition, world);

            PlayerNumber = playerNumber;
            

        }

        public void LoadContent(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("dude.png");
            playerOrigin = new Vector2(playerTexture.Width / 2f, playerTexture.Height / 2f);
            fireGun.LoadContent(content); 
        }

        public void Update(Controls controls, GameTime gameTime, List<Keys> playerControls)
        {
            Move(controls, playerControls);
            LimitVelocity();
            fireGun.Update(playerBody.Position, controls, playerControls);
            
           
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(playerTexture, ConvertUnits.ToDisplayUnits(playerBody.Position), null, Color.White, playerBody.Rotation, playerOrigin, .5f, SpriteEffects.None, 0f);
            fireGun.Draw(sb);
        }

        public void Move(Controls controls, List<Keys> playerControls)
        {


           

            //if (player.ContactList != null)
            //{
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
             
            //} else {
            if (controls.isHeld(playerControls[3], Buttons.B))
            {
                playerBody.ApplyForce(new Vector2(0, -20f));
            }

            if (controls.isThumbStick(Buttons.RightThumbstickDown) || controls.isThumbStick(Buttons.RightThumbstickUp) || controls.isThumbStick(Buttons.RightThumbstickLeft) || controls.isThumbStick(Buttons.RightThumbstickRight))
            {
                flyDirection = controls.Fly();
                flyDirection = new Vector2(-1 * flyDirection.X, flyDirection.Y);
                playerBody.ApplyForce(flyDirection);
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
    }
}
