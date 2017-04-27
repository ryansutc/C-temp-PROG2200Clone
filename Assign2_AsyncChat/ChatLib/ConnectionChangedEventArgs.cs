using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLib
{
    public class ConnectionChangedEventArgs : EventArgs
    {

        /// <summary>
        /// Constructor that collects the event data
        /// </summary>
        /// <param name="progress">The Current progress value</param>
        public ConnectionChangedEventArgs(bool Connection, string ConnMsg)
        {
            this.Connection = Connection;
            this.ConnMsg = ConnMsg;
        }

        /// <summary>
        /// A public readonly property that exposes the current progress value
        /// </summary>
        public bool Connection { get; }
        public string ConnMsg { get; }

    }
    

}
