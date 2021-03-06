﻿using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
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

namespace WinformClient
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

            MessagesHubConnection = new HubConnectionBuilder()
                                            .WithUrl($"{SignalRUri}/messages")
                                            .Build();
            MessagesHubConnection.On<string, string>("SendToAll", (n, m) =>
            {
                MessageBox.Show($"Name: {n}, Message: {m}");
            });
        }

        private void Connect()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Task.Run(() => MessagesHubConnection.StartAsync()).GetAwaiter().GetResult();
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
            MessagesHubConnection.SendAsync("SendToAll", "WinFormClientX", messageTextBox.Text);
            messageTextBox.Text = String.Empty;
        }


        private readonly HubConnection MessagesHubConnection;
        private readonly StringBuilder OutputBuilder;
        private readonly string SignalRUri;
    }
}
