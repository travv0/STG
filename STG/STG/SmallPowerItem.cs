using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace STG
{
    class SmallPowerItem:CollectibleItem
    {
        public SmallPowerItem(Vector2 pos, float vel)
        {
            this.sprite = MainGame.SpriteDict["SmallItem"];
            this.pos = pos;
            this.vel = vel;
            this.powerLevel = .01f;
        }
    }
}
