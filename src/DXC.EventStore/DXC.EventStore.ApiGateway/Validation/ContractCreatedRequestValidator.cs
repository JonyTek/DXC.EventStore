using DXC.EventStore.ApiGateway.App.Commands;

namespace DXC.EventStore.ApiGateway.Validation
{
    public class ContractCreatedRequestValidator : IValidator<ContractCreatedRequest>
    {
        public ValidationResponse Validate(ContractCreatedRequest request)
        {
            var response = new ValidationResponse();

            if (string.IsNullOrWhiteSpace(request.ContactId))
            {
                response.AddError("Invalid contract id provided");
            }

            if (string.IsNullOrWhiteSpace(request.BrokerId))
            {
                response.AddError("Invalid broker id provided");
            }

            return response;
        }
    }
}