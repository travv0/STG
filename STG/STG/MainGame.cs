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
        int totalFrames = 0;

        enum GameStates { TitleScreen, Playing };
        GameStates gameState = GameStates.TitleScreen;

        private static int windowWidth;
        private static int windowHeight;
        private static Rectangle playingArea = new Rectangle(20, 20, 620, 680);

        Texture2D titleBackgroundTexture;
        Rectangle titleViewportRect;

        Texture2D startButton;
        Rectangle startRect;
        Texture2D quitButton;
        Rectangle quitRect;

        private static GameObjectManager objectManager = new GameObjectManager();
        private static Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();

        /// <summary>
        /// Player one.
        /// </summary>
        public static Player player1;

        /// <summary>
        /// Player two.
        /// </summary>
        public static Player player2;

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

            FPSfont = Content.Load<SpriteFont>("FPS");

            //background stuff
            SpriteDict["HUD"] = new Sprite(Content.Load<Texture2D>("HUD"));

            //option texture
            SpriteDict["option"] = new Sprite(Content.Load<Texture2D>("option"));

            //player 1 stuff

            SpriteDict["CloudGirlAnimation"] = new Sprite(Content.Load<Texture2D>("attack sprites\\CloudGirlAnimation"), 4, 5);

            player1 = new Player(SpriteDict["CloudGirlAnimation"], Player.PlayerNum.One, new Vector2(playingArea.X + playingArea.Width / 2, playingArea.Y + playingArea.Height - ((float)SpriteDict["CloudGirlAnimation"].Height * 1.5f)), 5, 5);
            ObjectManager.Add(player1);

            //player 2 stuff
            SpriteDict["peanutBallerina"] = new Sprite(Content.Load<Texture2D>("boss sprites\\peanutBallerina"));
            player2 = new Player(SpriteDict["peanutBallerina"], Player.PlayerNum.Two, new Vector2(playingArea.X + playingArea.Width / 2, 100));
            ObjectManager.Add(player2);

            //bullet textures
            SpriteDict["umbrellaBullet"] = new Sprite(Content.Load<Texture2D>("bullet sprites\\umbrellaBullet"));
            SpriteDict["duckyBullet"] = new Sprite(Content.Load<Texture2D>("bullet sprites\\duckyBullet"));
            SpriteDict["prettyArrowBullet"] = new Sprite(Content.Load<Texture2D>("bullet sprites\\prettyArrowBullet"));
            //hitbox texture
            SpriteDict["hitbox"] = new Sprite(Content.Load<Texture2D>("hitbox"));

            //Item textures
            SpriteDict["FullItem"] = new Sprite(Content.Load<Texture2D>("Item sprites\\FullItem"));
            SpriteDict["LargeItem"] = new Sprite(Content.Load<Texture2D>("Item sprites\\LargeItem"));
            SpriteDict["SmallItem"] = new Sprite(Content.Load<Texture2D>("Item sprites\\SmallItem"));
            ObjectManager.Add(new CollectibleItem(SpriteDict["FullItem"], new Vector2(200, 20 + (SpriteDict["FullItem"].Height / 2)), 0, 5f)); //I adjusted starting position to fit in playing area

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
            //call all objects' update function
            ObjectManager.Update();

            switch (gameState)
            {
                // title screen section 
                case GameStates.TitleScreen:

                    MouseState mouse = Mouse.GetState();
                    KeyboardState keyboard = Keyboard.GetState();

                    if ((mouse.LeftButton == ButtonState.Pressed) && (startRect.Contains(mouse.X, mouse.Y)))
                    {
                        gameState = GameStates.Playing;
                        this.IsMouseVisible = false;
                    }
                    if((keyboard.IsKeyDown(Keys.Enter)) && (gameState == GameStates.TitleScreen))
                    {
                        gameState = GameStates.Playing;
                        this.IsMouseVisible = false;
                    }
                    if ((mouse.LeftButton == ButtonState.Pressed) && (quitRect.Contains(mouse.X, mouse.Y)))
                    {
                        this.Exit();
                    }
                    break;
                // end of title screen section
                // game play section
                case GameStates.Playing:
                    break;
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

                spriteBatch.DrawString(FPSfont, "FPS: " + drawFPS.ToString("0.0"), new Vector2(16, 16), Color.White);
                spriteBatch.DrawString(FPSfont, "Dropped frames: " + drawDropped.ToString("P"), new Vector2(16, 32), Color.White);
                spriteBatch.DrawString(FPSfont, "Object count: " + ObjectManager.Count, new Vector2(16, 48), Color.White);

                spriteBatch.End();

                //call all objects' draw function
                ObjectManager.Draw(spriteBatch);

                spriteBatch.Begin();

                spriteDict["HUD"].Draw(spriteBatch, new Rectangle(0, 0, windowWidth, WindowHeight), Color.White);

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
