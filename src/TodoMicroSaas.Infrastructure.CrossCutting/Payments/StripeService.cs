using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using TodoMicroSaas.Domain.Interfaces;

namespace TodoMicroSaas.Infrastructure.CrossCutting.Payments;

public class StripeService(IConfiguration configuration) : IPaymentService
{
    private readonly RequestOptions _stripeOptions = new()
    {
        ApiKey = configuration["Stripe:ApiSecretKey"]
    };

    private readonly CustomerService _customerService = new();
    private readonly SessionService _sessionService = new();

    public async Task<string> CreateCustomer(CreateCustomerRequest request)
    {
        var customer = await _customerService.CreateAsync(new CustomerCreateOptions
        {
            Name = request.Name,
            Email = request.Email
        }, _stripeOptions);

        return customer.Id;
    }

    public async Task<string> CreateCheckoutSession(CreateCheckoutSessionRequest request)
    {
        var checkoutSession = await _sessionService.CreateAsync(new SessionCreateOptions
        {
            ClientReferenceId = request.CustomerId,
            SuccessUrl = "https://localhost:7059/success",
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Price = configuration["Stripe:PremiumKey"],
                    Quantity = 1
                }
            ],
            Mode = "subscription"
        }, _stripeOptions);

        return checkoutSession.Url;
    }
}