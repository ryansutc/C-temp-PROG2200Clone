using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Logging;

namespace ChatLib
{
    public class Client : Messenger
    {
        public static bool connected = false;
        
        bool connectedStatusChange;
        string ErrorMsg;
        String msg; //stores incoming messages from server
        public volatile bool stopListening;
        public AppLogger logger;

        public event ConnectionChangedEventHandler ConnectionChanged;
        public Client()
        {
            logger = new AppLogger();
            ConnectionChanged += 
                new ChatLib.ConnectionChangedEventHandler(Client_ConnectionChanged2);
        }
       

        /// <summary>
        /// Connect to Server and Listen for data/msgs
        /// </summary>
        /// <param name="server">The IP address of the server (port is hardcoded)</param>
        /// <returns>1 = success, -1 = fail</returns>
        public override int Start(String ipAddress, int port, out string errorMsg)
        {
            try
            {
                // Create a TcpClient.
                tcpClient = new TcpClient(ipAddress, port);
                errorMsg = "";
                return 1; //success
            }
            catch (ArgumentNullException e)
            {
                errorMsg = String.Format("ArgumentNullException: {0}", e);

                return -1;
            }
            catch (SocketException e)
            {
                errorMsg = String.Format("SocketException: {0}", e);
                return -1;
            }
        }

        /// <summary>
        /// NEW: Async Continously listen and create events when data comes in
        /// </summary>
        public void Listen()
        {
            bool firstLoop = true;
            stopListening = false;
            while (true)
            {
                if (stopListening) break;
                //has the connectionStatus changed?
                connectedStatusChange = ConnectionStatusChanged();
                
                //No Connection
                if (!Messenger.IsConnected())
                {
                    if (firstLoop)
                    {
                        ConnectionChanged(this, 
                            new ConnectionChangedEventArgs(false, "<<Waiting for Server Connection...>>"));

                        firstLoop = false;
                    }
                    else if (connectedStatusChange)
                    {
                        ConnectionChanged(this,
                           new ConnectionChangedEventArgs(false, 
                           "<<Your connection has been lost. Trying to reconnect...>>"));
                    }
                    //FINALLY, TRY TO CONNECT
                    Start(ipAddress, port, out ErrorMsg);
                }
                //Connection Established
                else if (Messenger.IsConnected())
                {
                    if (connectedStatusChange)
                    {
                        ConnectionChanged(this,
                           new ConnectionChangedEventArgs(true,
                           "<<Connection Made>>"));
                    }
                    if ((msg = Messenger.CheckForMessages()) != null)
                    {
                        //hack for now
                        if (msg.StartsWith("<<"))
                        {
                            //raise Message event (ERROR)
                            ConnectionChanged(this, new ConnectionChangedEventArgs(true, msg));
                        }
                        else
                        {
                            //raise message event
                            ConnectionChanged(this, new ConnectionChangedEventArgs(true, "Received: " + msg));
                        }

                    }
                }

                //Call the delegate variable like a function to raise the event
                //broadcast that some work was done
                //raise an event
                //this will trigger the connected function in the UI to execute

                   

            }//end while
        }

        //Connection Changed Event handler
        public void Client_ConnectionChanged2(object sender, ConnectionChangedEventArgs e)
        {
            //event gets called by Listener thread via Delegate
            if (!logger.Write(e.ConnMsg, out ErrorMsg))
            {
                return;
                //somethign bad happened
            }
        }

        /// <summary>
        /// Log message to writer
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            //event gets called by Listener thread via Delegate
            if (!logger.Write(message, out ErrorMsg))
            {
                return;
                //somethign bad happened
            }
        }

    }//end class
}
