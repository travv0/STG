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
    class BombCenter:GameObject
    {
        float vel;
        int centRad = 50;
        
        public BombCenter(Sprite sprite, Vector2 pos, float vel)
        {
            this.sprite = sprite;
            this.pos = pos;
            this.vel = vel;
            this.boundingBox = new Rectangle((int)pos.X, (int)pos.Y, centRad, centRad);
            this.color = Color.CornflowerBlue;
        }

        public override void Update()
        {
            if (centRad < 1500)
            {
                this.boundingBox.Width += 5;
                this.boundingBox.Height += 5;
                centRad++;
            }
            if ((this.boundingBox.Width > (MainGame.WindowWidth * 2)) && (this.boundingBox.Height > (MainGame.WindowHeight * 2)))
            {
                MainGame.ObjectManager.Remove(this);
                base.Update();
            }
            base.Update();
        }

    }
}
