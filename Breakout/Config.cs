using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    public static class Config
    {
        //game properties
        public static int startLifes = 3;

        //brick properties
        public static int brickWidth = 50;
        public static int brickHeight = 20;

        //ball properties
        public static int ballRadius = 8;
        public static int ballVelocity = 8;

        //platform properties
        public static int platformWidth = 80;
        public static int platformHeight = 15;

        //powerUp properties
        public static int powerUpWidth = 20;
        public static int powerUpHeight = 20;
        public static int powerUpVelocity = 2;

        //buff times
        public static int pierceTime = 5;
        public static int scoreTime = 30;
        public static int platformWidthTime = 30;
    }
}
