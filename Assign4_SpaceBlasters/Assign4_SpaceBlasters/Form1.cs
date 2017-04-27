using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Assign4_SpaceBlasters
{
    public partial class Form1 : Form
    {
        public Background background;
        public GameEngine gameEngine;

        public Form1()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            this.gameEngine = new GameEngine(this.Size.Width, this.Size.Height);
           
            timer1.Start();
           
        }
        private void Form1_Resize(object sender, System.EventArgs e)
        {
            Console.WriteLine("I was resized");
            if (gameEngine != null)
            {
                gameEngine.Resize(this.Size.Width, this.Size.Height);
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Draw graphics
            State state = gameEngine.CurrentState();
            state.Draw(e.Graphics);
            gameEngine.UpdateCurrentState();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();          
        }

      private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //Let the graphics engine handle the button pushing
            //move the paddle
            if(e.KeyCode == Keys.Left)
            {
                gameEngine.KeyboardPress("Left");
            }
            else if(e.KeyCode == Keys.Right)
            {
                gameEngine.KeyboardPress("Right");
            }
            else if(e.KeyCode == Keys.G)
            {
                gameEngine.KeyboardPress("G");
            }
            else if (e.KeyCode == Keys.Space)
            {
                gameEngine.KeyboardPress("Space");
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) || char.IsDigit(e.KeyChar))
            {
                gameEngine.KeyPressSpecial(e.KeyChar.ToString());
            }
           
        }
    } //endclass
}
