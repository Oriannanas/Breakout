using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    class Ball : GameObject
    {
        public bool isReleased
        {
            get;
            set;
        }
        public float maxVelocity
        {
            get; set;
        }
        private Vector2 _velocity;
        public Vector2 velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                _velocity = value;
            }
        }

        public float ballSpeedMultiplier
        {
            get; set;
        }

        public Ball(int x, int y, Texture2D texture, Color color) : base (x, y, Config.ballRadius*2, Config.ballRadius*2, texture, color)
        {
            this.maxVelocity = Config.ballVelocity;
            ballSpeedMultiplier = 1;
        }

        public bool BrickCollision(Brick brick)
        {
            int intersectResult = this.Rect.IntersectSub(brick.Rect, 10);

            while (intersectResult == 6)
            {
                this.Position -= this.velocity / 7.5f;
                intersectResult = this.Rect.IntersectSub(brick.Rect, 10);
                Console.WriteLine(Position);
            }
            if (intersectResult == 1 || intersectResult == 4)
            {
                this.velocity = new Vector2(this.velocity.X, -this.velocity.Y);
                return true;
            }
            else if (intersectResult == 2 || intersectResult == 3)
            {

                this.velocity = new Vector2(-this.velocity.X, this.velocity.Y);
                return true;
            }
            else if (intersectResult == 5)
            {
                //check if the ball is going straight towards the brick or sort of hits it while passing
                if ((brick.Position.X - this.Position.X < 0 && this.velocity.X < 0 && brick.Position.Y - this.Position.Y < 0 && this.velocity.Y < 0)
                   || (brick.Position.X - this.Position.X > 0 && this.velocity.X > 0 && brick.Position.Y - this.Position.Y < 0 && this.velocity.Y < 0)
                   || (brick.Position.X - this.Position.X < 0 && this.velocity.X < 0 && brick.Position.Y - this.Position.Y > 0 && this.velocity.Y > 0)
                   || (brick.Position.X - this.Position.X > 0 && this.velocity.X > 0 && brick.Position.Y - this.Position.Y > 0 && this.velocity.Y > 0)
                   )
                {
                    this.velocity = new Vector2(-this.velocity.X, -this.velocity.Y);
                    return true;

                }
            }
            return false;
        }
        public void PlatformCollision(Platform platform)
        {

            this.Position = new Vector2(this.Position.X, platform.Position.Y - (platform.Rect.Height / 2) - this.Rect.Width / 2);
            float angle =((float)Math.PI * (-(this.Position.X - platform.Position.X) / (platform.Rect.Width + this.Rect.Width) + 1f));

            _velocity = new Vector2((float)Math.Sin(angle) * maxVelocity, (float)Math.Cos(angle) * maxVelocity);
            if (_velocity.Y < 1f && _velocity.Y > 0)
            {
                _velocity.Y = 1f;
            }

            if (_velocity.Y > -1f && _velocity.Y < 0)
            {
                _velocity.Y = -1f;
            }
        }
    }
}
