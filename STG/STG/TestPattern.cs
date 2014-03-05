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
    class TestPattern:BulletPattern
    {
        float angle = 0;

        List<Tuple<Bullet.Action, float, int, int>> actionList = new List<Tuple<Bullet.Action, float, int, int>>();

        public TestPattern(GameObject parent)
        {
            this.parent = parent;
            this.pos = parent.Position;
        }

        protected override void Initialize()
        {
            maxTime = 200;

 	        base.Initialize();
        }

        public override void Update()
        {
            if (time == 0 || time == 50 || time == 100)
            {
                actionList.Add(new Tuple<Bullet.Action, float, int, int>(Bullet.Action.speed, 10, 10, 5));
                actionList.Add(new Tuple<Bullet.Action, float, int, int>(Bullet.Action.speed, -2, 50, 10));
                actionList.Add(new Tuple<Bullet.Action, float, int, int>(Bullet.Action.angle, 90, 50, 50));

                for (int i = 0; i < 360; i += 10)
                {
                    Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -10, i, 0, parent, actionList));
                }

                actionList.Clear();
                actionList.Add(new Tuple<Bullet.Action, float, int, int>(Bullet.Action.speed, 10, 10, 5));
                actionList.Add(new Tuple<Bullet.Action, float, int, int>(Bullet.Action.speed, -2, 50, 10));
                actionList.Add(new Tuple<Bullet.Action, float, int, int>(Bullet.Action.angle, -90, 50, 50));

                for (int i = 5; i < 365; i += 10)
                {
                    Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -10, i, 0, parent, actionList));
                }
            }

            if (time == 100)
            {
                actionList.Clear();

                for (int i = 0; i < 360; i += 10)
                {
                    Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -10, i, 0, parent, actionList));
                }
            }

            if (time == 120)
            {
                for (int i = 5; i < 365; i += 10)
                {
                    Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -10, i, 0, parent, actionList));
                }
            }

            if (time == 140)
            {
                actionList.Clear();

                for (int i = 0; i < 360; i += 10)
                {
                    Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -10, i, 0, parent, actionList));
                }
            }

            if (time == 160)
            {
                for (int i = 5; i < 365; i += 10)
                {
                    Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -10, i, 0, parent, actionList));
                }
            }

            if (time == 180)
            {
                actionList.Clear();

                for (int i = 0; i < 360; i += 10)
                {
                    Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -10, i, 0, parent, actionList));
                }
            }

            if (time == 200)
            {
                for (int i = 5; i < 365; i += 10)
                {
                    Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -10, i, 0, parent, actionList));
                }
            }

            actionList.Clear();

            if (time % 10 == 0)
                Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -10, angle, -2, parent, actionList));

            angle = angle + 2;

            if (time == maxTime)
            {
                Game1.objectManager.Remove(this);
            }

            base.Update();
        }
    }
}
