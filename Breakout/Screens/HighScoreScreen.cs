using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    class HighScoreScreen
    {
        private ScreenController game;

        private Score[] scoreList;

        public MouseState prevMouseState { get; private set; }
        public MouseState curMouseState { get; private set; }

        private Texture2D highscoreHead;
        private Texture2D menuButton;

        private MenuButton mainMenuButton;
        private MenuButton quitButton;

        private int viewportWidth;
        private int viewportHeight;

        public HighScoreScreen(ScreenController game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            menuButton = game.menuButton;
            highscoreHead = game.TextToTexture("Highscore", 48);
            viewportHeight = game.GraphicsDevice.Viewport.Height;
            viewportWidth = game.GraphicsDevice.Viewport.Width;
            mainMenuButton = new MenuButton(menuButton, game.TextToTexture("Back", 30), (viewportWidth / 2) - (int)(menuButton.Width*1.10f), viewportHeight - 100);
            quitButton = new MenuButton(menuButton, game.TextToTexture("Quit", 30), (viewportWidth / 2)+ (int)(menuButton.Width * 0.10f), viewportHeight - 100);
            scoreList = game.highscore.scoreList;
        }
        public void Unload()
        {
            highscoreHead = null;
            mainMenuButton = null;
            quitButton = null;
            scoreList = null;
        }
        public void Update(GameTime gameTime)
        {
            prevMouseState = game.prevMouseState;
            curMouseState = game.curMouseState;

            if (prevMouseState.LeftButton == ButtonState.Released && curMouseState.LeftButton == ButtonState.Pressed)
            {
                if (mainMenuButton.Rect.Contains(curMouseState.Position))
                {
                    game.SwitchCase(GameState.MainMenu);
                }
                else if (quitButton.Rect.Contains(curMouseState.Position))
                {
                    game.Exit();
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(highscoreHead, new Rectangle(viewportWidth/2 - highscoreHead.Width/2, 40, highscoreHead.Width, highscoreHead.Height), Color.White);
            Texture2D nameColumnText = game.TextToTexture("Name:", 18);
            Texture2D scoreColumnText = game.TextToTexture("Score:", 18);
            spriteBatch.Draw(nameColumnText, new Rectangle(viewportWidth / 2 - 200, 100, nameColumnText.Width, nameColumnText.Height), Color.White);
            spriteBatch.Draw(scoreColumnText, new Rectangle(viewportWidth / 2 + (200 - scoreColumnText.Width), 100, scoreColumnText.Width, scoreColumnText.Height), Color.White);

            for (int i = 0; i < scoreList.Length; i++)
            {
                Texture2D nameText = game.TextToTexture(scoreList[i].name, 16);
                Texture2D scoreText = game.TextToTexture(scoreList[i].value.ToString(), 16);
                spriteBatch.Draw(nameText, new Rectangle(viewportWidth / 2 - 200, 125 + i * 25, nameText.Width, nameText.Height), Color.White);
                spriteBatch.Draw(scoreText , new Rectangle(viewportWidth/2 + (200-scoreText.Width), 125 + i*25, scoreText.Width , scoreText.Height), Color.White);
            }
            mainMenuButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
        }
    }
}

