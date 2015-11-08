using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    class PowerUp : GameObject
    {

        public PowerUpType powerUpType;

        public PowerUp(PowerUpType powerUpType, int x, int y, Texture2D texture, Color color) : base(x, y, Config.powerUpWidth, Config.powerUpHeight, texture, color)
        {
            this.powerUpType = powerUpType;
        }
    }

    public enum PowerUpType
    {
        LifeUp,
        ExtraBall,
        Pierce,
        SpeedDown,
        ScoreMultiplier,
        PlatformWidth//,
        //Gun
    }
}
