using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatLib;
using System.Threading;

namespace ChatConsole
{
    /// <summary>
    /// handles front end functionality and holds Main()
    /// </summary>
    class Program
    {
        
        static void Main(string[] args)
        {
            Client client;
            Server server;
            String msg;
            string ip;
            int port;
            String consoleType;
            String otherConsoleType;
            bool connectedStatusChange;
            ConsoleColor consoleColor = ConsoleColor.Yellow;
            Messenger messenger;
            string errorMessage = string.Empty;




            Program myprogram = new Program();
            myprogram.WriteConnectionHelpMsg();
            
            /*
            Console.WriteLine("Please enter an IPAddress");
            ip = Console.ReadLine();
            Console.WriteLine("Please enter a Port");
            port = Int32.Parse(Console.ReadLine());
            */
            ip = "127.0.0.1";
            port = 1302;

            //SERVER
            if (args.Length > 0 && args[0] == "-server")
            {
                server = new Server();
                messenger = server;
                consoleType = "Server";
                otherConsoleType = "Client";
            }
            //CLIENT
            else
            {
                client = new Client();
                messenger = client;
                consoleType = "Client";
                otherConsoleType = "Server";
                //wait for server if not started
            }
            Console.WriteLine("{0} Started", consoleType);


            bool firstLoop = true;
            while (true)
            {
                connectedStatusChange = Messenger.ConnectionStatusChanged();

                //check for keypress
                if ((msg = CheckForKeyPress(myprogram)) != null)
                {
                    if (msg == "--[q]--")
                    {
                        messenger.Stop();
                        Environment.Exit(1); //close program
                    }

                    if (Server.WriteMessage(msg) != "Success")
                    {
                        myprogram.ResetConsoleColor();
                        Console.WriteLine("error " + msg);
                    }
                    else
                    {
                        myprogram.ResetConsoleColor();
                        Console.WriteLine("message sent");
                    }  
                }

                //Connection
                if(!Messenger.IsConnected())
                { 
                    if(firstLoop)
                    {
                        Console.Write("Waiting for {0}...", otherConsoleType);
                        firstLoop = false;
                    }
                    else if (connectedStatusChange)
                    {
                        Console.WriteLine("Your connection has been lost. Trying to reconnect...");
                    }

                    if (consoleType == "Server")
                    {
                        if (messenger.Start(ip, port, out errorMessage)) //1 = tcplistener started success
                        {
                            //check for tcpclient
                            Server.CheckForClient();
                        }
                        else
                        {
                            Console.WriteLine(errorMessage);
                        }                  
                    }
                    else if (consoleType == "Client")
                    {                      
                        messenger.Start(ip, port, out errorMessage);
                    }
                }
                //Connection Reestablished
                else if(Messenger.IsConnected())
                {
                    if(connectedStatusChange)
                    {
                        Console.WriteLine("connected!");
                    }
                    if ((msg = Messenger.CheckForMessages()) != null)
                    {
                        //hack for now
                        if(msg.StartsWith("<<"))
                        {
                            Console.WriteLine("error. " + msg);
                        }
                        else
                        {
                            myprogram.ChangeConsoleColor(consoleColor);
                            Console.WriteLine("Received {0}", msg);
                            myprogram.ResetConsoleColor();
                        }
                        
                    }
                }             
            } 
        }

        /// <summary>
        /// Listens for key press to enter writing mode or quit.
        /// [i] will enter writing mode.
        /// [q] will quit application.
        /// 
        /// If [i] is pressed and writing mode is entered,
        /// will capture string text for message.
        /// </summary>
        /// <returns>returns 'null' if invalid key, returns'--[q]---' to quit, or message</returns>
        static String CheckForKeyPress(Program myprogram)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.I)
                {
                    if (!Messenger.IsConnected())
                    {
                        Console.WriteLine("Cannot send messages. Not Connected");
                        return null;
                    }
                    else
                    {
                        ConsoleColor consoleColor = ConsoleColor.Yellow;
                        //[todo]: change keypress to console read.
                        myprogram.ChangeConsoleColor(consoleColor);
                        Console.Write(">>");

                        string msg = Console.ReadLine();
                        return msg;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Q)
                {
                    Console.WriteLine("Are you sure you want to quit? [y/n]");
                    keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.Y)
                    {
                        Console.WriteLine("Shutting Down");
                        return "--[q]--";
                    }
                    else
                    {
                        Console.WriteLine("Cancelled");
                    }
                        
                }
            }
            return null;
        }

        /// <summary>
        /// Writes a simple welcome message to console when program started
        /// </summary>
        protected void WriteConnectionHelpMsg()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine("Welcome to the Messaging App.");
            Console.WriteLine("Press [i] at any time to toggle message writing mode. Press [q] at any time to quit");
            Console.ResetColor();
        }

        /// <summary>
        /// Changes Console Text Color
        /// </summary>
        /// <param name="consoleColor">Color to change text to</param>
        protected void ChangeConsoleColor(ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
        }

        /// <summary>
        /// Resets ConsoleColor to default
        /// </summary>
        protected void ResetConsoleColor()
        {
            Console.ResetColor();
        }
        
           
    }
}
