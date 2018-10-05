using System;
using System.Threading.Tasks;
using DXC.EventStore.ApiGateway.App.Commands;
using DXC.EventStore.ApiGateway.Validation;
using DXC.EventStore.Contracts;
using DXC.EventStore.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace DXC.EventStore.ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IValidator<ContractCreatedRequest> validator;
        private readonly IBus bus;

        public ContractController(IValidator<ContractCreatedRequest> validator, IBus bus)
        {
            this.validator = validator;
            this.bus = bus;
        }

        [HttpPost]
        [Route("created")]
        public async Task<IActionResult> Created([FromBody] ContractCreatedRequest request)
        {
            try
            {
                var validationResult = validator.Validate(request);

                if (validationResult.IsInvalid)
                {
                    return BadRequest(validationResult.Errors);
                }

                await bus.Publish(new CreateWorkItem
                {
                    ContractId = request.ContactId,
                    BrokerId = request.BrokerId
                });

                return Accepted();
            }
            catch (Exception)
            {
                //Log
                return BadRequest();
            }
        }
    }
}
