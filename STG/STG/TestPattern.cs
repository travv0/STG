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

        List<Tuple<Bullet.Action, float, int, int, bool>> actionList = new List<Tuple<Bullet.Action, float, int, int, bool>>();

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
            if (time == 0 || time == 100)
            {
                actionList.Clear();
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.speed, 0, 10, 5, false));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.speed, -2, 50, 10, false));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.angle, AngleToTarget(Game1.player1.Position), 50, 0, false));

                for (int i = 0; i < 360; i += 10)
                {
                    Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -10, i, 0, parent, actionList));
                }

                actionList.Clear();
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.speed, 0, 10, 5, false));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.speed, -2, 50, 10, false));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.angle, AngleToTarget(Game1.player1.Position), 50, 0, false));

                for (int i = 5; i < 365; i += 10)
                {
                    Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -10, i, 0, parent, actionList));
                }
            }

            if (time == 100 || time == 140 || time == 180)
            {
                actionList.Clear();

                for (int i = 0; i < 360; i += 10)
                {
                    Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -5, i, 0, parent, actionList));
                }
            }

            if (time == 120 || time == 160 || time == 200)
            {
                for (int i = 5; i < 365; i += 10)
                {
                    Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -5, i, 0, parent, actionList));
                }
            }

            actionList.Clear();

            if (time % 10 == 0)
            {
                Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -3, angle, -.5f, parent, actionList, true));
                Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -3, angle - 90, -.5f, parent, actionList, true));
                Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -3, angle - 180, -.5f, parent, actionList, true));
                Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(Position.X, Position.Y), -3, angle - 270, -.5f, parent, actionList, true));
            }

            angle = angle + 2;

            if (time == maxTime)
            {
                Game1.objectManager.Remove(this);
            }

            base.Update();
        }
    }
}
