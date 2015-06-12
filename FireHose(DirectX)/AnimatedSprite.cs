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
    class AnimatedSprite
    {
        public Texture2D Texture;
        public int Rows;
        public int Columns;
        private int currentFrame;
        private int totalFrames;
        Color PlayerColor;
        Vector2 PlayerOrigin;

        private int frameDelay = 3;

        public AnimatedSprite (Texture2D texture, int rows, int columns, Color playerColor)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            PlayerColor = playerColor;
        }

        public void Update()
        {
            frameDelay--;
            if (frameDelay < 0)
            {
                currentFrame++;
                Console.WriteLine(currentFrame);
                frameDelay = 3;
            }
            
            if (currentFrame == totalFrames)
                currentFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color playerColor, Vector2 playerOrigin, float playerRotation)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);
            PlayerOrigin = playerOrigin / 2;
            PlayerColor = playerColor;
           
            spriteBatch.Draw(Texture, null, destinationRectangle, sourceRectangle, PlayerOrigin, playerRotation, new Vector2(1f, 1f), PlayerColor, SpriteEffects.None, 0f);
           
        }
    }
}
