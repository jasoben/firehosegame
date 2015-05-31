using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
namespace Firehose
{
	abstract class Sprite
	{
		protected int spriteX, spriteY;
		protected int spriteWidth, spriteHeight;
		protected Texture2D image;
        protected Texture2D crossHairImage;
        protected Texture2D fireImage;

		public Sprite ()
		{
		}
	}
}

