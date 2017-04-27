using Assign4_SpaceBlasters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Assign4_SpaceBlasters
{
    public class Background
    {
        private int width = 0;
        private int height = 0;
        private int minVelocity = 1;
        private int maxVelocity = 5;
        private int starCount = 50;
        private List<BackgroundStar> BackgroundStars;

        private SolidBrush brush;


        public Background(int Width, int Height)
        {
            //  Store the div.
           
            this.width = Width;
            this.height = Height;

            this.brush = new SolidBrush(Color.LightYellow);

        }

        public List<BackgroundStar> Start()
        {
            Random random = new Random();
            //  Create the stars.
            BackgroundStars = new List<BackgroundStar>();
            for (var i = 0; i < this.starCount; i++)
            {
                BackgroundStars.Add(new BackgroundStar(
                    random.Next(1, width),
                    random.Next(1, height), 
                    random.Next(1, 4), 
                    random.Next(minVelocity, maxVelocity)));
                
            }

            return BackgroundStars;
        }

        public List<BackgroundStar> Update()
        {
            Random random = new Random();
            foreach (BackgroundStar star in BackgroundStars)
            {
                star.Fall();
                if (star.y >= height)
                {
                    //  If the star has moved from the bottom of the screen, spawn it at the top.
                    if (star.y > height)
                    {
                        star.x = random.Next(1, width);
                        star.y = 1;
                        star.size = random.Next(1, 4);
                        star.velocity = random.Next(minVelocity, maxVelocity);

                    }
                }
            }

            return BackgroundStars;
        }


    }
}
