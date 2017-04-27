using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign4_SpaceBlasters
{
    /// <summary>
    /// The ship is your piece in the Space Invadors Game
    /// It has a location and nothing else
    /// </summary>
    public class Ship
    {
        public int x;
        public int y;
        public int width = 40;
        public int height = 25;
        public DateTime lastHitTime;
        public bool hitBool = false; //for flashing colors
        public DateTime lastRocketTime;
        public Ship(int x, int y)
        {
            this.x = x;
            this.y = y;
            lastHitTime = DateTime.Now;
            lastRocketTime = DateTime.Now;
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(x - (width/2), y - (height/2), width, height);
        }

        public Color GetColor()
        {
            DateTime now = DateTime.Now;
            if((now - lastHitTime).TotalSeconds < 2)
            {
                if (hitBool)
                {
                    hitBool = false;
                    return Color.Orange;
                }
                else
                {
                    hitBool = true;
                    return Color.OrangeRed;
                }
            }
            else
            {
                return Color.Red;
            }
        }
    }
}
