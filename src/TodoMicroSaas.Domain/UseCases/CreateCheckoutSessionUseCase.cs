using TodoMicroSaas.Domain.Entities;
using TodoMicroSaas.Domain.Exceptions;
using TodoMicroSaas.Domain.Interfaces;
using TodoMicroSaas.Domain.Repositories;

namespace TodoMicroSaas.Domain.UseCases;

public record CreateCheckoutSessionRequest(Guid UserId);

public class CreateCheckoutSessionUseCase(IUserRepository userRepository, IPaymentService paymentService)
{
    public async Task<string> Execute(CreateCheckoutSessionRequest request)
    {
        var user = await userRepository.GetById(request.UserId);

        if (user is null)
            throw new ResourceNotFoundException(nameof(User));

        var checkoutUrl =
            await paymentService.CreateCheckoutSession(
                new Interfaces.CreateCheckoutSessionRequest(user.Id.ToString(), user.CustomerId));

        return checkoutUrl;
    }
}