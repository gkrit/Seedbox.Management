using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Seedbox.Manager.WebAPI.Models;
using Transmission.API.RPC;
using Transmission.API.RPC.Arguments;
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

        public ManagerController(ILogger<ManagerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("getSession")]
        public SessionInfo GetSessionInfo(eSeedBoxIndex seedbox)
        {
            var client = new Client(string.Format(HOST, (int)seedbox), SESSION_ID);
            SessionInfo info = client.GetSessionInformation();
            return info;
        }

        [HttpPost("changeAltSpeedDown")]
        public bool ChangeAlternativeSpeedDown(eSeedBoxIndex seedbox, int offset)
        {
            var client = new Client(string.Format(HOST, (int)seedbox), SESSION_ID);
            SessionInfo info = client.GetSessionInformation();

            //Set new session settings
            client.SetSessionSettings(new SessionSettings() { AlternativeSpeedDown = info.AlternativeSpeedDown += offset });
            return true;
        }

        [HttpPost("PortTest")]
        public bool PortTest(eSeedBoxIndex seedbox)
        {
            var client = new Client(string.Format(HOST, (int)seedbox), SESSION_ID);

            bool ok = client.PortTest();
            return ok;
        }

    }

}