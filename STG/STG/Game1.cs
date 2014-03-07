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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int windowWidth;
        public static int windowHeight;

        //static containers for accessing objects and textures
        public static GameObjectManager objectManager = new GameObjectManager();
        public static Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>(); //use filename as key

        public static Player player1;
        public static Player player2;

        public Game1()
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

            //option texture
            spriteDict["option"] = new Sprite(Content.Load<Texture2D>("option"));

            //player 1 stuff
            spriteDict["CloudGirlAnimation"] = new Sprite(Content.Load<Texture2D>("attack sprites\\CloudGirlAnimation"), 3, 5);
            player1 = new Player(spriteDict["CloudGirlAnimation"], Player.PlayerNum.One, new Vector2(windowWidth / 2, 200), 5, 5);
            objectManager.Add(player1);

            //player 2 stuff
            spriteDict["peanutBallerina"] = new Sprite(Content.Load<Texture2D>("boss sprites\\peanutBallerina"));
            player2 = new Player(spriteDict["peanutBallerina"], Player.PlayerNum.Two, new Vector2(windowWidth / 2, 100));
            objectManager.Add(player2);

            //bullet textures
            spriteDict["umbrellaBullet"] = new Sprite(Content.Load<Texture2D>("bullet sprites\\umbrellaBullet"));
            spriteDict["duckyBullet"] = new Sprite(Content.Load<Texture2D>("bullet sprites\\duckyBullet"));
            spriteDict["bullet3"] = new Sprite(Content.Load<Texture2D>("bullet3"));
            //hitbox texture
            spriteDict["hitbox"] = new Sprite(Content.Load<Texture2D>("hitbox"));

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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //call all objects' update function
            objectManager.Update();

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
            objectManager.Draw(spriteBatch);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
