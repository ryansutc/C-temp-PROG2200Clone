using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatLib
{
    public class Client : Messenger
    {
        
        /// <summary>
        /// Connect to Server and Listen for data/msgs
        /// </summary>
        /// <param name="server">The IP address of the server (port is hardcoded)</param>
        /// <returns>1 = success, -1 = fail</returns>
        public override bool Start(String ipAddress, int port, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                // Create a TcpClient.
                tcpClient = new TcpClient(ipAddress, port);
                return true; //success
            }
            catch (ArgumentNullException e)
            {
                //Console.WriteLine("ArgumentNullException: {0}", e);
                errorMessage = e.Message;
                return false;
            }
            catch (SocketException e)
            {
                //Console.WriteLine("SocketException: {0}", e);
                errorMessage = e.Message;
                return false;
            }
        }

    }
}
