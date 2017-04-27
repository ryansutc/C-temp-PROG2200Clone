using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assign4_SpaceBlasters
{
    public class GameEngine
    {
        public int lives = 3;
        public int width;
        public int height;
        public Graphics graphics;

        Stack<State> StateStack = new Stack<State>(); //Welcome, Play, GameOver, Pause, etc.
        public GameEngine(int width, int height)
        {
            this.width = width;
            this.height = height;
           
            StateStack.Push(new WelcomeState(width, height));

            State currentState = CurrentState();
           
        }

        /// <summary>
        /// Get the Current State at top of stack
        /// </summary>
        /// <returns></returns>
        public State CurrentState()
        {
            return StateStack.Peek();
        }
        
        /// <summary>
        /// Destroy current state and replace with NEW state
        /// Update StateStack and call leave and load methods
        /// </summary>
        public void ReplaceState(State state)
        {
            State curState = CurrentState();
            if (curState != null)
            {
                curState.Leave();
            }
                StateStack.Pop();

            StateStack.Push(state);
            state.Load();
            
        }

        /// <summary>
        /// Pop a state, call leave method first
        /// </summary>
        public void RemoveState()
        {
            State curState = CurrentState();
            curState.Leave();
            StateStack.Pop();
        }

        public void AddState(State state)
        {
            State curState = CurrentState();
            if(curState != null)
            {
                curState.Leave();
            }
            StateStack.Push(state);
        }

        /// <summary>
        /// Tells current state to update itself and checks return message
        /// if the state needs to be left.
        /// </summary>
        public void UpdateCurrentState()
        {
            State curState = CurrentState();
            string curStateMsg = curState.Update();
            if (curStateMsg == "Done")
            {
                ReplaceState(curState.Leave());
            }
            else if(curStateMsg == "Game Over")
            {
                ReplaceState(new WelcomeState(width, height));
            }
        }
        /// <summary>
        /// 
        /// Receives Game based KeyDown Events from form
        /// and relays to current state
        /// </summary>
        /// <param name="key"></param>
        public void KeyboardPress(string key)
        {
            State currentState = CurrentState();

            currentState.KeyboardPress(this, key);
           
        }
        /// <summary>
        /// Receives Game based KeyDown Events from form
        /// and passes to gamestate. Only for high score.
        /// </summary>
        /// <param name="key"></param>
        public void KeyPressSpecial(string key)
        {
            State state = CurrentState();
            if (state.GetType() == typeof(GameState))
            {
                GameState gameState = (GameState) CurrentState();
                gameState.KeyPress(key);
            }
        }
       
        public void Resize(int width, int height)
        {
            this.width = width;
            this.height = height;

            foreach (State state in StateStack)
            {
                state.width = width;
                state.height = height;

                if (state.GetType() == typeof(GameState))
                {
                    GameState mygamestate = (GameState)state;
                    mygamestate.ship.x = width / 2;
                    mygamestate.ship.y = height - 100;
                }
            }
        }
    }
}
