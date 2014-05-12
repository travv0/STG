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
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //FPS stuff
        SpriteFont FPSfont;
        float FPS, drawFPS, droppedFrames, droppedPercent, drawDropped;
        int totalFrames;

        public enum GameStates { TitleScreen, Playing, GameOver, GameWin, CreditPage, InstructionPage };
        public static GameStates gameState;

        private static int windowWidth;
        private static int windowHeight;
        private static Rectangle playingArea = new Rectangle(20, 20, 620, 680);

        Texture2D titleBackgroundTexture;
        Rectangle titleViewportRect;

        Texture2D startButton;
        Rectangle startRect;
        Texture2D quitButton;
        Rectangle quitRect;

        Texture2D creditPage;
        Rectangle creditPageRect;

        Texture2D instructionPage;
        Rectangle instructionPageRect;

        Texture2D player1Win;
        Rectangle player1WinRect;

        Texture2D player2Win;
        Rectangle player2WinRect;

        private ScrollingBackground scrollBack;
        Texture2D scrollTexture;

        Song titleSong;
        bool titleSongStart;
        Song gameBGSong;
        bool gameSongStart;

        public static SoundEffect shootSound;
        public static SoundEffectInstance shootSoundInstance;
        public static SoundEffect bombSound;
        public static SoundEffectInstance bombSoundInstance;


        public static GameObjectManager objectManager;
        private static Dictionary<string, Sprite> spriteDict;

        int solTime = 0;
        int lunaTime = 0;

        /// <summary>
        /// Player one.
        /// </summary>
        public static Player player1;

        /// <summary>
        /// Player two.
        /// </summary>
        public static Player player2;

        public static Pawn Luna;

        public static Pawn Sol;

        /// <summary>
        /// Initializes the game.
        /// </summary>
        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //Set the screen height and width       
            graphics.PreferredBackBufferWidth = 960;
            graphics.PreferredBackBufferHeight = 720;

            //Apply the changes made to the device
            graphics.ApplyChanges();

            windowWidth = GraphicsDevice.Viewport.Width;
            windowHeight = GraphicsDevice.Viewport.Height;

            IsFixedTimeStep = false;

            this.IsMouseVisible = true;

            base.Initialize();
        }
        
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            totalFrames = 0;

            gameState = GameStates.TitleScreen;

            spriteDict = new Dictionary<string, Sprite>();
            objectManager = new GameObjectManager();

            titleSongStart = false;
            gameSongStart = false;

            titleBackgroundTexture =
                    Content.Load<Texture2D>("BG");

            startButton =
                    Content.Load<Texture2D>("StartButton");
            quitButton =
                    Content.Load<Texture2D>("QuitButton");

            titleViewportRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            startRect = new Rectangle((graphics.GraphicsDevice.Viewport.Width / 2 - (startButton.Width / 2)), graphics.GraphicsDevice.Viewport.Height / 3, startButton.Width, startButton.Height);

            quitRect = new Rectangle((graphics.GraphicsDevice.Viewport.Width / 2 - (quitButton.Width / 2)), graphics.GraphicsDevice.Viewport.Height * 2 / 3, quitButton.Width, quitButton.Height);

            creditPage = Content.Load<Texture2D>("credits");

            creditPageRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            instructionPage = Content.Load<Texture2D>("Instruction Page");

            instructionPageRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);


            player1Win = Content.Load<Texture2D>("Player1Win");

            player1WinRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            player2Win = Content.Load<Texture2D>("Player2Win");

            player2WinRect = new Rectangle(0, 0,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);

            scrollBack = new ScrollingBackground();
            scrollTexture = Content.Load<Texture2D>("clouds");
            scrollBack.Load(GraphicsDevice, scrollTexture);

            titleSong = Content.Load<Song>("Music and Sound\\dearly-beloved");
            gameBGSong = Content.Load<Song>("Music and Sound\\FireAndFlame");

            shootSound = Content.Load<SoundEffect>("Music and Sound\\ssw");
            shootSoundInstance = shootSound.CreateInstance();

            bombSound = Content.Load<SoundEffect>("Music and Sound\\bombSound");
            bombSoundInstance = bombSound.CreateInstance();


            FPSfont = Content.Load<SpriteFont>("FPS");



            //background stuff
            SpriteDict["HUD"] = new Sprite(Content.Load<Texture2D>("HUD2"));
            SpriteDict["bomb"] = new Sprite(Content.Load<Texture2D>("Item sprites\\bomb"));
            SpriteDict["lifeHeart"] = new Sprite(Content.Load<Texture2D>("Item sprites\\lifeHeart"));

            //option texture
            SpriteDict["option"] = new Sprite(Content.Load<Texture2D>("option"));

            //bullet textures
            SpriteDict["umbrellaBullet"] = new Sprite(Content.Load<Texture2D>("bullet sprites\\umbrellaBullet"));
            SpriteDict["duckyBullet"] = new Sprite(Content.Load<Texture2D>("bullet sprites\\duckyBullet"));
            SpriteDict["prettyArrowBullet"] = new Sprite(Content.Load<Texture2D>("bullet sprites\\prettyArrowBullet"));
            SpriteDict["suns"] = new Sprite(Content.Load<Texture2D>("bullet sprites\\suns"));
            SpriteDict["moons"] = new Sprite(Content.Load<Texture2D>("bullet sprites\\moons"));

            //player 1 stuff

            SpriteDict["CloudGirlAnimation"] = new Sprite(Content.Load<Texture2D>("attack sprites\\CloudGirlAnimation"), 4, 5);

            player1 = new Player(SpriteDict["CloudGirlAnimation"], Player.PlayerNum.One, new Vector2(playingArea.X + playingArea.Width / 2, playingArea.Y + playingArea.Height - ((float)SpriteDict["CloudGirlAnimation"].Height * 1.5f)), 5, 5);
            player1.setHealth(1);
            player1.setLives(3);
            ObjectManager.Add(player1);

            //player 2 stuff
            SpriteDict["peanutBallerina"] = new Sprite(Content.Load<Texture2D>("boss sprites\\peanutBallerina"));
            player2 = new Player(SpriteDict["peanutBallerina"], Player.PlayerNum.Two, new Vector2(playingArea.X + playingArea.Width / 2, 100));
            player2.setHealth(200);
            player2.setLives(1);
            ObjectManager.Add(player2);

            //Luna Stuff
            SpriteDict["moonGirl"] = new Sprite(Content.Load<Texture2D>("moonGirl"));
            Luna = new Pawn(SpriteDict["moonGirl"], new Vector2(playingArea.Width + 100, -100), SpriteDict["moons"], 666);
            ObjectManager.Add(Luna);

            //Sol Stuff
            SpriteDict["sunGirl"] = new Sprite(Content.Load<Texture2D>("sunGirl"));
            Sol = new Pawn(SpriteDict["sunGirl"], new Vector2(-100, -100), SpriteDict["suns"], 999);
            ObjectManager.Add(Sol);

            
            //hitbox texture
            SpriteDict["hitbox"] = new Sprite(Content.Load<Texture2D>("hitbox"));

            //Bomb Texture
            SpriteDict["bombRad"] = new Sprite(Content.Load<Texture2D>("bombRad"));

            //Item textures
            SpriteDict["FullItem"] = new Sprite(Content.Load<Texture2D>("Item sprites\\fullPower"));
            SpriteDict["LargeItem"] = new Sprite(Content.Load<Texture2D>("Item sprites\\largePower"));
            SpriteDict["SmallItem"] = new Sprite(Content.Load<Texture2D>("Item sprites\\smallPower"));

            //Item Spawner
            ObjectManager.Add(new ItemSpawner());

            

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            MouseState mouse = Mouse.GetState();
            KeyboardState keyboard = Keyboard.GetState();
            switch (gameState)
            {
                // title screen section 
                case GameStates.TitleScreen:
                    mouse = Mouse.GetState();
                    this.IsMouseVisible = true;

                    if (!titleSongStart)
                    {
                        MediaPlayer.Play(titleSong);
                        titleSongStart = true;
                    }

                    if ((mouse.LeftButton == ButtonState.Pressed) && (startRect.Contains(mouse.X, mouse.Y)))
                    {
                        gameState = GameStates.Playing;
                        this.IsMouseVisible = false;
                    }
                    if((keyboard.IsKeyDown(Keys.Enter)) && (gameState == GameStates.TitleScreen))
                    {
                        gameState = GameStates.Playing;
                        MediaPlayer.Stop();
                        this.IsMouseVisible = false;
                    }
                    if ((keyboard.IsKeyDown(Keys.D1)) && (gameState == GameStates.TitleScreen))
                    {
                        gameState = GameStates.InstructionPage;
                    }
                    if ((keyboard.IsKeyDown(Keys.D2)) && (gameState == GameStates.TitleScreen))
                    {
                        gameState = GameStates.CreditPage;
                    }
                    if ((mouse.LeftButton == ButtonState.Pressed) && (quitRect.Contains(mouse.X, mouse.Y)))
                    {
                        this.Exit();
                    }
                    break;
                // end of title screen section
                // game play section
                case GameStates.Playing:
                    if (keyboard.IsKeyDown(Keys.R))
                    {
                        MainGame.ObjectManager.moveAllBoxes('B');
                        LoadContent();
                    }
                    if (keyboard.IsKeyDown(Keys.Escape))
                    {
                        gameState = GameStates.TitleScreen;
                    }

                    //call all objects' update function
                    ObjectManager.Update();
                    scrollBack.Update(elapsed * 75);

                    if (objectManager.Find(Sol) == null)
                    {
                        solTime++;
                        if (solTime == 1800)
                        {
                            solTime = 0;
                            Sol = new Pawn(SpriteDict["sunGirl"], new Vector2(-100, -100), SpriteDict["suns"], 999);
                            ObjectManager.Add(Sol);
                        }
                    }

                    if (objectManager.Find(Luna) == null)
                    {
                        lunaTime++;
                        if (lunaTime == 1800)
                        {
                            lunaTime = 0;
                            Luna = new Pawn(SpriteDict["moonGirl"], new Vector2(playingArea.Width + 100, -100), SpriteDict["moons"], 666);
                            ObjectManager.Add(Luna);
                        }
                    }

                    if (!gameSongStart)
                    {
                        MediaPlayer.Play(gameBGSong);
                        gameSongStart = true;
                    }

                    FPS = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            
                    if (FPS <= 60)
                        droppedFrames += (60 - FPS) / 60;
                    totalFrames++;

                    droppedPercent = droppedFrames / totalFrames;
            
                    if (totalFrames % 60 == 0)
                    {
                        drawFPS = FPS;
                        drawDropped = droppedPercent;
                    }

                    break;

                case GameStates.CreditPage:

                    KeyboardState keyboard2 = Keyboard.GetState();

                    if ((keyboard2.IsKeyDown(Keys.Escape)) && (gameState == GameStates.CreditPage))
                    {
                        gameState = GameStates.TitleScreen;
                    }

                    break;

                case GameStates.InstructionPage:

                    KeyboardState keyboard3 = Keyboard.GetState();

                    if ((keyboard3.IsKeyDown(Keys.Escape)) && (gameState == GameStates.InstructionPage))
                    {
                        gameState = GameStates.TitleScreen;
                    }

                    break;

                case GameStates.GameWin:
                    keyboard = Keyboard.GetState();
                    mouse = Mouse.GetState();
                    this.IsMouseVisible = true;

                    if (keyboard.IsKeyDown(Keys.Enter))
                    {
                        this.Exit();
                    }

                    if ((mouse.LeftButton == ButtonState.Pressed) && (quitRect.Contains(mouse.X, mouse.Y)))
                    {
                        this.Exit();
                    }
                    
                    break;

                case GameStates.GameOver:
                    keyboard = Keyboard.GetState();
                    mouse = Mouse.GetState();
                    this.IsMouseVisible = true;

                    if (keyboard.IsKeyDown(Keys.Enter))
                    {
                        this.Exit();
                    }

                    if ((mouse.LeftButton == ButtonState.Pressed) && (quitRect.Contains(mouse.X, mouse.Y)))
                    {
                        this.Exit();
                    }

                    break;
            }

            

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            

            if (gameState == GameStates.TitleScreen)//drawing for title screen
            {
                spriteBatch.Begin();

                //Draw the backgroundTexture sized to the width
                //and height of the screen.
                spriteBatch.Draw(titleBackgroundTexture, titleViewportRect,
                    Color.White);

                spriteBatch.Draw(startButton, startRect, Color.CornflowerBlue);

                spriteBatch.Draw(quitButton, quitRect, Color.CornflowerBlue);

                spriteBatch.End();
            }

            if (gameState == GameStates.Playing)
            {
                spriteBatch.Begin();

                scrollBack.Draw(spriteBatch);

                spriteBatch.DrawString(FPSfont, "FPS: " + drawFPS.ToString("0.0"), new Vector2(16, 16), Color.Black);
                spriteBatch.DrawString(FPSfont, "Dropped frames: " + drawDropped.ToString("P"), new Vector2(16, 32), Color.Black);
                spriteBatch.DrawString(FPSfont, "Object count: " + ObjectManager.Count, new Vector2(16, 48), Color.Black);
                spriteBatch.DrawString(FPSfont, "Power Level: " + player1.Power, new Vector2(16, 64), Color.Black);
                
                spriteBatch.DrawString(FPSfont, "Player Lives: " + player1.Lives, new Vector2(16, 96), Color.Black);
                spriteBatch.DrawString(FPSfont, "Timer: " + player1.invincibilityTimer, new Vector2(16, 112), Color.Black);
                


                spriteBatch.End();

                //call all objects' draw function
                ObjectManager.Draw(spriteBatch);

                spriteBatch.Begin();

                spriteDict["HUD"].Draw(spriteBatch, new Rectangle(0, 0, windowWidth, WindowHeight), Color.White);

                spriteBatch.DrawString(FPSfont, "Player 1", new Vector2(WindowWidth - 200, WindowHeight / 2 - 60), Color.Black);
                spriteBatch.DrawString(FPSfont, "Lives: ", new Vector2(WindowWidth - 225, WindowHeight / 2 - 30), Color.Black);
                spriteBatch.DrawString(FPSfont, "Bomb: ", new Vector2(WindowWidth - 225, WindowHeight / 2), Color.Black);
                

                spriteBatch.DrawString(FPSfont, "Player 2", new Vector2(WindowWidth - 200, WindowHeight / 2 - 200), Color.Black);
                spriteBatch.DrawString(FPSfont, "Boss Health: " + player2.Health, new Vector2(WindowWidth - 225, WindowHeight / 2 - 150), Color.Black);

                if (player1.Bombs == 3)
                {
                    spriteDict["bomb"].Draw(spriteBatch, new Rectangle((WindowWidth - 170), (windowHeight / 2) - 5, spriteDict["bomb"].Width, spriteDict["bomb"].Height), Color.White);
                    spriteDict["bomb"].Draw(spriteBatch, new Rectangle((WindowWidth - 140), (windowHeight / 2) - 5, spriteDict["bomb"].Width, spriteDict["bomb"].Height), Color.White);
                    spriteDict["bomb"].Draw(spriteBatch, new Rectangle((WindowWidth - 110), (windowHeight / 2) - 5, spriteDict["bomb"].Width, spriteDict["bomb"].Height), Color.White);
                }

                if (player1.Bombs == 2)
                {
                    spriteDict["bomb"].Draw(spriteBatch, new Rectangle((WindowWidth - 170), (windowHeight / 2) - 5, spriteDict["bomb"].Width, spriteDict["bomb"].Height), Color.White);
                    spriteDict["bomb"].Draw(spriteBatch, new Rectangle((WindowWidth - 140), (windowHeight / 2) - 5, spriteDict["bomb"].Width, spriteDict["bomb"].Height), Color.White);
                }

                if (player1.Bombs == 1)
                {
                    spriteDict["bomb"].Draw(spriteBatch, new Rectangle((WindowWidth - 170), (windowHeight / 2) - 5, spriteDict["bomb"].Width, spriteDict["bomb"].Height), Color.White);
                }

                if (player1.Lives == 3)
                {
                    spriteDict["lifeHeart"].Draw(spriteBatch, new Rectangle((WindowWidth - 170), (windowHeight / 2) - 35, spriteDict["lifeHeart"].Width, spriteDict["lifeHeart"].Height), Color.White);
                    spriteDict["lifeHeart"].Draw(spriteBatch, new Rectangle((WindowWidth - 140), (windowHeight / 2) - 35, spriteDict["lifeHeart"].Width, spriteDict["lifeHeart"].Height), Color.White);
                    spriteDict["lifeHeart"].Draw(spriteBatch, new Rectangle((WindowWidth - 110), (windowHeight / 2) - 35, spriteDict["lifeHeart"].Width, spriteDict["lifeHeart"].Height), Color.White);
                }

                if (player1.Lives == 2)
                {
                    spriteDict["lifeHeart"].Draw(spriteBatch, new Rectangle((WindowWidth - 170), (windowHeight / 2) - 35, spriteDict["lifeHeart"].Width, spriteDict["lifeHeart"].Height), Color.White);
                    spriteDict["lifeHeart"].Draw(spriteBatch, new Rectangle((WindowWidth - 140), (windowHeight / 2) - 35, spriteDict["lifeHeart"].Width, spriteDict["lifeHeart"].Height), Color.White);
                }

                if (player1.Lives == 1)
                {
                    spriteDict["lifeHeart"].Draw(spriteBatch, new Rectangle((WindowWidth - 170), (windowHeight / 2) - 35, spriteDict["lifeHeart"].Width, spriteDict["lifeHeart"].Height), Color.White);
                }

                spriteBatch.End();
            }

            if (gameState == GameStates.CreditPage)
            {
                spriteBatch.Begin();

                //Draw the backgroundTexture sized to the width
                //and height of the screen.
                spriteBatch.Draw(creditPage, creditPageRect,
                    Color.White);

                spriteBatch.End();
            }

            if (gameState == GameStates.InstructionPage)
            {
                spriteBatch.Begin();

                //Draw the backgroundTexture sized to the width
                //and height of the screen.
                spriteBatch.Draw(instructionPage, instructionPageRect,
                    Color.White);

                spriteBatch.End();
            }

            if (gameState == GameStates.GameWin)
            {
                spriteBatch.Begin();

                //Draw the backgroundTexture sized to the width
                //and height of the screen.
                spriteBatch.Draw(player1Win, player1WinRect,
                    Color.White);

                spriteBatch.Draw(quitButton, quitRect, Color.CornflowerBlue);

                spriteBatch.End();
            }

            if (gameState == GameStates.GameOver)
            {
                spriteBatch.Begin();

                //Draw the backgroundTexture sized to the width
                //and height of the screen.
                spriteBatch.Draw(player2Win, player2WinRect,
                    Color.White);

                spriteBatch.Draw(quitButton, quitRect, Color.CornflowerBlue);

                spriteBatch.End();
            }
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        /// <summary>
        /// A list of every object in the game.
        /// </summary>
        public static GameObjectManager ObjectManager { get { return objectManager; } }

        /// <summary>
        /// A dictionary of every sprite in the game.  The index should have the same name as the file.
        /// </summary>
        public static Dictionary<string, Sprite> SpriteDict { get { return spriteDict; } }

        /// <summary>
        /// The width of the game window.
        /// </summary>
        public static int WindowWidth { get { return windowWidth; } }

        /// <summary>
        /// The height of the game window.
        /// </summary>
        public static int WindowHeight { get { return windowHeight; } }

        /// <summary>
        /// The ractangle containing the playing area.
        /// </summary>
        public static Rectangle PlayingArea { get { return playingArea; } }
    }
}
