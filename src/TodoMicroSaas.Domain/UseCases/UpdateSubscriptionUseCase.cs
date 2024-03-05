using TodoMicroSaas.Domain.Exceptions;
using TodoMicroSaas.Domain.Repositories;

namespace TodoMicroSaas.Domain.UseCases;

public record UpdateSubscriptionRequest(string CustomerId, string SubscriptionId);

public class UpdateSubscriptionUseCase(IUserRepository userRepository)
{
    public async Task Execute(UpdateSubscriptionRequest request)
    {
        var user = await userRepository.GetByCustomerId(request.CustomerId);

        if (user is null)
            throw new StripeException(string.Format("Error when finding customer {customerId}", request.CustomerId));
        
        user.Subscribe(request.SubscriptionId);
        
        await userRepository.Update(user);
    }
}