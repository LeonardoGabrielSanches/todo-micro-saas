using TodoMicroSaas.Domain.Entities;
using TodoMicroSaas.Domain.Exceptions;
using TodoMicroSaas.Domain.Repositories;

namespace TodoMicroSaas.Domain.UseCases;

public record CreateTodoRequest(string Description, Guid OwnerId);

public record CreateTodoResponse(Guid Id, string Description, bool Done)
{
    public static implicit operator CreateTodoResponse(Todo todo)
        => new(todo.Id, todo.Description, todo.Done);
}

public class CreateTodoUseCase(
    IUserRepository userRepository,
    ITodoRepository todoRepository)
{
    public async Task<CreateTodoResponse> Execute(CreateTodoRequest request)
    {
        var user = await userRepository.GetById(request.OwnerId);

        if (user is null)
            throw new ResourceNotFoundException(nameof(User));

        var todo = new Todo(request.Description, request.OwnerId);

        await todoRepository.Create(todo);

        return todo;
    }
}