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
    class FullPowerItem:CollectibleItem
    {
        public FullPowerItem(Vector2 pos, float vel)
        {
            this.sprite = MainGame.SpriteDict["FullItem"];
            this.pos = pos;
            this.vel = vel;
            this.powerLevel = 3f;
        }
    }
}
