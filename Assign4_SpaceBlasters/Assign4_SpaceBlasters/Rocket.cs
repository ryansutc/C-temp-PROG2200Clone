using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign4_SpaceBlasters
{
    /// <summary>
    /// Fired by You. They have position and velocity.
    /// </summary>
    public class Rocket
    {
        public int x;
        public int y;
        public int velocity;
        public Rocket(int x, int y, int velocity)
        {
            this.x = x;
            this.y = y;
            this.velocity = velocity;
        }
        public Rectangle GetRectangle()
        {
            return new Rectangle(x, y, 2, 2);
        }
    }
}
