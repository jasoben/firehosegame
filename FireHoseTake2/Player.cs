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

namespace FireHoseTake2
{
    class Player
    {
        Body playerBody;
        Texture2D playerTexture;
        Vector2 playerOrigin;
        WaterGun waterGun;

        public Player(Vector2 playerStartPosition, World world)
        {

           ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);

            Vector2 playerPosition = ConvertUnits.ToSimUnits(playerStartPosition + new Vector2(0, 1.25f));
            
            playerBody = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(20 / 2f), ConvertUnits.ToSimUnits(20 / 2f), 50f, playerPosition);
            playerBody.BodyType = BodyType.Dynamic;

            playerBody.Restitution = .3f;
            playerBody.Friction = .5f;

            waterGun = new WaterGun(this, playerPosition);
            

        }

        public void LoadContent(ContentManager content)
        {
            playerTexture = content.Load<Texture2D>("dude.png");
            playerOrigin = new Vector2(playerTexture.Width / 2f, playerTexture.Height / 2f);
            waterGun.LoadContent(content); 
        }

        public void Update(Controls controls, GameTime gameTime, List<Keys> playerControls)
        {
            Move(controls, playerControls);
            LimitVelocity();
            waterGun.Update(playerBody.Position, controls, playerControls);
           
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(playerTexture, ConvertUnits.ToDisplayUnits(playerBody.Position), null, Color.White, playerBody.Rotation, playerOrigin, 1f, SpriteEffects.None, 0f);
            waterGun.Draw(sb);
        }

        public void Move(Controls controls, List<Keys> playerControls)
        {

            //if (player.ContactList != null)
            //{
            if (controls.isHeld(playerControls[0], Buttons.LeftThumbstickRight))
                playerBody.ApplyLinearImpulse(new Vector2(-.2f, 0));

            if (controls.isHeld(playerControls[1], Buttons.LeftThumbstickLeft))
                playerBody.ApplyLinearImpulse(new Vector2(.2f, 0));

            if (controls.onPress(playerControls[2], Buttons.A))
                playerBody.ApplyLinearImpulse(new Vector2(0, -4));
            //} else {
            if (controls.isHeld(playerControls[3], Buttons.X))
                playerBody.ApplyForce(new Vector2(0, -20f));
            //}

        }

        public void LimitVelocity()
        {
            if (playerBody.LinearVelocity.X > 1)
            {
                playerBody.LinearVelocity = new Vector2(1, playerBody.LinearVelocity.Y);
            }
            if (playerBody.LinearVelocity.X < -1)
            {
                playerBody.LinearVelocity = new Vector2(-1, playerBody.LinearVelocity.Y);
            }
        }
    }
}
