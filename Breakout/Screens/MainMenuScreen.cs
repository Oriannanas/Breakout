using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    class MainMenuScreen
    {
        private ScreenController game;
        
        public MouseState prevMouseState { get; private set; }
        public MouseState curMouseState { get; private set; }

        private Texture2D menuButton;

        private MenuButton startButton;
        private MenuButton highScoreButton;
        private MenuButton quitButton;

        private int viewportWidth;
        private int viewportHeight;

        public MainMenuScreen(ScreenController game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            menuButton = game.menuButton;
            viewportHeight = game.GraphicsDevice.Viewport.Height;
            viewportWidth = game.GraphicsDevice.Viewport.Width;
            startButton = new MenuButton(menuButton, game.TextToTexture("Start", 30), viewportWidth / 2 - menuButton.Width / 2, 200);
            highScoreButton = new MenuButton(menuButton, game.TextToTexture("Highscore", 30), viewportWidth / 2 - menuButton.Width / 2, startButton.Rect.Bottom + 25);
            quitButton = new MenuButton(menuButton, game.TextToTexture("Quit", 30), viewportWidth / 2 - menuButton.Width / 2, highScoreButton.Rect.Bottom + 25);
        }
        public void Unload()
        {
            startButton = null;
            quitButton = null;
        }
        public void Update(GameTime gameTime)
        {
            prevMouseState = game.prevMouseState;
            curMouseState = game.curMouseState;

            if (prevMouseState.LeftButton ==  ButtonState.Released && curMouseState.LeftButton == ButtonState.Pressed)
            {
                if (startButton.Rect.Contains(curMouseState.Position)){
                    game.SwitchCase(GameState.InGame);
                }
                else if (highScoreButton.Rect.Contains(curMouseState.Position))
                {
                    game.SwitchCase(GameState.HighScore);
                }
                else if (quitButton.Rect.Contains(curMouseState.Position))
                {
                    game.Exit();
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            startButton.Draw(spriteBatch);
            highScoreButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
        }
    }
    
}
