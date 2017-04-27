using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * SOURCE ACKNOWLEDGEMENT:
 * Much of the code logic and design is based off of the 
 * JavaScript Game Project developed by Dave Kerr
 * and his accompanying tutorials:
 * https://www.codeproject.com/articles/681130/learn-javascript-part-space-invaders
 */
namespace Assign4_SpaceBlasters
{
    /// <summary>
    /// Game State
    /// </summary>
    public class GameState : State
    {
        string UpdateStatus = "";
        Rectangle myrect;
        Background background;
        private Brush brush;
        int dx = 5;
        int dy = 5;
        int score;
        int level;
        public Ship ship; //your ship piece

        //CONFIG ITEMS
        double bombRate;
        int bombMinVelocity = 2;
        int bombMaxVelocity = 7;

        int invaderVelocity = 2;
        int invaderDropDistance = 0;
        int invaderRanks = 5;
        int invaderFiles = 7;

        int rocketVelocity = 5;
        int shipLives;

        double levelMultiplier;
        double levelDifficultyMultiplier = 0.20;

        DateTime lastRocketTime;

        List<Invader> invaders;
        List<Rocket> rockets;
        List<Bomb> bombs;

        string YourName = "";
        bool newHighScoreFlag = false;
        bool newHighScorePosted = false;

        public GameState(int width, int height, int score, int level, int shipLives) : base(width, height)
        {
            
            background = new Background(width, height);
            brush = new SolidBrush(Color.White);
            background.Start();
            myrect = new Rectangle(dx, dy, 200, 100);

            this.score = score;
            this.level = level;
            this.shipLives = shipLives;
            this.rocketVelocity += level;
            invaderVelocity = invaderVelocity * (level / 2 + 1);
            this.ship = new Ship(width / 2, height - 100);
            levelMultiplier = level * levelDifficultyMultiplier;

            invaders = new List<Invader>();
            rockets = new List<Rocket>();
            bombs = new List<Bomb>();
            bombRate = 6 - (level);
            bombMinVelocity += level;
            bombMaxVelocity += level;
            rocketVelocity += level;
            CreateInvaders();

        }

        /// <summary>
        /// Draw the graphics
        /// Also checks if no lives (end of game) and draws gameover instead.
        /// </summary>
        /// <param name="graphics"></param>
        public override void Draw(Graphics graphics)
        {
            //Draw Background Stars
            List < BackgroundStar > backgroundStarsList = background.Update();
            foreach(BackgroundStar star in backgroundStarsList)
            {
                graphics.FillRectangle(brush, star.x, star.y, star.size, star.size);
            }

            //If out of Lives Draw Game Over Message
            if (shipLives == 0)
            {
                using (Font font1 = new Font("ComicSans", 20))
                {
                    brush = new SolidBrush(Color.White);
                    Rectangle rect1 = new Rectangle(width / 2 - 300, height / 2 - 300, 600, 300);
                    graphics.FillRectangle(new SolidBrush(Color.Black), rect1);
                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    graphics.DrawString("Game Over", font1, brush, rect1, stringFormat);
                    
                    rect1 = new Rectangle(width / 2 - 300, height / 2, 600, 300);
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    string ScoreNote = "Final Score: " + score;

                    if (score > Int32.Parse(Properties.Settings.Default.HighScore3)) 
                    {
                        if(YourName.Length < 4)
                        {
                            newHighScoreFlag = true;
                            ScoreNote = "New High Score. Type your Name \n" + YourName;
                            graphics.DrawString(ScoreNote,
                                new Font("ComicSans", 12), brush, new Rectangle(width / 2 - 300, height / 2 - 150, 600, 300), 
                                stringFormat);
                            return;
                        }
                        else
                        {
                            ScoreNote += YourName + "\nNew High Score! \n Push space to go back to welcome";
                            if (!newHighScorePosted)
                            {
                                newHighScore(score, YourName);
                            }               
                        }                                        
                    }

                    graphics.DrawString(ScoreNote,
                        new Font("ComicSans", 12), brush, rect1, stringFormat);
                    
                }
                return;
            }

            //Draw Score/Level Text
            Font font = new Font("ComicSans", 14);
            graphics.DrawString(String.Format("Score: {0}", score), font, new SolidBrush(Color.Blue), new Point(20, 20));
            graphics.DrawString(String.Format("Level: {0}", level), font, new SolidBrush(Color.Blue), new Point(20, 40));
            graphics.DrawString(String.Format("Lives: {0}", shipLives), font, new SolidBrush(Color.Blue), new Point(20, 60));
            //graphics.FillRectangle(new SolidBrush(Color.Red), myrect);

            //Draw Ship!
            graphics.FillRectangle(new SolidBrush(ship.GetColor()), ship.GetRectangle());

            //Draw Bombs
            foreach(Bomb bomb in bombs)
            {
                 graphics.FillEllipse(new SolidBrush(Color.Red), bomb.GetRectangle());
            }

            //Draw Rockets!
            foreach(Rocket rocket in rockets)
            {
                graphics.FillEllipse(new SolidBrush(Color.Aqua), rocket.GetRectangle());
            }

            //Draw Invaders
            foreach(Invader invader in invaders)
            {
                graphics.FillRectangle(new SolidBrush(Color.GreenYellow), invader.GetRectangle());
            }
        }

        /// <summary>
        /// 
        /// Special Method to receive keyboard chars
        /// in case of high score.
        /// Otherwise it is ignored.
        /// </summary>
        /// <param name="letter"></param>
        public void KeyPress(String letter)
        {
            if (newHighScoreFlag)
            {
                YourName += letter;
            }
        }

        /// <summary>
        /// Updates settings with new high score and initials provided
        /// </summary>
        /// <param name="score"></param>
        /// <param name="name"></param>
        private void newHighScore(int score, string name)
        {
            if(score > int.Parse(Properties.Settings.Default.HighScore))
            {
                Properties.Settings.Default.HighScore3 = Properties.Settings.Default.HighScore2;
                Properties.Settings.Default.HighScoreInitials3 = Properties.Settings.Default.HighScoreInitials2;
                Properties.Settings.Default.HighScore2 = Properties.Settings.Default.HighScore;
                Properties.Settings.Default.HighScoreInitials2 = Properties.Settings.Default.HighScoreInitials;
                Properties.Settings.Default.HighScore = score.ToString();
                Properties.Settings.Default.HighScoreInitials = name;
                newHighScorePosted = true;
                return;
            }
            if (score > int.Parse(Properties.Settings.Default.HighScore2))
            {
                Properties.Settings.Default.HighScore3 = Properties.Settings.Default.HighScore2;
                Properties.Settings.Default.HighScoreInitials3 = Properties.Settings.Default.HighScoreInitials2;
                Properties.Settings.Default.HighScore2 = score.ToString();
                Properties.Settings.Default.HighScoreInitials2 = name;
                newHighScorePosted = true;
                return;
            }
            if (score > int.Parse(Properties.Settings.Default.HighScore3))
            {
                Properties.Settings.Default.HighScore3 = score.ToString();
                Properties.Settings.Default.HighScoreInitials3 = name;
                newHighScorePosted = true;
                return;
            }
        }

        /// <summary>
        /// Handles Game based keyboard presses passed from game engine
        /// </summary>
        /// <param name="gameEngine"></param>
        /// <param name="key">string of key pressed</param>
        public override void KeyboardPress(GameEngine gameEngine, string key)
        {
            
            //Special Keyboard presses for this state are programmed here
            if (key == "Space")
            {
                //If no lives left exit gameState
                if(shipLives == 0)
                {
                    if(newHighScoreFlag && YourName.Length < 3)
                    {
                        return;
                    }
                    UpdateStatus = "Done";
                }
                else //pause game:
                {
                    gameEngine.AddState(new PauseState(width, height));
                }
            }
            else if (key == "Left")
            {
                ship.x = ship.x - 5;
                if(ship.x < 0)
                {
                    ship.x = 0;
                }
            }
            else if (key == "Right")
            {
                ship.x = ship.x + 5;
                if (ship.x > width -5)
                {
                    ship.x = width -5;
                }
            }
            else if (key == "G")
            {
                FireRocket();
            }
            
        }

        /// <summary>
        /// Called by game engine after 
        /// the state returns Done in the update method
        /// 
        /// Passes the next state and handles closing current state
        /// </summary>
        /// <returns></returns>
        public override State Leave()
        {
            //Failure
            if(shipLives <= 0)
            {
                return new WelcomeState(width, height);
            }
            else
            {
                return new GameState(width, height, score, level + 1, shipLives);
            }
            
        }
        /// <summary>
        /// Called by engine after drawing to move graphics
        /// returns a status based on gameplay 
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Update()
        {
            if (invaders.Count == 0)
            {
                return "Done"; //Tell GameEngine to Call our leave method which will load next Level
            }

            if (shipLives == 0 && UpdateStatus == "Done")
            {
                return "Game Over"; //Tell GameEngine to Replace us with Welcome Screen
            }
            if(shipLives == 0)
            {
                return null; //No updates to do. Just leave screen until space hit
            }
           

            //Update Blob
            myrect.X += 1;
            myrect.Y += 1;
            

            //Update/move Bombs
            if(bombs.Count > 0)
            {
                foreach(Bomb bomb in bombs.ToList<Bomb>())
                {
                    bomb.y += bomb.velocity;

                    //if bomb went off of screen remove
                    if(bomb.y > height)
                    {
                        bombs.Remove(bomb);
                    }
                }
            }

            //Update/Move Rockets
            if (rockets.Count > 0)
            {
                foreach (Rocket rocket in rockets.ToList<Rocket>())
                {
                    rocket.y -= rocket.velocity;
                    //if rocket went off of screen remove
                    if (rocket.y < 0)
                    {
                        rockets.Remove(rocket);
                    }
                }
            }

            //Call to check for Rocket/Invader Collision
            CheckForInvaderHits();
            //Call to Special function to move all invaders
            MoveInvaders();
            //Call to Special function to drop more bombs
            InvaderDropBombs();
            //Call to check if Ship was hit
            CheckForShipHit();

            return null;
        }


        /// <summary>
        /// Create the invaders
        /// </summary>
        public void CreateInvaders()
        {
            invaders = new List<Invader>();
            for(int rank = 0; rank < invaderRanks; rank++)
            {
                for(int file = 0; file < invaderFiles; file++)
                {
                    invaders.Add(new Invader(width/ 2 + ((invaderFiles/2 - file) * 300 / invaderFiles),
                        (rank * 40 + 50), 
                        rank,
                        file,
                        "Invader",
                        20, 25));
                }
            }
        }

        /// <summary>
        /// Loop through invadors and move them to next position
        /// </summary>
        public void MoveInvaders()
        {
            
            bool HitLeft = false;
            bool HitRight = false;

            foreach(Invader invader in invaders)
            {
                int newx = invader.x + invaderVelocity;
                //int newy = invader.y + invaderVelocity;

                if(HitLeft == false && newx < 0)
                {
                    HitLeft = true;
                }
                else if(HitRight == false && newx > width)
                {
                    HitRight = true;
                }
            }

            if (HitLeft || HitRight)
            {
                invaderVelocity = -invaderVelocity;
                invaderDropDistance = 15 + (invaderVelocity * 2);
            }

            foreach (Invader invader in invaders)
            {
                int newx = invader.x + invaderVelocity;
                int newy = invader.y + invaderDropDistance;

                invader.x = newx;
                invader.y = newy;

                if (invader.y < 0)
                {
                    //Game Over
                    Console.Write("GAME OVER!!!!!!!!!!"); //or lost life
                    return;
                }
            }
            invaderDropDistance = 0;

        }


        /// <summary>
        /// Randomly drops bombs for invaders
        /// </summary>
        public void InvaderDropBombs()
        {
            List<Invader> frontInvaders = new List<Invader>();
            foreach(Invader invader in invaders)
            {
                bool front = true;
                foreach (Invader otherInvader in invaders)
                {
                    if(otherInvader.file == invader.file &&
                        otherInvader.rank > invader.rank)
                    {
                        front = false;
                        break;
                    }       
                }
                if(front) frontInvaders.Add(invader);
            }

            foreach(Invader invader in frontInvaders)
            {
                DateTime now = DateTime.Now;
                if((now - invader.lastBombTime).TotalSeconds > bombRate)
                {
                    invader.lastBombTime = now;
                    Random random = new Random();
                  
                    if (random.Next(1, 4) == 2)
                    {
                        bombs.Add(new Bomb(invader.x, invader.y, random.Next(bombMinVelocity, bombMaxVelocity)));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Fire Rockets from the ship
        /// </summary>
        public void FireRocket()
        {
            DateTime now = DateTime.Now;
            if ((now - lastRocketTime).TotalSeconds > .5) //so ya can't fire like crazy
            {
                rockets.Add(new Rocket(ship.x + 3, ship.y + 2, this.rocketVelocity));
                this.lastRocketTime = DateTime.Now;

            }
        }

        /// <summary>
        /// Collision Detection between ship and bombs/invaders.
        /// Hit reduces ship life
        /// </summary>
        /// <returns>boolean true = hit, false = no it</returns>
        private void CheckForShipHit()
        {
            foreach (Bomb bomb in bombs.ToList<Bomb>())
            {
                if (bomb.x >= (ship.x - (ship.width / 2)) &&
                       bomb.x <= (ship.x + (ship.width / 2)) &&
                       bomb.y >= (ship.y - (ship.height / 2)) &&
                       bomb.y <= (ship.y + (ship.height / 2)))
                {
                    bombs.Remove(bomb);
                    shipLives -= 1;
                    ship.lastHitTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Check for Rockets hitting/killing Invaders
        /// and for Invaders colliding with your ship.
        /// Updates Invaders and Ship Lives.
        /// </summary>
        public void CheckForInvaderHits()
        {
            //Rocket Invader Collsion
            foreach(Invader invader in invaders.ToList<Invader>())
            {
                bool bang = false;
                foreach(Rocket rocket in rockets)
                {
                    if(rocket.x >= (invader.x - invader.width/2) && 
                        rocket.x <= (invader.x + invader.width/2) &&
                        rocket.y >= (invader.y - invader.height/2) && 
                        rocket.y <= (invader.y + invader.height/2))
                    {
                        //Hit!

                        rockets.Remove(rocket);
                        bang = true;
                        score += 5;
                        break;
                    }
                }
                if (bang)
                {
                    invaders.Remove(invader);
                }

                //Ship / Invader Collsion
                if((invader.x + invader.width / 2) > (ship.x - this.ship.width/2) &&
                    (invader.x - invader.width / 2) < (ship.x + this.ship.width / 2) &&
                    (invader.y + invader.height/2) > (ship.y - ship.height/2) &&
                    (invader.y - invader.height/2) < (ship.y + ship.height/2))
                {
                    //Dead by Collision with Invader
                    shipLives = 0; //GAME OVER!!
                }
            }
        }
    }
}
