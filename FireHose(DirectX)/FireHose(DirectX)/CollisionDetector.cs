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

namespace FireHose_DirectX_
{
    class CollisionDetector
    {
        
        public bool Collided = false; 

        public CollisionDetector()
        {
            
        }

        public bool CheckCollided(List<Rectangle> rectangles, Rectangle rectangle2)
        {
            if (rectangles != null)
            {
                foreach (Rectangle rectangle in rectangles)
                {
                    //Console.WriteLine(rectangle.X);
                    if (rectangle.Intersects(rectangle2))
                        return Collided = true;

                    else
                        Collided = false;
                }
            }
            return Collided;
            
        }
    }
}
