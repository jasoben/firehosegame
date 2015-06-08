#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
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
    class SoundItem
    {
        public SoundEffect Sound;
        public bool SoundIsPlaying = false;
        public SoundEffectInstance soundInstance;
        public float SoundVolume = 1f;

        public SoundItem(SoundEffect sound)
        {
            Sound = sound;
            soundInstance = Sound.CreateInstance();
        }

        public void PlaySound()
        {
            if (SoundIsPlaying == false)
            {
                soundInstance.IsLooped = true;
                soundInstance.Volume = SoundVolume;
                soundInstance.Play();
                SoundIsPlaying = true;
            }

        }

        public void StopSound()
        {
            soundInstance.Stop();
            SoundIsPlaying = false;
        }

        public void PlaySingleSound()
        {
            soundInstance.IsLooped = false;
            soundInstance.Volume = SoundVolume;
            soundInstance.Play();
    
        }
    }
}
