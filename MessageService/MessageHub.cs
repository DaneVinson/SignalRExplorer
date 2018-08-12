using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageService
{
    public class MessageHub : Hub
    {
        public void SendToAll(string name, string message)
        {
            Clients.All.SendAsync("sendtoall", name, message);
        }
    }
}
