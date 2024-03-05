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
    private readonly SubscriptionService _subscriptionService = new();

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
            ClientReferenceId = request.UserId,
            Customer = request.CustomerId,
            SuccessUrl = "https://localhost:7059/success",
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Price = configuration["Stripe:PremiumPriceKey"],
                    Quantity = 1
                }
            ],
            Mode = "subscription"
        }, _stripeOptions);

        return checkoutSession.Url;
    }

    public async Task<bool> HasValidSubscription(string? subscriptionId)
    {
        if (string.IsNullOrEmpty(subscriptionId))
            return false;

        var subscription = await _subscriptionService.GetAsync(subscriptionId, requestOptions: _stripeOptions);

        return subscription.Status is "active";
    }

    public async Task Unsubscribe(string subscriptionId)
        => await _subscriptionService.CancelAsync(subscriptionId, requestOptions: _stripeOptions);
}