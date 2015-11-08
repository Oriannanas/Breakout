using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    public static class RectangleExtend
    {


        public static Rectangle[] Subdivide(this Rectangle rect)
        {
            Rectangle[] returnRect = new Rectangle[4];
            returnRect[0] = new Rectangle(rect.X, rect.Y, rect.Width / 2, rect.Height / 2);
            returnRect[1] = new Rectangle(rect.X + rect.Width/2, rect.Y, rect.Width / 2, rect.Height / 2);
            returnRect[2] = new Rectangle(rect.X, rect.Y + rect.Height / 2, rect.Width / 2, rect.Height / 2);
            returnRect[3] = new Rectangle(rect.X + rect.Width/2, rect.Y+rect.Height/2, rect.Width / 2, rect.Height / 2);
            return returnRect;
        }

        public static int IntersectSub(this Rectangle rect, Rectangle intersect, int subdivisions)
        {
            List<int> subIntCount = new List<int>(); // the number of subdivided Rectangles intersecting with the ball
            Rectangle[] subRects = rect.Subdivide();
            for (int i = 0; i < 4; i++)
            {
                if (intersect.Intersects(subRects[i]))
                {
                    subIntCount.Add(i + 1);
                }
            }
            if (subIntCount.Count == 2)
            {
                if (subIntCount[0] == 1 && subIntCount[1] == 2)
                {
                    //ball hits from below
                    return 1;
                }
                else if (subIntCount[0] == 1 && subIntCount[1] == 3)
                {
                    //ball hits from the right
                    return 2;
                }
                else if (subIntCount[0] == 2 && subIntCount[1] == 4)
                {
                    //ball hits from the left
                    return 3;
                }
                else
                {
                    //ball hits from above
                    return  4;
                }
            }
            else if (subIntCount.Count == 1)
            {
                //possible corner
                if (subdivisions > 0)
                {
                    return subRects[subIntCount[0]-1].IntersectSub(intersect, subdivisions - 1);
                }
                else
                {
                    return 5;
                }
            }
            else if(subIntCount.Count == 4)
            {
                //ball is going too fast
                return 6;
            }
            else
            {
                return 0;
            }
        }
    }
}
