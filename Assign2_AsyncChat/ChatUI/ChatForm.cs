using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatLib;
using System.Threading;
using Logging;

namespace ChatUI
{
    public partial class ChatForm : Form
    {
        Client client;
        
        string ipAddress = "127.0.0.1";
        int port = 1302;
        bool connectedStatusChange;

        //declare a thread for our background work
        Thread connectionThread;

        string errorMessage = string.Empty;
        public ChatForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load chat form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatForm_Load(object sender, EventArgs e)
        {
            this.Text = "ChatUI Form";
        }

        /// <summary>
        /// handle connect button to connect to server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            client = new Client();
            ConversationTextBox.AppendText("<< Client Started >>", Color.Black);
            ConversationTextBox.AppendText(Environment.NewLine);


            //Start thread to listen for messages from server
            client.Start(ipAddress, port, out errorMessage);

            //Main thread handles user input
            client.ConnectionChanged
                += new ChatLib.ConnectionChangedEventHandler(Client_ConnectionChanged);

            connectionThread = new Thread(client.Listen);
            connectionThread.IsBackground = true; //this will allow front end to kill it when closing
            connectionThread.Name = "ConnectionThread";
            connectionThread.Start();

            disconnectToolStripMenuItem.Enabled = true;
            connectToolStripMenuItem.Enabled = false;
            MessageTextBox.Enabled = true;
        }

        /// <summary>
        /// Incoming data received event handler method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_ConnectionChanged(object sender, ConnectionChangedEventArgs e)
        {
            //event gets called by Listener thread via Delegate

            if (ConversationTextBox.InvokeRequired) //element is on a different thread
            {

                MethodInvoker invoker
                    = new MethodInvoker(delegate () {
                        if (e.ConnMsg.StartsWith("Received"))
                        {
                            ConversationTextBox.AppendText(e.ConnMsg, Color.Blue);
                            ConversationTextBox.AppendText(Environment.NewLine);
                        }
                        else
                        {
                            ConversationTextBox.AppendText(e.ConnMsg, Color.Black);
                            ConversationTextBox.AppendText(Environment.NewLine);
                        }

                        //ConversationTextBox.Text += e.ConnMsg;
                    });

                ConversationTextBox.BeginInvoke(invoker);
            }
            else
            {
                //business as usual
                //update the progress bar
                ConversationTextBox.Text += e.ConnMsg;
            }
        }

        /// <summary>
        /// send a message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendMsgButton_Click(object sender, EventArgs e)
        {
            Client.WriteMessage(MessageTextBox.Text);
            client.Log(MessageTextBox.Text);
            ConversationTextBox.AppendText(">>" + MessageTextBox.Text, Color.Blue);
            ConversationTextBox.AppendText(Environment.NewLine);
            MessageTextBox.Text = "";
            
        }

        /// <summary>
        /// Exit Program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //write to logger
            
            //close the application
            this.Close();
            Application.Exit();
        }

        /// <summary>
        /// disconnect chat app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Start thread to listen for messages from server
            if (client != null)
            {
                client.Stop();
                ConversationTextBox.AppendText("<< Connection Closed >>", Color.Black);
                ConversationTextBox.AppendText(Environment.NewLine);
                connectToolStripMenuItem.Enabled = true;
                MessageTextBox.Enabled = false;
            }
        }
    }
}
