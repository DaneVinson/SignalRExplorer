using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformClient2
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            SignalRUri = ConfigurationManager.AppSettings["SignalRUri"];
            OutputBuilder = new StringBuilder();
            connectButton.Click += (o, e) => Connect();
            sendButton.Click += (o, e) => SendCurrentMessage();
            FormClosing += (o, e) => CloseForm();

            HubConnection = new HubConnection($"{SignalRUri}/messages");
            HubConnection.Closed += ConnectionClosed;
            MessagesHubProxy = HubConnection.CreateHubProxy("MessagesHub");
            MessagesHubProxy.On<string, string>("SendToAll", (name, message) =>
            {
                Invoke((Action)(() => OutputBuilder.AppendLine($"Received: name: {name}, message: {message}")));
            });
        }

        private void CloseForm()
        {
            if (HubConnection != null)
            {
                HubConnection.Stop(TimeSpan.FromMilliseconds(500));
                HubConnection.Dispose();
            }
        }

        private void ConnectionClosed()
        {
            connectButton.Enabled = true;
            messageTextBox.Enabled = false;
            sendButton.Enabled = false;
        }

        private void Connect()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Task.Run(() => HubConnection.Start()).GetAwaiter().GetResult();
                OutputBuilder.AppendLine("Connected to SignarR Hub");
                connectButton.Enabled = false;
                messageTextBox.Enabled = true;
                sendButton.Enabled = true;
            }
            catch (Exception exception)
            {
                OutputBuilder.AppendLine(exception.GetType().ToString())
                            .AppendLine(exception.Message)
                            .AppendLine(exception.StackTrace)
                            .AppendLine("------------------");
                outputTextBox.Text = OutputBuilder.ToString();
            }
            finally { Cursor = Cursors.Default; }
        }

        private void SendCurrentMessage()
        {
            MessagesHubProxy.Invoke("SendToAll", "WinFormClientX", messageTextBox.Text);
            messageTextBox.Text = String.Empty;
        }


        private readonly HubConnection HubConnection;
        private IHubProxy MessagesHubProxy { get; set; }
        private readonly StringBuilder OutputBuilder;
        private readonly string SignalRUri;
    }
}
