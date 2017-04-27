using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign4_SpaceBlasters
{
    public class Bomb
    {
        public int x;
        public int y;
        public int velocity;
        public Bomb(int x, int y, int velocity)
        {
            this.x = x;
            this.y = y;
            this.velocity = velocity;
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(x-4, y-4, 8, 8);
        }
    }
}
