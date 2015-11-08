using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    class Brick : GameObject
    {
        private int score;
        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
            }
        }
        public Color Color { get { return this.color; } }
        private int initIntegrity;
        private int integrity;
        public int Integrity
        {
            get
            {
                return integrity;
            }
            set
            {
                integrity = value;
            }
        }
        public Brick(int x, int y, Texture2D texture, Color color) : base (x, y, Config.brickWidth, Config.brickHeight, texture, color)
        {
        }

        public void Init(int score, int integrity, Color color)
        {
            this.color = color;
            this.score = score;
            this.initIntegrity = integrity;
            this.integrity = integrity;
            this.isEnabled = true;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float integritypercentage = ((float)integrity / 3);
            spriteBatch.Draw(texture, rect, null, new Color(color, integritypercentage), 0, origin, SpriteEffects.None, 0);
        }

    }
}
