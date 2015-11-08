using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    public class EndGameScreen
    {
        private ScreenController game;
        private string name = "No name"; 
        public int score;
        public MouseState prevMouseState { get; private set; }
        public MouseState curMouseState { get; private set; }

        private Texture2D menuButton;

        private MenuButton playAgainButton;
        private MenuButton submitScoreButton;
        private MenuButton quitButton;

        private int viewportWidth;
        private int viewportHeight;

        public EndGameScreen(ScreenController game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            menuButton = game.menuButton;
            viewportHeight = game.GraphicsDevice.Viewport.Height;
            viewportWidth = game.GraphicsDevice.Viewport.Width;
            submitScoreButton = new MenuButton(menuButton, game.TextToTexture("Submit", 30), viewportWidth / 2 - menuButton.Width / 2, 200);
            playAgainButton = new MenuButton(menuButton, game.TextToTexture("Play Again", 30), viewportWidth / 2 - menuButton.Width / 2, submitScoreButton.Rect.Bottom + 25);
            quitButton = new MenuButton(menuButton, game.TextToTexture("Quit", 30), viewportWidth / 2 - menuButton.Width / 2, playAgainButton.Rect.Bottom + 25);
        }
        public void Unload()
        {
            menuButton = null;
            playAgainButton = null;
            submitScoreButton = null;
            quitButton = null;
        }
        public void Update(GameTime gameTime)
        {
            prevMouseState = game.prevMouseState;
            curMouseState = game.curMouseState;

            if (prevMouseState.LeftButton ==  ButtonState.Released && curMouseState.LeftButton == ButtonState.Pressed)
            {
                if (playAgainButton.Rect.Contains(curMouseState.Position)){
                    game.SwitchCase(GameState.InGame);
                }
                else if (submitScoreButton.Rect.Contains(curMouseState.Position))
                {
                    game.highscore.AddScore(name, score);
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

            Texture2D gameOverText = game.TextToTexture("Game over", 48);
            spriteBatch.Draw(gameOverText, new Rectangle(viewportWidth / 2 - gameOverText.Width/2, 50, gameOverText.Width, gameOverText.Height), Color.White);

            Texture2D nameText = game.TextToTexture(name, 24);
            spriteBatch.Draw(nameText, new Rectangle(viewportWidth / 2 - nameText.Width / 2, 125, nameText.Width, nameText.Height), Color.White);
            Texture2D scoreText = game.TextToTexture(this.score.ToString(), 24);
            spriteBatch.Draw(scoreText, new Rectangle(viewportWidth / 2 - scoreText.Width/2, 150, scoreText.Width, scoreText.Height), Color.White);
            playAgainButton.Draw(spriteBatch);
            submitScoreButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
        }
    }
    
}
