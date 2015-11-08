using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    class Platform : GameObject
    {
        public Platform(int x, int y, Texture2D texture, Color color) : base (x, y, Config.platformWidth, Config.platformHeight, texture, color)
        {
        }
    }
}
