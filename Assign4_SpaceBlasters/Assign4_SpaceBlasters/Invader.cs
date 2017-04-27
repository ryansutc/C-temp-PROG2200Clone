using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign4_SpaceBlasters
{
    /// <summary>
    /// Invadors are bad guys. This is a simple class for each one
    /// </summary>
    public class Invader
    {
        public int x;
        public int y;
        public int rank;
        public int file;
        public string type;
        public int width;
        public int height;
        public  DateTime lastBombTime;
        public Invader(int x, int y, int rank, int file, string type, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.rank = rank;
            this.file = file;
            this.type = type;
            this.width = width;
            this.height = height;
            lastBombTime = DateTime.Now;
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle(x - (width/2), y - (height/2), width, height);
        }

    }
}
