﻿using System;
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
    public class PawnBulletPattern2 : BulletPattern
    {
        List<Tuple<Bullet.Action, float, int, int, bool>> actionList = new List<Tuple<Bullet.Action, float, int, int, bool>>();

        int i = 0;

        bool reverse = false;

        public PawnBulletPattern2(GameObject parent, Sprite bulletSprite)
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
            MainGame.ObjectManager.Add(new Bullet(sprite, new Vector2(Position.X, Position.Y), 2, i, 0, parent, actionList));

            i += 30;

            if (i > 1440)
                MainGame.ObjectManager.Remove(this);

            base.Update();
        }
    }
}