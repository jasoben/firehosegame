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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision;

namespace FarseerPlatformer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        World world;
        Texture2D squareTex;
        CompositeCharacter box;
        PhysicsObject seeSawBottom;
        PhysicsObject seeSawTop;
        StaticPhysicsObject ground;
        SpringPhysicsObject seeSaw;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            world = new World(new Vector2(0, 20f));
            ConvertUnits.SetDisplayUnitToSimUnitRatio(30);
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
            squareTex = Content.Load<Texture2D>("square");
            box = new CompositeCharacter(world, new Vector2(100, 0), 64, 128, 10, squareTex);
            box.forcePower = 100;
            seeSawBottom = new PhysicsObject(world, new Vector2(175f, 300), 50, 50, 1000, squareTex);
            seeSawTop = new PhysicsObject(world, new Vector2(175f, 285), 300, 30, 25, squareTex);
            seeSaw = new SpringPhysicsObject(world, seeSawTop, seeSawBottom, new Vector2(0, 15), 0.998f, 5);
            ground = new StaticPhysicsObject(world, new Vector2(GraphicsDevice.Viewport.Width / 2, 500), GraphicsDevice.Viewport.Width, 64, squareTex);
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

            // TODO: Add your update logic here
            box.Update(gameTime);
            world.Step((float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.001));

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            box.Draw(spriteBatch);
            seeSaw.Draw(spriteBatch);
            ground.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
