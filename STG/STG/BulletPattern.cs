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
    public class BulletPattern:GameObject
    {
        protected GameObject parent;
        protected int time, maxTime, cycle = 0;

        public BulletPattern() { }

        public override void Update()
        {
            this.pos = parent.Position;

            time++;
            if (time == maxTime)
            {
                time = 0;
                cycle++;
            }
            

            base.Update();
        }
    }
}
