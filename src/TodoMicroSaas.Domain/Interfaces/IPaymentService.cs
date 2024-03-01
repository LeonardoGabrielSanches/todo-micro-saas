namespace TodoMicroSaas.Domain.Interfaces;

public record CreateCustomerRequest(string Name, string Email);

public interface IPaymentService
{
    Task<string> CreateCustomer(CreateCustomerRequest request);
}