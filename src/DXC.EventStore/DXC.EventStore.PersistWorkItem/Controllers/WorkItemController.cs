using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DXC.EventStore.PersistWorkItem.App.Application;
using Microsoft.AspNetCore.Mvc;
using CreateWorkItem = DXC.EventStore.Contracts.CreateWorkItem;

namespace DXC.EventStore.PersistWorkItem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkItemController : ControllerBase
    {
        private readonly ICreateWorkItem createWorkItem;

        public WorkItemController(ICreateWorkItem createWorkItem)
        {
            this.createWorkItem = createWorkItem;
        }

        [HttpPost]
        [Route("persist")]
        public async Task<IActionResult> Persist([FromBody] CreateWorkItem request)
        {
            try
            {
                await createWorkItem.Execute(request);

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
