using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign4_SpaceBlasters
{
    public class CountdownState : State
    {

        string message = "";
        string submessage = "Starting Mission In:";
        Font font;
        SolidBrush brush;
        int CountdownTimer = 90;

        public CountdownState (int width, int height) : base(width, height)
        {
        }

        public override void Draw(Graphics graphics)
        {
            graphics.Clear(Color.Black);
            if (base.loaded == false) Load();

            using (font = new Font("ComicSans", 20))
            {
                brush = new SolidBrush(Color.White);
                Rectangle rect1 = new Rectangle(width / 2 - 300, height / 2 - 150, 600, 300);
                StringFormat stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                graphics.DrawString(message, font, brush, rect1, stringFormat);

                rect1 = new Rectangle(width / 2 - 300, height / 2 - 300, 600, 300);
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                graphics.DrawString(submessage, new Font("ComicSans", 14), brush, rect1, stringFormat);

                //graphics.DrawRectangle(Pens.Red, rect1);
            }
            Update();

        }

        public override State Leave()
        {
            return new GameState(width, height, 0, 1, 2); //CHANGED LEVEL TO 3!
        }

        public override void KeyboardPress(GameEngine gameEngine, string key)
        {
           //No Keyboard events are accepted for this State
        }

        public override void Load()
        {
            base.Load();
            
            // Create a StringFormat object with the each line of text, and the block
            // of text centered on the page.
        }

        public override string Update()
        {
            CountdownTimer -= 1;
            if (CountdownTimer > 60)
            {
                message = "3";
                return null;
            }
            else if (CountdownTimer > 30)
            {
                message = "2";
                return null;
            }
            else if (CountdownTimer > 0)
            {
                message = "1";
                return null;
            }
            else
            {
                return "Done";
            }

        }


    }
}
