using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign4_SpaceBlasters
{
    public abstract class State
    {
        protected bool loaded = false; //whether state has been loaded

        public int width;
        public int height;

        public State(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public virtual void Load()
        {
            loaded = true;
        }

        public abstract State Leave();

        /// <summary>
        /// Can return "Done", "Pause", "Restart"
        /// </summary>
        /// <param name="gameEngine"></param>
        /// <returns></returns>
        public abstract string Update();


        public abstract void Draw(Graphics graphics);

        public abstract void KeyboardPress(GameEngine gameEngine, string key);
    }
}
