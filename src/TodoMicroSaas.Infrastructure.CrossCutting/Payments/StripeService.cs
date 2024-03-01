using Microsoft.Extensions.Configuration;
using Stripe;
using TodoMicroSaas.Domain.Interfaces;

namespace TodoMicroSaas.Infrastructure.CrossCutting.Payments;

public class StripeService(IConfiguration configuration) : IPaymentService
{
    private readonly RequestOptions _stripeOptions = new()
    {
        ApiKey = configuration["Stripe:ApiSecretKey"]
    };

    private readonly CustomerService _customerService = new();

    public async Task<string> CreateCustomer(CreateCustomerRequest request)
    {
        var customer = await _customerService.CreateAsync(new CustomerCreateOptions
        {
            Name = request.Name,
            Email = request.Email
        }, _stripeOptions);

        return customer.Id;
    }
}