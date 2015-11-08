using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    class Particle
    {
        public int id
        {
            get; private set;
        }
        public bool isEnabled { get; set; }
        private Rectangle rect;
        private Vector2 acceleration;
        public Vector2 Acceleration
        {
            get {
                return acceleration;
             }
            set
            {
                acceleration = value;
            }
        }
        private Vector2 velocity;
        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
        }

        private Texture2D texture;
        private Color color;

        public Particle(int id, float x, float y, int width,Texture2D texture, Color color)
        {
            this.id = id;
            isEnabled = true;
            position = new Vector2(x, y);
            rect = new Rectangle((int)x, (int)y, width, width);
            this.texture = texture;
            this.color = color;
        }
        public void Update(GameTime gameTime)
        {
            velocity += acceleration;
            velocity.X *= 0.99f;
            position += velocity;
            rect.Location = position.ToPoint();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, color);
        }
    }
}
