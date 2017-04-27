using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign4_SpaceBlasters
{
    public class WelcomeState: State
    {
        
        string message = "";
        string submessage = "";
        Font font;
        SolidBrush brush;

        public WelcomeState(int width, int height) : base(width, height)
        {
        }

        public override void Draw(Graphics graphics)
        {
            
            if(base.loaded == false) Load();
                       
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

                string highscore = "High Scores: \n" + Properties.Settings.Default.HighScore + "pts" +
                    " (" + Properties.Settings.Default.HighScoreInitials + ")\n" +
                    Properties.Settings.Default.HighScore2 + "pts" +
                    " (" + Properties.Settings.Default.HighScoreInitials2 + ")\n" +
                    Properties.Settings.Default.HighScore3 + "pts" +
                    " (" + Properties.Settings.Default.HighScoreInitials3 + ")\n";

                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Far;
                graphics.DrawString(highscore, new Font("ComicSans", 10), brush, new Rectangle(width - 150, 50, 150, 150));
                //graphics.DrawRectangle(Pens.Red, rect1);
            }

        }

        public override State Leave()
        {
            return new CountdownState(width, height);
        }

        public override void KeyboardPress(GameEngine gameEngine, string key)
        {
            //Special Keyboard presses for this state are programmed here
            if(key == "Space")
            {
                //load countdown screen:
                gameEngine.ReplaceState(new CountdownState(width, height));

            }
        }

        public override void Load()
        {
            base.Load();
            message = "Space Invaders";
            // Create a StringFormat object with the each line of text, and the block
            // of text centered on the page.
        }

        public override string Update()
        {
            //do something
            submessage = "Press Space to Start Game";
            return null;
        }

        
    }
}
