using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLib
{
    public class Server : Messenger
    {

        /// <summary>
        /// Start the tcplistener. Returns int:
        /// </summary>
        /// <param name="ipAddress">ipaddress to connect to</param>
        /// <param name="port">port to use</param>
        /// <returns>-1 = bad arguments (crap IPAddress or Port), 1 = success</returns>
        public override int Start(String ipAddress, int port, out string ErrorMsg)
        {
            try
            {
                if (tcplistener == null)
                {
                    IPAddress localAddr = IPAddress.Parse(ipAddress);
                    tcplistener = new TcpListener(localAddr, port);
                    tcplistener.Start();
                }
                ErrorMsg = "";
                return 1;
            }
            catch (Exception e)
            {
                ErrorMsg = String.Format("ArgumentNullException: {0}", e);
                return -1;
            }
        }
        /// <summary>
        /// stop method customized to also close tcplistener.
        /// </summary>
        public new void Stop()
        {
            base.Stop();
            tcplistener.Stop();
        }

        /// <summary>
        /// Non-blocking call to check for client connection
        /// </summary>
        static public bool CheckForClient()
        {
            if (tcplistener.Pending())
            {
                tcpClient = tcplistener.AcceptTcpClient();
                return true;
            }
            return false; //no client trying to connect
        }

    }//end class
}//end namespace
