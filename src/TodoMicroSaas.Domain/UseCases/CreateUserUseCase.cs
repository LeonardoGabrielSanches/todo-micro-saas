using TodoMicroSaas.Domain.Entities;
using TodoMicroSaas.Domain.Repositories;

namespace TodoMicroSaas.Domain.UseCases;

public record CreateUserRequest(string Name, string Email);

public record CreateUserResponse(Guid Id, string Name, string Email)
{
    public static implicit operator CreateUserResponse(User user)
        => new(user.Id, user.Name, user.Email);
};

public class CreateUserUseCase(IUserRepository userRepository)
{
    public async Task<CreateUserResponse> Execute(CreateUserRequest request)
    {
        var user = new User(request.Name, request.Email, "customer-id");

        await userRepository.Create(user);

        return user;
    }
}