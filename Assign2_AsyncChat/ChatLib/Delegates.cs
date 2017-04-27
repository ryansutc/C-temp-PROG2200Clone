using System;

namespace ChatLib
{


    /// <summary>
    /// A delegate that defines the Connection Changed event
    /// </summary>
    /// <param name="sender">The object that raises the event</param>
    /// <param name="e">The object that houses the progress data</param>
    public delegate void ConnectionChangedEventHandler(object sender, ConnectionChangedEventArgs e);

    /// <summary>
    /// Define a delegate whose signature is a return of void with no params
    /// </summary>
    /// <param name="sender">The object that raises the event</param>
    /// <param name="e">An empty EventArgs object</param>
    public delegate void TaskCompletedEventHandler(object sender, EventArgs e);
    
}
