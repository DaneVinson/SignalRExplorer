using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MessagesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HandshakeController : ControllerBase
    {
        [HttpGet]
        public IActionResult HandShake()
        {
            return Ok(true);
        }
    }
}