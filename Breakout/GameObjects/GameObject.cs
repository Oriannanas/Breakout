using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    class GameObject
    {
        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                rect.Location = new Point((int)(value.X - rect.Width / 2), (int)(value.Y - rect.Height / 2));
            }
        }
        protected Rectangle rect;
        public Rectangle Rect
        {
            get
            {
                return rect;
            }
            set
            {
                rect = value;
            }
        }
        protected Vector2 origin;
        protected Texture2D texture;
        protected Color color;
        public bool isEnabled { get; set; } = true;

        public GameObject(int x, int y, int width, int height, Texture2D texture, Color color)
        {
            this.texture = texture;
            this.color = color;
            this.origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.Position = new Vector2(x+width/2, y+height/2);
            this.rect = new Rectangle(x, y, width, height);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, null, color, 0, origin, SpriteEffects.None, 0);
        }
    }
}
