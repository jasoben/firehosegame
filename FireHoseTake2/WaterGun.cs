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
    class WaterGun
    {

        //Player player;
        public Vector2 PlayerPosition;
        Texture2D waterGunTexture;
        WaterDrop waterDrop; 

        public WaterGun(Player player, Vector2 playerPosition)
        {
            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);
            PlayerPosition = playerPosition;
            
        }

        public void LoadContent(ContentManager content)
        {
            waterGunTexture = content.Load<Texture2D>("waterdrop.png");

        }

        public void Update(Vector2 playerPosition, Controls controls, List<Keys> playerControls)
        {
            PlayerPosition = playerPosition;
            Console.WriteLine(PlayerPosition);
            BlastWater(controls, playerControls);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(waterGunTexture, ConvertUnits.ToDisplayUnits(PlayerPosition), null, Color.White);
        }

        public void BlastWater(Controls controls, List<Keys> playerControls)
        {
            if (controls.isHeld(playerControls[4], Buttons.LeftThumbstickRight))
                waterDrop = new WaterDrop(PlayerPosition);
        }
    }
}
