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
    /// <summary>
    /// A playable character.
    /// </summary>
    public class Player:GameObject
    {
        /// <summary>
        /// The player number.
        /// </summary>
        public enum PlayerNum 
        {
            /// <summary>
            /// First player.
            /// </summary>
            One, 
            /// <summary>
            /// Second player.
            /// </summary>
            Two
        };
        PlayerNum playerNum;
        bool inFocus = false;
        int power = 0;
        float maxRotation = .2f; //max number of degrees for player to turn when moving left and right

        int mainCooldown = 0; //frames until another bullet can be fired
        int optionCooldown = 0; //frames until another option bullet can be fired
        Stack<Option> options = new Stack<Option>();

        float speed = 5; //player's speed
        float prevX;
        bool againstWall = false;

        /// <summary>
        /// A playable character.
        /// </summary>
        /// <param name="sprite">Character's sprite.</param>
        /// <param name="playerNum">Player one or player two.</param>
        /// <param name="pos">Player's position.</param>
        /// <param name="hitboxWidth">Width of the player's hitbox.</param>
        /// <param name="hitboxHeight">Height of the player's hitbox.</param>
        public Player(Sprite sprite, PlayerNum playerNum, Vector2 pos, int hitboxWidth, int hitboxHeight)
        {
            this.sprite = sprite;
            this.pos = pos;
            this.boundingBox = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);
            this.playerNum = playerNum;
        }

        /// <summary>
        /// A playable character.
        /// </summary>
        /// <param name="sprite">Character's sprite.</param>
        /// <param name="playerNum">Player one or player two.</param>
        /// <param name="boundingBox">The bounding box of the player.</param>
        /// <param name="hitboxWidth">The width of the player's hitbox.</param>
        /// <param name="hitboxHeight">The height of the player's hitbox.</param>
        public Player(Sprite sprite, PlayerNum playerNum, Rectangle boundingBox, int hitboxWidth, int hitboxHeight)
        {
            this.sprite = sprite;
            this.pos = new Vector2(boundingBox.X, boundingBox.Y);
            this.boundingBox = boundingBox;
            this.playerNum = playerNum;
        }

        /// <summary>
        /// A playable character.
        /// </summary>
        /// <param name="sprite">Character's sprite.</param>
        /// <param name="playerNum">Player one or player two.</param>
        /// <param name="pos">Player's position.</param>
        public Player(Sprite sprite, PlayerNum playerNum, Vector2 pos)
        {
            this.sprite = sprite;
            this.pos = pos;
            this.boundingBox = new Rectangle((int)pos.X, (int)pos.Y, sprite.Width, sprite.Height);
            this.playerNum = playerNum;
            objType = 'P';
        }
        
        /// <summary>
        /// A playable character.
        /// </summary>
        /// <param name="sprite">Character's sprite.</param>
        /// <param name="playerNum">Player one or player two.</param>
        /// <param name="boundingBox">The bounding box of the player.</param>
        public Player(Sprite sprite, PlayerNum playerNum, Rectangle boundingBox)
        {
            this.sprite = sprite;
            this.pos = new Vector2(boundingBox.X, boundingBox.Y);
            this.boundingBox = boundingBox;
            this.playerNum = playerNum;
            objType = 'P';
        }

        /// <summary>
        /// Runs on creation of a new player object, adds any options in the option list.
        /// </summary>
        protected override void Initialize()
        {
            foreach (Option option in options)
                MainGame.ObjectManager.Add(option);

            base.Initialize();
        }

        /// <summary>
        /// Updates the player object.
        /// </summary>
        public override void Update()
        {
            KeyboardState keyboard = Keyboard.GetState();
            Option tempOption;

            switch (playerNum)
            {
                case PlayerNum.One:
                    if (keyboard.IsKeyDown(Keys.Q) && power == 1)
                        power = 0;
                    if (keyboard.IsKeyDown(Keys.W) && (power == 0 || power == 2))
                        power = 1;
                    if (keyboard.IsKeyDown(Keys.E) && (power == 1 || power == 3))
                        power = 2;
                    if (keyboard.IsKeyDown(Keys.R) && power == 2)
                        power = 3;

                    //changing options at different powers
                    if (power == 0 && options.Count != 0)
                    {
                        while (options.Count > 0)
                            MainGame.ObjectManager.Remove(options.Pop());
                    }
                    if (power == 1 && options.Count != 2)
                    {
                        if (options.Count < 2)
                        {
                            tempOption = new Option(this, MainGame.SpriteDict["option"], new Vector2(-30, 0));
                            options.Push(tempOption);
                            MainGame.ObjectManager.Add(tempOption);
                            //start them in focus mode if player is in focus
                            if (inFocus == true)
                            {
                                tempOption.relativePosition.X = tempOption.RelativePosition.X * 3 / 4;
                                tempOption.relativePosition.Y = tempOption.RelativePosition.Y - boundingBox.Height / 2;
                            }

                            tempOption = new Option(this, MainGame.SpriteDict["option"], new Vector2(30, 0));
                            options.Push(tempOption);
                            MainGame.ObjectManager.Add(tempOption);
                            //start them in focus mode if player is in focus
                            if (inFocus == true)
                            {
                                tempOption.relativePosition.X = tempOption.RelativePosition.X * 3 / 4;
                                tempOption.relativePosition.Y = tempOption.RelativePosition.Y - boundingBox.Height / 2;
                            }

                        }
                        else while (options.Count > 2)
                                MainGame.ObjectManager.Remove(options.Pop());

                        
                    }
                    if (power == 2 && options.Count != 4)
                    {
                        if (options.Count < 4)
                        {
                            tempOption = new Option(this, MainGame.SpriteDict["option"], new Vector2(-50, 10));
                            options.Push(tempOption);
                            MainGame.ObjectManager.Add(tempOption);
                            //start them in focus mode if player is in focus
                            if (inFocus == true)
                            {
                                tempOption.relativePosition.X = tempOption.RelativePosition.X * 3 / 4;
                                tempOption.relativePosition.Y = tempOption.RelativePosition.Y - boundingBox.Height / 2;
                            }

                            tempOption = new Option(this, MainGame.SpriteDict["option"], new Vector2(50, 10));
                            options.Push(tempOption);
                            MainGame.ObjectManager.Add(tempOption);
                            //start them in focus mode if player is in focus
                            if (inFocus == true)
                            {
                                tempOption.relativePosition.X = tempOption.RelativePosition.X * 3 / 4;
                                tempOption.relativePosition.Y = tempOption.RelativePosition.Y - boundingBox.Height / 2;
                            }

                        }
                        else while (options.Count > 4)
                                MainGame.ObjectManager.Remove(options.Pop());
                    }
                    if (power == 3 && options.Count != 6)
                    {
                        tempOption = new Option(this, MainGame.SpriteDict["option"], new Vector2(-70, 20));
                        options.Push(tempOption);
                        MainGame.ObjectManager.Add(tempOption);
                        //start them in focus mode if player is in focus
                        if (inFocus == true)
                        {
                            tempOption.relativePosition.X = tempOption.RelativePosition.X * 3 / 4;
                            tempOption.relativePosition.Y = tempOption.RelativePosition.Y - boundingBox.Height / 2;
                        }

                        tempOption = new Option(this, MainGame.SpriteDict["option"], new Vector2(70, 20));
                        options.Push(tempOption);
                        MainGame.ObjectManager.Add(tempOption);
                        //start them in focus mode if player is in focus
                        if (inFocus == true)
                        {
                            tempOption.relativePosition.X = tempOption.RelativePosition.X * 3 / 4;
                            tempOption.relativePosition.Y = tempOption.RelativePosition.Y - boundingBox.Height / 2;
                        }

                    }

                    //going in and out of focus mode
                    if (keyboard.IsKeyDown(Keys.Enter) && inFocus == false)
                    {
                        speed = 2;
                        maxRotation = 0;
                        inFocus = true;
                        foreach (Option option in options)
                        {
                            option.relativePosition.X = option.RelativePosition.X * 3 / 4;
                            option.relativePosition.Y = option.RelativePosition.Y - boundingBox.Height / 2;
                        }
                    }
                    else if (keyboard.IsKeyUp(Keys.Enter) && inFocus == true)
                    {
                        speed = 5;
                        maxRotation = .2f;
                        inFocus = false;
                        foreach (Option option in options)
                        {
                            option.relativePosition.X = option.RelativePosition.X * 4 / 3;
                            option.relativePosition.Y = option.RelativePosition.Y + boundingBox.Height / 2;
                        }
                    }

                    if (rotation < -maxRotation)
                        rotation += 0.05f;
                    if (rotation > maxRotation)
                        rotation -= 0.05f;

                    //movement
                    if (keyboard.IsKeyDown(Keys.Left) && pos.X - (sprite.Width / 2) + 20 > MainGame.PlayingArea.X)
                    {
                        pos.X -= speed;

                        if (rotation > -maxRotation)
                            rotation -= 0.05f;
                    }

                    if (keyboard.IsKeyDown(Keys.Down) && pos.Y + sprite.Height / 2 < MainGame.PlayingArea.Y + MainGame.PlayingArea.Height)
                        pos.Y += speed;


                    if (keyboard.IsKeyDown(Keys.Right) && pos.X + (sprite.Width / 2) - 20 < MainGame.PlayingArea.X + MainGame.PlayingArea.Width)
                    {
                        
                        pos.X += speed;

                        if (rotation < maxRotation)
                            rotation += 0.05f;
                    }

                    if (keyboard.IsKeyDown(Keys.Up) && pos.Y - sprite.Height / 2 > MainGame.PlayingArea.Y)
                        pos.Y -= speed;

                    if (pos.X - (sprite.Width / 2) + 20 <= MainGame.PlayingArea.X || pos.X + (sprite.Width / 2) - 20 >= MainGame.PlayingArea.X + MainGame.PlayingArea.Width)
                        againstWall = true;
                    else
                        againstWall = false;

                    if ((!keyboard.IsKeyDown(Keys.Left) && !keyboard.IsKeyDown(Keys.Right)) || againstWall == true)
                    {
                        if (rotation < 0)
                            rotation += 0.05f;
                        if (rotation > 0)
                            rotation -= 0.05f;                     
                    }


                    //shootin
                    if (keyboard.IsKeyDown(Keys.NumPad1) && mainCooldown == 0)
                    {
                        MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["umbrellaBullet"], new Vector2(Position.X, Position.Y - 20), 20, 270, 0, this, null));
                        mainCooldown = 5;
                    }
                    if (keyboard.IsKeyDown(Keys.NumPad1) && optionCooldown == 0)
                    {
                        if (inFocus == false)
                            foreach (Option option in options)
                                MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["duckyBullet"], new Vector2(option.Position.X, option.Position.Y - 20), 10, 270 + option.RelativePosition.X / 4, 0, this, null, false, true));
                        if (inFocus == true)
                            foreach (Option option in options)
                                MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["duckyBullet"], new Vector2(option.Position.X, option.Position.Y - 20), 10, 270 + option.RelativePosition.X / 4, 0, this, null, true, true));
                        optionCooldown = 50;
                    }
                    if (mainCooldown > 0)
                        mainCooldown--;
                    if (optionCooldown > 0)
                        optionCooldown--;

                    break;

                case PlayerNum.Two:
                    //movement
                    if (keyboard.IsKeyDown(Keys.A) && pos.X - sprite.Width / 2 > MainGame.PlayingArea.X)
                        pos.X -= speed;
                    if (keyboard.IsKeyDown(Keys.S) && pos.Y + sprite.Height / 2 < MainGame.PlayingArea.Y + MainGame.PlayingArea.Height / 2)
                        pos.Y += speed;
                    if (keyboard.IsKeyDown(Keys.D) && pos.X + sprite.Width / 2 < MainGame.PlayingArea.X + MainGame.PlayingArea.Width)
                        pos.X += speed;
                    if (keyboard.IsKeyDown(Keys.W) && pos.Y - sprite.Height / 2 > MainGame.PlayingArea.Y)
                        pos.Y -= speed;

                    //shootin
                    if (keyboard.IsKeyDown(Keys.G) && mainCooldown == 0)
                    {
                        MainGame.ObjectManager.Add(new TestPattern(this));
                        mainCooldown = 50;
                    }
                    if (mainCooldown > 0)
                        mainCooldown--;

                    break;
            }

            base.Update();
        }

        /// <summary>
        /// Draws the hitbox on the player.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to be used for drawing.</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            //if (inFocus)
            //{
                spriteBatch.Begin();

                MainGame.SpriteDict["hitbox"].Draw(spriteBatch, new Rectangle((int)pos.X - 2, (int)pos.Y - 2, 4, 4), Color.White);

                spriteBatch.End();
            //}
        }

        /// <summary>
        /// Returns the player's speed.
        /// </summary>
        public float Speed { get { return speed; } }
    }
}
