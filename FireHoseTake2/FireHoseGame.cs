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

namespace FireHoseTake2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FireHoseGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Controls controls;

        World world;
        
        Body level;

        Player player1;
        Player player2;

        public List<Keys> Player1Controls;
        public List<Keys> Player2Controls;
       
        Texture2D levelTexture;
        Texture2D waterTexture;

        private Vector2 levelOrigin;

        public Vector2 LevelOrigin
        {
            get { return levelOrigin; }
        }

        private Vector2 screenCenter;

        public Vector2 ScreenCenter
        {
            get { return screenCenter; }
        }

        private Vector2 levelPosition; 

        public Vector2 LevelPosition
        {
            get { return levelPosition; }
            set { levelPosition = value;  }
        }

        
        public Vector2 Player1StartPosition
        {
            get { return (ScreenCenter - new Vector2(200, 0));  }

        }

        public Vector2 Player2StartPosition
        {
            get { return (ScreenCenter + new Vector2(200, 0)); }

        }

        public FireHoseGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1800;
            graphics.PreferredBackBufferHeight = 480;

            Content.RootDirectory = "Content";

            world = new World(new Vector2(0, 9.8f));
            

            ConvertUnits.SetDisplayUnitToSimUnitRatio(64f);

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

            controls = new Controls();

            Player1Controls = new List<Keys>() 
            {
                Keys.A, Keys.D, Keys.Space, Keys.W, Keys.F
            };
            Player2Controls = new List<Keys>() 
            {
                Keys.J, Keys.L, Keys.LeftControl, Keys.I, Keys.H
            };


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            // TODO: use this.Content to load your game content here

            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            

            waterTexture = Content.Load<Texture2D>("waterdrop.png");
            levelTexture = Content.Load<Texture2D>("level.png");

            screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);
            levelOrigin = new Vector2(levelTexture.Width / 2f, levelTexture.Height / 2f);

            //player.LinearDamping = 2f;


            levelPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(0, 1.25f);

            level = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(1400f), ConvertUnits.ToSimUnits(100f), 1f, levelPosition);
            level.IsStatic = true;
            level.Restitution = .3f;
            level.Friction = .5f;

            player1 = new Player(Player1StartPosition, world);
            player1.LoadContent(this.Content);

            player2 = new Player(Player2StartPosition, world);
            player2.LoadContent(this.Content);

            
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here


            controls.Update();

            player1.Update(controls, gameTime, Player1Controls);
            player2.Update(controls, gameTime, Player2Controls);

            world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            base.Update(gameTime);

            
            
             
        }


        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //Draw circle and ground
            spriteBatch.Begin();
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(level.Position), null, Color.White, 0f, LevelOrigin, 1f, SpriteEffects.None, 0f);
            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);
            spriteBatch.End();

            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        

        
      
        
    }
}