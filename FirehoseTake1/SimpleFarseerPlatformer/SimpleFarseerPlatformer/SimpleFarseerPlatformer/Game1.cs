using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;

namespace SimpleFarseerPlatformer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private enum CharState
        {
            Idle,
            Jumping,
            Running
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        World world;
        Fixture platform1;
        Fixture platform2;
        Fixture platform3;
        Fixture platform4;
        Fixture platform5;
        Fixture character;
        Texture2D platformTex;
        Texture2D charTex;
        Fixture ground;
        Fixture leftWall;
        Fixture rightWall;
        Fixture ceiling;
        CharState state;
        Vector2 jumpImpulse = new Vector2(0, -50);
        float launchSpeed;

        float runSpeed = 10f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            world = new World(Vector2.UnitY * 45f);
            ConvertUnits.SetDisplayUnitToSimUnitRatio(50);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

            state = CharState.Idle;
            
        }

        private int RoundToInt(float inFloat)
        {
            int retInt = (int)inFloat;
            if (inFloat - retInt >= 0.5f)
            {
                retInt++;
            }
            return retInt;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            character = FixtureFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(48), ConvertUnits.ToSimUnits(64), 1.75f, ConvertUnits.ToSimUnits(new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2)));
            character.Body.BodyType = BodyType.Dynamic;
            character.Friction = 0f;
            character.Restitution = 0.00f;

            Joint rotationJoint = JointFactory.CreateFixedAngleJoint(world, character.Body);

            character.OnCollision += new OnCollisionEventHandler(OnCollision);
            
           
            ground = FixtureFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width), ConvertUnits.ToSimUnits(20), 1, ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height - 10));
            ground.Body.BodyType = BodyType.Static;
            ground.Body.IsStatic = true;
            ground.Friction = 0f;
            ground.Restitution = 0.0f;

            leftWall = FixtureFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(20), ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Height - 40), 1, ConvertUnits.ToSimUnits(10, graphics.GraphicsDevice.Viewport.Height / 2));
            leftWall.Body.BodyType = BodyType.Static;
            leftWall.Body.IsStatic = true;
            leftWall.Friction = 0f;
            leftWall.Restitution = 0f;

            rightWall = FixtureFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(20), ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Height - 40), 1, ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width - 10, graphics.GraphicsDevice.Viewport.Height / 2));
            rightWall.Body.BodyType = BodyType.Static;
            rightWall.Body.IsStatic = true;
            rightWall.Friction = 0f;
            rightWall.Restitution = 0f;

            ceiling = FixtureFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width), ConvertUnits.ToSimUnits(20), 1, ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width / 2, 10));
            ceiling.Body.BodyType = BodyType.Static;
            ceiling.Body.IsStatic = true;
            ceiling.Friction = 0f;
            ceiling.Restitution = 0f;

            platform1 = FixtureFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width / 4), ConvertUnits.ToSimUnits(20), 1, ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2));
            platform1.Body.BodyType = BodyType.Static;
            platform1.Body.IsStatic = true;
            platform1.Friction = 0f;
            platform1.Restitution = 0f;

            platform2 = FixtureFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width / 4), ConvertUnits.ToSimUnits(20), 1, ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width / 4, (3 * graphics.GraphicsDevice.Viewport.Height) / 4));
            platform2.Body.BodyType = BodyType.Static;
            platform2.Body.IsStatic = true;
            platform2.Friction = 0f;
            platform2.Restitution = 0f;

            platform3 = FixtureFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width / 4), ConvertUnits.ToSimUnits(20), 1, ConvertUnits.ToSimUnits((3 * graphics.GraphicsDevice.Viewport.Width) / 4, (3 * graphics.GraphicsDevice.Viewport.Height) / 4));
            platform3.Body.BodyType = BodyType.Static;
            platform3.Body.IsStatic = true;
            platform3.Friction = 0f;
            platform3.Restitution = 0f;

            platform4 = FixtureFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width / 4), ConvertUnits.ToSimUnits(20), 1, ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width / 4, graphics.GraphicsDevice.Viewport.Height / 4));
            platform4.Body.BodyType = BodyType.Static;
            platform4.Body.IsStatic = true;
            platform4.Friction = 0f;
            platform4.Restitution = 0f;

            platform5 = FixtureFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(graphics.GraphicsDevice.Viewport.Width / 4), ConvertUnits.ToSimUnits(20), 1, ConvertUnits.ToSimUnits((3 * graphics.GraphicsDevice.Viewport.Width) / 4, graphics.GraphicsDevice.Viewport.Height / 4));
            platform5.Body.BodyType = BodyType.Static;
            platform5.Body.IsStatic = true;
            platform5.Friction = 0f;
            platform5.Restitution = 0f;


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            platformTex = Content.Load<Texture2D>("platformTex");
            charTex = Content.Load<Texture2D>("breakableBlock");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            if (state != CharState.Jumping)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                {
                    Jump();
                }
                else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.1f)
                {
                    RunRight();
                }
                else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -0.1f)
                {
                    RunLeft();
                }
                else
                {
                    Stop();
                    state = CharState.Idle;
                }
            }
            else
            {
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0.1f)
                {
                    AirMoveRight();
                }
                else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -0.1f)
                {
                    AirMoveLeft();
                }
                else
                {
                    Stop();
                }
            }

            // TODO: Add your update logic here
            world.Step(gameTime.ElapsedGameTime.Milliseconds * 0.001f);
            base.Update(gameTime);
        }

        private void Stop()
        {
            character.Body.LinearVelocity = new Vector2(0, character.Body.LinearVelocity.Y);
        }

        private void Jump()
        {
            launchSpeed = character.Body.LinearVelocity.X;
            character.Body.ApplyLinearImpulse(jumpImpulse, character.Body.Position);
            state = CharState.Jumping;
        }

        private void RunRight()
        {
            character.Body.LinearVelocity = new Vector2(runSpeed, character.Body.LinearVelocity.Y);
            state = CharState.Running;
        }

        private void RunLeft()
        {
            character.Body.LinearVelocity = new Vector2(-runSpeed, character.Body.LinearVelocity.Y);
            state = CharState.Running;
        }

        private void AirMoveRight()
        {
            character.Body.LinearVelocity = new Vector2(Math.Abs(launchSpeed), character.Body.LinearVelocity.Y);
        }

        private void AirMoveLeft()
        {
            character.Body.LinearVelocity = new Vector2(-Math.Abs(launchSpeed), character.Body.LinearVelocity.Y);
        }

        public bool OnCollision(Fixture fix1, Fixture fix2, Contact contact)
        {
            if (fix1 == character || fix2 == character)
            {
                Vector2 normal;
                FixedArray2<Vector2> points;
                contact.GetWorldManifold(out normal, out points);
                if (normal.Y > 0.9f && Math.Abs(normal.X) < 0.2f)
                {
                    state = CharState.Idle;
                }
            }

            return true;
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(charTex, new Rectangle(RoundToInt(ConvertUnits.ToDisplayUnits(character.Body.Position.X)), RoundToInt(ConvertUnits.ToDisplayUnits(character.Body.Position.Y)), 49, 65), new Rectangle(0, 0, charTex.Width, charTex.Height), Color.White, 0, new Vector2(charTex.Width / 2, charTex.Height / 2), SpriteEffects.None, 0.5f);
            spriteBatch.Draw(platformTex, new Rectangle(RoundToInt(ConvertUnits.ToDisplayUnits(ground.Body.Position.X)), RoundToInt(ConvertUnits.ToDisplayUnits(ground.Body.Position.Y)), GraphicsDevice.Viewport.Width + 1, 21), new Rectangle(0, 0, platformTex.Width, platformTex.Height), Color.White, 0, new Vector2(platformTex.Width / 2, platformTex.Height / 2), SpriteEffects.None, 0.5f);
            spriteBatch.Draw(platformTex, new Rectangle(RoundToInt(ConvertUnits.ToDisplayUnits(leftWall.Body.Position.X)), RoundToInt(ConvertUnits.ToDisplayUnits(leftWall.Body.Position.Y)), 21, GraphicsDevice.Viewport.Height - 39), new Rectangle(0, 0, platformTex.Width, platformTex.Height), Color.White, 0, new Vector2(platformTex.Width / 2, platformTex.Height / 2), SpriteEffects.None, 0.5f);
            spriteBatch.Draw(platformTex, new Rectangle(RoundToInt(ConvertUnits.ToDisplayUnits(rightWall.Body.Position.X)), RoundToInt(ConvertUnits.ToDisplayUnits(rightWall.Body.Position.Y)), 21, GraphicsDevice.Viewport.Height - 39), new Rectangle(0, 0, platformTex.Width, platformTex.Height), Color.White, 0, new Vector2(platformTex.Width / 2, platformTex.Height / 2), SpriteEffects.None, 0.5f);
            spriteBatch.Draw(platformTex, new Rectangle(RoundToInt(ConvertUnits.ToDisplayUnits(ceiling.Body.Position.X)), RoundToInt(ConvertUnits.ToDisplayUnits(ceiling.Body.Position.Y)), GraphicsDevice.Viewport.Width + 1, 21), new Rectangle(0, 0, platformTex.Width, platformTex.Height), Color.White, 0, new Vector2(platformTex.Width / 2, platformTex.Height / 2), SpriteEffects.None, 0.5f);
            spriteBatch.Draw(platformTex, new Rectangle(RoundToInt(ConvertUnits.ToDisplayUnits(platform1.Body.Position.X)), RoundToInt(ConvertUnits.ToDisplayUnits(platform1.Body.Position.Y)), (GraphicsDevice.Viewport.Width / 4) + 1, 21), new Rectangle(0, 0, platformTex.Width, platformTex.Height), Color.White, 0, new Vector2(platformTex.Width / 2, platformTex.Height / 2), SpriteEffects.None, 0.5f);
            spriteBatch.Draw(platformTex, new Rectangle(RoundToInt(ConvertUnits.ToDisplayUnits(platform2.Body.Position.X)), RoundToInt(ConvertUnits.ToDisplayUnits(platform2.Body.Position.Y)), (GraphicsDevice.Viewport.Width / 4) + 1, 21), new Rectangle(0, 0, platformTex.Width, platformTex.Height), Color.White, 0, new Vector2(platformTex.Width / 2, platformTex.Height / 2), SpriteEffects.None, 0.5f);
            spriteBatch.Draw(platformTex, new Rectangle(RoundToInt(ConvertUnits.ToDisplayUnits(platform3.Body.Position.X)), RoundToInt(ConvertUnits.ToDisplayUnits(platform3.Body.Position.Y)), (GraphicsDevice.Viewport.Width / 4) + 1, 21), new Rectangle(0, 0, platformTex.Width, platformTex.Height), Color.White, 0, new Vector2(platformTex.Width / 2, platformTex.Height / 2), SpriteEffects.None, 0.5f);
            spriteBatch.Draw(platformTex, new Rectangle(RoundToInt(ConvertUnits.ToDisplayUnits(platform4.Body.Position.X)), RoundToInt(ConvertUnits.ToDisplayUnits(platform4.Body.Position.Y)), (GraphicsDevice.Viewport.Width / 4) + 1, 21), new Rectangle(0, 0, platformTex.Width, platformTex.Height), Color.White, 0, new Vector2(platformTex.Width / 2, platformTex.Height / 2), SpriteEffects.None, 0.5f);
            spriteBatch.Draw(platformTex, new Rectangle(RoundToInt(ConvertUnits.ToDisplayUnits(platform5.Body.Position.X)), RoundToInt(ConvertUnits.ToDisplayUnits(platform5.Body.Position.Y)), (GraphicsDevice.Viewport.Width / 4) + 1, 21), new Rectangle(0, 0, platformTex.Width, platformTex.Height), Color.White, 0, new Vector2(platformTex.Width / 2, platformTex.Height / 2), SpriteEffects.None, 0.5f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
