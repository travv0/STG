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

        private static int windowWidth;
        private static int windowHeight;

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
            windowWidth = GraphicsDevice.Viewport.Width;
            windowHeight = GraphicsDevice.Viewport.Height;

            IsFixedTimeStep = false;

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

            FPSfont = Content.Load<SpriteFont>("FPS");

            //option texture
            SpriteDict["option"] = new Sprite(Content.Load<Texture2D>("option"));

            //player 1 stuff
            SpriteDict["CloudGirlAnimation"] = new Sprite(Content.Load<Texture2D>("attack sprites\\CloudGirlAnimation"), 3, 5);
            player1 = new Player(SpriteDict["CloudGirlAnimation"], Player.PlayerNum.One, new Vector2(WindowWidth / 2, 200), 5, 5);
            ObjectManager.Add(player1);

            //player 2 stuff
            SpriteDict["peanutBallerina"] = new Sprite(Content.Load<Texture2D>("boss sprites\\peanutBallerina"));
            player2 = new Player(SpriteDict["peanutBallerina"], Player.PlayerNum.Two, new Vector2(WindowWidth / 2, 100));
            ObjectManager.Add(player2);

            //bullet textures
            SpriteDict["umbrellaBullet"] = new Sprite(Content.Load<Texture2D>("bullet sprites\\umbrellaBullet"));
            SpriteDict["duckyBullet"] = new Sprite(Content.Load<Texture2D>("bullet sprites\\duckyBullet"));
            SpriteDict["bullet3"] = new Sprite(Content.Load<Texture2D>("bullet3"));
            //hitbox texture
            SpriteDict["hitbox"] = new Sprite(Content.Load<Texture2D>("hitbox"));

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

            //call all objects' draw function
            ObjectManager.Draw(spriteBatch);

            spriteBatch.Begin();

            spriteBatch.DrawString(FPSfont, "FPS: " + drawFPS.ToString("0.0"), new Vector2(16, 16), Color.White);
            spriteBatch.DrawString(FPSfont, "Dropped frames: " + drawDropped.ToString("P"), new Vector2(16, 32), Color.White);
            spriteBatch.DrawString(FPSfont, "Object count: " + ObjectManager.Count, new Vector2(16, 48), Color.White);

            spriteBatch.End();

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
    }
}
