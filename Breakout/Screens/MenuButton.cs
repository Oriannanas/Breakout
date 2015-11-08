using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    public class MenuButton
    {
        public Rectangle Rect { get; private set; }
        public Texture2D Texture { get; private set; }
        public Texture2D TextTexture { get; private set; }

        public MenuButton(Texture2D texture, Texture2D textTexture, int x, int y)
        {
            Rect = new Rectangle(x, y, texture.Width, texture.Height);
            this.Texture = texture;
            this.TextTexture = textTexture;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rect, Color.White * 0.8f);
            spriteBatch.Draw(TextTexture, Rect.Center.ToVector2() - new Vector2(TextTexture.Width / 2, TextTexture.Height / 2), Color.White * 0.8f);
        }
    }
}
