using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign4_SpaceBlasters
{
    public class BackgroundStar
    {
        public int x;
        public int y;
        public int size;
        public int velocity;
        
        public BackgroundStar(int x, int y, int size, int velocity)
        {
            this.x = x;
            this.y = y;
            this.size = size;
            this.velocity = velocity;
        }

        public void Fall()
        {
            this.y = this.y + (1 * this.velocity);
        }
    }
}
