using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign4_SpaceBlasters
{
    public class PauseState : State
    {
        string message = "";
        string submessage = "";
        Font font;
        SolidBrush brush;

        public PauseState(int width, int height) : base(width, height)
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

                rect1 = new Rectangle(width / 2 - 300, height / 2, 600, 300);
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;
                graphics.DrawString(submessage, new Font("ComicSans", 14), brush, rect1, stringFormat);
            }
            Update();

        }

        public override State Leave()
        {
            return new CountdownState(width, height);
        }

        public override void KeyboardPress(GameEngine gameEngine, string key)
        {
            //Special Keyboard presses for this state are programmed here
            if (key == "Space")
            {
                //load countdown screen:
                gameEngine.RemoveState(); //go back to game\
            }
        }

        public override void Load()
        {
            base.Load();
            message = "Game Paused";
            // Create a StringFormat object with the each line of text, and the block
            // of text centered on the page.
        }

        public override string Update()
        {
            submessage = "Press Space to Resume Game";
            return null;
        }
    }
}

       