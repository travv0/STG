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
        float power = 0f;
        float maxRotation = .2f; //max number of degrees for player to turn when moving left and right

        int mainCooldown = 0; //frames until another bullet can be fired
        int optionCooldown = 0; //frames until another option bullet can be fired
        Stack<Option> options = new Stack<Option>();

        const float SPEED = 7; //player's speed
        float speed = SPEED; //change to player's x and y value (smaller if moving diagonal)
        bool againstWall = false;
        int health;
        int lives;
        public bool invincible = false;
        public int invincibilityTimer = 0;
        Vector2 startingPos;

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
            startingPos = pos;
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
            startingPos = pos;
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
            Option tempOption1, tempOption2, tempOption3;
            bool startFocus = false;
            if (playerNum == PlayerNum.One)
            {
                GameObject hitBullet = Collides('B');
                if (hitBullet != null && invincible == false)
                {
                    if (this != ((Bullet)hitBullet).Parent)
                    {
                        MainGame.ObjectManager.Remove(hitBullet);
                        lives--;
                        invincible = true;
                        MainGame.ObjectManager.DeleteAll('B');
                        power--;
                        if (lives <= 0)
                        {
                            MainGame.ObjectManager.Remove(this);
                            MediaPlayer.Stop();
                            MainGame.gameState = MainGame.GameStates.GameOver;
                        }
                        this.pos = startingPos;
                    }
                    
                }
                else if(invincible == true)
                {
                    invincibilityTimer++;
                    if (invincibilityTimer == 120)
                    {
                        invincibilityTimer = 0;
                        invincible = false;
                    }
                }

                GameObject hitPower = Collides('C');
                if (hitPower != null)
                {
                    MainGame.ObjectManager.Remove(hitPower);
                    if(power < 3)
                        power += ((CollectibleItem)hitPower).powerLevel;
                }
            }

            if (playerNum == PlayerNum.Two)
            {
                GameObject hitBullet = Collides('B');
                if (hitBullet != null)
                {
                    if (this != ((Bullet)hitBullet).Parent && ((Bullet)hitBullet).Parent != MainGame.Sol && ((Bullet)hitBullet).Parent != MainGame.Luna)
                    {
                        //this.sprite = MainGame.SpriteDict["hitbox"];
                        MainGame.ObjectManager.Remove(hitBullet);
                        health--;
                        if (health == 0)
                            lives--;
                        if (lives <= 0)
                        {
                            MainGame.ObjectManager.Remove(this);
                            MediaPlayer.Stop();
                            MainGame.gameState = MainGame.GameStates.GameWin;
                        }
                    }
                }
            }

            switch (playerNum)
            {
                case PlayerNum.One:
                    if (keyboard.IsKeyDown(Keys.O) && keyboard.IsKeyUp(Keys.P) && power > 0)
                        power -= 0.0083f;
                    if (keyboard.IsKeyDown(Keys.P) && keyboard.IsKeyUp(Keys.O) && power < 3)
                        power += 0.01f;
                    if (keyboard.IsKeyDown(Keys.I))
                        power = 3;
                    if (keyboard.IsKeyDown(Keys.U))
                        power = 0;

                    //changing options at different powers
                    if ((power >= 0 && power < 1) && options.Count != 0)
                    {
                        while (options.Count > 0)
                            MainGame.ObjectManager.Remove(options.Pop());
                    }
                    if ((power >= 1 && power < 2) && options.Count != 2)
                    {
                        if (options.Count < 2)
                        {
                            if (inFocus == true)
                            {
                                tempOption1 = new Option(this, MainGame.SpriteDict["option"], new Vector2(-30 * 3 / 4, 0 - boundingBox.Height / 2));
                                startFocus = true;
                            }

                            else
                                tempOption1 = new Option(this, MainGame.SpriteDict["option"], new Vector2(-30, 0));

                            options.Push(tempOption1);
                            MainGame.ObjectManager.Add(tempOption1);
                            //start them in focus mode if player is in focus
                            if (inFocus == true && startFocus == false)
                            {
                                tempOption1.relativePosition.X = tempOption1.RelativePosition.X * 3 / 4;
                                tempOption1.relativePosition.Y = tempOption1.RelativePosition.Y - boundingBox.Height / 2;
                            }

                            if (inFocus == true)
                            {
                                tempOption1 = new Option(this, MainGame.SpriteDict["option"], new Vector2((30 * 3 / 4), (0 - boundingBox.Height / 2)));
                                startFocus = true;
                            }

                            else
                                tempOption1 = new Option(this, MainGame.SpriteDict["option"], new Vector2(30, 0));

                            options.Push(tempOption1);
                            MainGame.ObjectManager.Add(tempOption1);
                            //start them in focus mode if player is in focus
                            if (inFocus == true && startFocus == false)
                            {
                                tempOption1.relativePosition.X = tempOption1.RelativePosition.X * 3 / 4;
                                tempOption1.relativePosition.Y = tempOption1.RelativePosition.Y - boundingBox.Height / 2;
                            }
                            

                        }
                        else while (options.Count > 2)
                                MainGame.ObjectManager.Remove(options.Pop());

                        
                    }
                    if ((power >= 2 && power < 3) && options.Count != 4)
                    {
                        if (options.Count < 2)
                        {
                            if (inFocus == true)
                            {
                                tempOption1 = new Option(this, MainGame.SpriteDict["option"], new Vector2(-30 * 3 / 4, 0 - boundingBox.Height / 2));
                                startFocus = true;
                            }

                            else
                                tempOption1 = new Option(this, MainGame.SpriteDict["option"], new Vector2(-30, 0));

                            options.Push(tempOption1);
                            MainGame.ObjectManager.Add(tempOption1);
                            //start them in focus mode if player is in focus
                            if (inFocus == true && startFocus == false)
                            {
                                tempOption1.relativePosition.X = tempOption1.RelativePosition.X * 3 / 4;
                                tempOption1.relativePosition.Y = tempOption1.RelativePosition.Y - boundingBox.Height / 2;
                            }

                            if (inFocus == true)
                            {
                                tempOption1 = new Option(this, MainGame.SpriteDict["option"], new Vector2(30 * 3 / 4, 0 - boundingBox.Height / 2));
                                startFocus = true;
                            }

                            else
                                tempOption1 = new Option(this, MainGame.SpriteDict["option"], new Vector2(30, 0));

                            options.Push(tempOption1);
                            MainGame.ObjectManager.Add(tempOption1);
                            //start them in focus mode if player is in focus
                            if (inFocus == true && startFocus == false)
                            {
                                tempOption1.relativePosition.X = tempOption1.RelativePosition.X * 3 / 4;
                                tempOption1.relativePosition.Y = tempOption1.RelativePosition.Y - boundingBox.Height / 2;
                            }

                        }
                        else while (options.Count > 2)
                                MainGame.ObjectManager.Remove(options.Pop());

                        ////////////////////////////////////////////

                        if (options.Count < 4)
                        {
                            if(inFocus == true)
                            {
                                tempOption2 = new Option(this, MainGame.SpriteDict["option"], new Vector2(-50 * 3 / 4, 10 - boundingBox.Height / 2));
                                startFocus = true;
                            }
                            else
                                tempOption2 = new Option(this, MainGame.SpriteDict["option"], new Vector2(-50, 10));

                            options.Push(tempOption2);
                            MainGame.ObjectManager.Add(tempOption2);
                            //start them in focus mode if player is in focus
                            if (inFocus == true && startFocus == false)
                            {
                                tempOption2.relativePosition.X = tempOption2.RelativePosition.X * 3 / 4;
                                tempOption2.relativePosition.Y = tempOption2.RelativePosition.Y - boundingBox.Height / 2;
                            }

                            if(inFocus == true)
                            {
                                tempOption2 = new Option(this, MainGame.SpriteDict["option"], new Vector2(50 * 3 / 4, 10 - boundingBox.Height / 2));
                                startFocus = true;
                            }
                            else
                                tempOption2 = new Option(this, MainGame.SpriteDict["option"], new Vector2(50, 10));

                            options.Push(tempOption2);
                            MainGame.ObjectManager.Add(tempOption2);
                            //start them in focus mode if player is in focus
                            if (inFocus == true && startFocus == false)
                            {
                                tempOption2.relativePosition.X = tempOption2.RelativePosition.X * 3 / 4;
                                tempOption2.relativePosition.Y = tempOption2.RelativePosition.Y - boundingBox.Height / 2;
                            }

                        }
                        else while (options.Count > 4)
                                MainGame.ObjectManager.Remove(options.Pop());
                        //////////////////////////////////////////////////////////
                    }
                    if (power >= 3 && options.Count != 6)
                    {
                        if (options.Count < 2)
                        {
                            if (inFocus == true)
                            {
                                tempOption1 = new Option(this, MainGame.SpriteDict["option"], new Vector2(-30 * 3 / 4, 0 - boundingBox.Height / 2));
                                startFocus = true;
                            }

                            else
                                tempOption1 = new Option(this, MainGame.SpriteDict["option"], new Vector2(-30, 0));

                            options.Push(tempOption1);
                            MainGame.ObjectManager.Add(tempOption1);
                            //start them in focus mode if player is in focus
                            if (inFocus == true && startFocus == false)
                            {
                                tempOption1.relativePosition.X = tempOption1.RelativePosition.X * 3 / 4;
                                tempOption1.relativePosition.Y = tempOption1.RelativePosition.Y - boundingBox.Height / 2;
                            }

                            if (inFocus == true)
                            {
                                tempOption1 = new Option(this, MainGame.SpriteDict["option"], new Vector2(30 * 3 / 4, 0 - boundingBox.Height / 2));
                                startFocus = true;
                            }

                            else
                                tempOption1 = new Option(this, MainGame.SpriteDict["option"], new Vector2(30, 0));

                            options.Push(tempOption1);
                            MainGame.ObjectManager.Add(tempOption1);
                            //start them in focus mode if player is in focus
                            if (inFocus == true && startFocus == false)
                            {
                                tempOption1.relativePosition.X = tempOption1.RelativePosition.X * 3 / 4;
                                tempOption1.relativePosition.Y = tempOption1.RelativePosition.Y - boundingBox.Height / 2;
                            }

                        }
                        else while (options.Count > 2)
                                MainGame.ObjectManager.Remove(options.Pop());

                        ////////////////////////////////////////////

                        if (options.Count < 4)
                        {
                            if (inFocus == true)
                            {
                                tempOption2 = new Option(this, MainGame.SpriteDict["option"], new Vector2(-50 * 3 / 4, 10 - boundingBox.Height / 2));
                                startFocus = true;
                            }
                            else
                                tempOption2 = new Option(this, MainGame.SpriteDict["option"], new Vector2(-50, 10));

                            options.Push(tempOption2);
                            MainGame.ObjectManager.Add(tempOption2);
                            //start them in focus mode if player is in focus
                            if (inFocus == true && startFocus == false)
                            {
                                tempOption2.relativePosition.X = tempOption2.RelativePosition.X * 3 / 4;
                                tempOption2.relativePosition.Y = tempOption2.RelativePosition.Y - boundingBox.Height / 2;
                            }

                            if (inFocus == true)
                            {
                                tempOption2 = new Option(this, MainGame.SpriteDict["option"], new Vector2(50 * 3 / 4, 10 - boundingBox.Height / 2));
                                startFocus = true;
                            }
                            else
                                tempOption2 = new Option(this, MainGame.SpriteDict["option"], new Vector2(50, 10));

                            options.Push(tempOption2);
                            MainGame.ObjectManager.Add(tempOption2);
                            //start them in focus mode if player is in focus
                            if (inFocus == true && startFocus == false)
                            {
                                tempOption2.relativePosition.X = tempOption2.RelativePosition.X * 3 / 4;
                                tempOption2.relativePosition.Y = tempOption2.RelativePosition.Y - boundingBox.Height / 2;
                            }

                        }
                        else while (options.Count > 4)
                                MainGame.ObjectManager.Remove(options.Pop());
                        //////////////////////////////////////////////////////////

                        if (inFocus == true)
                        {
                            tempOption3 = new Option(this, MainGame.SpriteDict["option"], new Vector2(-70 * 3 / 4, 20 - boundingBox.Height / 2));
                            startFocus = true;
                        }
                        else
                            tempOption3 = new Option(this, MainGame.SpriteDict["option"], new Vector2(-70, 20));

                        options.Push(tempOption3);
                        MainGame.ObjectManager.Add(tempOption3);
                        //start them in focus mode if player is in focus
                        if (inFocus == true && startFocus == false)
                        {
                            tempOption3.relativePosition.X = tempOption3.RelativePosition.X * 3 / 4;
                            tempOption3.relativePosition.Y = tempOption3.RelativePosition.Y - boundingBox.Height / 2;
                        }


                        if (inFocus == true)
                        {
                            tempOption3 = new Option(this, MainGame.SpriteDict["option"], new Vector2(70 * 3 / 4, 20 - boundingBox.Height / 2));
                            startFocus = true;
                        }
                        else
                            tempOption3 = new Option(this, MainGame.SpriteDict["option"], new Vector2(70, 20));
                        options.Push(tempOption3);
                        MainGame.ObjectManager.Add(tempOption3);
                        //start them in focus mode if player is in focus
                        if (inFocus == true && startFocus == false)
                        {
                            tempOption3.relativePosition.X = tempOption3.RelativePosition.X * 3 / 4;
                            tempOption3.relativePosition.Y = tempOption3.RelativePosition.Y - boundingBox.Height / 2;
                        }

                    }

                    //going in and out of focus mode
                    if (keyboard.IsKeyDown(Keys.LeftShift))
                    {
                        speed = SPEED * (2 / 5.0f);
                        maxRotation = 0;
                        if (inFocus == false)
                            foreach (Option option in options)
                            {
                                option.relativePosition.X = option.RelativePosition.X * 3 / 4;
                                option.relativePosition.Y = option.RelativePosition.Y - boundingBox.Height / 2;
                            }
                        inFocus = true;
                    }
                    else if (keyboard.IsKeyUp(Keys.LeftShift))
                    {
                        speed = SPEED;
                        maxRotation = .2f;
                        if (inFocus == true)
                            foreach (Option option in options)
                            {
                                option.relativePosition.X = option.RelativePosition.X * 4 / 3;
                                option.relativePosition.Y = option.RelativePosition.Y + boundingBox.Height / 2;
                            }
                        inFocus = false;
                    }

                    if (rotation < -maxRotation)
                        rotation += 0.05f;
                    if (rotation > maxRotation)
                        rotation -= 0.05f;

                    //movement
                    if ((keyboard.IsKeyDown(Keys.A) && keyboard.IsKeyDown(Keys.W)
                        || keyboard.IsKeyDown(Keys.A) && keyboard.IsKeyDown(Keys.S)
                        || keyboard.IsKeyDown(Keys.W) && keyboard.IsKeyDown(Keys.D)
                        || keyboard.IsKeyDown(Keys.D) && keyboard.IsKeyDown(Keys.S))
                        && (speed == SPEED || speed == SPEED * (2 / 5.0f)))
                        speed = (float)(speed / Math.Sqrt(2));


                    if (keyboard.IsKeyDown(Keys.A) && pos.X - (sprite.Width / 2) + 20 > MainGame.PlayingArea.X)
                    {
                        pos.X -= speed;

                        if (rotation > -maxRotation)
                            rotation -= 0.05f;
                    }

                    if (keyboard.IsKeyDown(Keys.S) && pos.Y + sprite.Height / 2 < MainGame.PlayingArea.Y + MainGame.PlayingArea.Height)
                        pos.Y += speed;


                    if (keyboard.IsKeyDown(Keys.D) && pos.X + (sprite.Width / 2) - 20 < MainGame.PlayingArea.X + MainGame.PlayingArea.Width)
                    {
                        
                        pos.X += speed;

                        if (rotation < maxRotation)
                            rotation += 0.05f;
                    }

                    if (keyboard.IsKeyDown(Keys.W) && pos.Y - sprite.Height / 2 > MainGame.PlayingArea.Y)
                        pos.Y -= speed;

                    if (pos.X - (sprite.Width / 2) + 20 <= MainGame.PlayingArea.X || pos.X + (sprite.Width / 2) - 20 >= MainGame.PlayingArea.X + MainGame.PlayingArea.Width)
                        againstWall = true;
                    else
                        againstWall = false;

                    if ((!keyboard.IsKeyDown(Keys.A) && !keyboard.IsKeyDown(Keys.D)) || againstWall == true || ((keyboard.IsKeyDown(Keys.A) == true) && (keyboard.IsKeyDown(Keys.D) == true)))
                    {
                        if (rotation < 0)
                            rotation += 0.05f;
                        if (rotation > 0)
                            rotation -= 0.05f;                     
                    }


                    //shootin
                    if (keyboard.IsKeyDown(Keys.Space) && mainCooldown == 0)
                    {
                        MainGame.ObjectManager.Add(new Bullet(MainGame.SpriteDict["umbrellaBullet"], new Vector2(Position.X, Position.Y - 20), 20, 270, 0, this, null));
                        mainCooldown = 5;
                    }
                    if (keyboard.IsKeyDown(Keys.B) && optionCooldown == 0)
                    {
                        MainGame.ObjectManager.Add(new Bomb(MainGame.SpriteDict["bombRad"], new Vector2(Position.X, Position.Y), 5));
                        optionCooldown = 200;
                    }
                    if (keyboard.IsKeyDown(Keys.Space) && optionCooldown == 0)
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
                    if (keyboard.IsKeyDown(Keys.Left) && pos.X - sprite.Width / 2 > MainGame.PlayingArea.X)
                        pos.X -= speed;
                    if (keyboard.IsKeyDown(Keys.Down) && pos.Y + sprite.Height / 2 < MainGame.PlayingArea.Y + MainGame.PlayingArea.Height / 2)
                        pos.Y += speed;
                    if (keyboard.IsKeyDown(Keys.Right) && pos.X + sprite.Width / 2 < MainGame.PlayingArea.X + MainGame.PlayingArea.Width)
                        pos.X += speed;
                    if (keyboard.IsKeyDown(Keys.Up) && pos.Y - sprite.Height / 2 > MainGame.PlayingArea.Y)
                        pos.Y -= speed;

                    //shootin
                    if (keyboard.IsKeyDown(Keys.G) && mainCooldown == 0)
                    {
                        MainGame.ObjectManager.Add(new BossPatternMedium(this));
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

            if (invincibilityTimer % 2 == 0)
            {
                base.Draw(spriteBatch);

                //if (inFocus)
                //{
                spriteBatch.Begin();

                MainGame.SpriteDict["hitbox"].Draw(spriteBatch, new Rectangle((int)pos.X - 2, (int)pos.Y - 2, 4, 4), Color.White);

                spriteBatch.End();
                //}
            }
        }

        /// <summary>
        /// Returns the player's speed.
        /// </summary>
        public float Speed { get { return speed; } }

        /// <summary>
        /// Returns the player's power.
        /// </summary>
        public float Power { get { return power; } }

        /// <summary>
        /// Returns the player's health.
        /// </summary>
        public int Health { get { return health; } }

        /// <summary>
        /// Returns the player's lives.
        /// </summary>
        public int Lives { get { return lives; } }

        /// <summary>
        /// Sets the player's health.
        /// </summary>
        /// <param name="health"></param>
        public void setHealth(int health) { this.health = health; }
        
        /// <summary>
        /// Sets the player's lives.
        /// </summary>
        /// <param name="lives"></param>
        public void setLives(int lives) { this.lives = lives; }
    }
}
