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
    class BossPatternHard:BulletPattern
    {
        List<Tuple<Bullet.Action, float, int, int, bool>> actionList = new List<Tuple<Bullet.Action, float, int, int, bool>>();

        Random rand = new Random();
        Random rand3 = new Random(54654);
        bool reverse = false;
        int i = 0;

        public BossPatternHard(GameObject parent)
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
            actionList.Clear();
            if (i % 2 == 0)
                MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 7, i * 10, 0, parent, actionList));
            if (i < 18 && reverse == false)
                i++;
            else
            {
                reverse = true;
                i--;
            }
            if (i < 0)
                reverse = false;

            if (time % 35 < 20)
            {
                MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 10, AngleToTarget(MainGame.player1.Position), 0, parent, actionList));
                MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 10, AngleToTarget(MainGame.player1.Position) + 20, 0, parent, actionList));
                MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 10, AngleToTarget(MainGame.player1.Position) - 20, 0, parent, actionList));
            }
            
            base.Update();
        }
    }
}
