using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

using Tao.Sdl; 


namespace FireHose_DirectX_
{
	public class Controls
	{
		public KeyboardState kb;
		public KeyboardState kbo;
		
        public GamePadState gp;
		public GamePadState gpo;
               
        //PlayerNumber allows the Controls class to direct gamepad input to the right Player object based on the method calls 
        public int PlayerNumber;

        public Controls(int playerNumber)
		{
			this.kb = Keyboard.GetState();
			this.kbo = Keyboard.GetState();

            //set field to property
            PlayerNumber = playerNumber; 

            //determine which player we want to pass the controls to, based on the method calls from that player object
            //we need two gamepad states here, because one is passing the "Pressed" state, and the other is passing the "Released" state
            //if we had only one, we couldn't have methods in this class to determine states like "isHeld" 

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

            //determine which player we want to pass the controls to, based on the method calls from that player object
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

        //the difference between isPressed and onPress is that onPress methods a full press instead of a tap
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

        //This method calculates the vector of flying when the player blasts fire
        
        public Vector2 Fly(bool isItFire)
        {
            
            float maxAccel = 20f;
            float rawThumbStickX = gp.ThumbSticks.Right.X * maxAccel;
            float rawThumbStickY = gp.ThumbSticks.Right.Y * maxAccel;

            if (isItFire == false)
            {
                maxAccel = 15f;
                rawThumbStickX = gp.ThumbSticks.Left.X * maxAccel;
                rawThumbStickY = gp.ThumbSticks.Left.Y * maxAccel;
            }
            

            int thumbStickX = (int)rawThumbStickX;
            int thumbStickY = (int)rawThumbStickY;

            return new Vector2(thumbStickX, thumbStickY);

        }

	}
}

