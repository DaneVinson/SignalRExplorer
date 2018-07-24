using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MessagesApi.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        // SignalR appears to wire up the DI for the hub argument by itself.
        public MessagesController(IHubContext<MessageHub> hub)
        {
            MessageHub = hub ?? throw new ArgumentNullException();
        }


        [HttpPost]
        public IActionResult SendAllMessage([FromBody]string message)
        {
            MessageHub.Clients.All.SendAsync("SendToAll", message);
            return Ok();
        }

        [HttpGet]
        [Route("{message}")]
        public IActionResult EchoMessage(string message)
        {
            //MessageHub.SendToAll(message);
            return Ok();
        }


        //private readonly MessageHub MessageHub;
        private readonly IHubContext<MessageHub> MessageHub;
    }
}