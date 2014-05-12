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
    public class PawnBulletPattern : BulletPattern
    {
        List<Tuple<Bullet.Action, float, int, int, bool>> actionList = new List<Tuple<Bullet.Action, float, int, int, bool>>();

        public PawnBulletPattern(GameObject parent, Sprite bulletSprite)
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
            for (int i = 0; i < 360; i += 10)
            {
                MainGame.ObjectManager.Add(new Bullet(sprite, new Vector2(Position.X, Position.Y), 10, i, 0, parent, actionList));
            }

            for (int i = 5; i < 365; i += 10)
            {
                MainGame.ObjectManager.Add(new Bullet(sprite, new Vector2(Position.X, Position.Y), 10, i, 0, parent, actionList));
            }

            MainGame.ObjectManager.Remove(this);
            
            base.Update();
        }
    }
}
