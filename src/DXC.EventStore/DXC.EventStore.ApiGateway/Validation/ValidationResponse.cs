using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;

namespace DXC.EventStore.ApiGateway.Validation
{
    public class ValidationResponse
    {
        private readonly ICollection<string> errors;

        public bool IsValid => !errors.Any();

        public bool IsInvalid => !IsValid;

        public ValidationResponse()
        {
            errors = new List<string>();
        }

        public ValidationResponse AddError(string error)
        {
            errors.Add(error);
            return this;
        }

        public IEnumerable<string> Errors => errors;
    }
}