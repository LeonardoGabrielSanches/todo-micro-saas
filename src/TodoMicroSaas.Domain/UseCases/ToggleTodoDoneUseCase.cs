using TodoMicroSaas.Domain.Entities;
using TodoMicroSaas.Domain.Exceptions;
using TodoMicroSaas.Domain.Repositories;

namespace TodoMicroSaas.Domain.UseCases;

public record ToggleTodoDoneRequest(Guid TodoId, Guid OwnerId);

public class ToggleTodoDoneUseCase(ITodoRepository todoRepository)
{
    public async Task Execute(ToggleTodoDoneRequest request)
    {
        var todo = await todoRepository.GetById(request.TodoId);

        if (todo is null)
            throw new ResourceNotFoundException(nameof(Todo));

        if (todo.OwnerId != request.OwnerId)
            throw new TodoToggleDoneException();

        todo.ToggleDone();

        await todoRepository.Update(todo);
    }
}