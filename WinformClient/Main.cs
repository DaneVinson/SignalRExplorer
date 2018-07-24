using Microsoft.AspNetCore.Http.Connections;
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
            MessagesApiClient = new HttpClient() { BaseAddress = new Uri(ConfigurationManager.AppSettings["MessagesApiUri"]) };
            MessagesApiClient.DefaultRequestHeaders.Accept.Clear();
            MessagesApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            SignalRUri = ConfigurationManager.AppSettings["SignalRUri"];

            InitializeComponent();
            sendButton.Click += (o, e) => { SendCurrentMessage(); };

            MessagesHubConnection = new HubConnectionBuilder()
                                            .WithUrl($"{SignalRUri}/messages", HttpTransportType.WebSockets)
                                            .Build();
            Task.Run(() => MessagesHubConnection.StartAsync()).GetAwaiter().GetResult();

            MessagesHubConnection.On<string>("SendToAll", (s) => {
                MessageBox.Show(s);
            });
        }

        private void SendCurrentMessage()
        {
            string message = messageTextBox.Text;

            var r1 = MessagesApiClient.GetAsync($"{MessagesApiClient.BaseAddress}/{message}").GetAwaiter().GetResult();
            var response = MessagesApiClient.PostAsync(MessagesApiClient.BaseAddress, new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
            outputTextBox.Text = $"{r1.ReasonPhrase}{Environment.NewLine}{response.ReasonPhrase}";
            messageTextBox.Text = String.Empty;
        }


        private readonly HttpClient MessagesApiClient;
        private readonly HubConnection MessagesHubConnection;
        private readonly string SignalRUri;
    }
}
