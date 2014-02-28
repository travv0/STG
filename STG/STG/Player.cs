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
        bool inFocus = false;
        int power = 0;

        int cooldown = 0; //frames until another bullet can be fired
        Stack<Option> options = new Stack<Option>();

        float speed = 5; //player's speed

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

        protected override void Initialize()
        {
            foreach (Option option in options)
                Game1.objectManager.Add(option);

            base.Initialize();
        }

        public override void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();                

            switch (playerNum)
            {
                case PlayerNum.One:
                    if (keyboard.IsKeyDown(Keys.Q))
                        power = 0;
                    if (keyboard.IsKeyDown(Keys.W))
                        power = 1;
                    if (keyboard.IsKeyDown(Keys.E))
                        power = 2;
                    if (keyboard.IsKeyDown(Keys.R))
                        power = 3;

                    //changing options at different powers
                    if (power == 0 && options.Count != 0)
                    {
                        while (options.Count > 0)
                            options.Pop();
                    }
                    if (power == 1 && options.Count != 2)
                    {
                        if (options.Count < 2)
                        {
                            options.Push(new Option(this, Game1.spriteDict["option"], new Vector2(-20, 0)));
                            options.Push(new Option(this, Game1.spriteDict["option"], new Vector2(20, 0)));
                        }
                        else while (options.Count > 2)
                            options.Pop();
                    }
                    if (power == 2 && options.Count != 4)
                    {
                        if (options.Count < 4)
                        {
                            options.Push(new Option(this, Game1.spriteDict["option"], new Vector2(-30, 10)));
                            options.Push(new Option(this, Game1.spriteDict["option"], new Vector2(30, 10)));
                        }
                        else while (options.Count > 4)
                                options.Pop();
                    }
                    if (power == 3 && options.Count != 6)
                    {
                        options.Push(new Option(this, Game1.spriteDict["option"], new Vector2(-40, 20)));
                        options.Push(new Option(this, Game1.spriteDict["option"], new Vector2(40, 20)));
                    }

                    foreach (Option option in options)
                        Game1.objectManager.Add(option);

                    if (keyboard.IsKeyDown(Keys.Enter) && inFocus == false)
                    {
                        speed = 2;
                        inFocus = true;
                        foreach (Option option in options)
                        {
                            option.relativePosition.X = option.RelativePosition.X / 2;
                            option.relativePosition.Y = option.RelativePosition.Y - boundingBox.Height / 2;
                        }
                    }
                    else if (keyboard.IsKeyUp(Keys.Enter) && inFocus == true)
                    {
                        speed = 5;
                        inFocus = false;
                        foreach (Option option in options)
                        {
                            option.relativePosition.X = option.RelativePosition.X * 2;
                            option.relativePosition.Y = option.RelativePosition.Y + boundingBox.Height / 2;
                        }
                    }

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
                        Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet"], new Vector2(Position.X + 10, Position.Y - 20), -40, 90, this, actionList));
                        Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet"], new Vector2(Position.X - 10, Position.Y - 20), -40, 90, this, actionList));
                        if (inFocus == false)
                            foreach (Option option in options)
                                Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet3"], new Vector2(option.Position.X, option.Position.Y - 20), -40, 90 + option.RelativePosition.X / 4, this, actionList));
                        if (inFocus == true)
                            foreach (Option option in options)
                                Game1.objectManager.Add(new Bullet(Game1.spriteDict["bullet2"], new Vector2(option.Position.X, option.Position.Y - 20), -40, 90 + option.RelativePosition.X / 4, this, actionList, true));
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

        public float Speed { get { return speed; } }
    }
}
