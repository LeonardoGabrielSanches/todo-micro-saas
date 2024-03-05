using TodoMicroSaas.Domain.Entities;
using TodoMicroSaas.Domain.Exceptions;
using TodoMicroSaas.Domain.Interfaces;
using TodoMicroSaas.Domain.Repositories;

namespace TodoMicroSaas.Domain.UseCases;

public record CancelSubscriptionRequest(Guid UserId);

public class CancelSubscriptionUseCase(IUserRepository userRepository, IPaymentService paymentService)
{
    public async Task Execute(CancelSubscriptionRequest request)
    {
        var user = await userRepository.GetById(request.UserId);

        if (user is null)
            throw new ResourceNotFoundException(nameof(User));

        if (string.IsNullOrEmpty(user.SubscriptionId))
            throw new ActiveSubscriptionException();

        await paymentService.Unsubscribe(user.SubscriptionId);

        user.Unsubscribe();

        await userRepository.Update(user);
    }
}