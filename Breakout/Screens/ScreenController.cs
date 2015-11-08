using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Breakout
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>

    public enum GameState
    {
        MainMenu,
        InGame,
        EndGame,
        HighScore
    }

    public class ScreenController : Game
    {
        private GameState gameState;
        private InGameScreen inGameScreen;
        private MainMenuScreen mainMenuScreen;
        public EndGameScreen endGameScreen;
        private HighScoreScreen highScoreScreen;

        public Highscore highscore { get; private set; }
        
        public MouseState prevMouseState { get; private set; }
        public MouseState curMouseState { get; private set; }
        public KeyboardState prevKeyState { get; private set; }
        public KeyboardState curKeyState { get; private set; }

        public Texture2D menuButton { get; private set; }
        public Texture2D dotTexture { get; private set; }
        public Texture2D backgroundTexture { get; private set; }
        private Color[] backgroundData;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public ScreenController()
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
            this.IsMouseVisible = true;
            highscore = new Highscore();
            inGameScreen = new InGameScreen(this);
            mainMenuScreen = new MainMenuScreen(this);
            endGameScreen = new EndGameScreen(this);
            highScoreScreen = new HighScoreScreen(this);
            gameState = GameState.MainMenu;
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
            
            menuButton = RectangleBorder(200, 50, 5);

            dotTexture = new Texture2D(GraphicsDevice, 1, 1);
            dotTexture.SetData(new Color[1] { Color.White });
            backgroundTexture = new Texture2D(GraphicsDevice, GraphicsDevice.Viewport.Width/2, GraphicsDevice.Viewport.Height/2);
            backgroundData = new Color[backgroundTexture.Width * backgroundTexture.Height];

            float colorTime = (float)Math.Sin((double)DateTime.Now.Millisecond % Math.PI);
            for (int x = 0; x < backgroundTexture.Width; x++)
            {
                for (int y = 0; y < backgroundTexture.Height; y++){
                    int index = x + y*backgroundTexture.Width;
                    float colorX = (float)x / (backgroundTexture.Width);
                    float colorY = (float)y / (backgroundTexture.Height);
                    backgroundData[index] = new Color(colorX, colorY,  colorTime);
                }
            }
            backgroundTexture.SetData(backgroundData);

            mainMenuScreen.Initialize();

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            prevMouseState = curMouseState;
            curMouseState = Mouse.GetState();
            prevKeyState = curKeyState;
            curKeyState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //update the screen according to the current game state
            switch (gameState) {
                case GameState.InGame:
                    inGameScreen.Update(gameTime);
                    break;
                case GameState.MainMenu:
                    mainMenuScreen.Update(gameTime);
                    break;
                case GameState.EndGame:
                    endGameScreen.Update(gameTime);
                    break;
                case GameState.HighScore:
                    highScoreScreen.Update(gameTime);
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            switch (gameState)
            {
                case GameState.InGame:
                    inGameScreen.Draw(spriteBatch);
                    break;
                case GameState.MainMenu:
                    mainMenuScreen.Draw(spriteBatch);
                    break;
                case GameState.EndGame:
                    endGameScreen.Draw(spriteBatch);
                    break;
                case GameState.HighScore:
                    highScoreScreen.Draw(spriteBatch);
                    break;

            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }


        public Texture2D TextToTexture(string text, int fontSize)
        {
            System.Drawing.Bitmap textBmp = TextureGen.CreateBitmapImage(text, fontSize);

            Texture2D textTexture = new Texture2D(graphics.GraphicsDevice, textBmp.Width, textBmp.Height);

            textTexture.SetData<byte>(TextureGen.GetBytes(textBmp));

            return textTexture;

        }
        public Texture2D RectangleBorder(int width, int height, int borderWidth)
        {
            Texture2D rectText = new Texture2D(GraphicsDevice, width, height);
            Color[] rectData = new Color[width * height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int index = x + y * width;
                    if (x < borderWidth || x > width - borderWidth || y < borderWidth || y > height - borderWidth)
                    {
                        rectData[index] = Color.White;
                    }
                    else
                    {
                        rectData[index] = Color.Transparent;
                    }
                }
            }
            rectText.SetData(rectData);

            return rectText;
        }
        public void SwitchCase(GameState gameState)
        {
            switch (this.gameState)
            {
                case GameState.InGame:
                    inGameScreen.Unload();
                    break;
                case GameState.MainMenu:
                    mainMenuScreen.Unload();
                    break;
                case GameState.EndGame:
                    endGameScreen.Unload();
                    break;
                case GameState.HighScore:
                    highScoreScreen.Unload();
                    break;

            }
            this.gameState = gameState;

            switch (gameState)
            {
                case GameState.InGame:
                    inGameScreen.Initialize();
                    break;
                case GameState.MainMenu:
                    mainMenuScreen.Initialize();
                    break;
                case GameState.EndGame:
                    endGameScreen.Initialize();
                    break;
                case GameState.HighScore:
                    highScoreScreen.Initialize();
                    break;

            }
        }
    }
}
