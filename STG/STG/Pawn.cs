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
    public class Pawn : GameObject
    {
        Sprite bulletSprite;
        Rectangle moveableArea;
        int nextX, nextY;
        float vel;
        bool moving = false;
        Random rand, randShoot;
        int recharge = 0;
        int health;
        public Pawn(Sprite sprite, Vector2 pos, Sprite bulletSprite, int seed)
        {
            rand = new Random(Environment.TickCount + seed);
            randShoot = new Random(Environment.TickCount - seed);
            this.sprite = sprite;
            this.pos = pos;
            this.bulletSprite = bulletSprite;
            boundingBox = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Initialize();
            health = 100;
        }

        protected override void Initialize()
        {
            moveableArea = new Rectangle(MainGame.PlayingArea.X, MainGame.PlayingArea.Y, MainGame.PlayingArea.Width, MainGame.PlayingArea.Height / 2);
            vel = 5;

            base.Initialize();
        }

        public override void Update()
        {

            int chance; //integer used to see if pawn should move or not

            GameObject hitBullet = Collides('B');
            if (hitBullet != null)
            {
                if (((Bullet)hitBullet).Parent != MainGame.player2 && ((Bullet)hitBullet).Parent != MainGame.Sol && ((Bullet)hitBullet).Parent != MainGame.Luna)
                {
                    MainGame.objectManager.Remove(hitBullet);
                    health--;
                    if (health == 0)
                    {
                        MainGame.objectManager.Remove(this);
                        ((Bullet)hitBullet).boundingBox = new Rectangle(0, 0, 0, 0);
                    }
                }

            }

            recharge++;

            if (moving == false)
            {
                chance = rand.Next(100);

                if (chance > 98)
                {
                    nextX = rand.Next(moveableArea.X, moveableArea.X + moveableArea.Width);
                    nextY = rand.Next(moveableArea.X, moveableArea.Y + moveableArea.Height);
                    moving = true;
                }
            }
            else
            {
                pos.X += (nextX - pos.X) / 10;
                pos.Y += (nextY - pos.Y) / 10;

                if (pos.X < nextX + vel && pos.X > nextX - vel || pos.Y < nextY + vel && pos.Y > nextY - vel)
                {
                    moving = false;
                }
            }

            chance = randShoot.Next(500);

            if (chance > 498 && recharge >= 300 || recharge >= 600)
            {
                chance = rand.Next(3);
                switch (chance)
                {
                    case 0:
                        MainGame.ObjectManager.Add(new PawnBulletPattern(this, bulletSprite));
                        break;
                    case 1:
                        MainGame.ObjectManager.Add(new PawnBulletPattern2(this, bulletSprite));
                        break;
                    case 2:
                        MainGame.ObjectManager.Add(new PawnBulletPattern3(this, bulletSprite));
                        break;
                }
                recharge = 0;
            }


 	        base.Update();
        }
    }
}
