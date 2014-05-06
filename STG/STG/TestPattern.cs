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
        List<Tuple<Bullet.Action, float, int, int, bool>> actionList = new List<Tuple<Bullet.Action, float, int, int, bool>>();

        public TestPattern(GameObject parent)
        {
            this.parent = parent;
            this.pos = parent.Position;
        }

        protected override void Initialize()
        {
 	        base.Initialize();
        }

        public override void Update()
        {
            if (time % 100 == 0)
            {
                actionList.Clear();
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.speed, 0, 10, 5, false));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.speed, 2, 50, 10, false));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.angle, 80, 50, 50, true));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.aimed, 0, 80, 0, false));

                for (int i = 0; i < 360; i += 10)
                {
                    MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 10, i, 0, parent, actionList));
                }

                actionList.Clear();
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.speed, 0, 10, 5, false));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.speed, 2, 50, 10, false));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.angle, -80, 50, 50, true));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.aimed, 0, 80, 0, false));

                for (int i = 5; i < 365; i += 10)
                {
                    MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 10, i, 0, parent, actionList));
                }
            }
            if (time % 100 - 50 == 0)
            {
                actionList.Clear();
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.speed, 0, 10, 5, false));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.speed, 2, 50, 10, false));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.angle, 80, 50, 50, true));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.aimed, 0, 80, 0, false));

                for (int i = 2; i < 362; i += 10)
                {
                    MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 10, i, 0, parent, actionList));
                }

                actionList.Clear();
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.speed, 0, 10, 5, false));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.speed, 2, 50, 10, false));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.angle, -80, 50, 50, true));
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.aimed, 0, 80, 0, false));

                for (int i = 7; i < 367; i += 10)
                {
                    MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 10, i, 0, parent, actionList));
                }
            }

            /*if (time % 40 == 0)
            {
                actionList.Clear();

                for (int i = 0; i < 360; i += 10)
                {
                    MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 5, i, 0, parent, actionList));
                }
            }

            if (time % 40 - 20 == 0)
            {
                for (int i = 5; i < 365; i += 10)
                {
                    MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 5, i, 0, parent, actionList));
                }
            }
           
            /*actionList.Clear();
            actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.curve, 0, 600, 5, false));

            if (time % 10 == 0)
            {
               // MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 3, angle, -.5f, parent, actionList));
               // MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 3, angle - 90, -.5f, parent, actionList));
               // MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 3, angle - 180, -.5f, parent, actionList));
                MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 3, angle - 270, -.5f, parent, actionList));
            }

            angle = angle + 3.5f;*/
            
            base.Update();
        }
    }
}
