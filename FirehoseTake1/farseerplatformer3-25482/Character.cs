using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace FarseerPlatformer
{

    public class Character : PhysicsObject
    {
        public float forcePower;
        protected KeyboardState keyState;
        protected KeyboardState oldState;


        public Character(World world, Vector2 position, float width, float height, float mass, Texture2D texture)
            : base(world, position, width, height, mass, texture)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
        }

        protected virtual void HandleInput(GameTime gameTime)
        {
            keyState = Keyboard.GetState();

            //Apply force in the arrow key direction
            Vector2 force = Vector2.Zero;
            if (keyState.IsKeyDown(Keys.Left))
            {
                force.X -= forcePower * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                force.X += forcePower * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyState.IsKeyDown(Keys.Up))
            {
                force.Y -= forcePower * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                force.Y += forcePower * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            body.ApplyLinearImpulse(force, body.Position);

            oldState = keyState;
        }
    }
}