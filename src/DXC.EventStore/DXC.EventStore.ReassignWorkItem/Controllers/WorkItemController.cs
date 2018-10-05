using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DXC.EventStore.ReassignWorkItem.App.Application;
using Microsoft.AspNetCore.Mvc;

namespace DXC.EventStore.ReassignWorkItem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemController : ControllerBase
    {
        private readonly IReassignWorkItem reassignWorkItem;

        public WorkItemController(IReassignWorkItem reassignWorkItem)
        {
            this.reassignWorkItem = reassignWorkItem;
        }

        [HttpPost]
        [Route("reassign")]
        public async Task<IActionResult> Reassign([FromBody] Contracts.ReassignWorkItem request)
        {
            try
            {
                //TODO: Validate request
                //Will validation be part of a core library?
                //Ideally we would use FluentValidation which comes as middleware from the box
                await reassignWorkItem.Execute(request);

                return Ok();
            }
            catch (Exception exception)
            {
                Debugger.Break();
                return BadRequest();
            }
        }
    }
}
