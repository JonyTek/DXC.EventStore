namespace DXC.EventStore.ApiGateway.Validation
{
    public interface IValidator<in T>
    {
        ValidationResponse Validate(T request);
    }
}