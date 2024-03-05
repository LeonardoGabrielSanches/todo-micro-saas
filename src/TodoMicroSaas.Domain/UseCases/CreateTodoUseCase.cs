using System.Text.Json.Serialization;
using TodoMicroSaas.Domain.Entities;
using TodoMicroSaas.Domain.Exceptions;
using TodoMicroSaas.Domain.Interfaces;
using TodoMicroSaas.Domain.Repositories;

namespace TodoMicroSaas.Domain.UseCases;

public record CreateTodoRequest(string Description)
{
    public Guid OwnerId { get; private set; }

    public void SetOwner(Guid ownerId)
        => OwnerId = ownerId;
};

public record CreateTodoResponse(Guid Id, string Description, bool Done)
{
    public static implicit operator CreateTodoResponse(Todo todo)
        => new(todo.Id, todo.Description, todo.Done);
}

public class CreateTodoUseCase(
    IUserRepository userRepository,
    ITodoRepository todoRepository,
    IPaymentService paymentService)
{
    private const int TODO_MAXIMUM_LIMIT_WITHOUT_SUBSCRIPTION = 5;

    public async Task<CreateTodoResponse> Execute(CreateTodoRequest request)
    {
        var user = await userRepository.GetById(request.OwnerId);

        if (user is null)
            throw new ResourceNotFoundException(nameof(User));

        var hasValidSubscription = await paymentService.HasValidSubscription(user.SubscriptionId);

        if (user.Todos.Count() >= TODO_MAXIMUM_LIMIT_WITHOUT_SUBSCRIPTION && !hasValidSubscription)
            throw new TodoLimitExceededWithNoSubscriptionException();

        var todo = new Todo(request.Description, request.OwnerId);

        await todoRepository.Create(todo);

        return todo;
    }
}