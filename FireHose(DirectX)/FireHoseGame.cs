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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class FireHoseGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public List<Controls> Controls;

        World world;

        public World gameWorld
        {
            get { return world; }
        }
        Body level;

        Player player1;
        Player player2;

        public List<Keys> Player1KeyControls;
        public List<Keys> Player2KeyControls;
               
        Texture2D levelTexture;
        
        public Texture2D ParticleTexture;

        CollisionDetector Player1CollisionDetector;
        CollisionDetector Player2CollisionDetector;

        public Rectangle Player1Rectangle;
        public Rectangle Player2Rectangle;

        public List<Rectangle> Player1FireRectangles;
        public List<Rectangle> Player2FireRectangles;

        public bool Player1Damaged;
        public bool Player2Damaged;

        public int PlayerDamageMeter;
        public int Player1DamageMeter;
        public int Player2DamageMeter;


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
            graphics.PreferredBackBufferHeight = 900;

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

            List<Controls> controls = new List<Controls>(); 
            controls.Add(new Controls(1));
            controls.Add(new Controls(1));
            controls.Add(new Controls(2));

            Controls = controls; 

            Player1KeyControls = new List<Keys>() 
            {
                Keys.A, Keys.D, Keys.Space, Keys.W, Keys.F
            };

           
            Player2KeyControls = new List<Keys>() 
            {
                Keys.J, Keys.L, Keys.LeftControl, Keys.I, Keys.H
            };

            //Player1ButtonControls = new List<Buttons>() 
            //{
            //    Buttons.A, Buttons.B, Buttons.X, Buttons.Y, Buttons.LeftThumb
            //};
            //Player2ButtonControls = new List<Buttons>() 
            //{
            //    Keys.J, Keys.L, Keys.LeftControl, Keys.I, Keys.H
            //};

            Player1CollisionDetector = new CollisionDetector();
            Player2CollisionDetector = new CollisionDetector();
            Player1DamageMeter = 0;
            Player2DamageMeter = 0;

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
            

            
            levelTexture = Content.Load<Texture2D>("level.png");

            screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);
            levelOrigin = new Vector2(levelTexture.Width / 2f, levelTexture.Height / 2f);

            //player.LinearDamping = 2f;


            levelPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(0, 1.25f);

            level = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(1400f), ConvertUnits.ToSimUnits(100f), 1f, levelPosition);
            level.IsStatic = true;
            level.Restitution = .3f;
            level.Friction = .5f;

            player1 = new Player(Player1StartPosition, world, 1);
            player1.LoadContent(this.Content);

            player2 = new Player(Player2StartPosition, world, 2);
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

            

            
            Controls[1].Update();
            Controls[2].Update();

            player1.Update(Controls[1], gameTime, Player1KeyControls);
            player2.Update(Controls[2], gameTime, Player1KeyControls);

            Player1FireRectangles = player1.GetFireRectangles();
            Player2FireRectangles = player2.GetFireRectangles();
            Player1Rectangle = player1.GetPlayerRectangle();
            Player2Rectangle = player2.GetPlayerRectangle();

            Player2Damaged = Player1CollisionDetector.CheckCollided(Player1FireRectangles, Player2Rectangle);
            Player1Damaged = Player2CollisionDetector.CheckCollided(Player2FireRectangles, Player1Rectangle);

            Player1DamageMeter = CheckDamage(Player1Damaged, player1, Player1DamageMeter);
            Player2DamageMeter = CheckDamage(Player2Damaged, player2, Player2DamageMeter);

            world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            Console.WriteLine(Mouse.GetState().Position);
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

        private int CheckDamage(bool playerDamage, Player player, int playerDamageMeter)
        {
            PlayerDamageMeter = playerDamageMeter;

            if (playerDamage == true)
            {
                player.PlayerColor = Color.Orange;
                PlayerDamageMeter++;
            }
            else
            {
                player.PlayerColor = Color.White;
            }

            if (PlayerDamageMeter > 10)
            {
                player.Restart(player.PlayerStartPosition, world);
                PlayerDamageMeter = 0;
            }



            return PlayerDamageMeter;

        }
        

        
      
        
    }
}
