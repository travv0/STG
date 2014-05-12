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
    public class PawnBulletPattern3 : BulletPattern
    {
        List<Tuple<Bullet.Action, float, int, int, bool>> actionList = new List<Tuple<Bullet.Action, float, int, int, bool>>();

        int i = 0;

        bool reverse = false;

        public PawnBulletPattern3(GameObject parent, Sprite bulletSprite)
        {
            this.parent = parent;
            this.pos = parent.Position;
            this.sprite = bulletSprite;
        }

        protected override void Initialize()
        {
 	        base.Initialize();
        }

        public override void Update()
        {
            if (i % 5 == 0)
            {
                MainGame.ObjectManager.Add(new Bullet(sprite, new Vector2(Position.X, Position.Y), 5, AngleToTarget(MainGame.player1.Position), 0, parent, actionList));
                MainGame.ObjectManager.Add(new Bullet(sprite, new Vector2(Position.X, Position.Y), 5, AngleToTarget(MainGame.player1.Position) + 20, 0, parent, actionList));
                MainGame.ObjectManager.Add(new Bullet(sprite, new Vector2(Position.X, Position.Y), 5, AngleToTarget(MainGame.player1.Position) - 20, 0, parent, actionList));
            }

            i++;

            if (i > 100)
                MainGame.ObjectManager.Remove(this);

            base.Update();
        }
    }
}
