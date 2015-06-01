using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Tao.Sdl; 


namespace FireHoseTake2_DirectX_
{
	public class Controls
	{
		public KeyboardState kb;
		public KeyboardState kbo;
		
        public GamePadState gp;
		public GamePadState gpo;

        public int PlayerNumber;

        public Controls(int playerNumber)
		{
			this.kb = Keyboard.GetState();
			this.kbo = Keyboard.GetState();

            PlayerNumber = playerNumber; 

            if (PlayerNumber == 1)
            {
                this.gp = GamePad.GetState(PlayerIndex.One);
                this.gpo = GamePad.GetState(PlayerIndex.One);


            }
            else if (PlayerNumber == 2)
            {
                this.gp = GamePad.GetState(PlayerIndex.Two);
                this.gpo = GamePad.GetState(PlayerIndex.Two);

            }

            

		}
        
		public void Update()
		{ 
			kbo = kb;
			gpo = gp;
			kb = Keyboard.GetState();
            

            if (PlayerNumber == 1)
            {
                this.gp = GamePad.GetState(PlayerIndex.One);
               

              
            } else if (PlayerNumber == 2)
            {
                this.gp = GamePad.GetState(PlayerIndex.Two);
                
                
            }
            
		}

        
		public bool isPressed(Keys key, Buttons button)
		{
			return kb.IsKeyDown(key) || gp.IsButtonDown(button);
		}

		public bool onPress(Keys key, Buttons button)
		{
			
			return (kb.IsKeyDown(key) && kbo.IsKeyUp(key)) ||
				(gp.IsButtonDown(button) && gpo.IsButtonUp(button));
		}

		public bool onRelease(Keys key, Buttons button)
		{
			//Console.WriteLine (button);
			return (kb.IsKeyUp(key) && kbo.IsKeyDown(key)) ||
				(gp.IsButtonUp(button) && gpo.IsButtonDown(button));
		}

		public bool isHeld(Keys key, Buttons button)
		{
			//Console.WriteLine (button);
			return (kb.IsKeyDown(key) && kbo.IsKeyDown(key)) ||
				(gp.IsButtonDown(button) && gpo.IsButtonDown(button));
		}

        public bool isThumbStick(Buttons button)
        {

            return (gp.IsButtonDown(button) && gpo.IsButtonDown(button));
            
        }

        public Vector2 Fly()
        {
            float maxSpeed = 20f;
                        
            float rawThumbStickX = gp.ThumbSticks.Right.X * maxSpeed;
            float rawThumbStickY = gp.ThumbSticks.Right.Y * maxSpeed;

            int thumbStickX = (int)rawThumbStickX;
            int thumbStickY = (int)rawThumbStickY;

           return new Vector2(thumbStickX, thumbStickY);

            
        }

	
	}
}

