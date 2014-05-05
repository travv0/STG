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

namespace STG
{
    class Bomb:GameObject
    {
        float vel;
        int bombRad = 50;
        Color[] colors = new Color[7] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet };
        int colorNum = 0;

        public Bomb(Sprite sprite, Vector2 pos, float vel)
        {
            this.sprite = sprite;
            this.pos = pos;
            this.vel = vel;
            this.boundingBox = new Rectangle((int)pos.X, (int)pos.Y, bombRad, bombRad);
            this.color = colors[colorNum];
        }

        public override void Update()
        {
            if (bombRad < 1000)
            {
                this.boundingBox.Width += 5;
                this.boundingBox.Height += 5;
                bombRad++;
                colorNum++;
                this.color = colors[colorNum % 7];
                if (MainGame.bombSoundInstance.State == SoundState.Stopped)
                {
                    MainGame.bombSoundInstance.Volume = 0.75f;
                    MainGame.bombSoundInstance.IsLooped = true;
                    MainGame.bombSoundInstance.Play();
                }
                else
                    MainGame.bombSoundInstance.Resume();

            }
            if ((this.boundingBox.Width > (MainGame.WindowWidth*2))&&(this.boundingBox.Height > (MainGame.WindowHeight*2)))
            {
                MainGame.ObjectManager.Remove(this);
                MainGame.bombSoundInstance.Stop();
                base.Update();
            }
            base.Update();
        }
    }
}
