using System.Diagnostics;
using DXC.EventStore.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DXC.EventStore.PublishWorkItem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemController : ControllerBase
    {
        [HttpPost]
        [Route("publish")]
        public IActionResult Publish([FromBody] WorkItemCreated request)
        {
            //Log => Send to XSB => etc
            return Ok();
        }
    }
}
