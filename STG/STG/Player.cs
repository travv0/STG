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
    public class Player:GameObject
    {
        public enum PlayerNum {One, Two};
        PlayerNum playerNum;

        int cooldown = 0; //frames until another bullet can be fired

        const int speed = 5; //player's speed

        List<Tuple<Bullet.Action, float, int>> actionList = new List<Tuple<Bullet.Action, float, int>>();

        public Player(Sprite sprite, PlayerNum playerNum, Vector2 pos, int hitboxWidth, int hitboxHeight)
        {
            this.sprite = sprite;
            this.pos = pos;
            this.boundingBox = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);
            this.hitbox.Width = hitboxWidth;
            this.hitbox.Height = hitboxHeight;
            this.playerNum = playerNum;
        }
        public Player(Sprite sprite, PlayerNum playerNum, Rectangle boundingBox, int hitboxWidth, int hitboxHeight)
        {
            this.sprite = sprite;
            this.pos = new Vector2(boundingBox.X, boundingBox.Y);
            this.boundingBox = boundingBox;
            this.hitbox.Width = hitboxWidth;
            this.hitbox.Height = hitboxHeight;
            this.playerNum = playerNum;
        }
        public Player(Sprite sprite, PlayerNum playerNum, Vector2 pos)
        {
            this.sprite = sprite;
            this.pos = pos;
            this.boundingBox = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);
            this.hitbox = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);
            this.playerNum = playerNum;
        }
        public Player(Sprite sprite, PlayerNum playerNum, Rectangle boundingBox)
        {
            this.sprite = sprite;
            this.pos = new Vector2(boundingBox.X, boundingBox.Y);
            this.boundingBox = boundingBox;
            this.hitbox = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);
            this.playerNum = playerNum;
        }

        public override void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();                

            switch (playerNum)
            {
                case PlayerNum.One:
                    //movement
                    if (keyboard.IsKeyDown(Keys.Left) && pos.X - sprite.Width / 2 > 0)
                        pos.X -= speed;
                    if (keyboard.IsKeyDown(Keys.Down) && pos.Y + sprite.Height / 2 < Game1.windowHeight)
                        pos.Y += speed;
                    if (keyboard.IsKeyDown(Keys.Right) && pos.X + sprite.Width / 2 < Game1.windowWidth)
                        pos.X += speed;
                    if (keyboard.IsKeyDown(Keys.Up) && pos.Y - sprite.Height / 2 > 0)
                        pos.Y -= speed;

                    //shootin
                    if (keyboard.IsKeyDown(Keys.NumPad1) && cooldown == 0)
                    {
                        Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet"], new Vector2(pos.X - 10, pos.Y - 20), -40, 90, this, actionList));
                        Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet"], new Vector2(pos.X + 10, pos.Y - 20), -40, 90, this, actionList));
                        cooldown = 2;
                    }
                    if (cooldown > 0)
                        cooldown--;

                    break;

                case PlayerNum.Two:
                    //movement
                    if (keyboard.IsKeyDown(Keys.A) && pos.X - sprite.Width / 2 > 0)
                        pos.X -= speed;
                    if (keyboard.IsKeyDown(Keys.S) && pos.Y + sprite.Height / 2 < Game1.windowHeight)
                        pos.Y += speed;
                    if (keyboard.IsKeyDown(Keys.D) && pos.X + sprite.Width / 2 < Game1.windowWidth)
                        pos.X += speed;
                    if (keyboard.IsKeyDown(Keys.W) && pos.Y - sprite.Height / 2 > 0)
                        pos.Y -= speed;

                    //shootin
                    if (keyboard.IsKeyDown(Keys.G) && cooldown == 0)
                    {
                        //Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet"], pos,, this, actionList));
                        cooldown = 2;
                    }
                    if (cooldown > 0)
                        cooldown--;

                    break;
            }

            foreach (GameObject o in CollidingWith())
            {
                if (o.GetType() == typeof(Bullet))
                {
                    Bullet t = (Bullet)o; //make bullet variable t so you can check it's parent
                    if (t.Parent != this)
                    {
                        Game1.objectManager.Remove(o);
                        collisionGrid[(int)topLeft.X, (int)topLeft.Y].Remove(o);
                        collisionGrid[(int)topRight.X, (int)topRight.Y].Remove(o);
                        collisionGrid[(int)bottomLeft.X, (int)bottomLeft.Y].Remove(o);
                        collisionGrid[(int)bottomRight.X, (int)bottomRight.Y].Remove(o);
                    }
                }
            }

            base.Update();
        }

        public int Speed { get { return speed; } }
    }
}
