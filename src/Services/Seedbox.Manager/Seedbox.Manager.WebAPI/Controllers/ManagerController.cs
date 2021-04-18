using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Seedbox.Manager.WebAPI.Models;
using Transmission.API.RPC;
using Transmission.API.RPC.Entity;

namespace Seedbox.Manager.WebAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly ILogger<ManagerController> _logger;
        private static readonly string HOST = "http://seedbox{0}:9091/transmission/rpc";
        private static readonly string SESSION_ID = "";
        private Client client = null;

        public ManagerController(ILogger<ManagerController> logger)
        {
            _logger = logger;
            //client = new Client(HOST, SESSION_ID);
        }

        [HttpGet("getSession")]
        public SessionInfo GetSessionInfo(eSeedBoxIndex seedbox)
        {
            SessionInfo info = null;
            //int port = 9091 + (int)seedbox;
            client = new Client(string.Format(HOST, (int)seedbox), SESSION_ID);
            info = client.GetSessionInformation();
            return info;
        }

    }

}