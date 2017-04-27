using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatLib
{
    public abstract class Messenger
    {
        public static TcpListener tcplistener = null;
        public static TcpClient tcpClient;
        public static NetworkStream stream;
        public static bool connected = false;
        public string ipAddress;
        public int port;

        bool connectedStatusChange;

        /// <summary>
        /// open stream and check if incoming data
        /// if so, read message and return text.
        /// </summary>
        /// <returns>String is message or null if none</returns>
        static public String CheckForMessages()
        {
            try
            {
                // Receive the TcpServer.response.
                stream = tcpClient.GetStream();

                // String to store the response ASCII representation.
                String incomingData = String.Empty;
                Byte[] data = new Byte[256];
                // Loop to receive all the data sent by the tcpclient.
                int i;
                if (stream.CanRead)
                {
                    if (stream.DataAvailable)
                    {
                        // Read the first batch of the TcpServer response bytes.
                        Int32 bytes = stream.Read(data, 0, data.Length);
                        incomingData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                        return incomingData;
                    }
                }
            }
            catch (ArgumentNullException e)
            {
                return "<<ArgumentNullException: " + e + ">>";
            }
            catch (SocketException e)
            {

            }
            return null;
        }

        /// <summary>
        /// Handle converting a string message to byte data and
        /// send through a stream
        /// </summary>
        /// <param name="message"></param>
        static public string WriteMessage(String message)
        {
            //todo: return success or fail boolean
            try
            {
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
                return "Success";
            }
            catch (ArgumentNullException e)
            {
                return "<<ArgumentNullException: " + e + ">>";
            }
            catch (SocketException e)
            {
                return "<<SocketException: " + e + ">>";
            }
        }

        /// <summary>
        /// stop the messenger. Close stream, tcpClient.
        /// </summary>
        public void Stop()
        {
            stream.Close();
            tcpClient.Close();
        }

        /// <summary>
        /// /// abstract placeholder for starting messenger
        /// children classes handle their own ways
        /// </summary>
        /// <param name="ipAddress">ipaddress to connect to</param>
        /// <param name="port">port to connect to</param>
        public abstract int Start(String ipAddress, int port, out string ErrorMsg);

        /// <summary>
        /// checks if messenger is connected
        /// </summary>
        /// <returns>true = connected, false = disconnected</returns>
        public static bool IsConnected()
        {
            //this code copied from here: http://stackoverflow.com/questions/6993295/how-to-determine-if-the-tcp-is-connected-or-not
            try
            {
                Socket s = tcpClient.Client;
                bool part1 = s.Poll(1000, SelectMode.SelectRead);
                bool part2 = (s.Available == 0);
                if (part1 && part2)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the connection status has changed,
        /// either connecting or disconnecting
        /// </summary>
        /// <returns>true = change, false = no change</returns>
        public static bool ConnectionStatusChanged()
        {
            if (connected != IsConnected())
            {
                connected = IsConnected();
                return true;
            }
            else
            {
                return false;
            }
        }

    }//end class
}
