using Microsoft.AspNetCore.SignalR;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessagesApi
{
    public class MessageHub : Hub
    {
        //public void SendToAll(string message)
        //{
        //    Clients.All.SendAsync("SendToAll", message);
        //}

        //public void SendToClient(ResultMessage resultMessage)
        //{
        //    Clients.Client(resultMessage.ClientName)
        //            .SendAsync("AlertClient", resultMessage);
        //}
    }
}
