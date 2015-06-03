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

        #region fields and properties
        //Standard mono stuff
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        //We make a list of Controls so we can easily pass the right control information from separate player objects to their associated (by an integer value) control object
        public List<Controls> Controls;

        //Define the farseer physics world variable
        World world;

        //define the GameWorld property so we can access it from other classes
        public World GameWorld
        {
            get { return world; }
        }

        //Define the level variable (these are just boxes that are static and register collisions)
        Body level;
        Texture2D levelTexture;
        private Vector2 levelOrigin;

        //TODO: Do I need these properties linked to these fields?
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
            set { levelPosition = value; }
        }

        Player player1;
        Player player2;

        //Define the list of keys-- so we can easily pass the right keys to the right player object AND define the keys later on
        //TODO: Probably need to do the same thing for gamepad controls

        public List<Keys> Player1KeyControls;
        public List<Keys> Player2KeyControls;
        
        //define the particle texture for either water or fire variables        
        public Texture2D ParticleTexture;

        //define two different collision detection objects, one for each player objects
        CollisionDetector Player1CollisionDetector;
        CollisionDetector Player2CollisionDetector;

        public Rectangle Player1Rectangle;
        public Rectangle Player2Rectangle;

        public List<Rectangle> Player1FireRectangles;
        public List<Rectangle> Player2FireRectangles;

        //this bool activates whether there is damage taken or not 
        public bool Player1Damaged;
        public bool Player2Damaged;

        //These are the damage meters
        //The first property is to generally calculate player damage in the method that checks for either player-specific 

        public int PlayerDamageMeter;
        public int Player1DamageMeter;
        public int Player2DamageMeter;


        //these properties grab the screencenter property and determine where the players start       
        public Vector2 Player1StartPosition
        {
            get { return (ScreenCenter - new Vector2(200, 0));  }

        }

        public Vector2 Player2StartPosition
        {
            get { return (ScreenCenter + new Vector2(200, 0)); }

        }
        #endregion

        public FireHoseGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1800;
            graphics.PreferredBackBufferHeight = 900;

            Content.RootDirectory = "Content";

            world = new World(new Vector2(0, 9.8f));
            
            //This conversion doohicky is from farseer, and it is necessary to define our conversion ratio because farseer is build to work with metric units
            //TODO: probably need a public variable for this.

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


            //This is where we make our list of control objects
            //I add in a dummy one for controls[0] so that I don't have to pass a zero to indicate player one, a one to indicate player two
            List<Controls> controls = new List<Controls>(); 
            controls.Add(new Controls(1));
            controls.Add(new Controls(1));
            controls.Add(new Controls(2));

            Controls = controls; 

            //Here we can define lists of our player keys
            //TODO: probably need to make one of these for gamepad buttons as well so we can 
            Player1KeyControls = new List<Keys>() 
            {
                Keys.A, Keys.D, Keys.Space, Keys.W, Keys.F
            };

           
            Player2KeyControls = new List<Keys>() 
            {
                Keys.J, Keys.L, Keys.LeftControl, Keys.I, Keys.H
            };

            
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

            //define the screen center and where the middle level piece texture should be placed in relation to the level object
            screenCenter = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);
            levelOrigin = new Vector2(levelTexture.Width / 2f, levelTexture.Height / 2f);

            //This lineardamping is for when we implement a fixture beneath the player body object that slows it while on the ground
            //player.LinearDamping = 2f;
            
            //define where the first piece of level goes
            levelPosition = ConvertUnits.ToSimUnits(ScreenCenter) + new Vector2(0, 1.25f);

            //make the level body
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

            //this is the collision detection stuff
            Player1FireRectangles = player1.GetFireRectangles();
            Player2FireRectangles = player2.GetFireRectangles();
            Player1Rectangle = player1.GetPlayerRectangle();
            Player2Rectangle = player2.GetPlayerRectangle();

            Player2Damaged = Player1CollisionDetector.CheckCollided(Player1FireRectangles, Player2Rectangle);
            Player1Damaged = Player2CollisionDetector.CheckCollided(Player2FireRectangles, Player1Rectangle);

            Player1DamageMeter = CheckDamage(Player1Damaged, player1, Player1DamageMeter);
            Player2DamageMeter = CheckDamage(Player2Damaged, player2, Player2DamageMeter);
            //end collision detection stuff
            
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

            //draw level and players
            spriteBatch.Begin();
            spriteBatch.Draw(levelTexture, ConvertUnits.ToDisplayUnits(level.Position), null, Color.White, 0f, LevelOrigin, 1f, SpriteEffects.None, 0f);
            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);
            spriteBatch.End();
                        
            base.Draw(gameTime);
        }

        //this method needs to be in the base class so that we can check all collisions
        private int CheckDamage(bool playerDamage, Player player, int playerDamageMeter)
        {
            PlayerDamageMeter = playerDamageMeter;

            if (playerDamage == true)
            {
                player.PlayerColor = Color.Orange;
                PlayerDamageMeter++;
            } else {
                player.PlayerColor = Color.White;
            }

            if (PlayerDamageMeter > 10)
            {
                //this calls the method to reset the player
                player.Restart(player.PlayerStartPosition, world);
                PlayerDamageMeter = 0;
            }
            
            return PlayerDamageMeter;

        }
        
    }
}
