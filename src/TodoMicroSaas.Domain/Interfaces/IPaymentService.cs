namespace TodoMicroSaas.Domain.Interfaces;

public record CreateCustomerRequest(string Name, string Email);

public record CreateCheckoutSessionRequest(string UserId, string CustomerId);

public interface IPaymentService
{
    Task<string> CreateCustomer(CreateCustomerRequest request);
    Task<string> CreateCheckoutSession(CreateCheckoutSessionRequest request);
    Task<bool> HasValidSubscription(string? subscriptionId);
}