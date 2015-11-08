using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    class InGameScreen
    {
        private ScreenController game;
        private bool paused;


        private int pastTicks;
        private float ballSpeedMultiplier;
        //private int gameTimer = 0;
        private Dictionary<string, float> buffTimers = new Dictionary<string, float>();
        private List<Brick> brickList = new List<Brick>();
        private List<ParticleManager> particleManagerList = new List<ParticleManager>();
        private List<PowerUp> powerUpList = new List<PowerUp>();
        private List<Ball> ballList = new List<Ball>();
        private Platform platform;


        public MouseState prevMouseState { get; private set; }
        public MouseState curMouseState { get; private set; }
        public KeyboardState prevKeyState { get; private set; }
        public KeyboardState curKeyState { get; private set; }

        private int score;
        private int lives;
        private int bricksEnabled;

        public int viewportWidth;
        public int viewportHeight;
        
        private Random rng = new Random();
        

        public InGameScreen(ScreenController game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            lives = Config.startLifes;
            score = 0;
            viewportHeight = game.GraphicsDevice.Viewport.Height;
            viewportWidth = game.GraphicsDevice.Viewport.Width;
            SpawnBricks();
            platform = new Platform(viewportWidth/2 - Config.platformWidth/2, viewportHeight - 50, game.dotTexture, Color.White);
            ballList.Add(new Ball((int)platform.Position.X - Config.ballRadius, platform.Rect.Y - Config.ballRadius * 2, game.dotTexture, Color.Black));
            pastTicks = 0;
            ballSpeedMultiplier = 1;

        }
        public void Unload()
        {
            brickList.Clear();
            particleManagerList.Clear();
            powerUpList.Clear();
            ballList.Clear();
            platform = null;
        }

        public void Update(GameTime gameTime)
        {
            prevMouseState = game.prevMouseState;
            curMouseState = game.curMouseState;
            prevKeyState = game.prevKeyState;
            curKeyState = game.curKeyState;

            if (curKeyState.IsKeyDown(Keys.P)&& prevKeyState.IsKeyUp(Keys.P)){
                paused = !paused;
            }
            if (!paused)
            {
                pastTicks += gameTime.ElapsedGameTime.Milliseconds;
                ballSpeedMultiplier += (float)gameTime.ElapsedGameTime.Milliseconds / 100000;


                #region platform

                int mouseX = curMouseState.Position.X;
                if (mouseX > viewportWidth - platform.Rect.Width / 2)
                {
                    platform.Position = new Vector2(viewportWidth - platform.Rect.Width / 2, platform.Position.Y);
                }
                else if (mouseX < (int)platform.Rect.Width / 2)
                {
                    platform.Position = new Vector2((int)platform.Rect.Width / 2, platform.Position.Y);
                }
                else
                {
                    platform.Position = new Vector2(mouseX, platform.Position.Y);
                }
                #endregion

                #region balls
                foreach (Ball ball in ballList)
                {
                    if (ball.isReleased)
                    {
                        //move the ball

                        ball.ballSpeedMultiplier = this.ballSpeedMultiplier;
                        ball.Position += ball.velocity * ball.ballSpeedMultiplier;

                        //check for interstection with platform
                        if (ball.Rect.Intersects(platform.Rect))
                        {
                            ball.PlatformCollision(platform);
                        }
                        //check for intersections with bricks (else because the ball cant intersect with bricks and the platform at the same time)

                        foreach (Brick brick in brickList)
                        {
                            if (brick.isEnabled)
                            {
                                if (ball.Rect.Intersects(brick.Rect))
                                {
                                    if (!buffTimers.ContainsKey("pierce"))
                                    {
                                        if (ball.BrickCollision(brick))
                                        {
                                            BreakBrick(brick);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        BreakBrick(brick);
                                        break;
                                    }
                                }
                            }

                        }
                        //check for level bounds

                        if (ball.Position.Y < ball.Rect.Width)
                        {
                            ball.Position = new Vector2(ball.Position.X, ball.Rect.Height);
                            ball.velocity = new Vector2(ball.velocity.X, -1 * ball.velocity.Y);
                        }


                        if (ball.Position.X < ball.Rect.Width)
                        {
                            ball.Position = new Vector2(ball.Rect.Width, ball.Position.Y);
                            ball.velocity = new Vector2(-ball.velocity.X, ball.velocity.Y);
                        }
                        if (ball.Position.X > viewportWidth - ball.Rect.Width)
                        {
                            ball.Position = new Vector2(viewportWidth - ball.Rect.Width, ball.Position.Y);
                            ball.velocity = new Vector2(-ball.velocity.X, ball.velocity.Y);
                        }
                        if (ball.Position.Y > viewportHeight)
                        {
                            if (ballList.Count == 1)
                            {
                                ball.isReleased = false;
                                lives -= 1;
                            }
                            else
                            {
                                ballList.Remove(ball);
                                break;
                            }
                        }
                    }
                    else
                    {
                        ball.Position = new Vector2(platform.Position.X, platform.Rect.Top - Config.ballRadius);
                        if (curMouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            ball.isReleased = true;
                            ball.velocity = new Vector2(0, -ball.maxVelocity);
                        }
                    }
                }
                #endregion

                #region bricks
                foreach (Brick brick in brickList)
                {
                    if (!brick.isEnabled)
                    {
                        if (rng.Next(0, 5000) == 1)
                        {
                            int generated = rng.Next(1, 4);
                            brick.Init(50 * (generated * 2), generated, new Color((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble()));
                            bricksEnabled += 1;
                        }
                    }
                }
                #endregion

                #region PowerUps
                foreach (PowerUp powerUp in powerUpList)
                {
                    powerUp.Position += new Vector2(0, Config.powerUpVelocity);
                    if (powerUp.Rect.Intersects(platform.Rect))
                    {
                        switch (powerUp.powerUpType)
                        {
                            case PowerUpType.ExtraBall:
                                ballList.Add(new Ball((int)platform.Position.X - Config.ballRadius, platform.Rect.Y - Config.ballRadius * 2, game.dotTexture, Color.Black));
                                break;
                            case PowerUpType.LifeUp:
                                lives += 1;
                                break;
                            case PowerUpType.Pierce:
                                if (!buffTimers.ContainsKey("pierce"))
                                {
                                    buffTimers.Add("pierce", Config.pierceTime);
                                }
                                else
                                {
                                    buffTimers["pierce"] = Config.pierceTime;
                                }
                                break;

                            case PowerUpType.ScoreMultiplier:
                                if (!buffTimers.ContainsKey("score"))
                                {
                                    buffTimers.Add("score", Config.scoreTime);
                                }
                                else
                                {
                                    buffTimers["score"] = Config.scoreTime;
                                }
                                break;
                            case PowerUpType.SpeedDown:
                                ballSpeedMultiplier = (ballSpeedMultiplier + 1) / 2;
                                break;
                            case PowerUpType.PlatformWidth:
                                if (!buffTimers.ContainsKey("platformWidth"))
                                {
                                    buffTimers.Add("platformWidth", Config.platformWidthTime);
                                }
                                else
                                {
                                    buffTimers["platformWidth"] = Config.platformWidthTime;
                                }
                                break;
                                // case PowerUpType.Gun:
                                //     Console.WriteLine("gun");
                                //     break;
                        }
                        powerUpList.Remove(powerUp);
                        break;
                    }
                }
                #endregion

                #region particleManagers
                for (int i = 0; i < particleManagerList.Count; i++)
                {
                    particleManagerList[i].Update(gameTime);
                }
                #endregion

                for (int index = 0; index < buffTimers.Count;)
                {
                    if (buffTimers[buffTimers.ElementAt(index).Key] > 0)
                    {
                        buffTimers[buffTimers.ElementAt(index).Key] -= (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
                        index++;
                    }
                    else
                    {
                        buffTimers.Remove(buffTimers.ElementAt(index).Key);
                    }

                }
                if (buffTimers.ContainsKey("platformWidth"))
                {
                    platform.Rect = new Rectangle(platform.Rect.X, platform.Rect.Y, (int)(Config.platformWidth * 1.5f), platform.Rect.Height);
                }
                else
                {
                    platform.Rect = new Rectangle(platform.Rect.X, platform.Rect.Y, Config.platformWidth, platform.Rect.Height);
                }

                if (lives <= 0)
                {
                    game.endGameScreen.score = score;
                    game.SwitchCase(GameState.EndGame);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (ParticleManager particleManager in particleManagerList)
            {
                particleManager.Draw(spriteBatch);
            }

            Texture2D scoreText = game.TextToTexture(score.ToString(), 24);
            spriteBatch.Draw(scoreText, new Rectangle(viewportWidth - scoreText.Width, viewportHeight - scoreText.Height, scoreText.Width, scoreText.Height), Color.Black);

            Texture2D livesText = game.TextToTexture(lives.ToString(), 24);
            spriteBatch.Draw(livesText, new Rectangle(0, viewportHeight - livesText.Height, livesText.Width, livesText.Height), Color.Black);

            platform.Draw(spriteBatch);
            foreach(Ball ball in ballList)
            {
                ball.Draw(spriteBatch);
            }
            foreach (Brick brick in brickList)
            {
                if (brick.isEnabled)
                {
                    brick.Draw(spriteBatch);
                }
            }
            foreach(PowerUp powerUp in powerUpList)
            {
                powerUp.Draw(spriteBatch);
            }

            if (paused)
            {
                Texture2D pauseText = game.TextToTexture("Paused", 50);
                spriteBatch.Draw(game.dotTexture, new Rectangle(0, 0, viewportWidth, viewportHeight), Color.Black*0.6f);
                spriteBatch.Draw(pauseText, new Rectangle(viewportWidth/2 - pauseText.Width/2, viewportHeight/2 - pauseText.Height/2, pauseText.Width, pauseText.Height), Color.White);
            }

        }


        private void SpawnBricks()
        {
            int distanceBetweenBricks = 3;
            int maxBricksX = (int)Math.Floor((double)(viewportWidth - 20) / (Config.brickWidth + distanceBetweenBricks));
            int maxBricksY = (int)Math.Floor((double)(viewportHeight - 250) / (Config.brickHeight + distanceBetweenBricks));
            for (int i = 0; i < maxBricksX; i++)
            {
                for (int j = 0; j < maxBricksY; j++)
                {
                    int x = (int)(viewportWidth - (maxBricksX * (Config.brickWidth + distanceBetweenBricks))) / 2 + (int)(Config.brickWidth + distanceBetweenBricks) * i;
                    int y = 10 + (int)(Config.brickHeight + distanceBetweenBricks) * j;
                    int generated = rng.Next(1, 4);
                    Brick brick =  new Brick(x, y, game.dotTexture, Color.Transparent);
                    brickList.Add(brick);
                    brick.Init(50 * (generated * 2), generated, new Color((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble()));
                    bricksEnabled += 1;
                }
            }
        }

        private void BreakBrick(Brick brick)
        {
            if (brick.Integrity <= 1)
            {
                particleManagerList.Add(new ParticleManager(this, brick.Rect, 50, game.dotTexture, brick.Color));
                brick.isEnabled = false;

                if (!buffTimers.ContainsKey("score"))
                {
                    this.score += brick.Score;
                }
                else
                {
                    this.score += brick.Score * 2;
                }
                //random drop?
                if (rng.NextDouble() <= 0.08f)
                {
                    int generated = (int)(rng.NextDouble()*Enum.GetValues(typeof(PowerUpType)).Length);
                    powerUpList.Add(new PowerUp((PowerUpType)generated, (int)brick.Position.X, (int)brick.Rect.Y, game.dotTexture, Color.Yellow));
                }
                //decrease brick count and check brickcount to see if level is finished
                bricksEnabled -= 1;
            }
            else
            {
                brick.Integrity -= 1;
            }

        }

        public void RemoveParticleManager(ParticleManager partMan)
        {
            particleManagerList.Remove(partMan);
        }
    }

}
