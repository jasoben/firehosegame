#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Tao.Sdl;
using FarseerPhysics.Common.PhysicsLogic;
using FarseerPhysics.Dynamics;

#endregion

namespace Firehose
{
    /// <summary>
    /// Firehose game type
    /// </summary>
    public class FirehoseMain : Game  
    {

        #region Firehose fields
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player1;
        Player player2;

        Controls controls;

        FlameGun flameGun1;
        WaterGun waterGun1;
        FlameGun flameGun2;
        WaterGun waterGun2;
        
        Altar altar1;
        Altar altar2;
        Altar altar3;
        Altar altar4;
        Altar altar5;

        FireParticleEngine fireParticleEngine;
        #endregion

        
      
        #region Farseer fields
        // Farseer Physics Engine fields

        private World firehoseWorld;

        private Body player1Body;
        private Body player2Body;

        private Body groundBody;

        #endregion


        public FirehoseMain()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = true; 

            Content.RootDirectory = "Content";

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

            firehoseWorld = new World(new Vector2(0, 9.82f)); // defines the new world and it's gravitational constant

            player1 = new Player(50, 50, 50, 50);
            player2 = new Player(50, 50, 50, 50);

            flameGun1 = new FlameGun(10, 10);
            flameGun2 = new FlameGun(10, 10);

            waterGun1 = new WaterGun(10, 10);
            waterGun2 = new WaterGun(10, 10);

            altar1 = new Altar();
            altar2 = new Altar();
            altar3 = new Altar();
            altar4 = new Altar();
            altar5 = new Altar();


            base.Initialize();
          
            controls = new Controls();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player1.LoadContent(this.Content);
            flameGun1.LoadContent(this.Content);

            // TODO: use this.Content to load your game content here

            List<Texture2D> fireTextures = new List<Texture2D>();
            fireTextures.Add(Content.Load<Texture2D>("fire.png"));
            fireTextures.Add(Content.Load<Texture2D>("fire1.png"));
            fireTextures.Add(Content.Load<Texture2D>("fire2.png"));
            
            // the fireParticleEngine is what generates the water or fire particles
            fireParticleEngine = new FireParticleEngine(fireTextures, new Vector2(flameGun1.FlameGunLocationX, flameGun1.FlameGunLocationY), new Vector2 (player1.PlayerLocationX, player1.PlayerLocationY));

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
            
            //update the world physics

            firehoseWorld.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            
            //set our keyboardstate tracker update can change the gamestate on every cycle
            controls.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //Up, down, left, right affect the coordinates of the sprite

            player1.Update(controls, gameTime);
            flameGun1.Update(player1, controls, gameTime, fireParticleEngine);
            waterGun1.Update(player1, controls, gameTime, waterParticleEngine);

            player2.Update(controls, gameTime);
            flameGun2.Update(player2, controls, gameTime, fireParticleEngine);
            waterGun2.Update(player1, controls, gameTime, waterParticleEngine);
               
          

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
            spriteBatch.Begin();
            
            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);
            
            flameGun1.Draw(spriteBatch);
            flameGun2.Draw(spriteBatch);
            waterGun1.Draw(spriteBatch);
            waterGun2.Draw(spriteBatch);

            fireParticleEngine.Draw(spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}

