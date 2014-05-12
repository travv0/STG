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
    class BossPatternMedium:BulletPattern
    {
        List<Tuple<Bullet.Action, float, int, int, bool>> actionList = new List<Tuple<Bullet.Action, float, int, int, bool>>();

        Random rand = new Random();
        Random rand2 = new Random(23432);
        Random rand4 = new Random(543576);
        Random rand3 = new Random(54654);
        bool set = false;

        public BossPatternMedium(GameObject parent)
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
            if (rand4.Next(5) == 0)
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.aimed, 0, 60, 0, false));
            else
                actionList.Add(new Tuple<Bullet.Action, float, int, int, bool>(Bullet.Action.speed, 1.5f, 0, 60, false));

            if (time % 2 == 0)
            {
                MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 3, (float)rand.NextDouble() * 360, 0, parent, actionList));
                MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 3, (float)rand2.NextDouble() * 360, 0, parent, actionList));
                MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["prettyArrowBullet"], new Vector2(Position.X, Position.Y), 3, (float)rand3.NextDouble() * 360, 0, parent, actionList));
            }

            if (time > 450)
            {
                MainGame.player2.shooting = false;
                MainGame.objectManager.Remove(this);
            }
            
            base.Update();
        }
    }
}
